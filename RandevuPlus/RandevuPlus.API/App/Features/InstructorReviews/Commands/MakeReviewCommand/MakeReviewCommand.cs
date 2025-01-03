using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;

namespace RandevuPlus.API.App.Features.InstructorReviews.Commands.MakeReviewCommand
{
    public sealed record MakeReviewCommand(Guid InstructorId, byte Rating, string Comment) : IRequest<Result>;
}
