using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Model.Account
{
    public class RegisterAccountModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty;


        public RegisterAccountModel(string email, string password, string phone)
        {
            Email = email;
            Password = password;
            Phone = phone;
        }
    }
}
