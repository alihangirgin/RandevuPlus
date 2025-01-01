using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Commands.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            var user = new AppUser { UserName = command.Username, Email = command.Email };
            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
                return Result.Error(result.Errors.FirstOrDefault()?.Description);

            var userRole = "User";
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>(userRole));
                if (!roleResult.Succeeded)
                    return Result.Error(roleResult.Errors.FirstOrDefault()?.Description);
            }
            await _userManager.AddToRoleAsync(user, userRole);

            await _unitOfWork.CommitTransactionAsync();

            return Result.Success();
        }
    }
}
