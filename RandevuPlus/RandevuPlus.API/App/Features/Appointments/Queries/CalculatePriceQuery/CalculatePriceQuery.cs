using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand;

namespace RandevuPlus.API.App.Features.Appointments.Queries.CalculatePriceQuery
{
    public sealed record CalculatePriceQuery(Guid InstructorId, Guid CourseId, List<CreateAppointmentsCommandAppointments> Appointments) : IRequest<Result<CalculatePriceQueryResponse>>;
}
