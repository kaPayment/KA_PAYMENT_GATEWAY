using KA_PAYMENT_GATEWAY.Models;
using KA_PAYMENT_GATEWAY.Services.IContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Services
{
    public class MerchantBus : BaseBus, IMerchantBus
    {
        public async Task<Merchant> GetMerchant(int merchantId)
        {
            return await _DBContext.Merchants.FirstOrDefaultAsync(e => e.Id == merchantId);
        }

       
    }
}
