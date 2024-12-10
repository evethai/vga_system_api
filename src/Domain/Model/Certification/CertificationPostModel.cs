using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.Certification
{
    public class CertificationPostModel
    {
        //[Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; } = string.Empty;
        //[Required(ErrorMessage = "Image url is required.")]
        public string? ImageUrl { get; set; } = string.Empty;
        //[Required(ErrorMessage = "MajorId is required.")]
        public Guid? MajorId { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
