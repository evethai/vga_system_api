using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ConsultationTime;

namespace Domain.Model.ConsultationDay
{
    public class ConsultationDayViewModel
    {
        public Guid Id { get; set; }
        public Guid ExpertId { get; set; }
        public DateOnly Day { get; set; }
        public int Status { get; set; }
        public List<ConsultationTimeViewModel> ConsultationTimes { get; set; } = new();
    }
}
