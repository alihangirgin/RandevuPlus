using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Commands.SendMessageCommand
{
    public sealed record SendMessageCommand(Guid ReceiverId, string Title, string MessageText) : IRequest<Result>;
}
