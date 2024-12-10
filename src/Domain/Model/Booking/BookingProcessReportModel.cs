using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Booking
{
    public class BookingProcessReportModel
    {
        [Required(ErrorMessage = "Type is required")]
        public BookingStatus Type { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; } = string.Empty;
        public string? Image { get; set; }
    }
}
