using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Major : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public Guid OccupationalGroupId { get; set; }
        public virtual OccupationalGroup OccupationalGroup { get; set; } = null!;
        public virtual ICollection<MajorType> MajorTypes { get; set; } = null!;
        public virtual ICollection<AdmissionInformation> AdmissionInformation { get; set; } = null!;

    }
}
