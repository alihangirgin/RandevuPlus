using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery
{
    public class GetMyAppointmentsQueryHandler : IRequestHandler<GetMyAppointmentsQuery, Result<List<GetAppointmentsQueryResponse>>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetMyAppointmentsQueryHandler(ICurrentUserService currentUserService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAppointmentsQueryResponse>>> Handle(GetMyAppointmentsQuery query, CancellationToken cancellationToken)
        {
            var isInstructor = _currentUserService.Roles.Contains("Instructor");
            var userId = _currentUserService.UserId.Value;

            List<Appointment> appointments;
            if (isInstructor)
            {
                var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
                if (instructor == null) return Result.Error("InstructorNotFound");

                appointments = await _unitOfWork.Appointments.GetInstructorAppointmentsByDateAsync(instructor.Id, query.StartDate, query.EndDate);
            }
            else
            {
                appointments = await _unitOfWork.Appointments.GetUserAppointmentsByDateAsync(userId, query.StartDate, query.EndDate);
            }

            return Result.Success(_mapper.Map<List<GetAppointmentsQueryResponse>>(appointments));
        }
    }
}
