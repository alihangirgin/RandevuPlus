using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;

namespace RandevuPlus.API.App.Features.Users.Commands.RegisterCommand
{
    public sealed record RegisterCommand(string Email, string Password) : IRequest<Result<LoginCommandResponse>>;
}
