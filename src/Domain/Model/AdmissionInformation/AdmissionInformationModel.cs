using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;

namespace Domain.Model.AdmissionInformation
{
    public class AdmissionInformationModel
    {
        public int Id { get; set; }
        public Guid UniversityId { get; set; }
        public Guid MajorId { get; set; }
        public Guid AdmissionMethodId { get; set; }
        public double TuitionFee { get; set; }
        public int Year { get; set; }
        public int QuantityTarget { get; set; }
        public bool Status { get; set; }
    }
    public class ResponseAdmissionInformationModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<AdmissionInformationModel> _admissionInformationModels { get; set; }
    }
}
