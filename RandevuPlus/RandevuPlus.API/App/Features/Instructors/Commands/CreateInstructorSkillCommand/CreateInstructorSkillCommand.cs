using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorSkillCommand
{
    public sealed record CreateInstructorSkillCommand(string SkillName) : IRequest<Result>;
}
