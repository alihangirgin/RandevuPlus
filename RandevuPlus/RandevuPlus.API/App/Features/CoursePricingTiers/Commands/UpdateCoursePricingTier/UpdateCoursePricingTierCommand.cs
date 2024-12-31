using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.UpdateCoursePricingTier
{
    public sealed record UpdateCoursePricingTierCommand(Guid Id, int MinHours, int? MaxHours, decimal DiscountFee) : IRequest<Result>
    {
        public UpdateCoursePricingTierCommand SetId(Guid id)
        {
            return this with { Id = id };
        }
    }
}
