namespace RandevuPlus.API.Shared.Enums
{
    public enum PaymentStatus : byte
    {
        Draft = 0, //not paid yet
        Paid = 1,
        Cancelled = 2,
        Refunded = 3
    }
}
