using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Account
{
    public class RefreshTokenRequestModel
    {
        public Guid AccountId { get; set; }
        public string RefreshToken { get; set; }
    }
}
