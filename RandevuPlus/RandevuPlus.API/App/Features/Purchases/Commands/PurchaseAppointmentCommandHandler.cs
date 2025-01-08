using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Text;

namespace RandevuPlus.API.App.Features.Purchases.Commands
{
    public class PurchaseAppointmentCommandHandler : IRequestHandler<PurchaseAppointmentCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseAppointmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PurchaseAppointmentCommand command, CancellationToken cancellationToken)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(command.Id, include: "Appointments");
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
            }

            await _unitOfWork.Purchases.UpdateAsync(purchase);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
