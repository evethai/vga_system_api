using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Transaction;
using Domain.Model.Wallet;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly VgaDbContext _context;
        public TransactionRepository(VgaDbContext context) : base(context)
        {
            _context = context;
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

        public async Task<Transaction> CreateTransactionWhenUsingGold(TransactionType transactionType, TransactionPostModel transactionModel)
        {
            if (transactionModel == null)
            {
                throw new KeyNotFoundException("Null data");
            }
            Transaction transaction = null;
            switch (transactionType)
            {
                case TransactionType.Transferring:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = "Bạn đã chuyển " + transactionModel.GoldAmount + " Gold",
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                    break;
                case TransactionType.Receiving:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = "Bạn đã nhận " + transactionModel.GoldAmount + " Gold",
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                    break;
                case TransactionType.Using:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = "Bạn đã sử dụng " + transactionModel.GoldAmount + " Gold vào bài Test",
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                    break;
                default:
                    break;
            }
            var result = await _context.Transaction.AddAsync(transaction);
            _context.SaveChanges();
            return transaction;
        }
        public async Task<Boolean> UpdateWalletUsingByTestAsync(Guid AccountId, int GoldUsing)
        {
            var exitAccount = _context.Account.Where(s => s.Id.Equals(AccountId)).FirstOrDefault();
            if (exitAccount == null)
            {
                throw new InvalidOperationException("Account Id is not found");
            }
            var exitWallet = _context.Wallet.Where(s => s.AccountId.Equals(exitAccount.Id)).FirstOrDefault() 
                ?? throw new Exception("Wallet is not found");
            if(exitWallet.GoldBalance < GoldUsing)
            {
                throw new Exception("User is not enought gold");
            }
            exitAccount.Wallet.GoldBalance -= GoldUsing;
            TransactionPostModel transaction = new TransactionPostModel(exitAccount.Wallet.Id, GoldUsing);
            await CreateTransactionWhenUsingGold(TransactionType.Using, transaction);
            _context.Wallet.Update(exitWallet);
            _context.SaveChanges();
            return true;
        }
    }
}
