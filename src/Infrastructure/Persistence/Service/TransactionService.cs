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
using Microsoft.EntityFrameworkCore;

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
        public async Task<ResponseTransactionModel> GetListTransactionAsync(TransactionSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.TransactionRepository.BuildFilterAndOrderBy(searchModel);
            var transaction = await _unitOfWork.TransactionRepository.GetBySearchAsync(filter, orderBy,
            q => q.Include(s => s.Wallet)
                   .ThenInclude(a => a.Account),
            pageIndex: searchModel.currentPage,
            pageSize: searchModel.pageSize);
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
