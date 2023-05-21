using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Data
{
    //Stripe paymet gateway configuration
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishKey { get; set; }
    }
}
