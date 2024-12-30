using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetMyCoursesQuery
{
    public class GetMyCoursesQueryHandler : IRequestHandler<GetMyCoursesQuery, Result<List<GetCourseQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMyCoursesQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<GetCourseQueryResponse>>> Handle(GetMyCoursesQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            var courses = await _unitOfWork.Courses.GetPaginatedAsync(query.PageNumber, query.PageNumber, filter : x=> x.InstructorId == instructor.Id, orderBy: x => x.OrderBy(y => y.CreatedAt));
            var response = _mapper.Map<List<GetCourseQueryResponse>>(courses.Items);
            return Result<List<GetCourseQueryResponse>>.Success(response);
        }
    }
}
