using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.User.Commands.RegisterCommand
{
    public sealed record RegisterCommand(string Username, string Email, string Password) : IRequest<Result>;
}
