namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public sealed record GetProfileQueryResponse(Guid Id, string FullName, string Email, string[] Roles, string? PhotoUrl, int InboxCount)
    {
        public GetProfileQueryResponse() : this(Guid.Empty, string.Empty, string.Empty, Array.Empty<string>(), null, 0) { }
    }
}
