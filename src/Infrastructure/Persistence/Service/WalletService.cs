using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Infrastructure.Persistence.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSService _payOSService;
        private readonly IConfiguration _configuration;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, PayOSService payOSService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
            _configuration = configuration;
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
        public async Task<ResponseModel> RequestTopUpWalletWithPayOsAsync(Guid accountId,float amount, PayOSUrl url)
        {

            var pointValue = _configuration["Conversion_factor:Point"];
            int point = 0;

            if (int.TryParse(pointValue, out int pointExited))
            {
                point = pointExited;
            }
            else
            {
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Data point of config file is not correct !"
                };
            }
            var exitWallet = await _unitOfWork.WalletRepository.
                SingleOrDefaultAsync(predicate: s => s.AccountId.Equals(accountId)); if (exitWallet == null) { throw new Exception("Wallet is not found"); }
                //var points= amount/1000;
                var points= amount/ point;
            TransactionPostModel transaction = new TransactionPostModel(exitWallet.Id, (int)points);
            var trans = await _unitOfWork.TransactionRepository.CreateTransactionPayOS(TransactionType.Reject, transaction);
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
        public async Task<string> ConfirmWebhook(string webhookUrl)
        {
            return await _payOSService.ConfirmWebhook(webhookUrl);
        }
        public async Task<ResponseModel> HandleWebhook(WebhookType webhookBody)
        {
            WebhookData webhookData = _payOSService.VerifyPaymentWebhookData(webhookBody);           
            var rs = await _unitOfWork.TransactionRepository.CheckPayOsReturn(webhookBody.data.orderCode, webhookBody.desc);
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Deposit To Wallet Success",
                Data = webhookData +"--------------" + rs
            };
        }
    }
}
