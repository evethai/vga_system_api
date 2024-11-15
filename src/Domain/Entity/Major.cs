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
        public string AdmissionGroup { get; set;} = string.Empty;
        public bool Status { get; set; }
        public string Image { get; set; } = string.Empty;
        public Guid MajorCategoryId { get; set; }
        public virtual MajorCategory MajorCategory { get; set; } = null!;
        public virtual ICollection<AdmissionInformation> AdmissionInformation { get; set; } = null!;

    }
}
