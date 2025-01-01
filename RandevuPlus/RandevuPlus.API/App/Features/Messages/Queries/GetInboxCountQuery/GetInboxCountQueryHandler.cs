using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxCountQuery
{
    public class GetInboxCountQueryHandler : IRequestHandler<GetInboxCountQuery, Result<GetInboxCountQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public GetInboxCountQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetInboxCountQueryResponse>> Handle(GetInboxCountQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var inboxCount = await _unitOfWork.Messages.CountInboxAsync(userId);
            return Result.Success(new GetInboxCountQueryResponse(inboxCount));
        }
    }
}
