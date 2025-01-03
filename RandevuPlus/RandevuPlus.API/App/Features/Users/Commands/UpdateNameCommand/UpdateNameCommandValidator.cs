using FluentValidation;

namespace RandevuPlus.API.App.Features.Users.Commands.UpdateNameCommand
{
    public class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
    {
        public UpdateNameCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces.");
        }
    }
}
