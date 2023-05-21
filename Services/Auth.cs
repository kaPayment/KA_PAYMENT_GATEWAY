using KA_PAYMENT_GATEWAY.Models;
using KA_PAYMENT_GATEWAY.Services.IContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Services
{
    public class Auth : BaseBus, IAuth
    {
        readonly IPaymentBus _payment;
        

        public Auth(IPaymentBus payment)
        {
            _payment = payment;
           
        }
        public async Task<string> Authentication(string username, string password, Payment payment, string url)
        {
            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password) && payment != null && !String.IsNullOrEmpty(url))
            {
                try
                {

                    var merchant = await _DBContext.Users.FirstOrDefaultAsync(e => e.UserName == username && e.Deleted == false && e.Active == true);

                    if (merchant != null)
                    {
                        var salt = merchant.SecurityStamp;
                        var pass = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt)));

                        if (merchant.Password == pass)
                        {
                            string token = Convert.ToBase64String(ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(DateTime.Now.ToString()), Encoding.UTF8.GetBytes(salt)));

                            token = token.Replace("\\", "").Replace("/","").Replace("+","");

                            payment.MerchantId = merchant.Id;
                            
                            var newPayment = await _payment.CreatePayment(payment);
                            if (newPayment != null)
                            {
                                if (await CreateToken(merchant.Id, token, newPayment, url))
                                    return token;
                            }
                           
                        }

                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
           

        }



        public async Task<bool> CheckToken(string token)
        {
            if(!String.IsNullOrEmpty(token))
            {
                var entity = await _DBContext.Tokens.FirstOrDefaultAsync(e => e.ClientToken == token && e.ExpiredAt >= DateTime.Now);

                if (entity != null)
                    return true;
            }
           

            return false;
        }

        public async Task<bool> CreateToken(int userId, string token, Payment paymant, string url)
        {
            if(!String.IsNullOrEmpty(token))
            {
                try
                {
                    var merchant = await _DBContext.Merchants.FirstOrDefaultAsync(e => e.UserId == userId);
                    var newToken = new Token
                    {
                        MerchantId = merchant.Id,
                        ClientToken = token,
                        ExpiredAt = DateTime.Now.AddMinutes(1),
                        Merchant = merchant,
                        PaymentId = paymant.Id,
                        CurrentUrl = url
                    };


                    await _DBContext.Tokens.AddAsync(newToken);
                    await _DBContext.SaveChangesAsync();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
         
        }

        public async Task<Token> GetToken(string token)
        {
            return await _DBContext.Tokens.FirstOrDefaultAsync(e => e.ClientToken == token);
        }
    } 
}