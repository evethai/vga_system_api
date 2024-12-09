using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Booking
{
    public class BookingViewModel
    {
        public Guid Id { get; set; }
        public Guid ConsultationTimeId { get; set; }
        public string Note { get; set; } = string.Empty; 
        public int TimeSlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Guid ConsultationDayId { get; set; }
        public DateOnly ConsultationDay { get; set; }
        public Guid ConsultantId { get; set; }
        public string ConsultantName { get; set; } = string.Empty;
        public string ConsultantEmail { get; set; } = string.Empty;
        public string ConsultantPhone { get; set; } = string.Empty;
        public Guid StudentId { get; set; }  
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentPhone { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }
    }
}
