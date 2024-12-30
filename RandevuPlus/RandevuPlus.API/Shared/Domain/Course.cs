namespace RandevuPlus.API.Shared.Domain
{
    public class Course : Entity
    {
        public Guid InstructorId { get; set; }  
        public virtual Instructor Instructor { get; set; }  
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal BaseFee { get; set; } 
        public virtual ICollection<CoursePricingTier> PricingTiers { get; set; }
    }
}
