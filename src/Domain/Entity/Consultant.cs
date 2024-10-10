using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Consultant: BaseEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        public int ConsultantLevelId { get; set; }
        public virtual ConsultantLevel ConsultantLevel { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DoB { get; set; }
        public bool Gender { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; } = null!;
        public virtual ICollection<ConsultationDay> ConsultationDays { get; set; } = null!;

    }
}
