using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery
{
    public class GetMyAvailabilitiesQueryHandler : IRequestHandler<GetMyAvailabilitiesQuery, Result<List<GetInstructorQueryAvailabilityResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMyAvailabilitiesQueryHandler(IMapper mapper, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetInstructorQueryAvailabilityResponse>>> Handle(GetMyAvailabilitiesQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            var availabilities = await _unitOfWork.Availabilities.GetCurrentAvailabilities(instructor.Id);

            foreach (var availability in availabilities)  //slot stringde tarihi geçen saatleri çıkartmak istiyorum, mappera koyulabilir
            {
                var currentTime = DateTime.UtcNow.AddHours(3);
                var updatedSlotString = availability.SlotString.Select((slot, index) =>
                {
                    var slotTime = availability.Date.AddMinutes(index * 30);
                    return slotTime < currentTime ? '2' : slot;
                }).ToArray();
                availability.SlotString = new string(updatedSlotString);
            }
            availabilities = availabilities.Where(a => !a.SlotString.All(c => c == '0' || c == '2')).ToList();

            var availabilitiesResponse = _mapper.Map<List<GetInstructorQueryAvailabilityResponse>>(availabilities);
            return Result.Success(availabilitiesResponse);
        }
    }
}
