using FluentValidation;

namespace RandevuPlus.API.App.Features.Users.Commands.LoginCommand
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("PasswordIsRequired.")
                .MinimumLength(6).WithMessage("PasswordAtLeast6Char");
        }
    }
}
