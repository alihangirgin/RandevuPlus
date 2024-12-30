namespace RandevuPlus.API.Shared.Domain
{
    public class Instructor : Entity
    {
        public Guid UserId { get; set; } 
        public virtual AppUser AppUser { get; set; }
        public string? Bio { get; set; } 
    }
}
