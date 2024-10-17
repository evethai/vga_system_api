using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class EntryLevelEducation : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
        public virtual ICollection<Occupation> Occupations { get; set; } = null!;
    }
}
