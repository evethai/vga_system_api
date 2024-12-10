using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.EntryLevelEducation;
using Domain.Model.OccupationalGroup;
using Domain.Model.OccupationalSkills;

namespace Domain.Model.Occupation
{
    public class OccupationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string HowToWork { get; set; } = string.Empty;
        public string WorkEnvironment { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string? PayScale { get; set; }
        public string? JobOutlook { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsCare { get; set; } = false;
        public int CareLevel { get; set; } = 0;
        public int NumberCare { get; set; } = 0;
        public EntryLevelEducationViewModel EntryLevelEducation { get; set; } = null!;
        public OccupationalGroupViewModel OccupationalGroup { get; set; } = null!;
        public List<OccupationalSkillsViewModel> OccupationalSkills { get; set; } = new();
    }
}
