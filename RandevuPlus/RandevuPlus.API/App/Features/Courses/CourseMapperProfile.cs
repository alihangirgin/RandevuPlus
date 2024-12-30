using AutoMapper;
using RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Courses
{
    public class CourseMapperProfile : Profile
    {
        public CourseMapperProfile()
        {
            CreateMap<CreateCourseCommand, Course>();
            CreateMap<UpdateCourseCommand, Course>();
            CreateMap<Course, GetCourseQueryResponse>();
        }
    }
}
