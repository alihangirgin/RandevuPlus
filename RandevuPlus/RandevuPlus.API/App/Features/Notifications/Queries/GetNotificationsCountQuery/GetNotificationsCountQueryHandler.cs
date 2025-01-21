using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsCountQuery
{
    public class GetNotificationsCountQueryHandler : IRequestHandler<GetNotificationsCountQuery, Result<GetNotificationsCountQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetNotificationsCountQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetNotificationsCountQueryResponse>> Handle(GetNotificationsCountQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var inboxCount = await _unitOfWork.Notifications.CountNotificationsAsync(userId);
            return Result.Success(new GetNotificationsCountQueryResponse(inboxCount));
        }
    }
}
