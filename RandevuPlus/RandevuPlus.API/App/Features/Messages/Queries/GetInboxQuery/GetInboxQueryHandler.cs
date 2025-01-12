using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Extensions;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Globalization;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public class GetInboxQueryHandler : IRequestHandler<GetInboxQuery, Result<PaginatedResponse<GetInboxQueryResponseItem>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetInboxQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IUserService userService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<Result<PaginatedResponse<GetInboxQueryResponseItem>>> Handle(GetInboxQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var inboxQuery = _unitOfWork.Messages
                .GetQueryable()
                .Include(x => x.Sender)
                .Include(x=> x.Receiver)
                .Where(x =>
                    (x.SenderId == userId  && !x.IsRemovedFromSender) ||
                    (x.ReceiverId == userId && !x.IsRemovedFromReceiver)
                )
                .GroupBy(x => x.SenderId == userId ? x.ReceiverId : x.SenderId)
                .Select(y => new
                {
                    LastMessage = y.OrderByDescending(x => x.CreatedAt).FirstOrDefault(),
                    UnreadCount = y.Count(x => !x.IsRead && x.ReceiverId == userId)
                });


            var totalCount = await inboxQuery.CountAsync();

            var inboxMessages = await inboxQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var responseInbox = inboxMessages
            .Select(x => new GetInboxQueryResponseItem
            {
                Id = x.LastMessage.Id,
                SenderId = x.LastMessage.SenderId == userId ? x.LastMessage.ReceiverId : x.LastMessage.SenderId,
                SenderName = x.LastMessage.SenderId == userId ? x.LastMessage.Receiver.FullName : x.LastMessage.Sender.FullName,
                IsRead = x.LastMessage.IsRead,
                LastMessageDate = x.LastMessage.CreatedAt.ToString("d MMM yyyy", new CultureInfo("tr-TR")),
                ShortenedMessageText = MessageHelper.ShortenMessage(x.LastMessage.MessageText, 20),
                UnreadCount = x.UnreadCount,
                SenderStatus = _userService.GetUserStatus(x.LastMessage.SenderId == userId ? x.LastMessage.ReceiverId : x.LastMessage.SenderId),
                SenderPhotoUrl = x.LastMessage.SenderId == userId ? x.LastMessage.Receiver.PhotoUrl : x.LastMessage.Sender.PhotoUrl
            })
            .ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            return Result.Success(new PaginatedResponse<GetInboxQueryResponseItem>
            {
                Items = responseInbox,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
        }
    }
}
