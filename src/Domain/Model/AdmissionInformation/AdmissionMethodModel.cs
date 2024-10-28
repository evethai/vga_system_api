using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Model.AdmissionInformation
{
    public class AdmissionMethodModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
    public class ResponseAdmissionMethodModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<AdmissionMethod> _admissionMethodModels { get; set; }
    }
}
