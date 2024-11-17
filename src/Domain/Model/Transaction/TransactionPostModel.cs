using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "WalletHighSchoolId is required.")]
        public Guid WalletId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Gold must be a non-negative value.")]
        public int GoldAmount { get; set; }
    }
}
