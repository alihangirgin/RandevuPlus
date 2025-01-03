using FluentValidation;

namespace RandevuPlus.API.App.Features.Users.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name cannot be empty.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password cannot be empty.")
                .MinimumLength(6).WithMessage("Old password must be at least 6 characters long.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password cannot be empty.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .NotEqual(x => x.OldPassword).WithMessage("New password cannot be the same as the old password.");

            RuleFor(x => x.ProfilePicture)
                .Must(IsValidProfilePicture).WithMessage("Invalid profile picture format.");
        }

        private bool IsValidProfilePicture(IFormFile? file)
        {
            if (file == null)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}
