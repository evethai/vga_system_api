using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ConsultantLevel;

namespace Domain.Model.ConsultationDay
{
    public class ResponseConsultationDayModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<ConsultationDayViewModel> consultationDay { get; set; }
    }
}
