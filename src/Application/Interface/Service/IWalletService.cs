using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Library;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Region;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Domain.Model.Wallet;

namespace Application.Interface.Service
{
    public interface IWalletService
    {       
        Task<WalletModel> GetWalletByIdAsync(Guid Id);       
        Task<ResponseModel> UpdateWalletByTransferringAndReceivingAsync(WalletPutModel putModel, int goldTransaction);
        Task<ResponseWalletModel> GetAllWallet();
        Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(TransactionPutWalletModel model);
        Task<ResponseModel> UpdateWalletUsingByTestAsync(Guid WalletStudentId, int goldUsingTest);
        Task<ResponseModel> RequestTopUpWalletWithPayOsAsync(Guid accountId, float amount, PayOSUrl url);
        Task<ResponseModel> RequestDepositToWalletWithPayOs(Guid transactionId, string status);
    }
}
