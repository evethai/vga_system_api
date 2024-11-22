using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library
{
    public class PayOSModel
    {
        public int orderCode {  get; set; }
        public float amount { get; set; }
        public string description { get; set; }
        public string returnUrl { get; set; }
        public string cancelUrl { get; set; }

    }
    public class PayOSUrl
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
