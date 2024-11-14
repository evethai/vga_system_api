using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Wallet;

namespace Application.Interface.Repository
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<List<WalletModel>> GetInforStudentHasWalletReceiving(Guid id, int years);
    }
}
