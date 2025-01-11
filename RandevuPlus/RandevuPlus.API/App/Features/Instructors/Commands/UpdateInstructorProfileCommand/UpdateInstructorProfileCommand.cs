using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorProfileCommand
{
    public sealed record UpdateInstructorProfileCommand(string? Title, string? Bio) : IRequest<Result>;
}
