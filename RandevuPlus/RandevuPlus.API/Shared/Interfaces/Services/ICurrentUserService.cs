namespace RandevuPlus.API.Shared.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }
}
