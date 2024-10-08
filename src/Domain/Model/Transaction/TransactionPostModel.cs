using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Transaction
{
    public class TransactionPostModel
    {
        public TransactionPostModel(Guid walletId, int goldAmount)
        {
            WalletId = walletId;
            GoldAmount = goldAmount;
        }

        public Guid WalletId { get; set; }
        public int GoldAmount { get; set; }
    }
}
