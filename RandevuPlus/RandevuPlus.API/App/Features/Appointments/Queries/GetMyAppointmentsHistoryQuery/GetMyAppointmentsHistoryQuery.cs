using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsHistoryQuery
{
    public sealed record GetMyAppointmentsHistoryQuery(int PageNumber, int PageSize, string? Prefix, string? RelatedId, string? Status, string? OrderBy, bool Descending) : IRequest<Result<PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>>>;
}
