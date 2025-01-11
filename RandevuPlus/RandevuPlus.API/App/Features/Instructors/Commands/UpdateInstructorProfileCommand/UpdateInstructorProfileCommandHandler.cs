using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorProfileCommand
{
    public class UpdateInstructorProfileCommandHandler : IRequestHandler<UpdateInstructorProfileCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInstructorProfileCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateInstructorProfileCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            if(command.Title != null)
                instructor.Title = command.Title;
            if(command.Bio != null)
                instructor.Bio = command.Bio;

            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.CommitAsync();
            return Result.Success();    
        }
    }
}
