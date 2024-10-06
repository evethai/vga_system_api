using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Certification : BasicEntity
    {
        public Guid ConsultantId { get; set; }
        public virtual Consultant Consultant { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
