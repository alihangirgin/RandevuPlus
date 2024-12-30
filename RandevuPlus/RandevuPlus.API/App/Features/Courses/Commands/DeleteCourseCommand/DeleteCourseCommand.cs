using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Commands.DeleteCourseCommand
{
    public sealed record DeleteCourseCommand(Guid Id) : IRequest<Result>;
}
