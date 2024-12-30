using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery
{

    public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, Result<GetCourseQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetCourseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetCourseQueryResponse>> Handle(GetCourseQuery query, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(query.Id);
            if (course == null) return Result.Error("CourseNotFound");

            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");
            if (course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            return Result<GetCourseQueryResponse>.Success(_mapper.Map<GetCourseQueryResponse>(course));
        }
    }
}
