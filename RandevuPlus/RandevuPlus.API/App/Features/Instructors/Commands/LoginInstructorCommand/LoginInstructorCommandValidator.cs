using FluentValidation;

namespace RandevuPlus.API.App.Features.Instructors.Commands.LoginInstructorCommand
{
    public class LoginInstructorCommandValidator : AbstractValidator<LoginInstructorCommnad>
    {
        public LoginInstructorCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("UsernameIsRequired.")
                .MinimumLength(3).WithMessage("UserNameAtLeast6Char");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("PasswordIsRequired.")
                .MinimumLength(6).WithMessage("PasswordAtLeast6Char");
        }
    }
}
