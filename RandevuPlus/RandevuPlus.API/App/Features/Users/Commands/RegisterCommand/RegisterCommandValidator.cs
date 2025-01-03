namespace RandevuPlus.API.App.Features.Users.Commands.RegisterCommand
{
    using FluentValidation;

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmailIsRequired.")
                .EmailAddress().WithMessage("InvalidEmailFormat.");

            RuleFor(x => x.Password)
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
