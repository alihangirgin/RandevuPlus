using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Appointments.Queries.CalculatePriceQuery
{
    public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, Result<CalculatePriceQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalculatePriceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CalculatePriceQueryResponse>> Handle(CalculatePriceQuery query, CancellationToken cancellationToken)
        {
            var instructorExist = await _unitOfWork.Instructors.CheckAsync(query.InstructorId);
            if (!instructorExist) return Result.Error("InstructorNotFound");

            var course = await _unitOfWork.Courses.GetByIdAsync(query.CourseId, include: "CoursePricingTiers");
            if (course == null) return Result.Error("CourseNotFound");

            int totalHour = 0;
            foreach (var commandAppointment in query.Appointments)
            {
                totalHour = totalHour + (commandAppointment.SlotEndIndex - commandAppointment.SlotStartIndex);
            }

            var discountPricingTier = course.PricingTiers
                .FirstOrDefault(x =>
                    (x.MinHours ?? 0) <= totalHour &&
                    (x.MaxHours ?? int.MaxValue) >= totalHour);

            decimal basePrice = totalHour * course.BaseFee;
            decimal? discountedPrice = discountPricingTier != null ? (basePrice - discountPricingTier?.DiscountFee) : null;

            return Result.Success(new CalculatePriceQueryResponse(basePrice, discountedPrice));
        }
    }
}
