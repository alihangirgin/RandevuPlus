using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorProfileQuey
{
    public sealed record GetInstructorProfileResponse(string? Title, string? Bio, List<GetInstructorQuerySkillResponse> Skills, List<GetInstructorQueryExperienceResponse> Experiences, List<GetCourseQueryResponse> Courses);
}
