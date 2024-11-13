using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.AccountWallet
{
    public class AccountWalletModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public AccountStatus Status { get; set; }
        public WalletAccountModel Wallet { get; set; }
    }
    public class WalletAccountModel
    {
        public Guid Id { get; set; }
        public int GoldBalance { get; set; }
    }
}
