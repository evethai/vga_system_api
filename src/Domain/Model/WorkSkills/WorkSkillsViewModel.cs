using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.WorkSkills
{
    public class WorkSkillsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
