using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorExperienceCommand
{
    public class CreateInstructorExperienceCommandHandler : IRequestHandler<CreateInstructorExperienceCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public CreateInstructorExperienceCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateInstructorExperienceCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string>() { "Experiences" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            InstructorExperience experience = _mapper.Map<InstructorExperience>(command);
            experience.InstructorId = instructor.Id;
            experience.CreatedAt = DateTime.Now;
            experience.CreatedBy = "test";//TODO: remove
            instructor.Experiences.Add(experience);
            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
