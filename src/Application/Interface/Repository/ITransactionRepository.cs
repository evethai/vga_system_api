using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Transaction;

namespace Application.Interface.Repository
{
    public interface ITransactionRepository :IGenericRepository<Transaction>
    {
        (Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy) BuildFilterAndOrderBy(TransactionSearchModel searchModel);
        Task<Transaction> CreateTransactionWhenUsingGold(TransactionType transactionType, TransactionPostModel transactionModel);
    }
}
