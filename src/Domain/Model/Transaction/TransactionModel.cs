using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Highschool;

namespace Domain.Model.Transaction
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public int GoldAmount { get; set; }
        public string Description { get; set; } = null!;
        public DateTime TransactionDateTime { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Code { get; set; }
        public string? Image { get; set; }
    }
    public class ResponseTransactionModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<TransactionModel> transactions { get; set; }
    }
}
