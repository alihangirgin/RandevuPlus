using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Extensions;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Globalization;

namespace RandevuPlus.API.App.Features.Messages.Queries.SearchMessageQuery
{
    public class SearchMessageQueryHandler : IRequestHandler<SearchMessageQuery, Result<List<GetInboxQueryResponseItem>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SearchMessageQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetInboxQueryResponseItem>>> Handle(SearchMessageQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var searchMessages = await _unitOfWork.Messages
                .GetQueryable()
                .Include(x => x.Sender)
                .Where(x =>
                    (
                        (x.SenderId == userId && !x.IsRemovedFromSender) ||
                        (x.ReceiverId == userId && !x.IsRemovedFromReceiver) 
                    ) && x.MessageText.Contains(query.Prefix)
                )
                .GroupBy(x => x.SenderId)
                .Select(y => new
                {
                    LastMessage = y.OrderByDescending(x => x.CreatedAt).FirstOrDefault(),
                    UnreadCount = y.Count(x => !x.IsRead)
                })
                .ToListAsync();

            var responseMessages = searchMessages
            .Select(x => new GetInboxQueryResponseItem
            {
                Id = x.LastMessage.Id,
                SenderId = x.LastMessage.SenderId,
                SenderName = x.LastMessage.Sender.FullName,
                IsRead = x.LastMessage.IsRead,
                LastMessageDate = x.LastMessage.CreatedAt.ToString("d MMM yyyy", new CultureInfo("tr-TR")),
                ShortenedMessageText = MessageHelper.ShortenMessage(x.LastMessage.MessageText, 20),
                UnreadCount = x.UnreadCount,
                SenderStatus = UserStatus.Online,
                SenderPhotoUrl = x.LastMessage.Sender.PhotoUrl
            })
            .ToList();

            return Result<List<GetInboxQueryResponseItem>>.Success(responseMessages);
        }
    }
}
