using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;

namespace RandevuPlus.API.App.Features.Users.Commands.UpdateNameCommand
{
    public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        public UpdateNameCommandHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }
        public async Task<Result> Handle(UpdateNameCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return Result.Error("UserNotFound");
            user.FullName = command.Name;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }
    }
}
