using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsHistoryQuery
{
    public sealed record GetMyAppointmentsHistoryQueryInstructorResponse(Guid Id, string InstructorPhotoUrl, string InstructorName, string InstructorTitle);
    public sealed record GetMyAppointmentsHistoryQueryUserResponse(string UserPhotoUrl, string UserName, string UserTitle);
    public sealed record GetMyAppointmentsHistoryQueryResponse(Guid Id, string CourseName, GetMyAppointmentsHistoryQueryUserResponse User, GetMyAppointmentsHistoryQueryInstructorResponse Instructor, string Date, int StartHour, int EndHour, AppointmentStatus Status)
    {
        public GetMyAppointmentsHistoryQueryResponse()
            : this(Guid.Empty, string.Empty,
                   new GetMyAppointmentsHistoryQueryUserResponse(string.Empty, string.Empty, string.Empty),
                   new GetMyAppointmentsHistoryQueryInstructorResponse(Guid.Empty, string.Empty, string.Empty, string.Empty),
                   string.Empty,
                   0,
                   0,
                   AppointmentStatus.Draft)
        {
        }
    }
}
