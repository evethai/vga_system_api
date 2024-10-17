using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class WorkSkills : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
