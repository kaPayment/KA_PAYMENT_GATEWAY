using System;
using System.Collections.Generic;

#nullable disable

namespace KA_PAYMENT_GATEWAY.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public float NetAmount { get; set; }
        public float TaxAmount { get; set; }
        public float FeeAmount { get; set; }
        public float TotalAmount { get; set; }
        public string Describtion { get; set; }
        public string OrderNo { get; set; }
        public string TransactionId { get; set; }
        public string Provider { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Currency { get; set; }
        public bool Done { get; set; }

        public virtual Merchant Merchant { get; set; }
    }
}
