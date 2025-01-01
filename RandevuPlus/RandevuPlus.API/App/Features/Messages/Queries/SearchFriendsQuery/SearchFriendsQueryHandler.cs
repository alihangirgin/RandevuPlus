using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery
{
    public class SearchFriendsQueryHandler : IRequestHandler<SearchFriendsQuery, Result<List<SearchFriendsQueryResponseItem>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SearchFriendsQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<SearchFriendsQueryResponseItem>>> Handle(SearchFriendsQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var isInstructor = _currentUserService.Roles.Contains("Instructor");
            if (isInstructor)
            {
                var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
                if (instructor == null) return Result.Error("InstructorNotFound");
                var instructorFriends = await _unitOfWork.Appointments.SearchInstructorsAppointedUsersAsync(instructor.Id, query.Prefix);
                return Result.Success(_mapper.Map<List<SearchFriendsQueryResponseItem>>(instructorFriends));
            }
            else
            {
                var userFriends = await _unitOfWork.Appointments.SearchUsersAppointedInstructorsAsync(userId, query.Prefix);
                return Result.Success(_mapper.Map<List<SearchFriendsQueryResponseItem>>(userFriends));
            }
        }
    }
}
