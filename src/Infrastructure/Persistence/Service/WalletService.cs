using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Region;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<Wallet> GetWalletByIdAsync(Guid Id)
        {
            var wallet = await _unitOfWork.WalletRepository.SingleOrDefaultAsync
                (predicate: c => c.AccountId.Equals(Id));
            return _mapper.Map<Wallet>(wallet);
        }

        public async Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(Guid WalletHigchoolId, int goldDistribution)
        {
            var walletTransferring = await _unitOfWork.WalletRepository.GetByIdGuidAsync(WalletHigchoolId);
            var receivingWallets = await _unitOfWork.WalletRepository.GetInforStudentHasWalletReceiving(walletTransferring.AccountId);
            var totalgoldDistribution = goldDistribution * receivingWallets.Count();           
            if (walletTransferring.GoldBalance < totalgoldDistribution)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Distribution gold is fail when Gold not enough",
                };
            }
            walletTransferring.GoldBalance = walletTransferring.GoldBalance - totalgoldDistribution;
            TransactionPostModel transaction_Transferring =
               new TransactionPostModel(walletTransferring.Id, totalgoldDistribution);
            await _unitOfWork.TransactionRepository
                .CreateTransactionWhenUsingGold(TransactionType.Transferring, transaction_Transferring);

            await _unitOfWork.WalletRepository.UpdateAsync(walletTransferring);
            foreach (var receivingWallet in receivingWallets)
            {
                receivingWallet.GoldBalance = receivingWallet.GoldBalance + goldDistribution;
                TransactionPostModel transaction = new TransactionPostModel(receivingWallet.Id, goldDistribution);
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
        public async Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel , int gold)
        {
            var TransactionInfor = await _unitOfWork.TransactionRepository.UpdateWalletByTransferringAndReceivingAsync(putModel, gold);
            return TransactionInfor;
        }
        public async Task<ResponseModel> UpdateWalletUsingByTestAsync(Guid AccountId, int goldUsingTest)
        {
            var TransactionInfor =  await _unitOfWork.TransactionRepository.UpdateWalletUsingByTestAsync(AccountId, goldUsingTest);
            return new ResponseModel
            {
                Message = "Wallet using by test Successfully",
                IsSuccess = true,
                Data = TransactionInfor
            };
        }      
    }
}
