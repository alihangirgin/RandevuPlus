using AutoMapper;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.App.Features.Appointments
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            CreateMap<Appointment, GetAppointmentQueryResponse>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name));
        }
    }
}
