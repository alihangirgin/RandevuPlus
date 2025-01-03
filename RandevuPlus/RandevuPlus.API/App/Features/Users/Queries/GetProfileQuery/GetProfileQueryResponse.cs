namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public sealed record GetProfileQueryResponse(string FullName, string Email, string? PhotoUrl);
}
