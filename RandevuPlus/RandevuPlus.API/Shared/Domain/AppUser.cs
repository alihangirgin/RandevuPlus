using Microsoft.AspNetCore.Identity;

namespace RandevuPlus.API.Shared.Domain
{
    public class AppUser : IdentityUser<Guid>
    {
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<InstructorReview> Reviews { get; set; }
    }
}
