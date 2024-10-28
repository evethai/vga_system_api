using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.OccupationalSkills
{
    public class OccupationalSkillsPostModel
    {
        [Required(ErrorMessage = "WorkSkillsId is required.")]
        public Guid WorkSkillsId { get; set; }
        [Required(ErrorMessage = "OccupationId is required.")]
        public Guid OccupationId { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;
        [JsonIgnore]
        public bool Status { get; set; } = true;
    }
}
