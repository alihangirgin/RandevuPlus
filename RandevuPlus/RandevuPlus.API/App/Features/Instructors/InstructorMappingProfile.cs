using AutoMapper;
using RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorExperienceCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorSkillCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorExperienceCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorSkillCommand;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorProfileQuey;
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

            CreateMap<Instructor, GetInstructorProfileResponse>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                    src.Skills.Select(s => new GetInstructorQuerySkillResponse(s.Id, s.SkillName)).ToList()))
                .ForMember(dest => dest.Experiences, opt => opt.MapFrom(src =>
                    src.Experiences.Select(e => new GetInstructorQueryExperienceResponse(e.Id, e.Description, e.ExperienceType)).ToList()));


            CreateMap<CreateInstructorExperienceCommand, InstructorExperience>();
            CreateMap<UpdateInstructorExperienceCommand, InstructorExperience>();
            CreateMap<CreateInstructorSkillCommand, InstructorSkill>();
            CreateMap<UpdateInstructorSkillCommand, InstructorSkill>();

            CreateMap<InstructorReview, GetInstructorQueryReviewResponse>();
            CreateMap<InstructorSkill, GetInstructorQuerySkillResponse>();
            CreateMap<InstructorExperience, GetInstructorQueryExperienceResponse>();
            CreateMap<Availability, GetInstructorQueryAvailabilityResponse>()
            .ForMember(dest => dest.SlotString, opt => opt.MapFrom((src, dest) =>
            {
                // Mevcut zamanı al
                var currentTime = DateTime.Now;

                // SlotString içerisinde her zaman dilimini kontrol et ve geçmiş olanları 0 yap
                var updatedSlotString = src.SlotString.Select((slot, index) =>
                {
                    // Her slot'ın zamanını hesapla (30 dakikalık dilimlerle)
                    var slotTime = currentTime.Date.AddMinutes(index * 30); // Slot 30 dakikalık aralıklarla

                    // Eğer slot zamanı geçmişse, 0 olarak değiştir
                    return slotTime < currentTime ? '0' : slot;
                }).ToArray();

                // Güncellenmiş slot string'i döndür
                return new string(updatedSlotString);
            }));
        }
    }
}
