using KA_PAYMENT_GATEWAY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Services.IContract
{
    public interface IPaymentBus
    {
        //Payment Interface
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> GetPament(int paymentId);
        Task<Payment> GetPamentByToken(string token);
        Task<Payment> DonePayment(int paymentId, string provider);

    }
}
