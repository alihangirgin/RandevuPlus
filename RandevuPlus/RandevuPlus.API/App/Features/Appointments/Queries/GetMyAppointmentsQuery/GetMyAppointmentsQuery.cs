using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery
{
    public sealed record GetMyAppointmentsQuery(DateTime StartDate, DateTime EndDate) : IRequest<Result<List<GetAppointmentQueryResponse>>>;
}
