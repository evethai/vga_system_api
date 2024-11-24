using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;

namespace Application.Interface.Repository
{
    public interface ITransactionRepository :IGenericRepository<Transaction>
    {
        (Expression<Func<Transaction, bool>> filter, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy) BuildFilterAndOrderBy(TransactionSearchModel searchModel);
        Task<Transaction> CreateTransactionWhenUsingGold(TransactionType transactionType, TransactionPostModel transactionModel);
        Task<Boolean> UpdateWalletUsingByTestAsync(Guid AccountId, int GoldUsing);
        Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel, int gold);
        Task<ResponseModel> CreateTransactionRequest(Guid WalletId, int gold);
        Task<ResponseModel> ProcessWithdrawRequest(Guid transactionId, TransactionType type);
        Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(TransactionPutWalletModel model);
        Task<bool> CheckPayOsReturn(long OrderCode, string desc);
    }
}
