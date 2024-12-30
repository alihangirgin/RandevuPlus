using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;

namespace RandevuPlus.API.App.Features.Users.Commands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public LoginCommandHandler(SignInManager<AppUser> signInManager, IUserService userService, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(command.Username, command.Password, false, false);

            if (!result.Succeeded)
                return Result.Unauthorized(result.ToString());

            AppUser? user = await _userManager.FindByNameAsync(command.Username);
            if (user == null) return Result.Error("UserNotFound");

            var tokenDto = _userService.GenerateJwtToken(user);
            return Result.Success(new LoginCommandResponse(tokenDto.AccessToken, tokenDto.ExpiresIn));
        }
    }
}
