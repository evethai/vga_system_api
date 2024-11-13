using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum TransactionType
    {
        Receiving = 1,
        Transferring = 2,
        Using = 3,
        Request = 4,
        Reject = 5,
        Withdraw =6
    }
}
