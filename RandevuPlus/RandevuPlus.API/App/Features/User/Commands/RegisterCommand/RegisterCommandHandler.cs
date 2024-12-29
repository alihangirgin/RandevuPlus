using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.User.Commands.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var user = new AppUser { UserName = command.Username, Email = command.Email };
            var result = await _userManager.CreateAsync(user, command.Password);

            if(!result.Succeeded)
                return Result.Error(result.Errors.FirstOrDefault()?.Description);

            return Result.Success();
        }
    }
}
