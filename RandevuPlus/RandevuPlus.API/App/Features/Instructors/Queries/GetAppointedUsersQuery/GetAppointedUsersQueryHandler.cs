using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetAppointedUsersQuery
{
    public class GetAppointedUsersQueryHandler : IRequestHandler<GetAppointedUsersQuery, Result<List<GetAppointedUsersQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public GetAppointedUsersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetAppointedUsersQueryResponse>>> Handle(GetAppointedUsersQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string> { "User", "Experiences", "Skills", "Courses" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var response = await _unitOfWork.Instructors.GetQueryable()
                .Include(x => x.Appointments)
                .ThenInclude(x => x.User)
                .Where(x => x.Id == instructor.Id)
                .SelectMany(x => x.Appointments)
                .Select(y => y.User)
                .Distinct()
                .Select(k => new GetAppointedUsersQueryResponse(k.Id, k.FullName))
                .ToListAsync();

            return Result.Success(response);
        }
    }
}
