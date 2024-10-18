using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Occupation : BaseEntity
    {
        public Guid EntryLevelId { get; set; }
        public virtual EntryLevelEducation EntryLevelEducation { get; set; } = null!;
        public Guid OccupationGroupId { get; set; }
        public virtual OccupationalGroup OccupationalGroup { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string HowToWork { get; set; } = string.Empty;
        public string WorkEnvironment { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
