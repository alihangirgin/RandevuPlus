using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand
{
    public sealed record RegisterInstructorCommand(string Username, string Email, string Password) : IRequest<Result>;

}
