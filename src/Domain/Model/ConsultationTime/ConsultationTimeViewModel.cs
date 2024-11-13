using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.ConsultationTime
{
    public class ConsultationTimeViewModel
    {
        public Guid Id { get; set; }
        public int TimeSlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Guid ConsultationDayId { get; set; }
        public int Status { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
