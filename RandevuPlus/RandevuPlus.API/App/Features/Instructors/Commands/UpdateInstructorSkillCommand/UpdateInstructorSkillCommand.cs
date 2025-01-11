using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorSkillCommand
{
    public sealed record UpdateInstructorSkillCommand(Guid Id, string SkillName) : IRequest<Result>
    {
        public UpdateInstructorSkillCommand SetId(Guid id)
        {
            return this with { Id = id };
        }
    }
}
