using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Account;

namespace Domain.Model.University
{
    public class UniversityPostModel : AccountPostModel
    {
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "EstablishedYear is required.")]
        public string EstablishedYear { get; set; } = string.Empty;
        [Required(ErrorMessage = "Type is required.")]
        public UniversityType Type { get; set; }
    }
}
