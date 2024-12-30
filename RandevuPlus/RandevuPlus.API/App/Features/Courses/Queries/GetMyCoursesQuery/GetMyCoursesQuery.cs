using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetMyCoursesQuery
{
    public sealed record GetMyCoursesQuery(int PageNumber, int PageSize) : IRequest<Result<List<GetCourseQueryResponse>>>;

}
