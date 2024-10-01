using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Account
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public Guid Id { get; set; }
        public RoleEnum Role { get; set; }
        public AccountStatus Status { get; set; }
        public string? BrandPicUrl { get; set; }
    }
}
