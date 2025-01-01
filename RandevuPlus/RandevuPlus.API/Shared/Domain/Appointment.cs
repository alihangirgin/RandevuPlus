using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.Shared.Domain
{
    public class Appointment : Entity
    {
        public Guid UserId { get; set; } 
        public virtual AppUser User { get; set; }
        public Guid InstructorId { get; set; } 
        public virtual Instructor Instructor { get; set; }
        public Guid CourseId { get; set; } 
        public virtual Course Course { get; set; }
        public DateTime Date { get; set; } 
        public int SlotStartIndex { get; set; } 
        public int SlotEndIndex { get; set; } 
        public AppointmentStatus Status { get; set; } 
        public string MeetingUrl { get; set; }
        public Guid PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }  
        //public virtual ICollection<AppointmentChangeRequest> ChangeRequests { get; set; }
    }
}
