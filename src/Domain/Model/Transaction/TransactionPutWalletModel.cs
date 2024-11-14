using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Transaction
{
    public class TransactionPutWalletModel
    {
        [Required(ErrorMessage = "WalletHighSchoolId is required.")]
        public Guid  WalletHighSchoolId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Gold must be a non-negative value.")]
        public int Gold { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Years must be a non-negative value.")]
        public int Years { get; set; }
    }
}
