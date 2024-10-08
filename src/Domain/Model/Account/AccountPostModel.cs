using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Account
{
    public class AccountPostModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty!;
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?[0-9]\d{1,10}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; } = string.Empty;
    }
}
