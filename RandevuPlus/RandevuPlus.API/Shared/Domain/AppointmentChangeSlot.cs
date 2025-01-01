using System.ComponentModel.DataAnnotations.Schema;

namespace RandevuPlus.API.Shared.Domain
{
    public class AppointmentChangeSlot : Entity
    {
        public int AppointmentChangeRequestId { get; set; }
        public AppointmentChangeRequest AppointmentChangeRequest { get; set; }
        public int SlotStartIndex { get; set; }
        public int SlotEndIndex { get; set; }
    }
}
