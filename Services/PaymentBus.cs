using KA_PAYMENT_GATEWAY.Services.IContract;
using KA_PAYMENT_GATEWAY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KA_PAYMENT_GATEWAY.Services
{
    public class PaymentBus : BaseBus, IPaymentBus
    {
        public async Task<Payment> CreatePayment(Payment Payment)
        {
            try
            {
                if(Payment != null)
                {
                    var merchant = await _DBContext.Merchants.FirstOrDefaultAsync(e => e.UserId == Payment.MerchantId);
                    Payment.Merchant = merchant;
                    Payment.MerchantId = merchant.Id;
                    Payment.CreatedDate = DateTime.Now;
                    Payment.Done = false;

                    await _DBContext.Payments.AddAsync(Payment);
                    await _DBContext.SaveChangesAsync();
                    return Payment;
                }

                return null;
                
            }catch(Exception ex)
            {
                return null;
            }
           
        }

        public async Task<Payment> DonePayment(int paymentId, string provider)
        {
            if (!String.IsNullOrEmpty(provider))
            {
                var payment = await _DBContext.Payments.FirstOrDefaultAsync(e => e.Id == paymentId);

                if(payment != null)
                {
                    payment.Done = true;
                    payment.Provider = provider;

                    await _DBContext.SaveChangesAsync();
                    return payment;
                }
            }
            return null;
        }

        public async Task<Payment> GetPament(int paymentId)
        {
            return await _DBContext.Payments.FirstOrDefaultAsync(e => e.Id == paymentId);
        }

        public async Task<Payment> GetPamentByToken(string token)
        {
            var entity = await _DBContext.Tokens.FirstOrDefaultAsync(e => e.ClientToken == token);

            if(entity != null)
            {
                return await _DBContext.Payments.FirstOrDefaultAsync(e => e.Id == entity.PaymentId);
            }
            else
            {
                return null;
            }

        }
    }
}
