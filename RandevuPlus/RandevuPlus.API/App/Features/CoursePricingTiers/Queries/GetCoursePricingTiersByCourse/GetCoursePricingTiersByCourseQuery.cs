using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTiersByCourse
{
    public sealed record GetCoursePricingTiersByCourseQuery(Guid CourseId) : IRequest<Result<List<GetCoursePricingTierResponse>>>;
}
