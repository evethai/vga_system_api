using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.WorkSkills;

namespace Domain.Model.OccupationalGroup
{
    public class ResponseOccupationalGroupModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<OccupationalGroupViewModel> occupationalGroups { get; set; }
    }
}
