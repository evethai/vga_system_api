using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Booking: BaseEntity
    {
        public Guid ConsultationTimeId { get; set; }
        public ConsultationTime ConsultationTime { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public bool Status { get; set; }
    }
}
