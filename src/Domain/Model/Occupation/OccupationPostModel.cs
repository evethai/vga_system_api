using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Model.OccupationalSkills;

namespace Domain.Model.Occupation
{
    public class OccupationPostModel
    {
        [Required(ErrorMessage = "Entry level education id is required.")]
        public Guid EntryLevelEducationId { get; set; }
        [Required(ErrorMessage = "Occupational group id is required.")]
        public Guid OccupationalGroupId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty; 
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "How to work is required.")]
        public string HowToWork { get; set; } = string.Empty; 
        [Required(ErrorMessage = "Work environment is required.")]
        public string WorkEnvironment { get; set; } = string.Empty;
        [Required(ErrorMessage = "Education is required.")]
        public string Education { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pay scale is required.")]
        public string? PayScale { get; set; }
        [Required(ErrorMessage = "Job outlook is required.")]
        public string? JobOutlook { get; set; }
        [JsonIgnore]
        public bool Status { get; set; } = true;
        [Required(ErrorMessage = "Image is required.")]
        public string Image { get; set; } = string.Empty;
        public List<OccupationalSkillsPostModel> OccupationalSkills { get; set; } = new();
    }
}
