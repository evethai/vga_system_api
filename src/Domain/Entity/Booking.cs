using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class Booking: BaseEntity
    {
        public Guid ConsultationTimeId { get; set; }
        public ConsultationTime ConsultationTime { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public BookingStatus Status { get; set; }
        public string? Comment { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }

    }
}
