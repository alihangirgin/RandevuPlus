using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsHistoryQuery
{
    public class GetMyAppointmentsHistoryQueryHandler : IRequestHandler<GetMyAppointmentsHistoryQuery, Result<PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>>>
    {

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyAppointmentsHistoryQueryHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>>> Handle(GetMyAppointmentsHistoryQuery query, CancellationToken cancellationToken)
        {
            var isInstructor = _currentUserService.Roles.Contains("Instructor");
            var userId = _currentUserService.UserId.Value;

            PaginatedResult<Appointment> appointments;

            var dbQuery = _unitOfWork.Appointments.GetQueryable();

            int totalCount = 0;
            List<Appointment> items = new();

            if (isInstructor)
            {
                var instructor = await _unitOfWork.Instructors.GetByUserIdAsync(userId);
                if (instructor == null) return Result.Error("InstructorNotFound");

                dbQuery = dbQuery.Include("Course");
                dbQuery = dbQuery.Include("User");
                dbQuery = dbQuery.Where(x => x.InstructorId == instructor.Id);

                if (query.Prefix != null)
                {
                    dbQuery = dbQuery.Where(x => x.Course.Name.Contains(query.Prefix));
                }

                if (query.Status != null)
                {
                    if (Enum.TryParse(query.Status, true, out AppointmentStatus parsedStatus))
                    {
                        if (parsedStatus == AppointmentStatus.Draft) return Result.Error("StatusNotFound");
                        dbQuery = dbQuery.Where(x => x.Status == parsedStatus);
                    }
                }
                else
                {
                    dbQuery = dbQuery.Where(x => x.Status != AppointmentStatus.Draft);
                }

                if(query.RelatedId != null)
                {
                    if (Guid.TryParse(query.RelatedId, out Guid parsedInstructorId))
                    {
                        if(parsedInstructorId == Guid.Empty) return Result.Error("UserIdNotFound");
                        dbQuery = dbQuery.Where(x => x.InstructorId == parsedInstructorId);
                    }
                }

                if (query.OrderBy != null)
                {
                    if (query.OrderBy != "CreatedAt" && query.OrderBy != "CourseName") return Result.Error("InvalidOrderBy");
                    if (query.Descending)
                    {
                        dbQuery = query.OrderBy == "CreatedAt" ? dbQuery.OrderByDescending(x => x.Date).ThenBy(x => x.SlotStartIndex) : dbQuery.OrderByDescending(x => x.Course.Name);
                    }
                    else
                    {
                        dbQuery = query.OrderBy == "CreatedAt" ? dbQuery.OrderBy(x => x.Date).ThenBy(x => x.SlotStartIndex) : dbQuery.OrderBy(x => x.Course.Name);
                    }
                }

                totalCount = await dbQuery.CountAsync();
                items = await dbQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            }
            else
            {
                dbQuery = dbQuery.Include("Course");
                dbQuery = dbQuery.Include("Instructor.User");
                dbQuery = dbQuery.Where(x => x.UserId == userId);

                if(query.Prefix != null)
                {
                    dbQuery = dbQuery.Where(x => x.Course.Name.Contains(query.Prefix));
                }

                if (query.Status != null)
                {
                    if (Enum.TryParse(query.Status, true, out AppointmentStatus parsedStatus))
                    {
                        if (parsedStatus == AppointmentStatus.Draft) return Result.Error("StatusNotFound");
                        dbQuery = dbQuery.Where(x => x.Status == parsedStatus);
                    }
                }
                else
                {
                    dbQuery = dbQuery.Where(x => x.Status != AppointmentStatus.Draft);
                }

                if (query.RelatedId != null)
                {
                    if (Guid.TryParse(query.RelatedId, out Guid parsedUserId))
                    {
                        if (parsedUserId == Guid.Empty) return Result.Error("UserIdNotFound");
                        dbQuery = dbQuery.Where(x => x.UserId == parsedUserId);
                    }
                }

                if (query.OrderBy != null)
                {
                    if (query.OrderBy != "CreatedAt" && query.OrderBy != "CourseName") return Result.Error("InvalidOrderBy");
                    if (query.Descending)
                    {
                        dbQuery = query.OrderBy == "CreatedAt" ? dbQuery.OrderByDescending(x => x.Date).ThenBy(x => x.SlotStartIndex) : dbQuery.OrderByDescending(x => x.Course.Name);
                    }
                    else
                    {
                        dbQuery = query.OrderBy == "CreatedAt" ? dbQuery.OrderBy(x => x.Date).ThenBy(x => x.SlotStartIndex) : dbQuery.OrderBy(x => x.Course.Name);
                    }
                }

                totalCount = await dbQuery.CountAsync();
                items = await dbQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            }

            var responseItems = _mapper.Map<List<GetMyAppointmentsHistoryQueryResponse>>(items);
            var response = new PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>()
            {
                Items = responseItems,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize)
            };
            return Result.Success(response);
        }
    }
}
