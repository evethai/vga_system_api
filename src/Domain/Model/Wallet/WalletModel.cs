using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Wallet
{
    public class WalletModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public int GoldBalance { get; set; }
    }
    public class ResponseWalletModel
    {
        public List<WalletModel> wallets { get; set; }
    }
}
