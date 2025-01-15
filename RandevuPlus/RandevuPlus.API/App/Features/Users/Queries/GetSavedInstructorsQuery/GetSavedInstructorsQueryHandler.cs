using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Queries.GetSavedInstructorsQuery
{
    public class GetSavedInstructorsQueryHandler : IRequestHandler<GetSavedInstructorsQuery, Result<List<GetSavedInstructorsQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public GetSavedInstructorsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<Result<List<GetSavedInstructorsQueryResponse>>> Handle(GetSavedInstructorsQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            TimeSpan timeSinceStartOfDay = DateTime.UtcNow.AddHours(3) - DateTime.UtcNow.AddHours(3).Date;
            int currentSlotIndex = (int)(timeSinceStartOfDay.TotalMinutes / 30);

            var dbQuery = _unitOfWork.Users.GetQueryable();
            var instructors = await dbQuery
                .Include("SavedInstructors.Instructor.User")
                .Include("SavedInstructors.Instructor.Reviews")
                .Include("SavedInstructors.Instructor.Availabilities")
                .Where(x => x.Id == userId)
                .SelectMany(x => x.SavedInstructors)
                .Select(x => new
                {
                    Id = x.InstructorId,
                    PhotoUrl = x.Instructor.User.PhotoUrl,
                    FullName = x.Instructor.User.FullName,
                    Title = x.Instructor.Title,
                    Status = _userService.GetUserStatus(x.Instructor.UserId),
                    InstructorRating = x.Instructor.Reviews.Any() ? (decimal?)x.Instructor.Reviews.Average(y => y.Rating) : null,
                    Availabilities = x.Instructor.Availabilities
                })
                .ToListAsync();


            List<GetSavedInstructorsQueryResponse> response = instructors.Select(x =>
                new GetSavedInstructorsQueryResponse(
                    Id: x.Id,
                    PhotoUrl: x.PhotoUrl,
                    FullName: x.PhotoUrl,
                    Title: x.PhotoUrl,
                    Status: x.Status,
                    InstructorRating: x.InstructorRating,
                    IsAvailableToday: x.Availabilities.Any(y => y.Date.Date == DateTime.UtcNow.Date.AddHours(3).Date && y.SlotString.Substring(currentSlotIndex + 1).Contains('1'))
                    )).ToList();

            return Result.Success(response);
        }
    }
}
