using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorSkillCommand
{
    public class DeleteInstructorSkillCommandHandler : IRequestHandler<DeleteInstructorSkillCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteInstructorSkillCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result> Handle(DeleteInstructorSkillCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Skills" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var skill = instructor.Skills.FirstOrDefault(x => x.Id == command.Id);
            if (skill == null) return Result.Error("ExperienceNotFound");

            if (skill.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            instructor.Skills.Remove(skill);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
