using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.OccupationalSkills;

namespace Domain.Model.Occupation
{
    public class OccupationPostModel
    {
        public Guid EntryLevelEducationId { get; set; }
        public Guid OccupationalGroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string HowToWork { get; set; } = string.Empty;
        public string WorkEnvironment { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string? PayScale { get; set; }
        public string? JobOutlook { get; set; }
        public bool Status { get; set; }
        public List<OccupationalSkillsPostModel> OccupationalSkills { get; set; } = new();
    }
}
