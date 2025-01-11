using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorExperienceCommand
{
    public class UpdateInstructorExperienceCommandHandler : IRequestHandler<UpdateInstructorExperienceCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateInstructorExperienceCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateInstructorExperienceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Experiences" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var experience = instructor.Experiences.FirstOrDefault(x => x.Id == command.Id);
            if (experience == null) return Result.Error("ExperienceNotFound");

            if (experience.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            experience = _mapper.Map(command, experience);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
