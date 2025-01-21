using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsQuery
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<List<GetNotificationsQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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

            var response = notifications.Select(x => new GetNotificationsQueryResponse(x.Id, x.NotificationText)).ToList();
            return Result.Success(response);
        }
    }
}
