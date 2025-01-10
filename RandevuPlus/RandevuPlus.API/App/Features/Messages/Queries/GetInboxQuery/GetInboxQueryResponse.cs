using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public sealed record GetInboxQueryResponseItem(Guid Id, string ShortenedMessageText, bool IsRead, Guid SenderId, string SenderName, string LastMessageDate, int UnreadCount, UserStatus SenderStatus, string? SenderPhotoUrl)
    {
        public GetInboxQueryResponseItem() : this(Guid.Empty, string.Empty, false, Guid.Empty, string.Empty, string.Empty, 0 , UserStatus.NotSet, string.Empty) { }
    }
}
