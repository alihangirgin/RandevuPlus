namespace RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier
{
    public sealed record GetCoursePricingTierResponse(Guid Id, int? MinHours, int? MaxHours, decimal DiscountFee, Guid CourseId);
}
