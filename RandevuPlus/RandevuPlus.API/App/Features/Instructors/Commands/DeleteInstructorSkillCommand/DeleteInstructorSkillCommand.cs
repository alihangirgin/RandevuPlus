using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorSkillCommand
{
    public sealed record DeleteInstructorSkillCommand(Guid Id) : IRequest<Result>;
}
