using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class AdmissionInformation : BasicEntity
    {
        public Guid UniversityId { get; set; }
        public University University { get; set; } = null!;
        public Guid MajorId { get; set; }
        public Major Major { get; set; } = null!;
        public Guid AdmissionMethodId { get; set; }
        public AdmissionMethod AdmissionMethod { get; set; } = null!;
        public double TuitionFee { get; set; }
        public int Year { get; set; }
        public int QuantityTarget { get; set; }
        public bool Status { get; set; }
    }
}
