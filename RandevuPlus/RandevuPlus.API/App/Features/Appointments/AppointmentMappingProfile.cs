using AutoMapper;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsHistoryQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;
using RandevuPlus.API.Shared.Domain;
using System.Globalization;

namespace RandevuPlus.API.App.Features.Appointments
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            CreateMap<Appointment, GetAppointmentsQueryResponse>()
                 .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
                 .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.User.FullName))
                 .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                 .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                 .ForMember(dest => dest.SlotStartIndex, opt => opt.MapFrom(src => src.SlotStartIndex))
                 .ForMember(dest => dest.SlotEndIndex, opt => opt.MapFrom(src => src.SlotEndIndex))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Appointment, GetAppointmentQueryResponse>()
                 .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                 .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.User.FullName))
                 .ForMember(dest => dest.InstructorTitle, opt => opt.MapFrom(src => src.Instructor.Title))
                 .ForMember(dest => dest.InstructorPhotoUrl, opt => opt.MapFrom(src => src.Instructor.User.PhotoUrl))
                 .ForMember(dest => dest.UserPhotoUrl, opt => opt.MapFrom(src => src.User.PhotoUrl))
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                 .ForMember(dest => dest.UserTitle, opt => opt.MapFrom(src => "Öğrenci"))
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR"))))
                 .ForMember(dest => dest.StartHour, opt => opt.MapFrom(src => src.Date.AddHours(src.SlotStartIndex).Hour))
                 .ForMember(dest => dest.EndHour, opt => opt.MapFrom(src => src.Date.AddHours(src.SlotEndIndex).Hour));

            CreateMap<Appointment, GetMyAppointmentsHistoryQueryResponse>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForPath(dest => dest.Instructor.Id, opt => opt.MapFrom(src => src.Instructor.Id))
                .ForPath(dest => dest.Instructor.InstructorName, opt => opt.MapFrom(src => src.Instructor.User.FullName))
                .ForPath(dest => dest.Instructor.InstructorTitle, opt => opt.MapFrom(src => src.Instructor.Title))
                .ForPath(dest => dest.Instructor.InstructorPhotoUrl, opt => opt.MapFrom(src => src.Instructor.User.PhotoUrl))
                .ForPath(dest => dest.User.UserPhotoUrl, opt => opt.MapFrom(src => src.User.PhotoUrl))
                .ForPath(dest => dest.User.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForPath(dest => dest.User.UserTitle, opt => opt.MapFrom(src => "Öğrenci"))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR"))))
                .ForMember(dest => dest.StartHour, opt => opt.MapFrom(src => src.Date.AddHours(src.SlotStartIndex).Hour))
                .ForMember(dest => dest.EndHour, opt => opt.MapFrom(src => src.Date.AddHours(src.SlotEndIndex).Hour));

        }
    }
}
