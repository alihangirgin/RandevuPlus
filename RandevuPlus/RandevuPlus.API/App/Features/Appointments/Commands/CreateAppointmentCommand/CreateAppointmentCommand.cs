using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand
{
    public sealed record CreateAppointmentsCommandAppointments(DateTime Date, int SlotStartIndex, int SlotEndIndex);
    public sealed record CreateAppointmentCommand(Guid InstructorId, Guid CourseId, List<CreateAppointmentsCommandAppointments> Appointments) : IRequest<Result<CreateAppointmentCommandResponse>>;
}
