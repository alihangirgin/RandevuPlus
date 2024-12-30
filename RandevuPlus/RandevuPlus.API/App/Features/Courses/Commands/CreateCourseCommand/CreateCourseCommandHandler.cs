using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            Course course = _mapper.Map<Course>(command);
            course.InstructorId = instructor.Id;
            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.Commit();
            return Result.Success();
        }
    }
}
