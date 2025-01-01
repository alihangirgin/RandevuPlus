namespace RandevuPlus.API.Shared.Enums
{
    public enum AppointmentStatus : byte
    {
        Draft = 0, //not paid yet
        Scheduled = 1,
        Cancelled = 2,
        Completed = 3
    }
}
