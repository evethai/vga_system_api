using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class UniversityLocation : BasicEntity
    {
        public Guid UniversityId { get; set; }
        public virtual University University { get; set; } = null!;
        public Guid RegionId { get; set; }
        public virtual Region Region { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
    }
}
