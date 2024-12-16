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

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; } = string.Empty;
        public RegisterAccountModel(string name,string email, string password, string phone, string? image_Url)
        {
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
            Image_Url = image_Url;
        }
        public RegisterAccountModel(string name, string email, string password, string phone)
        {
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
        }
    }
}
