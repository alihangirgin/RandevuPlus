using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQueryHandler : IRequestHandler<GetMessageQuery, Result<GetMessageQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetMessageQueryResponse>> Handle(GetMessageQuery query, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(query.Id);
            if (message == null) return Result.Error("MessageNotFound");

            var userId = _currentUserService.UserId.Value;
            if (message.ReceiverId != userId || message.SenderId != userId) return Result.Error("Unauthorized");

            if (message.ReceiverId != userId)
            {
                message.IsRead = true;
                await _unitOfWork.Messages.UpdateAsync(message);
                await _unitOfWork.CommitAsync();
            }

            var response = _mapper.Map<GetMessageQueryResponse>(message);
            return Result.Success(response);
        }
    }
}
