using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.ConsultationTime;

namespace Domain.Model.ConsultationDay
{
    public class ConsultationDayPostModel
    {
        public Guid ConsultantId { get; set; }
        public DateOnly Day { get; set; }
        public List<ConsultationTimePostModel> ConsultationTimes { get; set; } = new();
    }
}
