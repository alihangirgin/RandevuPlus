using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public sealed record GetAppointmentQuery(Guid Id) : IRequest<Result<GetAppointmentQueryResponse>>;
}
