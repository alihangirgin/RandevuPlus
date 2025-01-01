using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public sealed record GetAppointmentQueryResponse(Guid InstructorId, string InstructorName, Guid CourseId, Guid CourseName, DateTime Date, int SlotStartIndex, int SlotEndIndex, AppointmentStatus Status);

}
