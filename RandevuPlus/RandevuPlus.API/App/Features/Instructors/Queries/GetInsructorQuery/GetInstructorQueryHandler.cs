using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery
{
    public class GetInstructorQueryHandler : IRequestHandler<GetInstructorQuery, Result<GetInstructorQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        public GetInstructorQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IUserService userService, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetInstructorQueryResponse>> Handle(GetInstructorQuery query, CancellationToken cancellationToken)
        {
            Guid? userId = _currentUserService.UserId;

            var instructor = await _unitOfWork.Instructors.GetByIdAsync(query.Id, includes: new List<string> { "User", "Experiences", "Reviews", "Skills", "Courses" });
            if (instructor == null) return Result<GetInstructorQueryResponse>.Error("InstructorNotFound");

            var user = await _userManager.FindByIdAsync(instructor.UserId.ToString());
            if (user == null) return Result<GetInstructorQueryResponse>.Error("UserNotFound");

            var response = _mapper.Map<GetInstructorQueryResponse>(instructor);

            var availabilities = await _unitOfWork.Availabilities.GetCurrentAvailabilities(instructor.Id);

            foreach (var availability in availabilities)  //slot stringde tarihi geçen saatleri çıkartmak istiyorum, mappera koyulabilir
            {
                var currentTime = DateTime.UtcNow.AddHours(3);
                var updatedSlotString = availability.SlotString.Select((slot, index) =>
                {
                    var slotTime = availability.Date.AddMinutes(index * 30);
                    return slotTime < currentTime ? '0' : slot;
                }).ToArray();
                availability.SlotString = new string(updatedSlotString);
            }
            availabilities = availabilities.Where(a => !a.SlotString.All(c => c == '0')).ToList();

            var availabilitiesResponse = _mapper.Map<List<GetInstructorQueryAvailabilityResponse>>(availabilities);
            response = response with { PhotoUrl = user.PhotoUrl, Status = _userService.GetUserStatus(instructor.UserId), Availabilities = availabilitiesResponse, IsSaved = _unitOfWork.Users.GetQueryable().Include(x => x.SavedInstructors).Where(x => x.Id == userId).SelectMany(x => x.SavedInstructors).Any(y => y.InstructorId == instructor.Id), };

            return Result<GetInstructorQueryResponse>.Success(response);
        }
    }
}
