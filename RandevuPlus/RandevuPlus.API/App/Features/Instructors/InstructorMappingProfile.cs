using AutoMapper;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Instructors
{
    public class InstructorMappingProfile : Profile
    {
        public InstructorMappingProfile()
        {
            CreateMap<Instructor, GetInstructorQueryResponse>();
        }
    }
}
