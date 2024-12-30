using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand
{
    public sealed record CreateCourseCommand(string Name, string Description, decimal BaseFee) : IRequest<Result>;
}
