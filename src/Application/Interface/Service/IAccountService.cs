using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IAccountService 
    {
        Task<LoginResponseModel> Login(LoginRequestModel loginRequest);
        Task<LoginResponseModel> LoginByZalo(ZaloLoginModel model);
        Task<ResponseModel> Logout(Guid AccountId);
        Task<ResponseModel> CreateRefreshToken(RefreshTokenRequestModel model);
        Task<ResponseModel> UpdateStatusAccount(Guid accountId, AccountStatus status);
        Task<ResponseModel> ResetPassword(Guid accountId, string password);
    }
}
