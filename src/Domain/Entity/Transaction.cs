using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Transaction : BaseEntity
    {
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;
        public int GoldAmount { get; set; }
        public string Description { get; set; } = null!;
        public DateTime TransactionDateTime { get; set; }
        public int TransactionType { get; set; }

    }
}
