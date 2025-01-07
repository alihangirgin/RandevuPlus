using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, Result<GetAppointmentQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAppointmentQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAppointmentQueryResponse>> Handle(GetAppointmentQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(query.Id, includes : new List<string> { "Instructor.User", "User", "Course" });
            if (appointment == null || appointment.Status == Shared.Enums.AppointmentStatus.Draft) return Result.Error("AppointmentNotFound");

            var isInstructor = _currentUserService.Roles.Contains("Instructor");
            if (isInstructor)
            {
                var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
                if (instructor == null) return Result.Error("InstructorNotFound");
                if (appointment.InstructorId != instructor.Id) return Result.Error("Unauthorized");
            }
            else
            {
                if (appointment.UserId != userId) return Result.Error("Unauthorized");
            }

            return Result.Success(_mapper.Map<GetAppointmentQueryResponse>(appointment));
        }
    }
}
