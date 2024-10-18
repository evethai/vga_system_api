using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task<Wallet> GetWalletByIdAsync(Guid Id);       
        Task<ResponseModel> UpdateWalletUsingGoldBookConsultantAsync(WalletPutModel putModel, int goldTransaction);
        Task<ResponseWalletModel> GetAllWallet();
        Task<ResponseModel> UpdateWalletUsingGoldDistributionAsync(Guid WalletHigchoolId, int goldTransaction);
        Task<ResponseModel> UpdateWalletUsingByTestAsync(Guid WalletStudentId, int goldUsingTest);
        Task<ResponseModel> UpdateWallet(Guid id, int gold);
    }
}
