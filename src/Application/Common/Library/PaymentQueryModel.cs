using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Application.Library
{
    public class PaymentQueryModel
    {
        public string vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_BankTranNo { get; set; }
        public string vnp_CardType { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }

        public Dictionary<string, StringValues> ToQueryDictionary()
        {
            return new Dictionary<string, StringValues>
            {
                { nameof(vnp_Amount), new StringValues(vnp_Amount) },
                { nameof(vnp_BankCode), new StringValues(vnp_BankCode) },
                { nameof(vnp_BankTranNo), new StringValues(vnp_BankTranNo) },
                { nameof(vnp_CardType), new StringValues(vnp_CardType) },
                { nameof(vnp_OrderInfo), new StringValues(vnp_OrderInfo) },
                { nameof(vnp_PayDate), new StringValues(vnp_PayDate) },
                { nameof(vnp_ResponseCode), new StringValues(vnp_ResponseCode) },
                { nameof(vnp_TmnCode), new StringValues(vnp_TmnCode) },
                { nameof(vnp_TransactionNo), new StringValues(vnp_TransactionNo) },
                { nameof(vnp_TransactionStatus), new StringValues(vnp_TransactionStatus) },
                { nameof(vnp_TxnRef), new StringValues(vnp_TxnRef) },
                { nameof(vnp_SecureHash), new StringValues(vnp_SecureHash) }
            };
        }
    }
}
