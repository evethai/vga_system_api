using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Transaction;

namespace Application.Interface.Service
{
    public interface ITransactionService
    {
        Task<ResponseTransactionModel> GetListTransactionAsync(TransactionSearchModel searchModel);
        Task<ResponseModel> CreateWithdrawAsync(Guid consultantId, int goldAmount);
        Task<ResponseModel> ProcessWithdrawRequestAsync(Guid transactionId, TransactionProcessRequestModel model);
    }
}
