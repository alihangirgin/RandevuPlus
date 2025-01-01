using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery
{
    public class GetInstructorQueryHandler : IRequestHandler<GetInstructorQuery, Result<GetInstructorQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetInstructorQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetInstructorQueryResponse>> Handle(GetInstructorQuery query, CancellationToken cancellationToken)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(query.Id);
            if (instructor == null) return Result<GetInstructorQueryResponse>.Error("InstructorNotFound");

            return Result<GetInstructorQueryResponse>.Success(_mapper.Map<GetInstructorQueryResponse>(instructor));
        }
    }
}
