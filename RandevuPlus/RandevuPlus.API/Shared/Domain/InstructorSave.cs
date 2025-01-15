namespace RandevuPlus.API.Shared.Domain
{
    public class InstructorSave : Entity
    {
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public Guid InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
