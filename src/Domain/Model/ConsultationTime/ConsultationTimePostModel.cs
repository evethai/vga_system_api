using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.ConsultationTime
{
    public class ConsultationTimePostModel
    {
        [Required(ErrorMessage = "Time slot id is required.")]
        public int TimeSlotId { get; set; }
        [Required(ErrorMessage = "Note is required.")]
        public string Note { get; set; } = string.Empty;
    }
}
