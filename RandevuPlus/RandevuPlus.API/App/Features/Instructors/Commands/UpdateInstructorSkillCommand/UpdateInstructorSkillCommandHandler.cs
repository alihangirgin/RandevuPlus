using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorSkillCommand
{
    public class UpdateInstructorSkillCommandHandler : IRequestHandler<UpdateInstructorSkillCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateInstructorSkillCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateInstructorSkillCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Skills" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var skill = instructor.Skills.FirstOrDefault(x => x.Id == command.Id);
            if (skill == null) return Result.Error("ExperienceNotFound");

            if (skill.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            skill = _mapper.Map(command, skill);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
