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
                 .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))  // InstructorId
                 .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.User.FullName)) // InstructorName
                 .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))            // CourseId
                 .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))        // CourseName
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))                     // Date
                 .ForMember(dest => dest.SlotStartIndex, opt => opt.MapFrom(src => src.SlotStartIndex))  // SlotStartIndex
                 .ForMember(dest => dest.SlotEndIndex, opt => opt.MapFrom(src => src.SlotEndIndex))      // SlotEndIndex
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
