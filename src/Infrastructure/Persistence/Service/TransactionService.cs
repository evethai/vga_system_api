using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;

namespace Application.Interface.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<ResponseModel> CreateTransactionReceivingAsync(TransactionPostModel postModel)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> CreateTransactionTransferringAsync(TransactionPostModel postModel)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> CreateTransactionUsingAsync(TransactionPostModel postModel)
        {
            var transaction = _mapper.Map<Transaction>(postModel);
            var result = await _unitOfWork.TransactionRepository.AddAsync(transaction);
            _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = " Transaction Created Successfully",
                IsSuccess = true,
                Data = postModel,
            };
        }

        public async Task<ResponseTransactionModel> GetListTransactionAsync(TransactionSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.TransactionRepository.BuildFilterAndOrderBy(searchModel);
            var transaction = await _unitOfWork.TransactionRepository.GetByConditionAsync(filter, orderBy, pageIndex: searchModel.currentPage, pageSize: searchModel.pageSize);
            var total = await _unitOfWork.TransactionRepository.CountAsync(filter);
            var listTransaction = _mapper.Map<List<TransactionModel>>(transaction);
            return new ResponseTransactionModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                transactions = listTransaction,
            };
        }
    }
}
