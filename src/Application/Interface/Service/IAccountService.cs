using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Account;

namespace Application.Interface.Service
{
    public interface IAccountService 
    {
        Task<LoginResponseModel> Login(LoginRequestModel loginRequest);
    }
}
