using AutoMapper;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Instructors
{
    public class InstructorMappingProfile : Profile
    {
        public InstructorMappingProfile()
        {
            CreateMap<Instructor, GetInstructorQueryResponse>()
                .ForMember(dest => dest.FullName, opt =>
                    opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.User.PhotoUrl))
                .ForMember(dest => dest.InstructorRating, opt => opt.MapFrom(src =>
                    src.Reviews.Any() ? (byte?)src.Reviews.Average(r => r.Rating) : null))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                    src.Skills.Select(s => new GetInstructorQuerySkillResponse(s.Id, s.SkillName)).ToList()))
                .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src =>
                    src.Experiences.Select(e => new GetInstructorQueryExperienceResponse(e.Id, e.Description, e.ExperienceType)).ToList()))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src =>
                    src.Reviews.Select(r => new GetInstructorQueryReviewResponse(r.Id, r.Rating, r.Comment)).ToList()));

            CreateMap<InstructorReview, GetInstructorQueryReviewResponse>();
            CreateMap<InstructorSkill, GetInstructorQuerySkillResponse>();
            CreateMap<InstructorExperience, GetInstructorQueryExperienceResponse>();
            CreateMap<Availability, GetInstructorQueryAvailabilityResponse>();
        }
    }
}
