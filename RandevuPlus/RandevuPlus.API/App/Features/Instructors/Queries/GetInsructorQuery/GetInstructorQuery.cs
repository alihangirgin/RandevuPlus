using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery
{
    public sealed record GetInstructorQuery(Guid Id) : IRequest<Result<GetInstructorQueryResponse>>;
}
