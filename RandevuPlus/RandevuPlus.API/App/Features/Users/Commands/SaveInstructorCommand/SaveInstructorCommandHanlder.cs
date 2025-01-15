using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Commands.SaveInstructorCommand
{
    public class SaveInstructorCommandHanlder : IRequestHandler<SaveInstructorCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SaveInstructorCommandHanlder(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(SaveInstructorCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var query = _unitOfWork.Users.GetQueryable();
            var user = await query
                .Include(x => x.SavedInstructors)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return Result.Error("UserNotFound");

            var instructorIsExists = await _unitOfWork.Instructors.CheckAsync(command.InstructorId);
            if (!instructorIsExists) return Result.Error("InstructorNotFound");

            var savedInstructor = user.SavedInstructors.FirstOrDefault(x => x.InstructorId == command.InstructorId);
            if (savedInstructor != null)
                user.SavedInstructors.Remove(savedInstructor);
            else
            {
                if (user.SavedInstructors.Count == 10) return Result.Error("MaxSavedInstructorCountReached");

                user.SavedInstructors.Add(new InstructorSave() { InstructorId = command.InstructorId, UserId = userId, CreatedAt = DateTime.UtcNow.AddHours(3), CreatedBy = "test" });
                //TODO:remove created items
            }
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
