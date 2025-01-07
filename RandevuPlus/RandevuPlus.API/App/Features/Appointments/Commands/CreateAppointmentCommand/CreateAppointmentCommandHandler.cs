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
            var course = await _unitOfWork.Courses.GetByIdAsync(command.CourseId);
            if (course == null) return Result.Error("CourseNotFound");

            var userId = _currentUserService.UserId.Value;

            var purchase = new Purchase()
            {
                PaymentStatus = PaymentStatus.Draft,
                UserId = userId
            };
            await _unitOfWork.Purchases.AddAsync(purchase);

            foreach (var commandAppointment in command.Appointments)
            {
                var availability = await _unitOfWork.Availabilities.GetAvailabilityByDateAsync(course.InstructorId, commandAppointment.Date);
                if (availability == null) return Result.Error("InstructorNotAvailable");

                var slotEndIndex = commandAppointment.SlotStartIndex + commandAppointment.SlotSize;
                string substring = availability.SlotString.Substring(commandAppointment.SlotStartIndex, slotEndIndex - commandAppointment.SlotStartIndex);
                bool allSlotsAvailable = substring.All(x => x == '1');
                if (!allSlotsAvailable) return Result.Error("InstructorNotAvailable");

                var newAppointment = new Appointment()
                {
                    CourseId = command.CourseId,
                    Date = commandAppointment.Date,
                    MeetingUrl = "test",
                    InstructorId = course.InstructorId,
                    SlotStartIndex = commandAppointment.SlotStartIndex,
                    SlotEndIndex = slotEndIndex,
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
