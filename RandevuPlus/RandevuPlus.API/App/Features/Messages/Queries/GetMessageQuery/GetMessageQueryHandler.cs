using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQueryHandler : IRequestHandler<GetMessageQuery, Result<GetMessageQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IUserService userService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<Result<GetMessageQueryResponse>> Handle(GetMessageQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var recipientUser = await _userManager.FindByIdAsync(query.RecipientId.ToString());
            if (recipientUser == null) return Result.Error("RecipientNotFound");
            var recipientRoles = await _userManager.GetRolesAsync(recipientUser);
            bool isInstructor = recipientRoles.Contains("Instructor");
            Instructor? instructor = null;
            if (isInstructor)
            {
                instructor = await _unitOfWork.Instructors.GetByUserIdAsync(query.RecipientId);
                if (instructor == null) return Result.Error("InstructorNotFound");
            }

            var messagesQuery = _unitOfWork.Messages
                .GetQueryable()
                .Include(x=> x.Reactions)
                .Where(x =>
                    (x.SenderId == userId && x.ReceiverId == query.RecipientId && !x.IsRemovedFromSender) ||
                    (x.SenderId == query.RecipientId && x.ReceiverId == userId && !x.IsRemovedFromReceiver)
                )
                .OrderByDescending(x => x.CreatedAt);


            var totalCount = await messagesQuery.CountAsync();

            var messages = await messagesQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            foreach (var message in messages)
            {
                if(message.ReceiverId == userId)
                    message.IsRead = true;
            }
            await _unitOfWork.Messages.UpdateRangeAsync(messages);
            await _unitOfWork.CommitAsync();

            var responseMessages = messages
                .Select(x => new GetMessageQueryMessageResponse(
                    x.Id,
                    x.MessageText,
                    x.CreatedAt,
                    x.SenderId == userId ? MessageType.Outgoing : MessageType.Incoming,
                    x.Reactions
                        .GroupBy(r => r.Reaction) 
                        .Select(g => new GetMessageQueryMessageReactionResponse(
                            g.Key,           
                            g.Count(),      
                            g.Any(r => r.ReactorId == userId) 
                        ))
                        .ToList()
                ))
                .ToList();

            var responseRecipient = new GetMessageQueryUserResponse(
                recipientUser.Id,
                recipientUser.FullName,
                isInstructor ? instructor?.Title : "Öğrenci",
                _userService.GetUserStatus(recipientUser.Id),
                recipientUser.PhotoUrl
            );

            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            return Result.Success(new GetMessageQueryResponse(responseRecipient, responseMessages, query.PageNumber, query.PageSize, totalCount, totalPages));
        }
    }
}
