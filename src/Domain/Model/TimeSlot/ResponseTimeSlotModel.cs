using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.TimeSlot
{
    public class ResponseTimeSlotModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<TimeSlotViewModel> timeSlots { get; set; }
    }
}
