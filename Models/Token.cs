using System;
using System.Collections.Generic;

#nullable disable

namespace KA_PAYMENT_GATEWAY.Models
{
    public partial class Token
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public string ClientToken { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int PaymentId { get; set; }
        public string CurrentUrl { get; set; }

        public virtual Merchant Merchant { get; set; }
    }
}
