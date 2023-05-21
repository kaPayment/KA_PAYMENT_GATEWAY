using KA_PAYMENT_GATEWAY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

//we can use this bus in all our services (shared)
namespace KA_PAYMENT_GATEWAY.Services
{
    public class BaseBus
    {
        public readonly KA_PAYMENT_DBContext _DBContext = new KA_PAYMENT_DBContext();

        public BaseBus()
        {

        }

        public byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[64];

                rng.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public byte[] ComputeHMAC_SHA256(byte[] data, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(data);
            }
        }

       

       

    }
}
