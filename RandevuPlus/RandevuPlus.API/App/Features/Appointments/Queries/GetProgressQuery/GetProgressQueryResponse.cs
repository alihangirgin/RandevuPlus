namespace RandevuPlus.API.App.Features.Appointments.Queries.GetProgressQuery
{
    public sealed record GetProgressQueryAchievementItemResponse(string Name, bool IsAchieved, string BadgeUrl);
    public sealed record GetProgressQueryAchievementResponse(List<GetProgressQueryAchievementItemResponse> CompleteAppointment,
        List<GetProgressQueryAchievementItemResponse> RevievInstructor,
        List<GetProgressQueryAchievementItemResponse> InviteFriend);

    public sealed record GetProgressQueryInstructorResponse(Guid Id, string? PhotoUrl);
    public sealed record GetProgressQueryResponse(string Title, 
        int WeeklyPlannedAppointmentHour, int WeeklyCompletedAppointmentHour, int TotalCompletedAppointmentHour,
        int MyInstructorCount, GetProgressQueryInstructorResponse? MostAppointedInstructor, GetProgressQueryInstructorResponse? FavouriteInstructor,
        double? AverageReviewPoint,
        GetProgressQueryAchievementResponse Achievement
        );
}
