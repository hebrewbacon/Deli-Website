using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ItalianDeli.Configuration;
using ItalianDeli.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace ItalianDeli.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        AppConfigurations appConfig = new AppConfigurations();

        public List<String> CreditCardTypes { get { return appConfig.CreditCardType;} }

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            ViewBag.CreditCardTypes = CreditCardTypes;
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());
            var previousOrder = storeDB.Orders.FirstOrDefault(x => x.Username == User.Identity.Name);
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel { User = user, Order = previousOrder };

            if (previousOrder != null || user != null)
                return View(checkoutViewModel);
            else
                return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            ViewBag.CreditCardTypes = CreditCardTypes;
            string result =  values[9];
            string deliveryType = values[11];

            var checkoutViewModel = new CheckoutViewModel();
            TryUpdateModel(checkoutViewModel);
            checkoutViewModel.Order.CreditCard = result;

            checkoutViewModel.Order.DeliveryOption = Convert.ToInt32(deliveryType) == 0 ? Common.DeliveryOption.Pickup : Common.DeliveryOption.Delivery;

            try
            {
                    checkoutViewModel.Order.Username = User.Identity.Name;
                    checkoutViewModel.Order.Email = User.Identity.Name;
                    checkoutViewModel.Order.OrderDate = DateTime.Now;

                    /*
                     * Alot of repition but just added this to make saving to db work coz without it its throwing exception
                     * errors for required fields
                     */ 
                    checkoutViewModel.Order.Address = checkoutViewModel.User.Address;
                    checkoutViewModel.Order.City = checkoutViewModel.User.City;
                    checkoutViewModel.Order.Country = checkoutViewModel.User.Country;
                    checkoutViewModel.Order.State = checkoutViewModel.User.State;
                    checkoutViewModel.Order.Phone = checkoutViewModel.User.Phone;
                    checkoutViewModel.Order.PostalCode = checkoutViewModel.User.PostalCode;
                    checkoutViewModel.Order.FirstName = checkoutViewModel.User.FirstName;
                    checkoutViewModel.Order.LastName = checkoutViewModel.User.LastName;

                    var currentUserId = User.Identity.GetUserId();
                    if (checkoutViewModel.Order.SaveInfo && !checkoutViewModel.Order.Username.Equals("guest@guest.com"))
                    {
                        
                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                        var ctx = store.Context;
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        currentUser.Address = checkoutViewModel.User.Address;
                        currentUser.City = checkoutViewModel.User.City;
                        currentUser.Country = checkoutViewModel.User.Country;
                        currentUser.State = checkoutViewModel.User.State;
                        currentUser.Phone = checkoutViewModel.User.Phone;
                        currentUser.PostalCode = checkoutViewModel.User.PostalCode;
                        currentUser.FirstName = checkoutViewModel.User.FirstName;
                        currentUser.LastName = checkoutViewModel.User.LastName;

                        //Save this back
                        //http://stackoverflow.com/questions/20444022/updating-user-data-asp-net-identity
                        //var result = await UserManager.UpdateAsync(currentUser);
                        await ctx.SaveChangesAsync();

                        await storeDB.SaveChangesAsync();
                    }
                    

                    //Save Order
                    storeDB.Orders.Add(checkoutViewModel.Order);
                    await storeDB.SaveChangesAsync();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    checkoutViewModel.Order = cart.CreateOrder(checkoutViewModel.Order);



                    CheckoutController.SendOrderMessage(checkoutViewModel.Order.FirstName, "New Order: " + checkoutViewModel.Order.OrderId, checkoutViewModel.Order.ToString(checkoutViewModel.Order), appConfig.OrderEmail);

                    return RedirectToAction("Complete",
                        new { id = checkoutViewModel.Order.OrderId, orderType = checkoutViewModel.OrderType });
                
            }
            catch(Exception ex)
            {
                if (ex is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = (ex as System.Data.Entity.Validation.DbEntityValidationException).EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                }
                //Invalid - redisplay with errors
                return View(checkoutViewModel);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id, string orderType)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                CompleteOrderViewodel completeOrderViewodel = new CompleteOrderViewodel()
                {
                    completeOrderId = id,
                    completeOrderType = orderType
                };
                return View(completeOrderViewodel);
            }
            else
            {
                return View("Error");
            }
        }

        private static RestResponse SendOrderMessage(String toName, String subject, String body, String destination)
        {
            RestClient client = new RestClient();
            //fix this we have this up top too
            AppConfigurations appConfig = new AppConfigurations();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              appConfig.EmailApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                appConfig.DomainForApiKey, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", appConfig.FromName + " <" + appConfig.FromEmail + ">");
            request.AddParameter("to", toName + " <" + destination + ">");
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            request.Method = Method.POST;
            IRestResponse executor = client.Execute(request);
            return executor as RestResponse;
        }
    }
}