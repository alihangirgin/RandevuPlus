using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorExperienceCommand
{
    public sealed record CreateInstructorExperienceCommand(ExperienceType ExperienceType, string Description) : IRequest<Result>;
}
