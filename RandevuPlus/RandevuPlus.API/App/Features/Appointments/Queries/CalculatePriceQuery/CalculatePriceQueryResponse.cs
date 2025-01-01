namespace RandevuPlus.API.App.Features.Appointments.Queries.CalculatePriceQuery
{
    public sealed record CalculatePriceQueryResponse(decimal BaseFee, decimal? DiscountedFee);   
}
