using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var check = await _unitOfWork.TransactionRepository.UpdateWalletUsingGoldDistributionAsync(model);
            return check;
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
                SingleOrDefaultAsync(predicate: s => s.AccountId.Equals(accountId)); if (exitWallet == null) { throw new Exception("Wallet is not found"); }
            TransactionPostModel transaction = new TransactionPostModel(exitWallet.Id, (int)amount);
            var trans = await _unitOfWork.TransactionRepository.CreateTransactionWhenUsingGold(TransactionType.Recharge, transaction);
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
            trans.Code = orderCode.ToString();
            CreatePaymentResult paymentUrl = await _payOSService.CreatePaymentLink(payOSModel);
            if (paymentUrl != null)
            {
                await _unitOfWork.SaveChangesAsync();
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
        public async Task<string> ConfirmWebhook(string webhookUrl)
        {
            return await _payOSService.ConfirmWebhook(webhookUrl);
        }

        public async Task<ResponseModel> HandleWebhook(WebhookType webhookBody)
        {
            WebhookData webhookData = _payOSService.VerifyPaymentWebhookData(webhookBody);
            var checkTrans = await _unitOfWork.TransactionRepository.SingleOrDefaultAsync(predicate: a => a.Code.Equals(webhookData.orderCode));
            if (checkTrans == null)
            {
                throw new Exception("Not exit Transaction");
            }
            if (webhookData.code != "00")
            {
                await _unitOfWork.TransactionRepository.DeleteAsync(checkTrans);
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Giao dịch đã được xóa"
                };

            }
            var updateWallet = await _unitOfWork.WalletRepository.GetByIdGuidAsync(checkTrans.WalletId);
            if (updateWallet == null)
            {
                throw new Exception("Wallet is not found");
            }
            checkTrans.Description = "Bạn đã nạp " + checkTrans.GoldAmount + " Gold";
            updateWallet.GoldBalance += checkTrans.GoldAmount;
            await _unitOfWork.TransactionRepository.UpdateAsync(checkTrans);
            await _unitOfWork.WalletRepository.UpdateAsync(updateWallet);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Deposit To Wallet Success"
            };
        }
    }
}
