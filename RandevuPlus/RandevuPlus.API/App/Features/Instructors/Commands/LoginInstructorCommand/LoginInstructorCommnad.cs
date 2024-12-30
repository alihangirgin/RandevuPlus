using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.LoginInstructorCommand
{
    public sealed record LoginInstructorCommnad(string Username, string Password) : IRequest<Result<LoginInstructorCommandResponse>>;
}
