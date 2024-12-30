using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery
{
    public sealed record GetCourseQuery(Guid Id) : IRequest<Result<GetCourseQueryResponse>>;
}
