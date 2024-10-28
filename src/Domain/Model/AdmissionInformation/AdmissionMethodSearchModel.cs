using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.AdmissionInformation
{
    public class AdmissionMethodSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        [FromQuery(Name = "description")]
        public string? Description { get; set; }
        [FromQuery(Name = "status")]
        public bool? Status { get; set; }
        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }
}
