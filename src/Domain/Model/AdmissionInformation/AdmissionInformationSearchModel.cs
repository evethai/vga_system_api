using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.AdmissionInformation
{
    public class AdmissionInformationSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "university-id")]
        public Guid? UniversityId { get; set; }
        [FromQuery(Name = "major-id")]
        public Guid? MajorId { get; set; }
        [FromQuery(Name = "addmission-method-id")]
        public Guid? AdmissionMethodId { get; set; }
        [FromQuery(Name = "year")]
        public int? Year { get; set; }
        [FromQuery(Name = "quantity-target")]
        public int? QuantityTarget { get; set; }
        [FromQuery(Name = "tuition-fee")]
        public double? TuitionFee { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }
}
