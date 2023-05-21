using KA_PAYMENT_GATEWAY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Services.IContract
{
    //Merchant Interface
    public interface IMerchantBus
    {
        Task<Merchant> GetMerchant(int merchantId);
    }
}
