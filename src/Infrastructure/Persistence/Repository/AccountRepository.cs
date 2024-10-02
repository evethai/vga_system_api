using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Account;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(VgaDbContext context) : base(context)
        {
        }

    }
}
