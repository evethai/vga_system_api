using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Booking
{
    public class BookingStudentReportModel
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; } = string.Empty;
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = string.Empty;
    }
}
