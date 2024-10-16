using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Booking
{
    public class BookingSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "student-name")]
        public string? studentName { get; set; }
        [FromQuery(Name = "consultant-name")]
        public string? consultantName { get; set; }
    }
}
