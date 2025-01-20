using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Text.RegularExpressions;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsFromAiQuery
{
    public class GetInstructorsFromAiQueryHandler : IRequestHandler<GetInstructorsFromAiQuery, Result<GetInstructorsFromAiQueryResponse>>
    {
        private readonly IAiService _aiService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetInstructorsFromAiQueryHandler(IAiService aiService, IUnitOfWork unitOfWork, IUserService userService, ICurrentUserService currentUserService)
        {
            _aiService = aiService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetInstructorsFromAiQueryResponse>> Handle(GetInstructorsFromAiQuery query, CancellationToken cancellationToken)
        {
            var response = await _aiService.AskQuestion(query.Question);

            var matches = Regex.Matches(response, @"\*\*(.*?)\s\((EğitmenId:([^\)]+))\):\*\*");
            List<string> instructorIds = new List<string>();
            string modifiedText = response;

            foreach (Match match in matches)
            {
                string fullMatch = match.Groups[0].Value;
                string instructorName = match.Groups[1].Value.Trim();
                string instructorId = match.Groups[3].Value.Trim();

                instructorIds.Add(instructorId);
                modifiedText = modifiedText.Replace(fullMatch, $"**{instructorName}**");
            }

            var responseQuery = _unitOfWork.Instructors.GetQueryable()
                .Include(i => i.User)
                .Include(i => i.Reviews)
                .Include(i => i.Availabilities)
                .Include(i => i.Courses)
                .AsQueryable();

            TimeSpan timeSinceStartOfDay = DateTime.UtcNow.AddHours(3) - DateTime.UtcNow.AddHours(3).Date;
            int currentSlotIndex = (int)(timeSinceStartOfDay.TotalMinutes / 30);

            Guid? userId = _currentUserService.UserId;

            var instructors = await responseQuery.Where(x => instructorIds.Select(x => new Guid(x)).Contains(x.Id)).ToListAsync();
            var instructorResponses = instructors.Select(i => new GetInstructorsQueryResponse(
            i.Id,
            i.User.PhotoUrl,
                i.User.FullName,
                i.Title ?? string.Empty,
                _userService.GetUserStatus(i.UserId),
                i.Reviews.Any() ? (byte?)i.Reviews.Average(r => r.Rating) : null,
                i.Availabilities.Any(y => y.Date.Date == DateTime.UtcNow.Date.AddHours(3).Date && y.SlotString.Substring(currentSlotIndex + 1).Contains('1')),
                _unitOfWork.Users.GetQueryable().Include(x => x.SavedInstructors).Where(x => x.Id == userId).SelectMany(x => x.SavedInstructors).Any(y => y.InstructorId == i.Id),
                i.Courses.Select(c => new GetInstructorQueryCourseResponse(
                    c.Id,
                    c.Name,
                    c.BaseFee
                )).ToList()
            )).ToList();

            return Result.Success(new GetInstructorsFromAiQueryResponse(modifiedText, instructorResponses));
        }
    }
}
