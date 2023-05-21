using KA_PAYMENT_GATEWAY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Services.IContract
{
    public interface IAuth
    {
        //Authentication Cycle -
        //1) check username & password.
        //2) Create Payment as draft.
        //3) Generate Token Expire after 1 minute.
        //4) return the Token.                           
        Task<string> Authentication(string username, string password, Payment payment, string url);

        Task<bool> CreateToken(int merchantId, string token, Payment payment, string url);

        Task<bool> CheckToken(string token);

        Task<Token> GetToken(string token);



    }
}
