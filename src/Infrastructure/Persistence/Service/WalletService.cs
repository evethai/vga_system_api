using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Wallet;

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

        public async Task<Wallet> GetWalletByIdAsync(Guid Id)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdGuidAsync(Id);
            return _mapper.Map<Wallet>(wallet);
        }

        public async Task<ResponseModel> UpdateUsingGoldWalletAsync(WalletPutModel putModel)
        {
            var walletReceiving = await _unitOfWork.WalletRepository.GetByIdGuidAsync(putModel.Receiving.Id);
            walletReceiving.GoldBalance = walletReceiving.GoldBalance + putModel.goldTransaction;
            var transactionReceiving =_mapper.Map<Transaction>(walletReceiving.Transactions);
            transactionReceiving = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletReceiving.Id,
                Description = "Bạn nhận được " + putModel.goldTransaction + " Gold",
                GoldAmount = putModel.goldTransaction,
                TransactionDateTime = DateTime.Now,
                TransactionType = TransactionType.Receiving,
            };
            await _unitOfWork.TransactionRepository.AddAsync(transactionReceiving);
            await _unitOfWork.WalletRepository.UpdateAsync(walletReceiving);           

            var walletTransferring = await _unitOfWork.WalletRepository.GetByIdGuidAsync(putModel.Transferring.Id);
            walletTransferring.GoldBalance = walletTransferring.GoldBalance - putModel.goldTransaction;
            var transactionTransferring = _mapper.Map<Transaction>(walletTransferring.Transactions);
            transactionTransferring = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletTransferring.Id,
                Description = "Bạn chuyển đi " + putModel.goldTransaction + " Gold",
                GoldAmount = putModel.goldTransaction,
                TransactionDateTime = DateTime.Now,
                TransactionType = TransactionType.Transferring,
            };
            await _unitOfWork.TransactionRepository.AddAsync(transactionTransferring);
            await _unitOfWork.WalletRepository.UpdateAsync(walletTransferring);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Wallet Transferrring Successfully",
                IsSuccess = true,
                Data = putModel,
            };
        }
    }
}
