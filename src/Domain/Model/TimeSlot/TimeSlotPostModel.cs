using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.TimeSlot
{
    public class TimeSlotPostModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Start Time is required.")]
        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage = "End Time is required.")]
        public TimeOnly EndTime { get; set; }
    }
}
