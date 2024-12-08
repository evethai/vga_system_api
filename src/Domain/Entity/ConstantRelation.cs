using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ConsultantRelation : BaseEntity
    {
        public Guid ConsultantId { get; set; }
        public virtual Consultant Consultant { get; set; } = null!;
        public Guid UniversityId { get; set; }
        public virtual University University { get; set; } = null!;
        public bool Status { get; set; }
    }
}
