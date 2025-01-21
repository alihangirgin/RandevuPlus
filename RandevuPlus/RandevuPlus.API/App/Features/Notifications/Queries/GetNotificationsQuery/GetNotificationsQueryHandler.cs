using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using RandevuPlus.API.Shared.Models;

namespace RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsQuery
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<List<GetNotificationsQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public GetNotificationsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUserService userService, IHubContext<UserHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userService = userService;
            _hubContext = hubContext;
        }

        public async Task<Result<List<GetNotificationsQueryResponse>>> Handle(GetNotificationsQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var notifications = await _unitOfWork.Notifications.GetQueryable()
                .Where(x => x.ReceiverId == userId && !x.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                await _unitOfWork.Notifications.UpdateAsync(notification);
            }
            await _unitOfWork.CommitAsync();

            if (_userService.GetOnlineUsers().Contains(userId.ToString()))
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("NotificationReceived");
            }

            var response = notifications.Select(x => new GetNotificationsQueryResponse(x.Id, x.NotificationText)).ToList();
            return Result.Success(response);
        }
    }
}
