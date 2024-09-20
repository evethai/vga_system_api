using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class AdmissionInformation : BaseEntity
    {
        public int UniversityId { get; set; }
        public University University { get; set; } = null!;
        public int MajorId { get; set; }
        public Major Major { get; set; } = null!;
        public int AdmissionMethodId { get; set; }
        public AdmissionMethod AdmissionMethod { get; set; } = null!;
        public double MinimumFees { get; set; }
        public bool Status { get; set; }
    }
}
