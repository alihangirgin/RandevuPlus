using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery
{
    public sealed record GetInstructorsQuery(int PageNumber, int PageSize, string? Prefix, DateTime? Date, int? SlotStartIndex, int? SlotEndIndex, int? SlotSize, bool IsOnline, bool IsSuperInstructor, string? OrderBy) : IRequest<Result<PaginatedResponse<GetInstructorsQueryResponse>>>;
}

