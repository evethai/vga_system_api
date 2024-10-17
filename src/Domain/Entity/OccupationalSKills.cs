using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class OccupationalSKills : BaseEntity
    {
        public Guid WorkSkillsId { get; set; }
        public virtual WorkSkills WorkSkills { get; set; } = null!;
        public Guid OccupationId { get; set; }
        public virtual Occupation Occupation { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
