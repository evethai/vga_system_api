using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Consultant
{
    public class ConsultantSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? name { get; set; }
        [FromQuery(Name = "consultant-level-id")]
        public int consultantLevelId { get; set; }
        [FromQuery(Name = "university-id")]
        public Guid? universityId { get; set; }

        [FromQuery(Name = "status")]
        public AccountStatus? status { get; set; }
    }
}
