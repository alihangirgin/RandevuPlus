using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<CreateAppointmentCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAppointmentCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateAppointmentCommandResponse>> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken)
        {
            var instructorExist = await _unitOfWork.Instructors.CheckAsync(command.InstructorId);
            if (!instructorExist) return Result.Error("InstructorNotFound");

            var courseExist = await _unitOfWork.Courses.CheckAsync(command.CourseId);
            if (!courseExist) return Result.Error("CourseNotFound");

            var userId = _currentUserService.UserId.Value;

            var purchase = new Purchase()
            {
                PaymentStatus = PaymentStatus.Draft,
                UserId = userId
            };
            await _unitOfWork.Purchases.AddAsync(purchase);

            foreach (var commandAppointment in command.Appointments)
            {
                var availability = await _unitOfWork.Availabilities.GetAvailabilityByDateAsync(command.InstructorId, commandAppointment.Date);
                if (availability == null) return Result.Error("InstructorNotAvailable");

                string substring = availability.SlotString.Substring(commandAppointment.SlotStartIndex, commandAppointment.SlotEndIndex - commandAppointment.SlotStartIndex + 1);
                bool allSlotsAvailable = substring.All(x => x == '1');
                if (!allSlotsAvailable) return Result.Error("InstructorNotAvailable");

                var newAppointment = new Appointment()
                {
                    CourseId = command.CourseId,
                    Date = commandAppointment.Date,
                    MeetingUrl = "test",
                    InstructorId = command.InstructorId,
                    SlotStartIndex = commandAppointment.SlotStartIndex,
                    SlotEndIndex = commandAppointment.SlotEndIndex,
                    Status = AppointmentStatus.Draft,
                    UserId = userId,
                    PurchaseId = purchase.Id
                };
                await _unitOfWork.Appointments.AddAsync(newAppointment);
            }
            await _unitOfWork.CommitAsync();
            return Result<CreateAppointmentCommandResponse>.Success(new CreateAppointmentCommandResponse(purchase.Id));
        }
    }
}
