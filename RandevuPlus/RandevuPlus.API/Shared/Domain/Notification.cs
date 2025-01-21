namespace RandevuPlus.API.Shared.Domain
{
    public class Notification : Entity
    {
        public Guid ReceiverId { get; set; }
        public virtual AppUser Receiver { get; set; }
        public string NotificationText { get; set; }
        public bool IsRead { get; set; }
    }
}
