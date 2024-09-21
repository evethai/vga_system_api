using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ConsultationDay : BaseEntity
    {
        public Guid ExpertId { get; set; }
        public CareerExpert Expert { get; set; } = null!;
        public DateOnly Day { get; set; }
        public int Status { get; set; }
        public virtual ICollection<ConsultationTime> ConsultationTimes { get; set; } = null!;

    }
}
