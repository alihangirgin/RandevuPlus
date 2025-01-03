using AutoMapper;
using RandevuPlus.API.App.Features.InstructorReviews.Commands.MakeReviewCommand;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.InstructorReviews
{
    public class InstructorReviewMappingProfile : Profile
    {
        public InstructorReviewMappingProfile()
        {
            CreateMap<MakeReviewCommand, InstructorReview>();
        }
    }
}
