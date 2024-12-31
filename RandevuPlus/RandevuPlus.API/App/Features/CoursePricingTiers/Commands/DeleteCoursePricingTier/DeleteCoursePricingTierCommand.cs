using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.DeleteCoursePricingTier
{
    public sealed record DeleteCoursePricingTierCommand(Guid Id) : IRequest<Result>;
}
