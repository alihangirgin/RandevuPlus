using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public sealed record GetAppointmentQueryResponse(Guid InstructorId, string InstructorName, Guid CourseId, string CourseName, DateTime Date, int SlotStartIndex, int SlotEndIndex, AppointmentStatus Status)
    { 
        public GetAppointmentQueryResponse() : this(Guid.Empty, string.Empty, Guid.Empty, string.Empty, DateTime.MinValue, 0, 0, AppointmentStatus.Draft) 
        { 
        } 
    }


}
