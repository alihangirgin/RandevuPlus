using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Commands.DeleteMessageCommand
{
    public sealed record DeleteMessageCommand(Guid Id) : IRequest<Result>;
}
