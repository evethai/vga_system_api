using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ConsultationTime : BaseEntity
    {
        public int TimeSlotId { get; set; }
        public TimeSlot SlotTime { get; set; } = null!;
        public int ConsultationDayId { get; set; }
        public ConsultationDay Day { get; set; } = null!;
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;
        public virtual ICollection<Booking> Bookings { get; set; } = null!;

    }
}
