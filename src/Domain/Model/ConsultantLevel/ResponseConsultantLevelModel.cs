using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ConsultantLevel;

namespace Domain.Model.ConsultantLevel
{
    public class ResponseConsultantLevelModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<ConsultantLevelViewModel> consultantLevels { get; set; }
    }
}
