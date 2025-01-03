using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Commands.LoginCommand
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginCommandResponse>>;
}
