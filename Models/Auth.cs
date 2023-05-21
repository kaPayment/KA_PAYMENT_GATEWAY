using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Models
{
    public class Auth
    {
        public string Username { get; set; }

        public string Password { get; set; }
        public Payment Payment { get; set; }

        public string CurrentURL { get; set; }
    }
}
