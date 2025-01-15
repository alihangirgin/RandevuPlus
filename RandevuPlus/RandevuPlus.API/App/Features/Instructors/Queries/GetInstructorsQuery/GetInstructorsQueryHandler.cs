using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Dtos;
using RandevuPlus.API.Shared.Enums;
using RandevuPlus.API.Shared.Interfaces.Services;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery
{
    public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, Result<PaginatedResponse<GetInstructorsQueryResponse>>>
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        public GetInstructorsQueryHandler(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<PaginatedResponse<GetInstructorsQueryResponse>>> Handle(GetInstructorsQuery query, CancellationToken cancellationToken)
        {
            var responseQuery = _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Reviews)
                .Include(i => i.Availabilities)
                .Include(i => i.Courses)
                .AsQueryable();

            // Filtreleme: Prefix User.FullName veya Instructor.Title
            if (!string.IsNullOrEmpty(query.Prefix))
            {
                responseQuery = responseQuery.Where(i => i.User.FullName.Contains(query.Prefix) || (i.Title != null && i.Title.Contains(query.Prefix)));
            }

            // Filtreleme: Date ve Slotlar
            if (query.Date.HasValue && query.SlotStartIndex.HasValue && query.SlotEndIndex.HasValue && query.SlotSize.HasValue)
            {
                
                var slotPattern = new string('1', query.SlotSize.Value);

                responseQuery = responseQuery.Where(i =>
                    i.Availabilities.Any(a =>
                        a.Date.Date == query.Date.Value.Date && // Aynı tarihte olmalı
                        a.SlotString.Substring(query.SlotStartIndex.Value, query.SlotEndIndex.Value - query.SlotStartIndex.Value + 1).Contains(slotPattern)   
                    )
                );
            }

            var totalCount = await responseQuery.CountAsync(cancellationToken);  // Toplam öğe sayısını al
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);  // Toplam sayfa sayısını hesapla

            // Sayfalama
            responseQuery = responseQuery.Skip((query.PageNumber - 1) * query.PageSize)
                         .Take(query.PageSize);

            // OrderBy: Rating, Cheapest, Most Expensive
            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                switch (query.OrderBy.ToLower())
                {
                    case "rating":
                        responseQuery = responseQuery.OrderByDescending(i => i.Reviews.Any() ? (byte?)i.Reviews.Average(r => r.Rating) : null);
                        break;

                    case "cheapest":
                        responseQuery = responseQuery.OrderBy(i => i.Courses.Min(c => c.BaseFee));
                        break;

                    case "expensive":
                        responseQuery = responseQuery.OrderByDescending(i => i.Courses.Max(c => c.BaseFee));
                        break;

                    default:
                        responseQuery = responseQuery.OrderBy(i => i.CreatedAt);
                        break;
                }
            }

            // Veriyi al
            var instructors = await responseQuery.ToListAsync(cancellationToken);

            TimeSpan timeSinceStartOfDay = DateTime.UtcNow.AddHours(3) - DateTime.UtcNow.AddHours(3).Date;
            int currentSlotIndex = (int)(timeSinceStartOfDay.TotalMinutes / 30);

            var instructorResponses = instructors.Select(i => new GetInstructorsQueryResponse(
                i.Id,
                i.User.PhotoUrl,
                i.User.FullName,
                i.Title ?? string.Empty,
                _userService.GetUserStatus(i.UserId),
                i.Reviews.Any() ? (byte?)i.Reviews.Average(r => r.Rating) : null,
                i.Availabilities.Any(y => y.Date.Date == DateTime.UtcNow.Date.AddHours(3).Date && y.SlotString.Substring(currentSlotIndex + 1).Contains('1')),
                i.Courses.Select(c => new GetInstructorQueryCourseResponse(
                    c.Id,
                    c.Name,
                    c.BaseFee
                )).ToList()
            )).ToList();

            var response = new PaginatedResponse<GetInstructorsQueryResponse>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = instructorResponses
            };

            return Result<PaginatedResponse<GetInstructorsQueryResponse>>.Success(response);
        }
    }
}
