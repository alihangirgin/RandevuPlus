using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery
{
    public class GetMyAvailabilitiesQueryHandler : IRequestHandler<GetMyAvailabilitiesQuery, Result<List<GetMyAvailabilityQueryResponse>>>
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

        public async Task<Result<List<GetMyAvailabilityQueryResponse>>> Handle(GetMyAvailabilitiesQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            var availabilities = await _unitOfWork.Availabilities.GetAvailabilitiesByDateAsync(instructor.Id, query.StartDate, query.EndDate);

            List<DateTime> betweenDates = Enumerable.Range(0, (query.EndDate.Date - query.StartDate.Date).Days + 1)
                     .Select(offset => query.StartDate.Date.AddDays(offset))
                     .ToList();
            var missingDates = betweenDates.Except(availabilities.Select(x => x.Date)).ToList();
            foreach (var missingDate in missingDates)
            {
                var emptyAvailability = new Availability
                {
                    Id = Guid.NewGuid(),
                    Date = missingDate,
                    InstructorId = instructor.Id,
                    SlotString = "000000000000000000000000000000000000000000000000"
                };
                availabilities.Add(emptyAvailability);
            }

            return Result.Success(_mapper.Map<List<GetMyAvailabilityQueryResponse>>(availabilities));
        }
    }
}
