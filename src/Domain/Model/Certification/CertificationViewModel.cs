using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Certification
{
    public class CertificationViewModel
    {
        public int Id { get; set; }
        public Guid ConsultantId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public Guid MajorId { get; set; }
        public string MajorName { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
