using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<AppointmentChangeRequest> AppointmentChangeRequests { get; set; }
        //public DbSet<AppointmentChangeSlot> AppointmentChangeSlots { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePricingTier> CoursePricingTiers { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorReview> InstructorReviews { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
