using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Courses.Commands.DeleteCourseCommand
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCourseCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(command.Id);
            if (course == null) return Result.Error("CourseNotFound");

            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            if (course.InstructorId != instructor.Id) return Result.Error("Unauthorized");

            await _unitOfWork.Courses.DeleteAsync(command.Id);
            await _unitOfWork.CommitAsync(); 
            return Result.Success();
        }
    }
}
