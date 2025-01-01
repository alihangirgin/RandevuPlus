using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Purchases.Commands
{
    public sealed record PurchaseAppointmentCommand(Guid Id) : IRequest<Result>;
}
