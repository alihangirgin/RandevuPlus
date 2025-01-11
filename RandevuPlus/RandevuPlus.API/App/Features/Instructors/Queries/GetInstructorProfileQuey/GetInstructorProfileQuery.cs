using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorProfileQuey
{
    public sealed record GetInstructorProfileQuery : IRequest<Result<GetInstructorProfileResponse>>;
    
}
