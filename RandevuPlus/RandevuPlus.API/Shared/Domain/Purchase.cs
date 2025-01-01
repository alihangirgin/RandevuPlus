using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.Shared.Domain
{
    public class Purchase : Entity
    {
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }  
    }
}
