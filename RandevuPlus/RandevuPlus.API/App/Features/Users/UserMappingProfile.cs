using AutoMapper;
using RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Users
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AppUser, GetProfileQueryResponse>();
        }
    }
}
