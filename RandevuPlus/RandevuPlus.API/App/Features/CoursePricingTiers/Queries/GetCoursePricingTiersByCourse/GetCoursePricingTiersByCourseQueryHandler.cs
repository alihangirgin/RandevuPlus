using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTiersByCourse
{
    public class GetCoursePricingTiersByCourseQueryHandler : IRequestHandler<GetCoursePricingTiersByCourseQuery, Result<List<GetCoursePricingTierResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetCoursePricingTiersByCourseQueryHandler(ICurrentUserService currentUserService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetCoursePricingTierResponse>>> Handle(GetCoursePricingTiersByCourseQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            var course = await _unitOfWork.Courses.GetByIdAsync(query.CourseId);
            if (course == null) return Result.Error("CourseNotFound");
            if (course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            var coursePricingTiers = await _unitOfWork.CoursePricingTiers.GetByCourseId(query.CourseId);
            return Result<List<GetCoursePricingTierResponse>>.Success(_mapper.Map<List<GetCoursePricingTierResponse>>(coursePricingTiers));
        }
    }
}
