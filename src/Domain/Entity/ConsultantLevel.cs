using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ConsultantLevel : BasicEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PriceOnSlot { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<Consultant> Consultants { get; set; } = null!;

    }
}
