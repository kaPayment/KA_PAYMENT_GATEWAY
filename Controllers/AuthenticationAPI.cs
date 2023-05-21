using KA_PAYMENT_GATEWAY.Models;
using KA_PAYMENT_GATEWAY.Services.IContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationAPI : ControllerBase
    {
        //this API use to fetch the data send from other party to create payment
        //and generate token for execute payment by this payment gateway
        private readonly IAuth _auth;

        public AuthenticationAPI(IAuth auth)
        {
            _auth = auth;
        }

        [Route("Authentication")]
        [HttpPost]
        public async Task<string> auth([FromBody] Auth user)
        {
            string token = await _auth.Authentication(user.Username, user.Password, user.Payment,user.CurrentURL);
            return token;
        }
        }
}
