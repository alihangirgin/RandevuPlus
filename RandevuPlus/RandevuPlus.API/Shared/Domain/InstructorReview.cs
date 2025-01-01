namespace RandevuPlus.API.Shared.Domain
{
    public class InstructorReview : Entity
    {
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public Guid InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }
    }
}
