using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Wallet
{
    public class WalletPutModel
    {
        public Guid Id { get; set; }
        public int GoldBalance { get; set; }
    }
}
