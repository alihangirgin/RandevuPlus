using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Globalization;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public class GetInboxQueryHandler : IRequestHandler<GetInboxQuery, Result<PaginatedResponse<GetInboxQueryResponseItem>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetInboxQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedResponse<GetInboxQueryResponseItem>>> Handle(GetInboxQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var inboxQuery = _unitOfWork.Messages
                .GetQueryable()
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == userId && !x.IsRemovedFromReceiver)
                .GroupBy(x => x.SenderId)
                .Select(y => new
                {
                    LastMessage = y.OrderByDescending(x => x.CreatedAt).FirstOrDefault(),
                    UnreadCount = y.Count(x => !x.IsRead)
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
                SenderId = x.LastMessage.SenderId,
                SenderName = x.LastMessage.Sender.FullName,
                IsRead = x.LastMessage.IsRead,
                LastMessageDate = x.LastMessage.CreatedAt.ToString("d MMM yyyy", new CultureInfo("tr-TR")),
                ShortenedMessageText = ShortenMessage(x.LastMessage.MessageText, 20),
                UnreadCount = x.UnreadCount,
                SenderStatus = UserStatus.Online,
                SenderPhotoUrl = x.LastMessage.Sender.PhotoUrl
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
        private string ShortenMessage(string messageText, int maxLength)
        {
            if (string.IsNullOrEmpty(messageText))
                return messageText;

            if (messageText.Contains("\n"))
            {
                var indexOfNewLine = messageText.IndexOf("\n");
                return messageText.Substring(0, indexOfNewLine) + "...";
            }

            if (messageText.Length > maxLength)
            {
                return $"{messageText.Substring(0, maxLength)} ...";
            }

            return messageText;
        }
    }
}
