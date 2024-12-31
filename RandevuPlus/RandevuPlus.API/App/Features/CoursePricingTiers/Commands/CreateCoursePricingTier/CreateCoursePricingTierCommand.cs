using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.CreateCoursePricingTier
{
    public sealed record CreateCoursePricingTierCommand(Guid CourseId, int MinHours, int? MaxHours, decimal DiscountFee) : IRequest<Result>;
}
