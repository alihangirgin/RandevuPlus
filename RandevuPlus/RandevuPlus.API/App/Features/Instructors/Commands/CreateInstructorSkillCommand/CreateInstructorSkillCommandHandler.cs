using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorSkillCommand
{
    public class CreateInstructorSkillCommandHandler : IRequestHandler<CreateInstructorSkillCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public CreateInstructorSkillCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateInstructorSkillCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Skills" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            InstructorSkill skill = _mapper.Map<InstructorSkill>(command);
            skill.InstructorId = instructor.Id;
            skill.CreatedAt = DateTime.Now;
            skill.CreatedBy = "test";//TODO: remove
            instructor.Skills.Add(skill);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
