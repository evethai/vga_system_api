using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Constants;
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
using static Application.Common.Constants.NotificationConstant;

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
            if (searchModel.transaction_type.HasValue)
            {
                filter = filter.And(p => p.TransactionType == searchModel.transaction_type.Value);
            }
            if (searchModel.account_name != null)
            {
                filter = filter.And(p => p.Wallet.Account.Name == searchModel.account_name);
            }
            if (searchModel.sort_by_gold_amount.HasValue && searchModel.sort_by_gold_amount.Value)
            {
                orderBy = query => searchModel.descending.HasValue && searchModel.descending.Value
                        ? query.OrderByDescending(p => p.GoldAmount)
                        : query.OrderBy(p => p.GoldAmount);
            }
            if (searchModel.sort_by_datetime.HasValue && searchModel.sort_by_datetime.Value)
            {
                orderBy = query => searchModel.descending.HasValue && searchModel.descending.Value
                        ? query.OrderByDescending(p => p.TransactionDateTime)
                        : query.OrderBy(p => p.TransactionDateTime);
            }
            if (searchModel.transaction_date_time.HasValue)
            {
                filter = filter.And(p => p.TransactionDateTime.Date == searchModel.transaction_date_time.Value.Date);
            }
            return (filter, orderBy);
        }

        public async Task<Transaction> CreateTransactionWhenUsingGold(TransactionType transactionType, TransactionPostModel transactionModel, string message)
        {
            var accoount = _context.Wallet.Where(a => a.Id.Equals(transactionModel.WalletId)).AsNoTracking().FirstOrDefault();
            if (transactionModel == null || accoount == null)
            {
                throw new KeyNotFoundException("Không tìm thấy tài khoản");
            }
            Notification notiPostModel = new Notification
            {
                AccountId = accoount.AccountId,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread,                
            };
            Transaction transaction = null;
            switch (transactionType)
            {
                case TransactionType.Transferring:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = message,
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };
                    notiPostModel.Message = "Bạn đã chuyển " + transactionModel.GoldAmount + " điểm";
                    notiPostModel.Title = NotificationConstant.Title.UpdateGold;
                    break;
                case TransactionType.Receiving:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = message,
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };
                    notiPostModel.Message ="Bạn đã nhận " + transactionModel.GoldAmount + " điểm";
                    notiPostModel.Title = NotificationConstant.Title.UpdateGold;
                    break;
                case TransactionType.Using:
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = transactionModel.WalletId,
                        TransactionType = transactionType,
                        Description = message,
                        GoldAmount = transactionModel.GoldAmount,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };
                    notiPostModel.Message = "Bạn đã sử dụng " + transactionModel.GoldAmount + " điểm vào bài Test";
                    notiPostModel.Title = NotificationConstant.Title.UpdateGold;
                    break;
                default:
                    break;
            }
            _context.Notification.Add(notiPostModel);
            await _context.Transaction.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Boolean> UpdateWalletUsingByTestAsync(Guid AccountId, int GoldUsing)
        {
            var exitAccount = _context.Account.Where(s => s.Id.Equals(AccountId)).FirstOrDefault()
                ?? throw new InvalidOperationException("Không tìm thấy Id tài khoản");
            var exitWallet = _context.Wallet.Where(s => s.AccountId.Equals(exitAccount.Id)).FirstOrDefault()
                ?? throw new Exception("Không tìm thấy ví");
            if (exitWallet.GoldBalance < GoldUsing)
            {
                throw new Exception("Người dùng không đủ điểm");
            }
            exitAccount.Wallet.GoldBalance -= GoldUsing;
            string mess = "Bạn đã sử dụng " + GoldUsing + " điểm vào bài Test";
            TransactionPostModel transaction = new TransactionPostModel(exitAccount.Wallet.Id, GoldUsing);
            await CreateTransactionWhenUsingGold(TransactionType.Using, transaction, mess);
            _context.Wallet.Update(exitWallet);
            _context.SaveChanges();
            return true;
        }
        public async Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel, int gold)
        {
            var walletTransferring = _context.Wallet.Where(s => s.AccountId.Equals(putModel.account_id_tranferring)).FirstOrDefault()
                ?? throw new InvalidOperationException("Không tìm thấy ID tài khoản đang chuyển"); ;
            var walletReceiving = _context.Wallet.Where(s => s.AccountId.Equals(putModel.account_id_receiving)).FirstOrDefault()
               ?? throw new InvalidOperationException("Không tìm thấy ID tài khoản Nhận");
            var RoleTransferring = _context.Account.Where(s => s.Id.Equals(walletTransferring.AccountId)).FirstOrDefault() ?? throw new Exception("Không tìm thấy Tài khoản");
            var RoleReceiving = _context.Account.Where(s => s.Id.Equals(walletReceiving.AccountId)).FirstOrDefault() ?? throw new Exception("Không tìm thấy Tài khoản");
            var messReceiving = "Bạn đã nhận " + gold + " điểm từ " + RoleTransferring.Name;
            var messTransferring = "Bạn đã chuyển " + gold + " điểm đến " + RoleReceiving.Name;
            if (RoleTransferring.Role == RoleEnum.Admin)
            {
                TransactionPostModel transaction_Transferring =
               new TransactionPostModel(walletTransferring.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring,messTransferring);

                walletReceiving.GoldBalance = walletReceiving.GoldBalance + gold;
                TransactionPostModel transaction_Receiving =
                  new TransactionPostModel(walletReceiving.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction_Receiving, messReceiving);
                _context.Wallet.Update(walletReceiving);
                _context.SaveChanges();
                return new ResponseModel
                {
                    Message = "Quản trị viên chuyển điểm thành công",
                    IsSuccess = true,
                    Data = transaction_Receiving,
                };
            }
            else
            {
                if (walletTransferring.GoldBalance < gold)
                {
                    throw new Exception("Không đủ điểm để chuyển");
                }
                walletTransferring.GoldBalance -= gold;
                TransactionPostModel transaction_Transferring =
                new TransactionPostModel(walletTransferring.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring, messTransferring);
                _context.Wallet.Update(walletTransferring);
                //-------------------------------------------
                walletReceiving.GoldBalance += gold;
                TransactionPostModel transaction_Receiving =
                  new TransactionPostModel(walletReceiving.Id, gold);
                await CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction_Receiving, messReceiving);
                _context.Wallet.Update(walletReceiving);
                _context.SaveChanges();
                return new ResponseModel
                {
                    Message = "Cập nhật ví thành công",
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

            //TransactionPostModel transaction_request =
            //     new TransactionPostModel(existedWallet.Id, gold);
            //await CreateTransactionWhenUsingGold(TransactionType.Request, transaction_request);

            var account = _context.Wallet.Where(a => a.Id.Equals(WalletId)).AsNoTracking().FirstOrDefault() 
                ?? throw new KeyNotFoundException("Không tìm thấy tài khoản");

            Transaction transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = WalletId,
                TransactionType = TransactionType.Request,
                Description = "Bạn yêu cầu rút " + gold + " điểm",
                GoldAmount = gold,
                TransactionDateTime = DateTime.UtcNow.AddHours(7),
            };
            await _context.Transaction.AddAsync(transaction);

            //hold point to process request
            //existedWallet.GoldBalance -= gold;
            //_context.Wallet.Update(existedWallet);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Tạo yêu cầu giao dịch thành công",
                IsSuccess = true,
                Data = transaction,
            };
        }
        public async Task<ResponseModel> ProcessWithdrawRequest(Guid transactionId, TransactionProcessRequestModel model)
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
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread
            };

            switch (model.type)
            {
                case TransactionType.Withdraw:
                    if (existedTransaction.GoldAmount > wallet.GoldBalance)
                        throw new Exception("Không đủ điểm để xử lý");
                    //update transaction type and description
                    existedTransaction.TransactionType = TransactionType.Withdraw;
                    existedTransaction.Description = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã xử lý thành công";
                    existedTransaction.TransactionDateTime = DateTime.UtcNow.AddHours(7);
                    existedTransaction.Image = model.Image;

                    //update wallet
                    wallet.GoldBalance -= existedTransaction.GoldAmount;
                    _context.Wallet.Update(wallet);

                    //create notification 
                    notiPostModel.Title = NotificationConstant.Title.Withdraw;
                    notiPostModel.Message = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm được xử lý thành công vào ngày {DateTime.UtcNow.AddHours(7)}";
                    break;
                case TransactionType.Reject:
                    //update transaction type and description
                    existedTransaction.TransactionType = TransactionType.Reject;
                    existedTransaction.Description = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã bị từ chối";
                    existedTransaction.TransactionDateTime = DateTime.UtcNow.AddHours(7);

                    //update wallet
                    //wallet.GoldBalance += existedTransaction.GoldAmount;
                    //_context.Wallet.Update(wallet);

                    //create notification 
                    notiPostModel.Title = NotificationConstant.Title.Reject;
                    notiPostModel.Message = $"Yêu cầu rút {existedTransaction.GoldAmount} điểm đã bị từ chối vào ngày {DateTime.UtcNow.AddHours(7)}";
                    break;
                default:
                    throw new Exception("Loại được chấp nhận chỉ là Rút hoặc Từ chối");
            }
            _context.Transaction.Update(existedTransaction);
            await _context.Notification.AddAsync(notiPostModel);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Quá trình yêu cầu rút tiền đã thành công",
                IsSuccess = true,
                Data = existedTransaction
            };
        }
        public async Task<List<Wallet>> GetInforStudentHasWalletReceiving(Guid id, int years)
        {
            var idHighschool = _context.HighSchool
                .Where(a => a.AccountId.Equals(id))
                .FirstOrDefault() ?? throw new Exception("Không tìm thấy ID tài khoản");
            var listStudent = await _context.Student
                .Where(a => a.HighSchoolId.Equals(idHighschool.Id) && a.SchoolYears.Equals(years)).AsNoTracking()
                .ToListAsync();
            if (listStudent.Count == 0)
            {
                throw new Exception("Không có danh sách sinh viên");
            }
            var wallets = await _context.Wallet
                .Where(w => listStudent.Select(s => s.AccountId).Contains(w.AccountId)).AsNoTracking()
                .ToListAsync();
            var responseWallets = wallets.Select(w => new Wallet
            {
                Id = w.Id,
                AccountId = w.AccountId,
                GoldBalance = w.GoldBalance,
            }).ToList();
            return responseWallets;
        }
        public async Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(TransactionPutWalletModel model)
        {
            var walletTransferring = _context.Wallet.Where(a => a.AccountId.Equals(model.AccountId)).FirstOrDefault();
            if (walletTransferring == null)
            {
                throw new Exception("Không tìm thấy ID tài khoản");
            }
            var receivingWallets = await GetInforStudentHasWalletReceiving(model.AccountId, model.Years);
            var totalgoldDistribution = model.Gold * receivingWallets.Count();
            var mess = "Bạn đã phân phối " + totalgoldDistribution + " điểm cho học sinh vào năm " + model.Years;          
            if (walletTransferring.GoldBalance < totalgoldDistribution)
            {
                throw new Exception("Điểm phân phối bị lỗi khi điểm không đủ");
            }
            walletTransferring.GoldBalance = walletTransferring.GoldBalance - totalgoldDistribution;
            TransactionPostModel transaction_Transferring =
               new TransactionPostModel(walletTransferring.Id, totalgoldDistribution);     
            var trans = await CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring, mess);           
            foreach (var receivingWallet in receivingWallets)
            {
                var messStudent = "Bạn đã nhận " + model.Gold + " điểm từ trường phân phối vào năm " + model.Years;
                receivingWallet.GoldBalance = receivingWallet.GoldBalance + model.Gold;
                TransactionPostModel transaction = new TransactionPostModel(receivingWallet.Id, model.Gold);
                var receiving = await
                    CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction, messStudent);
                _context.Wallet.Update(receivingWallet);
            }
            await _context.SaveChangesAsync();
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Phân phối điểm đã thành công",
                Data = walletTransferring
            };
        }
        public async Task<bool> CheckPayOsReturn(long OrderCode, string desc)
        {
            var checkTrans = _context.Transaction.Where(x => x.Code != null && x.Code.Equals(OrderCode.ToString())).FirstOrDefault();
            if (checkTrans == null)
            {
                throw new Exception("Không tồn tại trong giao dịch");
            }
            if (desc == "success")
            {
                var updateWallet = _context.Wallet.Where(a => a.Id.Equals(checkTrans.WalletId)).FirstOrDefault();
                if (updateWallet == null)
                {
                    throw new Exception("Không tìm thấy ví");
                }
                checkTrans.Description = "Bạn đã nạp " + checkTrans.GoldAmount + " điểm thành công";
                checkTrans.TransactionType = TransactionType.Recharge;
                updateWallet.GoldBalance += checkTrans.GoldAmount;
                Notification notiPostModel = new Notification
                {
                    AccountId = updateWallet.AccountId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    Status = Domain.Enum.NotiStatus.Unread,
                    Message = "Bạn đã nạp " + checkTrans.GoldAmount + " điểm thành công",
                    Title = NotificationConstant.Messages.UpdateGold
                };
                _context.Notification.Add(notiPostModel);
                _context.Transaction.Update(checkTrans);
                _context.Wallet.Update(updateWallet);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<Transaction> CreateTransactionPayOS(TransactionType transactionType, TransactionPostModel transactionModel)
        {
            Transaction transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = transactionModel.WalletId,
                TransactionType = transactionType,
                Description ="Bạn đã hủy yêu cầu thanh toán " + transactionModel.GoldAmount + " điểm",
                GoldAmount = transactionModel.GoldAmount,
                TransactionDateTime = DateTime.UtcNow.AddHours(7),
            };
            await _context.Transaction.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
