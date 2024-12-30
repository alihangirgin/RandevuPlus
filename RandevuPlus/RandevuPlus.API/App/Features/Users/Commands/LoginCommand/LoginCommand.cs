using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Commands.LoginCommand
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<Result<LoginCommandResponse>>;
}
