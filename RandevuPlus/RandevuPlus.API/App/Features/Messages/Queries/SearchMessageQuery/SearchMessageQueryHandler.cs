using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
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
        private readonly IUserService _userService;

        public SearchMessageQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IUserService userService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<Result<List<GetInboxQueryResponseItem>>> Handle(SearchMessageQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var searchMessages = await _unitOfWork.Messages
                .GetQueryable()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
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

            return Result<List<GetInboxQueryResponseItem>>.Success(responseMessages);
        }
    }
}
