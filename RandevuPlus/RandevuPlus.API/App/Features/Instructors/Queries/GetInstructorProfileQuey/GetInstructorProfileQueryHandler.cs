using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorProfileQuey
{
    public class GetInstructorProfileQueryHandler : IRequestHandler<GetInstructorProfileQuery, Result<GetInstructorProfileResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetInstructorProfileQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetInstructorProfileResponse>> Handle(GetInstructorProfileQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;  
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId, includes: new List<string> { "User", "Experiences", "Skills", "Courses" });
            if (instructor == null) return Result.Error("InstructorNotFound");

            var response = _mapper.Map<GetInstructorProfileResponse>(instructor);
            return Result<GetInstructorProfileResponse>.Success(response);
        }
    }
}
