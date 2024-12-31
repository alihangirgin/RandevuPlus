namespace RandevuPlus.API.Shared.Domain
{
    public class CoursePricingTier : Entity
    {
        public int? MinHours { get; set; }  
        public int? MaxHours { get; set; } 
        public decimal DiscountFee { get; set; }
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
