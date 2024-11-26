using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Transaction
{
    public class TransactionSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        
        [FromQuery(Name = "description")]
        public string? description { get; set; }
        
        [FromQuery(Name = "transaction-date-time")]
        public DateTime? transaction_date_time { get; set; }
        
        [FromQuery(Name = "account-id")]
        public Guid? account_id { get; set; }
        [FromQuery(Name = "account-name")]
        public string? account_name { get; set; }

        [FromQuery(Name = "transaction-type")]
        public TransactionType? transaction_type { get; set; }

        [FromQuery(Name = "sort-by-gold-amount")]
        public bool? sort_by_gold_amount { get; set; } = false;
        [FromQuery(Name = "sort-by-date-time")]
        public bool? sort_by_datetime { get; set; } = false;

        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }

}
