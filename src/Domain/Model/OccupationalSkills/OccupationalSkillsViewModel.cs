using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.WorkSkills;

namespace Domain.Model.OccupationalSkills
{
    public class OccupationalSkillsViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool Status { get; set; }
        public WorkSkillsViewModel WorkSkill { get; set; } = null!;
    }
}
