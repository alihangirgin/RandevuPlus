using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorExperienceCommand
{
    public sealed record UpdateInstructorExperienceCommand(Guid Id, string Description) : IRequest<Result>
    {
        public UpdateInstructorExperienceCommand SetId(Guid id)
        {
            return this with { Id = id };
        }
    }
}
