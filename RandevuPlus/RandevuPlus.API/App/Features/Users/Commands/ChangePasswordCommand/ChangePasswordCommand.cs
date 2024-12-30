using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Commands.ChangePasswordCommand
{
    public sealed record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : IRequest<Result>;
}
