using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Users.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null) return Result.Error("UserNotFound");

            var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
            if (!result.Succeeded)
                return Result.Error(result.ToString());

            return Result.Success();
        }
    }
}
