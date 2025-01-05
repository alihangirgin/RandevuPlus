namespace RandevuPlus.API.Shared.Domain
{
    public class InstructorSkill : Entity
    {
        public string SkillName { get; set; }
        public Guid InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
