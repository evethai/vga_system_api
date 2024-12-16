using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;

namespace Application.Interface.Repository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Guid> CreateAccountAndWallet(RegisterAccountModel registerAccount, RoleEnum _role);
        Task<bool> checkPhoneAndMail(Guid id,string Mail, string Phone);
    }
}
