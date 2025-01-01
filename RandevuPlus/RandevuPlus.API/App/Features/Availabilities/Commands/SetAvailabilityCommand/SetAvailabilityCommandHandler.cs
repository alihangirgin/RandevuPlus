using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Availabilities.Commands.SetAvailabilityCommand
{
    public class SetAvailabilityCommandHandler : IRequestHandler<SetAvailabilitiesCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SetAvailabilityCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(SetAvailabilitiesCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
            if (instructor == null) return Result.Error("InstructorNotFound");

            foreach (var commandAvailability in command.Availabilities)
            {
                var availability = await _unitOfWork.Availabilities.GetAvailabilityByDateAsync(instructor.Id, commandAvailability.Date);
                if (availability == null)
                {
                    var newAvailability = _mapper.Map<Availability>(commandAvailability);
                    newAvailability.InstructorId = instructor.Id;
                    await _unitOfWork.Availabilities.AddAsync(newAvailability);
                }
                else
                {
                    await _unitOfWork.Availabilities.UpdateAsync(_mapper.Map(commandAvailability, availability));
                }
            }
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
