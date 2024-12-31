using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier
{
    public class GetCoursePricingTierQueryHandler : IRequestHandler<GetCoursePricingTierQuery, Result<GetCoursePricingTierResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCoursePricingTierQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetCoursePricingTierResponse>> Handle(GetCoursePricingTierQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            var coursePricingTier = await _unitOfWork.CoursePricingTiers.GetByIdAsync(query.Id, "Courses");
            if (coursePricingTier == null) return Result.Error("CoursePricingTierNotFound");

            if (coursePricingTier.Course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            return Result<GetCoursePricingTierResponse>.Success(_mapper.Map<GetCoursePricingTierResponse>(coursePricingTier));
        }
    }
}
