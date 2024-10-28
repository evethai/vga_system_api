using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.EntryLevelEducation
{
    public class ResponseEntryLevelEducationModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<EntryLevelEducationViewModel> entryLevels { get; set; }
    }
}
