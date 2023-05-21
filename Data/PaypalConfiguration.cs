using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KA_PAYMENT_GATEWAY.Data
{
    //PayPal paymet gateway configuration
    public class PaypalConfiguration
    {
        static PaypalConfiguration()
        {

        }
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode",mode }
            };
        }

        private static string GetAccessToken(string ClientId, string ClientSecret, string mode)
        {

            //getting access token from paypal
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, new Dictionary<string, string>() {
                {"mode",mode }
            }).GetAccessToken();

            return accessToken;
        }

        public static APIContext GetAPIContext(string ClientId, string ClientSecret, string mode)
        {

            //return api context object by invoking it with the accesstoken
            APIContext apiContext = new APIContext(GetAccessToken(ClientId, ClientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;




        }
    }
}
