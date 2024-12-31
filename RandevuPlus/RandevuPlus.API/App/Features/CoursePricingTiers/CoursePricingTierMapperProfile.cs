using AutoMapper;
using RandevuPlus.API.App.Features.CoursePricingTiers.Commands.CreateCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Commands.UpdateCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.CoursePricingTiers
{
    public class CoursePricingTierMapperProfile : Profile
    {
        public CoursePricingTierMapperProfile()
        {
            CreateMap<CreateCoursePricingTierCommand, CoursePricingTier>();
            CreateMap<UpdateCoursePricingTierCommand, CoursePricingTier>();
            CreateMap<Course, GetCoursePricingTierResponse>();
        }
    }
}
