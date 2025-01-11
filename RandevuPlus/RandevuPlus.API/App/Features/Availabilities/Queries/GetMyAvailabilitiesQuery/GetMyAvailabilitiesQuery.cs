using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;

namespace RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery
{
    public sealed record GetMyAvailabilitiesQuery() : IRequest<Result<List<GetInstructorQueryAvailabilityResponse>>>;
}
