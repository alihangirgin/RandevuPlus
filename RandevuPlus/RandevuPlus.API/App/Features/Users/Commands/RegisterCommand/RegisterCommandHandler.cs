using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Commands.RegisterCommand
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<LoginCommandResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<Result<LoginCommandResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            var user = new AppUser { UserName = command.Email, Email = command.Email, FullName = command.Email.Split('@')[0] };
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

            var signInResult = await _signInManager.PasswordSignInAsync(command.Email, command.Password, false, false);

            if (!result.Succeeded)
                return Result.Unauthorized(result.ToString());

            var tokenDto = await _userService.GenerateJwtTokenAsync(user);

            await _unitOfWork.CommitTransactionAsync();

            return Result.Success(new LoginCommandResponse(tokenDto.AccessToken, tokenDto.ExpiresIn));
        }
    }
}
