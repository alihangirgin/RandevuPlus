using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Queries.GetMyAppointedInstructorsQuery
{
    public class GetMyAppointedInstructorsQueryHandler : IRequestHandler<GetMyAppointedInstructorsQuery, Result<List<GetMyAppointedInstructorsQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public GetMyAppointedInstructorsQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetMyAppointedInstructorsQueryResponse>>> Handle(GetMyAppointedInstructorsQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var response = await _unitOfWork.Users.GetQueryable()
                .Include(x => x.Appointments)
                .ThenInclude(x => x.Instructor)
                .ThenInclude(x => x.User)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.Appointments)
                .Select(y=> y.User)
                .Distinct()
                .Select(k => new GetMyAppointedInstructorsQueryResponse(k.Id, k.FullName))
                .ToListAsync();

            return Result.Success(response);    
        }
    }
}
