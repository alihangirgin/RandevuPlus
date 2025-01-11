using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorExperienceCommand
{
    public class DeleteInsturcorExperienceCommandHandler : IRequestHandler<DeleteInstructorExperienceCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteInsturcorExperienceCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result> Handle(DeleteInstructorExperienceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Experiences" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var experience = instructor.Experiences.FirstOrDefault(x => x.Id == command.Id);
            if (experience == null) return Result.Error("ExperienceNotFound");

            if (experience.InstructorId != instructor.Id) return Result.Error("Unauthorized");
            
            instructor.Experiences.Remove(experience);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
