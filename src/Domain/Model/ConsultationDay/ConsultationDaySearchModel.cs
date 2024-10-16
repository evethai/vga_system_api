using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.ConsultationDay
{
    public class ConsultationDaySearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? name { get; set; } 
        [FromQuery(Name = "day")]
        public DateOnly? Day { get; set; }
        [FromQuery(Name = "consultant-id")]
        public Guid? ConsultantId { get; set; }


    }
}
