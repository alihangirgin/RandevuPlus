using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetCoursesByInstructorIdQuery
{
    public class GetCoursesByInstructorIdQueryHandler : IRequestHandler<GetCoursesByInstructorIdQuery, Result<List<GetCourseQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCoursesByInstructorIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetCourseQueryResponse>>> Handle(GetCoursesByInstructorIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Courses.GetCoursesByInstructorId(query.InstructorId);
            var response = _mapper.Map<List<GetCourseQueryResponse>>(result);
            return Result<List<GetCourseQueryResponse>>.Success(response);
        }
    }
}
