namespace RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery
{
    public sealed record GetMyAvailabilitiesQueryResponse(List<GetMyAvailabilityQueryResponse> Availabilities);
    public sealed record GetMyAvailabilityQueryResponse(Guid Id, DateTime Date, string SlotString);
}
