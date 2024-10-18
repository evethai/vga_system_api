using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Consultant;

namespace Domain.Model.Booking
{
    public class ResponseBookingModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<BookingViewModel> bookings { get; set; }
    }
}
