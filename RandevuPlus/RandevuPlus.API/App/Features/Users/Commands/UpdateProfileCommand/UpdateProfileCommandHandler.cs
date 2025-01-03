using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;


namespace RandevuPlus.API.App.Features.Users.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProfileCommandHandler(IHostEnvironment env, ICurrentUserService currentUserService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _env = env;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
        {
            if (command.ProfilePicture != null && command.ProfilePicture.Length == 0) return Result.Error("ProfilePictureNotSelected");

            var userId = _currentUserService.UserId.Value;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Result.Error("UserNotFound");

            await _unitOfWork.BeginTransactionAsync();

            if (command.ProfilePicture != null)
            {
                var uploadsPath = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(command.ProfilePicture?.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.ProfilePicture?.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.PhotoUrl))
                {
                    var oldFilePath = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", Path.GetFileName(user.PhotoUrl));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                user.PhotoUrl = $"{command.UploadUrl?.TrimEnd('/')}/{fileName}";
                await _userManager.UpdateAsync(user);
            }

            if (!string.IsNullOrEmpty(command.FullName))
            {
                user.FullName = command.FullName;
                await _userManager.UpdateAsync(user);
            }

            if (!string.IsNullOrEmpty(command.OldPassword) && !string.IsNullOrEmpty(command.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
                if (!result.Succeeded)
                    return Result.Error(result.ToString());
            }

            await _unitOfWork.CommitTransactionAsync();

            return Result.Success();
        }
    }
}
