﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Transaction
{
    public class TransactionPutWalletModel
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public Guid  AccountId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Gold must be a non-negative value.")]
        public int Gold { get; set; }
        [Range(2000, int.MaxValue, ErrorMessage = "Years must be a non-negative value.")]
        public int Years { get; set; }
    }
}
