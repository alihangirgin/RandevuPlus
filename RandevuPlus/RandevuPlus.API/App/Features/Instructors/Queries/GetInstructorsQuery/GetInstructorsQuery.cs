using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery
{
    public sealed record GetInstructorsQuery(int PageNumber, int PageSize, Guid? InstructorId, string? Prefix) : IRequest<Result<List<GetInstructorQueryResponse>>>;
}

