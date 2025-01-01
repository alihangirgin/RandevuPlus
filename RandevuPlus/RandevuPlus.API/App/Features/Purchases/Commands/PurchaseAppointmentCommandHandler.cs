using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

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
            }

            await _unitOfWork.Purchases.UpdateAsync(purchase);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
