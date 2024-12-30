using FluentValidation;

namespace RandevuPlus.API.App.Features.Users.Commands.ChangePasswordCommand
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("PasswordAtLeast6Char")
                .Matches(@"[A-Z]").WithMessage("PasswordAtLeastOneUppercaseChar")
                .Matches(@"[a-z]").WithMessage("PasswordAtLeastOneLowercaseChar")
                .Matches(@"[0-9]").WithMessage("PasswordAtLeastOneNumberChar")
                .Matches(@"[\W]").WithMessage("PasswordAtLeastOneSpecialChar"); // Özel karakter
            //TODO:check password rules config
        }
    }
}
