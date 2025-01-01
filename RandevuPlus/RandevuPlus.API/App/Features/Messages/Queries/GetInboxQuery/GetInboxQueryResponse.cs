namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public sealed record GetInboxQueryResponseItem(Guid Id, string Title, string ShortenedMessageText, bool IsRead, Guid SenderId, string SenderName);
}
