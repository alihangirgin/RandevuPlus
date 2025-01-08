using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery
{
    public class GetInstructorQueryHandler : IRequestHandler<GetInstructorQuery, Result<GetInstructorQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public GetInstructorQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<GetInstructorQueryResponse>> Handle(GetInstructorQuery query, CancellationToken cancellationToken)
        {
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
                    var slotTime = currentTime.Date.AddMinutes(index * 30);
                    return slotTime < currentTime ? '0' : slot;
                }).ToArray();
                availability.SlotString = new string(updatedSlotString);
            }
            availabilities = availabilities.Where(a => !a.SlotString.All(c => c == '0')).ToList();

            var availabilitiesResponse = _mapper.Map<List<GetInstructorQueryAvailabilityResponse>>(availabilities);
            response = response with { PhotoUrl = user.PhotoUrl, Status = UserStatus.Online, Availabilities = availabilitiesResponse };

            return Result<GetInstructorQueryResponse>.Success(response);
        }
    }
}
