using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Availabilities.Commands.SetAvailabilityCommand
{
    public sealed record SetAvailabilitiesCommand(List<SetAvailabilityCommand> Availabilities) : IRequest<Result>;
    public sealed record SetAvailabilityCommand(DateTime Date, string SlotString) : IRequest<Result>;

}
