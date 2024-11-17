using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.OccupationalGroup
{
    public class OccupationalGroupPostModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [JsonIgnore]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string Image { get; set; } = string.Empty;
    }
}
