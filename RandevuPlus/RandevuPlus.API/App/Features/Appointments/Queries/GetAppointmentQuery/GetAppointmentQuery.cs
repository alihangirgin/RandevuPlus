using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public sealed record GetAppointmentQuery(Guid Id) : IRequest<Result<GetAppointmentQueryResponse>>;
}
