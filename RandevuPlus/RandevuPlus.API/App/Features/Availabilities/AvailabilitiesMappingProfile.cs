using AutoMapper;
using RandevuPlus.API.App.Features.Availabilities.Commands.SetAvailabilityCommand;
using RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Availabilities
{
    public class AvailabilitiesMappingProfile : Profile
    {
        public AvailabilitiesMappingProfile()
        {
            CreateMap<SetAvailabilityCommand, Availability>();
            CreateMap<Availability, GetMyAvailabilityQueryResponse>();
        }
    }
}
