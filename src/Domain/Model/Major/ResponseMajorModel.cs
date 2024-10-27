using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Major
{
    public class ResponseMajorModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<MajorViewModel> majors { get; set; }
    }
}
