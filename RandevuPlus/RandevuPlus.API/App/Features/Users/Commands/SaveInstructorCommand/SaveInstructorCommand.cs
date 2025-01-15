using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Commands.SaveInstructorCommand
{
    public sealed record SaveInstructorCommand(Guid InstructorId) : IRequest<Result>;
}
