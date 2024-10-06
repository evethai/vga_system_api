using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.ConsultationTime
{
    public class ConsultationTimePostModel
    {
        public int TimeSlotId { get; set; }          
        public string Note { get; set; } = string.Empty;
    }
}
