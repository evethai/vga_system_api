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
        public Guid account_id_tranferring { get; set; }
        public Guid account_id_receiving { get; set; }       
    }
}
