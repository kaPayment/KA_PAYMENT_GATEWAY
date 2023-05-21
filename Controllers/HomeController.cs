using KA_PAYMENT_GATEWAY.Models;
using KA_PAYMENT_GATEWAY.Services;
using KA_PAYMENT_GATEWAY.Services.IContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PayPal.Api;
using KA_PAYMENT_GATEWAY.Data;

namespace KA_PAYMENT_GATEWAY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IAuth _auth;
        readonly IPaymentBus _payment;
        readonly IMerchantBus _merchant;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IAuth auth, IPaymentBus payment, 
                            IHttpContextAccessor context, IConfiguration configuration, IMerchantBus merchant)
        {
            _logger = logger;
            _auth = auth;
            _payment = payment;
            httpContextAccessor = context;
            _configuration = configuration;
            _merchant = merchant;

        }

        public async Task<IActionResult> Index(string token)
        {
            if (!String.IsNullOrEmpty(token))
            {
                if (await _auth.CheckToken(token)) // Check if token doesn't expired
                {
                    var payment = await _payment.GetPamentByToken(token); //get datails of payment to display in payment page
                    if (payment != null)
                    {
                        ViewBag.Payment = payment;
                    }
                    var merchant = await _merchant.GetMerchant(payment.MerchantId); //get datails of merchant to display in payment page
                    if (merchant != null)
                    {
                        ViewBag.merchantName = merchant.Name;
                    }
                    ViewBag.clientToken = token;
                    return View("Index");
                }
                else
                {
                    var toketEntity = await _auth.GetToken(token);
                    ViewBag.orgURL = toketEntity.CurrentUrl;
                    return View("Failed");
                }

            }

            return View("Failed");
        }

#region Stripe Payment Section
        public async Task<IActionResult> PayWithStripe(string clientToken,string StripeEmail, string StripeToken, string Total,string orderId)
        {
            
           
            var entity = await _payment.GetPamentByToken(clientToken);
            var token = await _auth.GetToken(clientToken);
            ViewBag.orgURL = token.CurrentUrl;
            if (entity != null)
            {
                var customers = new CustomerService();
                var charges = new ChargeService();

                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = StripeEmail,
                    Source = StripeToken,

                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(entity.TotalAmount.ToString() + "00"),
                    Description = entity.Describtion,
                    Currency = entity.Currency,
                    Customer = customer.Id,
                    ReceiptEmail = StripeEmail,
                    Metadata = new Dictionary<string, string>()
                {
                    {"OrderId",entity.OrderNo }
                }
                });

                if (charge.Status == "succeeded")
                {
                    string BalanceTransactionId = charge.BalanceTransactionId;
                   
                        var donePayment = await _payment.DonePayment(entity.Id, "Stripe");

                    
                    
                    return View("Success");
                }
            }

            
            
            return View("Failed");

        }
        #endregion Stripe Payment Section

        #region Paypal Payment Section
        
        public async Task<ActionResult> PaymentWithPaypal(string clientToken,string cancel = null, string blogId = "", string PayerId = "", string guid = "")
        {
            try
            {
                if (String.IsNullOrEmpty(clientToken))
                {
                    clientToken = httpContextAccessor.HttpContext.Session.GetString("clientToken");
                }
            var entity = await _payment.GetPamentByToken(clientToken); //get details of payment
                var token = await _auth.GetToken(clientToken); //get token of payment
                ViewBag.orgURL = token.CurrentUrl;
                if (entity != null)
            {
                var ClientId = _configuration.GetValue<string>("PayPal:Key");
                var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
                var mode = _configuration.GetValue<string>("PayPal:mode");

                APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientId, ClientSecret, mode);

               
                    string payerId = PayerId;
                    if (string.IsNullOrEmpty(payerId))
                    {
                        string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Home/PaymentWithPaypal";

                        var guidd = Convert.ToString((new Random()).Next(1000));
                        guid = guidd;

                        var createdPayment = this.CreatePayment(apiContext, baseURI, blogId,entity);
                        //+ "guid=" + guid
                        //get links returned from paypal in response to create function call
                        var links = createdPayment.links.GetEnumerator();
                        string paypalRedirectURL = null;

                        while (links.MoveNext())
                        {
                            Links lnk = links.Current;

                            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                //saving the paypal redirect url to which user will be redirected for payment  
                                paypalRedirectURL = lnk.href;
                            }
                        }
                        //saving the payment id in the key guid
                        httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                        httpContextAccessor.HttpContext.Session.SetString("clientToken", clientToken);
                        return Redirect(paypalRedirectURL);
                    }
                    else
                    {
                        //this function executes after receving all parameters for the payment

                        var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                        var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);

                        //if executed payment failed then will show payment failure message to user

                        if (executedPayment.state.ToLower() == "approved")
                        {
                           
                                var donePayment = await _payment.DonePayment(entity.Id, "PayPal");
                            
                            return View("Success");
                        }
                        //var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                        return View("Failed");
                    }
               
            }
                }
                catch (Exception ex)
            {
                return View("Failed");
            }

            return View("Failed");
        }

        //Execute payment - Final Step
        private PayPal.Api.Payment payment;
        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new PayPal.Api.Payment()
            {
                id = paymentId
            };

            return this.payment.Execute(apiContext, paymentExecution);
        }

        //Prepare the payment before execute it
        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectURI, string blogId, Models.Payment payment)
        {
           

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectURI + "&cancel=true",
                return_url = redirectURI
            };

            var details = new Details()
            {
                tax = payment.TaxAmount.ToString() + ".00",
                shipping = payment.FeeAmount.ToString()+".00",
                subtotal = payment.TotalAmount.ToString()+".00"
            };
            var amount = new Amount()
            {
                currency = payment.Currency.ToUpper(),
                total = payment.TotalAmount.ToString()+".00",
                // details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Test Transaction",
                invoice_number = payment.OrderNo,
                amount = amount,
                
            });

            this.payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            //Create Payment using API Context
            return this.payment.Create(apiContext);
        }
        #endregion Paypal Payment Section



       
    }
}
