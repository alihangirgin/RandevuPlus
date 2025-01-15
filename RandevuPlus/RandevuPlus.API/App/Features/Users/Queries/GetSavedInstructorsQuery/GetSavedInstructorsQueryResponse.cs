using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Users.Queries.GetSavedInstructorsQuery
{
    public sealed record GetSavedInstructorsQueryResponse(Guid Id, string? PhotoUrl, string? FullName, string? Title, UserStatus Status, decimal? InstructorRating, bool IsAvailableToday)
    {
        public GetSavedInstructorsQueryResponse() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, UserStatus.NotSet, null, false)
        {
        }
    }
}
