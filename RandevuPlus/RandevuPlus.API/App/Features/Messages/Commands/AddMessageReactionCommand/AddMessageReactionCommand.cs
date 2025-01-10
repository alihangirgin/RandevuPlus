using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Commands.AddMessageReactionCommand
{
    public sealed record AddMessageReactionCommand(Guid MessageId, string Reaction) : IRequest<Result>
    {
        public AddMessageReactionCommand SetMessageId(Guid messageId)
        {
            return this with { MessageId = messageId };
        }
    }
}
