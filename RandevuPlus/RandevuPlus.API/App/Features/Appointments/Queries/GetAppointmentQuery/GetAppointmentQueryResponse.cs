using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public sealed record GetAppointmentQueryResponse(Guid Id, string CourseName, string UserPhotoUrl, string UserName, string UserTitle, string InstructorPhotoUrl, string InstructorName, string InstructorTitle, string Date, int StartHour, int EndHour, AppointmentStatus Status)
    {
        public GetAppointmentQueryResponse() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, AppointmentStatus.Draft)
        {
        }
    }
}
