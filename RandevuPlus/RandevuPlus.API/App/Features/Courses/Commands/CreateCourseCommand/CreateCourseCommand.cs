using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand
{
    public sealed record CreateCourseCommand(string Name, decimal BaseFee) : IRequest<Result>;
}
