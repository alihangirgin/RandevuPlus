namespace RandevuPlus.API.Shared.Domain
{
    public class Instructor : Entity
    {
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<InstructorReview> Reviews { get; set; }
        public virtual ICollection<InstructorSkill> Skills { get; set; }
        public virtual ICollection<InstructorExperience> Experiences { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
