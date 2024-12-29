using FluentValidation;

namespace RandevuPlus.API.App.Features.User.Commands.LoginCommand
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
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
