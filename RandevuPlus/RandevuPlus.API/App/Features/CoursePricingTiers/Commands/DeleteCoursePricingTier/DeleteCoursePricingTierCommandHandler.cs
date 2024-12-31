using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.DeleteCoursePricingTier
{
    public class DeleteCoursePricingTierCommandHandler : IRequestHandler<DeleteCoursePricingTierCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCoursePricingTierCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteCoursePricingTierCommand command, CancellationToken cancellationToken)
        {
            var coursePricingTier = await _unitOfWork.CoursePricingTiers.GetByIdAsync(command.Id, "Courses");
            if (coursePricingTier == null) return Result.Error("CoursePricingTierNotFound");

            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            if (coursePricingTier.Course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            await _unitOfWork.Courses.DeleteAsync(command.Id);
            await _unitOfWork.Commit();
            return Result.Success();
        }
    }
}
