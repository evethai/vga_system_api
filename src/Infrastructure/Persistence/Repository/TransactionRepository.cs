using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Transaction;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(VgaDbContext context) : base(context)
        {
        }
        public (Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy) BuildFilterAndOrderBy(TransactionSearchModel searchModel)
        {
            Expression<Func<Transaction, bool>> filter = p => true;
            Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.description))
            {
                filter = filter.And(p => p.Description.Contains(searchModel.description));
            }
           
            if (searchModel.wallet_id.HasValue)
            {
                filter = filter.And(p => p.WalletId == searchModel.wallet_id.Value);
            }

            if (searchModel.transaction_type.HasValue)
            {
                filter = filter.And(p => p.TransactionType == searchModel.transaction_type.Value);
            }

            if (searchModel.sort_by_gold_amount.HasValue && searchModel.sort_by_gold_amount.Value)
            {
                orderBy = query =>
                {
                    return searchModel.descending.HasValue && searchModel.descending.Value
                        ? query.OrderByDescending(p => p.GoldAmount)
                        : query.OrderBy(p => p.GoldAmount);
                };
            }

            if (searchModel.transaction_date_time.HasValue)
            {
                filter = filter.And(p => p.TransactionDateTime.Date == searchModel.transaction_date_time.Value.Date);
            }
            return (filter, orderBy);
        }
    }
}
