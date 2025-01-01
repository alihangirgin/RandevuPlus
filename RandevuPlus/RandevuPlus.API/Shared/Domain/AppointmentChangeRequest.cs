using RandevuPlus.API.Shared.Enums;

namespace RandevuPlus.API.Shared.Domain
{
    public class AppointmentChangeRequest : Entity
    {
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public AppointmentChangeRequestType Type { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Reason { get; set; }
        public bool IsApproved { get; set; }
        public virtual ICollection<AppointmentChangeSlot> Slots { get; set; }
    }
}
