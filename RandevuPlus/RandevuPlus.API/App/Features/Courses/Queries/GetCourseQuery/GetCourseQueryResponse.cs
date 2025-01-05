using MediatR;

namespace RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery
{
    public sealed record GetCourseQueryResponse(Guid Id, string Name, decimal BaseFee);

}
