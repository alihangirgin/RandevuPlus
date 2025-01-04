using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;

namespace RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand
{
    public sealed record RegisterInstructorCommand(string Email, string Password) : IRequest<Result<LoginCommandResponse>>;

}
