using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.Shared.Domain
{
    public class InstructorExperience : Entity
    {
        public ExperienceType ExperienceType { get; set; }
        public string Description { get; set; }
        public Guid InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
