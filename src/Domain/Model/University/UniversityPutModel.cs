using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Account;

namespace Domain.Model.University
{
    public class UniversityPutModel: AccountPostModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;
    }
}
