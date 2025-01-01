using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(UpdateCourseCommand command, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(command.Id);
            if (course == null) return Result.Error("CourseNotFound");

            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            if (course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            course = _mapper.Map(command, course);
            await _unitOfWork.Courses.UpdateAsync(course);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
