using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<LoginResponseModel> Login(LoginRequestModel loginRequest)
        //{
        //    Expression<Func<Account, bool>> searchFilter = x =>
        //    x.Email.Equals(loginRequest.Email) &&
        //    x.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));

        //    Account account = await _unitOfWork.AccountRepository
        //        .SingleOrDefaultAsync(predicate: searchFilter);
        //    if (account == null)
        //    {
        //        return null;
        //    }
        //    RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role);
        //}

    }
}
