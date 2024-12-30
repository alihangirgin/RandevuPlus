using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand
{
    public class RegisterInstructorCommandHandler : IRequestHandler<RegisterInstructorCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterInstructorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RegisterInstructorCommand command, CancellationToken cancellationToken)
        {
            var user = new AppUser()
            {
                Id = Guid.NewGuid(),
                UserName = command.Username,
                Email = command.Email,
                NormalizedUserName = command.Username.ToUpper(),
                NormalizedEmail = command.Email.ToUpper(),
                EmailConfirmed = false,
            };
            user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, command.Password);

            var addedUser = await _unitOfWork.Users.AddAsync(user);
            var instructor = new Instructor() { UserId = addedUser.Id };
            await _unitOfWork.Instructors.AddAsync(instructor);
            await _unitOfWork.Commit();

            return Result.Success();
        }
    }
}
