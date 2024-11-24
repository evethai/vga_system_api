using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Transaction
{
    public class TransactionProcessRequestModel
    {
        [Required]
        public TransactionType type { get; set; }
        public string? Image { get; set; } = string.Empty;
    }
}
