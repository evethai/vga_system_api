using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Wallet : BaseEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        public int GoldBalance { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = null!;

    }
}
