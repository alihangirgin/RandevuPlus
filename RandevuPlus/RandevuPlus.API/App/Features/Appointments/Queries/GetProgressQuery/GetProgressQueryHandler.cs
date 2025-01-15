using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetProgressQuery
{
    public class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, Result<GetProgressQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetProgressQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetProgressQueryResponse>> Handle(GetProgressQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var today = DateTime.UtcNow.AddHours(3);
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(7).AddMilliseconds(-1);

            var appointmentQuery = _unitOfWork.Appointments.GetQueryable();
            var weeklyPlannedAppointmentHour = await appointmentQuery.CountAsync(x => startOfWeek >= x.Date && x.Date <= endOfWeek && x.Status == AppointmentStatus.Scheduled);
            var weeklyCompletedAppointmentHour = await appointmentQuery.CountAsync(x => startOfWeek >= x.Date && x.Date <= endOfWeek && x.Status == AppointmentStatus.Completed);
            var totalCompletedAppointmentHour = await appointmentQuery.CountAsync(x => x.Status == AppointmentStatus.Completed);
            var myInstructorCount = await appointmentQuery
                .Where(x => x.Status != AppointmentStatus.Draft)
                .Select(x => x.InstructorId)
                .Distinct()
                .CountAsync();

            var mostAppointedInstructor = await appointmentQuery
                .Include(x => x.Instructor.User)
                .Where(x => x.Status != AppointmentStatus.Draft)
                .GroupBy(x => x.InstructorId)
                .OrderByDescending(x => x.Count())
                    .Select(y => new
                    {
                        UserId = y.FirstOrDefault().Instructor.User.Id,
                        UserPhotoPath = y.FirstOrDefault().Instructor.User.PhotoUrl
                    })
                .FirstOrDefaultAsync();

            var favouriteInstructor = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Reviews)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Reviews)
                .GroupBy(x => x.InstructorId)
                .OrderByDescending(x => x.Average(y => y.Rating))
                    .Select(y => new
                    {
                        UserId = y.FirstOrDefault().Instructor.User.Id,
                        UserPhotoPath = y.FirstOrDefault().Instructor.User.PhotoUrl
                    })
                .FirstOrDefaultAsync();


            var averageReviewPoint = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Reviews)
                .Where(x => x.Id == userId)
                .Select(x => x.Reviews.Any() ? x.Reviews.Average(y => y.Rating) : 0)  
                .FirstOrDefaultAsync();

            var totalReviewCount = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Reviews)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Reviews)
                .CountAsync();

            var invitationCount = 0;

            List<int> completeAppointmentAchievementList = new() { 1, 5, 10, 20, 50, 100 };
            List<int> inviteFriendAchievementList = new() { 1, 5, 10, 15 };
            List<int> reviewInstructorAchievementList = new() { 1, 3, 5, 8 };

            List<GetProgressQueryAchievementItemResponse> completeAppointment = completeAppointmentAchievementList
                .Select(item => new GetProgressQueryAchievementItemResponse(
                     Name: $"{item} saat Eğitim tamamla",
                     IsAchieved: totalCompletedAppointmentHour >= item,
                     BadgeUrl : string.Empty
                )).ToList();

            List<GetProgressQueryAchievementItemResponse> revievInstructor = inviteFriendAchievementList
                .Select(item => new GetProgressQueryAchievementItemResponse(
                     Name: $"{item} kişiyi davet et",
                     IsAchieved: invitationCount >= item,
                     BadgeUrl: string.Empty
                )).ToList();

            List<GetProgressQueryAchievementItemResponse> inviteFriend = reviewInstructorAchievementList
                .Select(item => new GetProgressQueryAchievementItemResponse(
                     Name: $"{item} Eğitmeni değerlendir",
                     IsAchieved: totalReviewCount >= item,
                     BadgeUrl: string.Empty
                )).ToList();

            var levelCount = completeAppointment.Count(x => x.IsAchieved) + revievInstructor.Count(x => x.IsAchieved) + inviteFriend.Count(x => x.IsAchieved) + 1;

            var response = new GetProgressQueryResponse(
                Title: $"{levelCount}. Seviye Öğrenci",
                WeeklyPlannedAppointmentHour: weeklyPlannedAppointmentHour,
                WeeklyCompletedAppointmentHour: weeklyCompletedAppointmentHour,
                TotalCompletedAppointmentHour: totalCompletedAppointmentHour,
                MyInstructorCount: myInstructorCount,
                MostAppointedInstructor: mostAppointedInstructor?.UserId != null ? new GetProgressQueryInstructorResponse(mostAppointedInstructor.UserId, mostAppointedInstructor.UserPhotoPath) : null,
                FavouriteInstructor: favouriteInstructor?.UserId != null ? new GetProgressQueryInstructorResponse(favouriteInstructor.UserId, favouriteInstructor.UserPhotoPath) : null,
                AverageReviewPoint: averageReviewPoint == 0 ? null : averageReviewPoint,
                Achievement: new GetProgressQueryAchievementResponse(completeAppointment, revievInstructor, inviteFriend)
                );

            return Result.Success(response);
        }
    }
}
