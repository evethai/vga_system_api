using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Model.Account;

namespace Domain.Model.Consultant
{
    public class ConsultantPostModel : AccountPostModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Consultant level id is required.")]
        public int ConsultantLevelId { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DoB { get; set; }
    }
}
