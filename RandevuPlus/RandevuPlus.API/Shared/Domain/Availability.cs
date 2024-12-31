using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RandevuPlus.API.Shared.Domain
{
    public class Availability : Entity
    {
        public Guid InstructorId { get; set; }
        public DateTime Date { get; set; }
        public string SlotString { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
