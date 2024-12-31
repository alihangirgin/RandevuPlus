using FluentValidation;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.CreateCoursePricingTier
{
    public class CreateCoursePricingTierCommandValidator : AbstractValidator<CreateCoursePricingTierCommand>
    {
        public CreateCoursePricingTierCommandValidator()
        {
            RuleFor(x => x.MaxHours)
                .GreaterThan(x => x.MinHours).WithMessage("MaxHoursMustGreaterThanMinHours.")
                .When(x => x.MaxHours.HasValue);
        }
    }
}
