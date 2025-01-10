namespace RandevuPlus.API.Shared.Domain
{
    public class Message : Entity
    {
        public Guid SenderId { get; set; }
        public virtual AppUser Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public virtual AppUser Receiver { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public bool IsRemovedFromSender { get; set; }
        public bool IsRemovedFromReceiver { get; set; }
        public virtual ICollection<MessageReaction> Reactions { get; set; }
    }
}
