using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQueryMessageReactionResponse(string Reaction, int Count, bool IsReactedByMe);
    public sealed record GetMessageQueryMessageResponse(Guid Id, string MessageText, DateTime Date, MessageType MessageType, List<GetMessageQueryMessageReactionResponse> Reactions);
    public sealed record GetMessageQueryUserResponse(Guid Id, string FullName, string? Title, UserStatus Status, string? PhotoUrl);
    public sealed record GetMessageQueryResponse(GetMessageQueryUserResponse Recipient, List<GetMessageQueryMessageResponse> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);
}
