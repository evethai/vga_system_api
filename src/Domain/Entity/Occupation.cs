using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Occupation : BaseEntity
    {
        public Guid EntryLevelEducationId { get; set; }
        public virtual EntryLevelEducation EntryLevelEducation { get; set; } = null!;
        public Guid OccupationalGroupId { get; set; }
        public virtual OccupationalGroup OccupationalGroup { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string HowToWork { get; set; } = string.Empty;
        public string WorkEnvironment { get; set; } = string.Empty;
        public string Education { get; set;} = string.Empty;
        public string? PayScale { get; set; }
        public string? JobOutlook { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; } = string.Empty;
        public virtual ICollection<OccupationalSKills> OccupationalSKills { get; set; } = null!;
    }
}
