using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier
{
    public sealed record GetCoursePricingTierQuery(Guid Id) : IRequest<Result<GetCoursePricingTierResponse>>;
}
