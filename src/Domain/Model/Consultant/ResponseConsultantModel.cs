using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Student;

namespace Domain.Model.Consultant
{
    public class ResponseConsultantModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<ConsultantViewModel> consultants { get; set; }
    }
}
