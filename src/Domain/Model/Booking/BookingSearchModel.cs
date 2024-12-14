using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
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
        [FromQuery(Name = "student-id")]
        public Guid? studentId { get; set; }
        [FromQuery(Name = "consultant-id")]
        public Guid? consultantId { get; set; }
        [FromQuery(Name = "university-id")]
        public Guid? universityId { get; set; }
        [FromQuery(Name = "booking-status")]
        public BookingStatus? status { get; set; }
        [FromQuery(Name = "day")]
        public DateOnly? Day { get; set; }
        [FromQuery(Name = "day-in-week")]
        public DateOnly? dayInWeek { get; set; }
        [FromQuery(Name = "sort-by-day")]
        public bool? sortByDay { get; set; } = false;
        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }
}
