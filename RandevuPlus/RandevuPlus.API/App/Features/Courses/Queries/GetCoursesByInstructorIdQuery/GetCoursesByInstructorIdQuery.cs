using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetCoursesByInstructorIdQuery
{
    public sealed record GetCoursesByInstructorIdQuery(Guid InstructorId) : IRequest<Result<List<GetCourseQueryResponse>>>;

}
