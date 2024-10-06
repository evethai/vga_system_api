using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;

namespace Application.Interface.Service
{
    public interface ITransactionService
    {
        Task<ResponseTransactionModel> GetListTransactionAsync(TransactionSearchModel searchModel);
        Task<ResponseModel> CreateTransactionUsingAsync(TransactionPostModel postModel);
        Task<ResponseModel> CreateTransactionReceivingAsync(TransactionPostModel postModel);
        Task<ResponseModel> CreateTransactionTransferringAsync(TransactionPostModel postModel);
    }
}
