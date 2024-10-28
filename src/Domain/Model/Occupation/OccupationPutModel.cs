using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.OccupationalSkills;

namespace Domain.Model.Occupation
{
    public class OccupationPutModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? HowToWork { get; set; }
        public string? WorkEnvironment { get; set; }
        public string? Education { get; set; }
        public string? PayScale { get; set; }
        public string? JobOutlook { get; set; }
        public bool Status { get; set; }
        public Guid? EntryLevelEducationId { get; set; }
        public Guid? OccupationalGroupId { get; set; }
        public List<OccupationalSkillsPutModel>? Skills { get; set; }
    }
}
