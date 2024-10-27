using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.OccupationalSkills
{
    public class OccupationalSkillsPutModel
    {
        public Guid? Id { get; set; }  
        public Guid? WorkSkillsId { get; set; }
        public string? Content { get; set; }
        public bool Status { get; set; }
    }
}
