using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class MajorCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
        public virtual ICollection<Major> Majors { get; set; } = null!;
    }
}
