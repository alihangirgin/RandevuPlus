using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Infrastructure.Services;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Shared.Constants;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Text;

namespace RandevuPlus.API.App.Features.Purchases.Commands
{
    public class PurchaseAppointmentCommandHandler : IRequestHandler<PurchaseAppointmentCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public PurchaseAppointmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IHubContext<UserHub> hubContext, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _hubContext = hubContext;
            _userService = userService;
        }

        public async Task<Result> Handle(PurchaseAppointmentCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var purchase = await _unitOfWork.Purchases.GetByIdAsync(command.Id, includes: new List<string> { "Appointments.Instructor.User", "Appointments.Course" });
            if (purchase == null) return Result.Error("PurchaseNotFound");

            foreach (var appointment in purchase.Appointments)
            {
                appointment.Status = Shared.Enums.AppointmentStatus.Scheduled;
                var availability = await _unitOfWork.Availabilities.GetAvailabilityByDateAsync(appointment.InstructorId, appointment.Date);
                if (availability == null) return Result.Error("InstructorsAvailabilityNotFound");

                string substring = availability.SlotString.Substring(appointment.SlotStartIndex, appointment.SlotEndIndex - appointment.SlotStartIndex);
                bool allSlotsAvailable = substring.All(x => x == '1');
                if (!allSlotsAvailable) return Result.Error("InstructorNotAvailable");

                var currentTime = DateTime.UtcNow.AddHours(3);
                var slotStartTime = currentTime.Date.AddMinutes(appointment.SlotStartIndex * 30);
                if (slotStartTime < currentTime) return Result.Error("AppointmentStartTimeExceed");

                var originalSlotString = availability.SlotString;
                StringBuilder sb = new StringBuilder();
                sb.Append(originalSlotString.Substring(0, appointment.SlotStartIndex));
                sb.Append(new string('2', appointment.SlotEndIndex - appointment.SlotStartIndex));
                sb.Append(originalSlotString.Substring(appointment.SlotEndIndex));
                availability.SlotString = sb.ToString();
                await _unitOfWork.Availabilities.UpdateAsync(availability);

                Notification userNotification = new()
                {
                    ReceiverId = userId,
                    NotificationText = NotificationTexts.PurchaseCompleteUser(appointment.Instructor.User.FullName,
                    appointment.Instructor.Title ?? string.Empty, appointment.Course.Name, appointment.Date, appointment.SlotStartIndex, appointment.SlotEndIndex)
                };
                await _unitOfWork.Notifications.AddAsync(userNotification);
                Notification instructorNotification = new()
                {
                    ReceiverId = appointment.Instructor.UserId,
                    NotificationText = NotificationTexts.PurchaseCompleteInstructor(appointment.Course.Name, appointment.Date, appointment.SlotStartIndex, appointment.SlotEndIndex)
                };
                await _unitOfWork.Notifications.AddAsync(instructorNotification);
            }

            await _unitOfWork.Purchases.UpdateAsync(purchase);
            await _unitOfWork.CommitAsync();

            foreach (var appointment in purchase.Appointments)
            {
                if (_userService.GetOnlineUsers().Contains(userId.ToString()))
                    await _hubContext.Clients.User(userId.ToString()).SendAsync("NotificationReceived");
                if (_userService.GetOnlineUsers().Contains(appointment.Instructor.UserId.ToString()))
                    await _hubContext.Clients.User(appointment.Instructor.UserId.ToString()).SendAsync("NotificationReceived");
            }

            return Result.Success();
        }
    }
}
