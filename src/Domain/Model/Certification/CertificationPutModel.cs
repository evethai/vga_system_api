using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.Certification
{
    public class CertificationPutModel
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? MajorId { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
