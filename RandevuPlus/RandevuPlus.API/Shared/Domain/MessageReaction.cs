namespace RandevuPlus.API.Shared.Domain
{
    public class MessageReaction : Entity
    {
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
        public Guid ReactorId { get; set; }
        public virtual AppUser Reactor { get; set; }
        public string Reaction { get; set; }
    }
}
