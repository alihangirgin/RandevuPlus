using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery
{
    public sealed record GetMyAvailabilitiesQuery(DateTime StartDate, DateTime EndDate) : IRequest<Result<List<GetMyAvailabilityQueryResponse>>>;
}
