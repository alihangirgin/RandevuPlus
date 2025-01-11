using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorExperienceCommand
{
    public sealed record DeleteInstructorExperienceCommand(Guid Id) : IRequest<Result>;
}
