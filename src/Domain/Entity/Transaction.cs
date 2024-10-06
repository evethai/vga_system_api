using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class Transaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;
        public int GoldAmount { get; set; }
        public string Description { get; set; } = null!;
        public DateTime TransactionDateTime { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
