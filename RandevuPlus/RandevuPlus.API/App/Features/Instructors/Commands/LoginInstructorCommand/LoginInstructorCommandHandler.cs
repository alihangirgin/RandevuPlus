using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;

namespace RandevuPlus.API.App.Features.Instructors.Commands.LoginInstructorCommand
{
    public class LoginInstructorCommandHandler : IRequestHandler<LoginInstructorCommnad, Result<LoginInstructorCommandResponse>>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public LoginInstructorCommandHandler(SignInManager<AppUser> signInManager, IUserService userService, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<Result<LoginInstructorCommandResponse>> Handle(LoginInstructorCommnad command, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(command.Username, command.Password, false, false);

            if (!result.Succeeded)
                return Result.Unauthorized(result.ToString());

            var user = await _userManager.FindByNameAsync(command.Username);
            if (user == null) return Result.Error("UserNotFound");

            var tokenDto = _userService.GenerateJwtToken(user);
            return Result.Success(new LoginInstructorCommandResponse(tokenDto.AccessToken, tokenDto.ExpiresIn));

        }
    }
}
