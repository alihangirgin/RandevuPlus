using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery
{
    public sealed record GetInstructorsQueryResponse(Guid Id, string? PhotoUrl, string FullName, string? Title, UserStatus Status, byte? InstructorRating, bool IsAvailableToday, List<GetInstructorQueryCourseResponse> Courses)
    {
        public GetInstructorsQueryResponse() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, UserStatus.NotSet, null, false, new List<GetInstructorQueryCourseResponse>()) 
        { 
        }
    }
    public sealed record GetInstructorQueryCourseResponse(Guid Id, string Name, decimal BaseFee);
}
