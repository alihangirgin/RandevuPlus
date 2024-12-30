using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand
{
    public sealed record UpdateCourseCommand(Guid Id, string Name, string Description, decimal BaseFee) : IRequest<Result>
    {
        public UpdateCourseCommand SetId(Guid id)
        {
            return this with { Id = id };
        }
    }
}
