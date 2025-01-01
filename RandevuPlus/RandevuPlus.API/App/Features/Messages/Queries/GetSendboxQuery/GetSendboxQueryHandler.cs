using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetSendboxQuery
{
    public class GetSendboxQueryHandler : IRequestHandler<GetSendboxQuery, Result<List<GetInboxQueryResponseItem>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetSendboxQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetInboxQueryResponseItem>>> Handle(GetSendboxQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var inboxMessages = await _unitOfWork.Messages.GetPaginatedAsync(query.PageNumber, query.PageSize,
                filter: x => x.SenderId == userId && !x.IsRemovedFromSender, orderBy: x => x.OrderBy(y => y.CreatedAt));

            var response = _mapper.Map<List<GetInboxQueryResponseItem>>(inboxMessages);
            return Result.Success(response);
        }
    }
}
