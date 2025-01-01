namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQueryResponse(Guid Id, string Title, string MessageText, bool IsRead, Guid SenderId, string SenderName);
}
