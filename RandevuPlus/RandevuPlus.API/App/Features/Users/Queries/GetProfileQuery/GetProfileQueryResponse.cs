namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public sealed record GetProfileQueryResponse(string FullName, string Email, string[] Roles, string? PhotoUrl)
    {
        // AutoMapper'ın doğru çalışması için varsayılan bir constructor ekleyebilirsiniz.
        public GetProfileQueryResponse() : this(string.Empty, string.Empty, Array.Empty<string>(), null) { }
    }
}
