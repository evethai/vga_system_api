using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Booking
{
    public class BookingConsultantUpdateModel
    {
        [Required(ErrorMessage ="Type is required")]
        public BookingStatus Type { get; set; }
        public string? Comment { get; set; }
    }
}
