using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery
{
    public sealed record GetMyAppointmentsQuery(DateTime StartDate, DateTime EndDate) : IRequest<Result<List<GetAppointmentsQueryResponse>>>;
}
