using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using Application.Library;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Region;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;

namespace Infrastructure.Persistence.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSService _payOSService;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, PayOSService payOSService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
        }
        public async Task<ResponseWalletModel> GetAllWallet()
        {
            var wallets = await _unitOfWork.WalletRepository.GetAllAsync();
            var result = _mapper.Map<List<WalletModel>>(wallets);
            return new ResponseWalletModel
            {
                wallets = result
            };
        }

        public async Task<WalletModel> GetWalletByIdAsync(Guid AccountId)
        {
            var wallet = await _unitOfWork.WalletRepository.SingleOrDefaultAsync
                (predicate: c => c.AccountId.Equals(AccountId)) ?? throw new Exception("Account Id is not found");
            return _mapper.Map<WalletModel>(wallet);
        }

        public async Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(TransactionPutWalletModel model)
        {
            var walletTransferring = await _unitOfWork.WalletRepository.
                SingleOrDefaultAsync(predicate: a => a.AccountId.Equals(model.AccountId)) ?? throw new Exception("Id Account is not found");
            var receivingWallets = await _unitOfWork.WalletRepository.GetInforStudentHasWalletReceiving(walletTransferring.AccountId, model.Years);
            var totalgoldDistribution = model.Gold * receivingWallets.Count();
            if (walletTransferring.GoldBalance < totalgoldDistribution)
            {
                throw new Exception("Distribution gold is fail when Gold not enough");
            }
            walletTransferring.GoldBalance = walletTransferring.GoldBalance - totalgoldDistribution;
            TransactionPostModel transaction_Transferring =
               new TransactionPostModel(walletTransferring.Id, totalgoldDistribution);
            await _unitOfWork.TransactionRepository
                .CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring);
            await _unitOfWork.WalletRepository.UpdateAsync(walletTransferring);
            foreach (var receivingWallet in receivingWallets)
            {
                receivingWallet.GoldBalance = receivingWallet.GoldBalance + model.Gold;
                TransactionPostModel transaction = new TransactionPostModel(receivingWallet.Id, model.Gold);
                await _unitOfWork.TransactionRepository.
                    CreateTransactionWhenUsingGold(TransactionType.Receiving, transaction);
                var result = _mapper.Map<Wallet>(receivingWallet);
                await _unitOfWork.WalletRepository.UpdateAsync(result);
            }
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Distribution gold is Successfully",
                Data = walletTransferring
            };
        }
        public async Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel, int gold)
        {
            var TransactionInfor = await _unitOfWork.TransactionRepository.UpdateWalletByTransferringAndReceivingAsync(putModel, gold);
            return TransactionInfor;
        }
        public async Task<ResponseModel> UpdateWalletUsingByTestAsync(Guid AccountId, int goldUsingTest)
        {
            if (goldUsingTest <= 0)
            {
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "This free.",
                };
            }

            var TransactionInfor = await _unitOfWork.TransactionRepository.UpdateWalletUsingByTestAsync(AccountId, goldUsingTest);
            if (TransactionInfor == false)
            {
                return new ResponseModel
                {
                    IsSuccess = false
                };
            }
            return new ResponseModel
            {
                Message = "Wallet using by test Successfully",
                IsSuccess = true,
                Data = TransactionInfor
            };
        }

        public async Task<ResponseModel> RequestTopUpWalletWithPayOsAsync([FromQuery] Guid accountId, [FromQuery] float amount, PayOSUrl url)
        {
            var exitWallet = await _unitOfWork.WalletRepository.
                SingleOrDefaultAsync(predicate: s => s.AccountId.Equals(accountId)) ?? throw new Exception("Wallet is not found");
            TransactionPostModel transaction = new TransactionPostModel(exitWallet.Id, (int)amount);
            var trans = await _unitOfWork.TransactionRepository.CreateTransactionWhenUsingGold(TransactionType.Recharge, transaction);
            await _unitOfWork.SaveChangesAsync();
            var orderId = trans.Id;
            var items = new List<ItemData>
            {
                new ItemData("NẠP TIỀN VÀO HỆ THỐNG", 1, (int)amount)
            };
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));
            var payOSModel = new PaymentData(
                orderCode: orderCode,
                amount: (int)amount,
                description: "Thanh toan don hang",
                items: items,
                returnUrl: url.ReturnUrl,
                cancelUrl: url.CancelUrl
            );
            var paymentUrl = await _payOSService.CreatePaymentLink(payOSModel);
            if (paymentUrl != null)
            {
                return new ResponseModel
                {
                    Message = "Create PayOs Is Successfully",
                    IsSuccess = true,
                    Data = paymentUrl.checkoutUrl
                };
            }
            return new ResponseModel
            {
                IsSuccess = false,
                Message = "Create URL failed"
            };
        }
        public async Task<ResponseModel> RequestDepositToWalletWithPayOs(Guid transactionId, string status)
        {
            var trans = await _unitOfWork.TransactionRepository.GetByIdGuidAsync(transactionId) ?? throw new Exception("Transaction is Not Found");
            if (status.ToUpper() == "PAID")
            {
                var updateWallet = await _unitOfWork.WalletRepository.GetByIdGuidAsync(trans.WalletId);
                if (updateWallet == null)
                {
                    throw new Exception("Wallet is not found");
                }
                trans.Description = "Bạn đã nạp " + trans.GoldAmount + " Gold";
                updateWallet.GoldBalance += trans.GoldAmount;
                await _unitOfWork.TransactionRepository.UpdateAsync(trans);
                await _unitOfWork.WalletRepository.UpdateAsync(updateWallet);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Deposit To Wallet Success",
                    Data = updateWallet
                };
            }
            else
            {
                await _unitOfWork.TransactionRepository.DeleteAsync(trans);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Deposit To Wallet Success"
                };
            }
        }
    }
}
