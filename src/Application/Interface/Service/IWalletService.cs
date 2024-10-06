using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Region;
using Domain.Model.Response;
using Domain.Model.Wallet;

namespace Application.Interface.Service
{
    public interface IWalletService
    {       
        Task<Wallet> GetWalletByIdAsync(Guid Id);       
        Task<ResponseModel> UpdateUsingGoldBookCareerExpertWalletAsync(WalletPutModel putModel, int goldTransaction);
        Task<ResponseWalletModel> GetAllWallet();
        Task<ResponseModel> UpdateGoldDistributionWalletAsync(Guid WalletHigchoolId, int goldTransaction);
    }
}
