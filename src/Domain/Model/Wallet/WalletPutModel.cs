using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Transaction;

namespace Domain.Model.Wallet
{
    public class WalletPutModel
    {
        public WalletTransferringModel wallet_id_tranferring { get; set; }
        public WalletReceivingModel wallet_id_receiving { get; set; }
    }
    public class WalletTransferringModel
    {
        public Guid Id { get; set; }
    }
    public class WalletReceivingModel
    {
        public Guid Id { get; set; }
    }
}
