using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Wallet;

namespace Application.Interface.Service
{
    public interface IWalletService
    {       
        Task<Wallet> GetWalletByIdAsync(Guid AccountId);       
        Task<ResponseModel> UpdateUsingGoldWalletAsync(WalletPutModel putModel, Guid Id);
    }
}
