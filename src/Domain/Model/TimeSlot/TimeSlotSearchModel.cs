using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.TimeSlot
{
    public class TimeSlotSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? name { get; set; }

        [FromQuery(Name = "start-time")]
        public TimeOnly? StartTime { get; set; }
        [FromQuery(Name = "end-time")]
        public TimeOnly? EndTime { get; set; }
        [FromQuery(Name = "status")]
        public bool? status { get; set; }
    }
}
