using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Occupation
{
    public class ResponseOccupationModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<OccupationViewModel> occupations { get; set; }
    }
}
