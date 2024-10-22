using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class University : BaseEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public virtual ICollection<News> News { get; set; } = null!;
        public virtual ICollection<AdmissionInformation> AdmissionInformation { get; set; } = null!;
    }
}
