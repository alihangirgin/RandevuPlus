using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery
{
    public sealed record GetInstructorQueryResponse(Guid Id, string? PhotoUrl, string FullName, string Title, UserStatus Status, byte? InstructorRating, List<GetInstructorQueryAvailabilityResponse> Availabilities, string? Bio, List<GetInstructorQuerySkillResponse> Skills, List<GetInstructorQueryExperienceResponse> Experiences, List<GetInstructorQueryReviewResponse> Reviews, List<GetCourseQueryResponse> Courses, bool IsSaved)
    {
        public GetInstructorQueryResponse() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, UserStatus.NotSet, null, new List<GetInstructorQueryAvailabilityResponse>(), null, new List<GetInstructorQuerySkillResponse>(), new List<GetInstructorQueryExperienceResponse>(), new List<GetInstructorQueryReviewResponse>(), new List<GetCourseQueryResponse>(), false)
        {
        }
    }
    public sealed record GetInstructorQuerySkillResponse(Guid Id, string SkillName);
    public sealed record GetInstructorQueryExperienceResponse(Guid Id, string Description, ExperienceType ExperienceType);
    public sealed record GetInstructorQueryReviewResponse(Guid Id, byte Rating, string Comment);
    public sealed record GetInstructorQueryAvailabilityResponse(Guid Id, DateTime Date, string SlotString);
}
