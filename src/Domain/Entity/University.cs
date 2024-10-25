using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class University : BaseEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        //public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string EstablishedYear { get; set; } = string.Empty;
        public UniversityType Type { get; set; }
        public virtual ICollection<News> News { get; set; } = null!;
        public virtual ICollection<AdmissionInformation> AdmissionInformation { get; set; } = null!;
        public virtual ICollection<UniversityLocation> UniversityLocations { get; set; } = null!;
        public virtual ICollection<Consultant> Consultants { get; set; } = null!;

    }
}
