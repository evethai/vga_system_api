using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.ConsultantRelation
{
    public class ConsultantRelationViewModel
    {
        public Guid Id { get; set; }
        public Guid UniversityId { get; set; }
        public string UniversityName { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
