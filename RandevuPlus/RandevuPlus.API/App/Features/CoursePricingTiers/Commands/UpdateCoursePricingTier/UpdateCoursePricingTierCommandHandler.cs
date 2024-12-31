using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.UpdateCoursePricingTier
{
    public class UpdateCoursePricingTierCommandHandler : IRequestHandler<UpdateCoursePricingTierCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCoursePricingTierCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCoursePricingTierCommand command, CancellationToken cancellationToken)
        {
            var coursePricingTier = await _unitOfWork.CoursePricingTiers.GetByIdAsync(command.Id,"Courses");
            if (coursePricingTier == null) return Result.Error("CoursePricingTierNotFound");

            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            if (coursePricingTier.Course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            if (await _unitOfWork.CoursePricingTiers.DuplicateMinHourExistAsync(coursePricingTier.CourseId, command.MinHours))
                return Result.Error("DuplicateMinHour");

            if (await _unitOfWork.CoursePricingTiers.DuplicateMaxHourExistAsync(coursePricingTier.CourseId, command.MaxHours))
                return Result.Error("DuplicateMaxHour");

            coursePricingTier = _mapper.Map(command, coursePricingTier);
            await _unitOfWork.CoursePricingTiers.UpdateAsync(coursePricingTier);
            await _unitOfWork.Commit();
            return Result.Success();
        }
    }
}
