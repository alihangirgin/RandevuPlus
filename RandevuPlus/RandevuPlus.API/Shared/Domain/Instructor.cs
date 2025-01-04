namespace RandevuPlus.API.Shared.Domain
{
    public class Instructor : Entity
    {
        public Guid UserId { get; set; } 
        public virtual AppUser User { get; set; }
        public string? Bio { get; set; } 
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<InstructorReview> Reviews { get; set; }
    }
}
