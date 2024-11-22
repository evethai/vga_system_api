using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interface;
using Application.Interface.Repository;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Response;
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

            if (searchModel.account_id.HasValue)
            {
                filter = filter.And(p => p.Wallet.AccountId == searchModel.account_id.Value);
            }

            if (searchModel.UniversityId.HasValue)
            {
                filter = filter.And(p => p.Wallet.Account.University.Id.Equals(searchModel.UniversityId));
            }

            if (searchModel.transaction_type.HasValue)
            {
                filter = filter.And(p => p.TransactionType == searchModel.transaction_type.Value);
            }

            if (searchModel.sort_by_gold_amount.HasValue && searchModel.sort_by_gold_amount.Value)
            {
                orderBy = query => searchModel.descending.HasValue && searchModel.descending.Value
                        ? query.OrderByDescending(p => p.GoldAmount)
                        : query.OrderBy(p => p.GoldAmount);
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
                case TransactionType.Request:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = "Bạn yêu cầu rút " + transactionModel.GoldAmount + " Gold",
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                    break;
                case TransactionType.Recharge:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = "Bạn đã nạp " + transactionModel.GoldAmount + " Gold",
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow,
                    };
                    break;
                default:
                    break;
            }
            await _context.Transaction.AddAsync(transaction);
            _context.SaveChanges();
            return transaction;
        }
        public async Task<Boolean> UpdateWalletUsingByTestAsync(Guid AccountId, int GoldUsing)
        {
            var exitAccount = _context.Account.Where(s => s.Id.Equals(AccountId)).FirstOrDefault()
                ?? throw new InvalidOperationException("Account Id is not found");   
            var exitWallet = _context.Wallet.Where(s => s.AccountId.Equals(exitAccount.Id)).FirstOrDefault()
                ?? throw new Exception("Wallet is not found");
            if (exitWallet.GoldBalance < GoldUsing)
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
        public async Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel, int gold)
        {
            var walletTransferring = _context.Wallet.Where(s => s.AccountId.Equals(putModel.account_id_tranferring)).FirstOrDefault() 
                ?? throw new InvalidOperationException("Account Id Tranffering is not found"); ;
            var walletReceiving = _context.Wallet.Where(s => s.AccountId.Equals(putModel.account_id_receiving)).FirstOrDefault()
               ?? throw new InvalidOperationException("Account Id Receiving is not found");
            var RoleTransferring = _context.Account.Where(s => s.Id.Equals(walletTransferring.AccountId)).FirstOrDefault() ?? throw new Exception("Not found Account");
            if (RoleTransferring.Role == RoleEnum.Admin)
            {
                TransactionPostModel transaction_Transferring =
               new TransactionPostModel(walletTransferring.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring);

                walletReceiving.GoldBalance = walletReceiving.GoldBalance + gold;
                TransactionPostModel transaction_Receiving =
                  new TransactionPostModel(walletReceiving.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction_Receiving);
                _context.Wallet.Update(walletReceiving);
                _context.SaveChanges();
                return new ResponseModel
                {
                    Message = "Admin transferring is Successfully",
                    IsSuccess = true,
                    Data = transaction_Receiving,
                };
            }
            else
            {
                if (walletTransferring.GoldBalance < gold)
                {
                    throw new Exception("Not enought gold to transferring");
                }
                walletTransferring.GoldBalance -= gold;
                TransactionPostModel transaction_Transferring =
                new TransactionPostModel(walletTransferring.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring);
                _context.Wallet.Update(walletTransferring);
                //-------------------------------------------
                walletReceiving.GoldBalance += gold;
                TransactionPostModel transaction_Receiving =
                  new TransactionPostModel(walletReceiving.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction_Receiving);
                _context.Wallet.Update(walletReceiving);
                _context.SaveChanges();
                return new ResponseModel
                {
                    Message = "Update wallet is Successfully",
                    IsSuccess = true,
                    Data = transaction_Receiving,
                };
            }
        }
        public async Task<ResponseModel> CreateTransactionRequest(Guid WalletId, int gold)
        {
            var existedWallet = _context.Wallet
                .Where(s => s.Id.Equals(WalletId))
                .FirstOrDefault() ?? throw new NotExistsException();

            TransactionPostModel transaction_request =
                 new TransactionPostModel(existedWallet.Id, gold);
            await CreateTransactionWhenUsingGold(TransactionType.Request, transaction_request);

            //hold point to process request
            existedWallet.GoldBalance -= gold;
            _context.Wallet.Update(existedWallet);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Create Request Transaction is Successfully",
                IsSuccess = true,
                Data = transaction_request,
            };
        }
        public async Task<ResponseModel> ProcessWithdrawRequest(Guid transactionId, TransactionType type)
        {
            var existedTransaction = _context.Transaction
                .Where(t => t.Id.Equals(transactionId))
                .FirstOrDefault() ?? throw new NotExistsException();

            var wallet = _context.Wallet
                .Where(w => w.Id.Equals(existedTransaction.WalletId))
                .FirstOrDefault() ?? throw new NotExistsException();

            Notification notiPostModel = new Notification
            {
                AccountId = wallet.AccountId,
                CreatedAt = DateTime.UtcNow,
                Status = Domain.Enum.NotiStatus.Unread
            };

            switch (type)
            {
                case TransactionType.Withdraw:
                    //update transaction type and description
                    existedTransaction.TransactionType = TransactionType.Withdraw;
                    existedTransaction.Description = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã xử lý thành công";
                    existedTransaction.TransactionDateTime = DateTime.UtcNow;

                    //create notification 
                    notiPostModel.Title = "Yêu cầu rút điểm đã xử lý thành công";
                    notiPostModel.Message = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm được xử lý thành công vào ngày {DateTime.UtcNow}";
                    break;
                case TransactionType.Reject:
                    //update transaction type and description
                    existedTransaction.TransactionType = TransactionType.Reject;
                    existedTransaction.Description = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã bị từ chối";
                    existedTransaction.TransactionDateTime = DateTime.UtcNow;

                    //update wallet
                    wallet.GoldBalance += existedTransaction.GoldAmount;
                    _context.Wallet.Update(wallet);

                    //create notification 
                    notiPostModel.Title = "Yêu cầu rút điểm đã bị từ chối";
                    notiPostModel.Message = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã bị từ chối vào ngày {DateTime.UtcNow}";
                    break;
                default:
                    throw new Exception("Appcepted type is Withdraw or Reject only");
            }
            _context.Transaction.Update(existedTransaction);
            await _context.Notification.AddAsync(notiPostModel);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Process withdraw request is successfully",
                IsSuccess = true,
                Data = existedTransaction
            };
        }


    }
}
