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
            var course = await _unitOfWork.Courses.GetByIdAsync(query.CourseId, include: "PricingTiers");
            if (course == null) return Result.Error("CourseNotFound");

            decimal totalHour = (decimal)query.SlotSize * 0.5m;

            var discountPricingTier = course.PricingTiers
                .FirstOrDefault(x =>
                    (x.MinHours ?? 0) <= totalHour &&
                    (x.MaxHours ?? int.MaxValue) >= totalHour);

            decimal basePrice = totalHour * course.BaseFee;
            decimal? discountedPrice = discountPricingTier != null ? basePrice - discountPricingTier.DiscountFee : (decimal?)null;

            return Result.Success(new CalculatePriceQueryResponse(basePrice, discountedPrice));
        }
    }
}
