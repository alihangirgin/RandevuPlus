using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Commands.UpdateNameCommand
{
    public sealed record UpdateNameCommand(string Name) : IRequest<Result>;
}
