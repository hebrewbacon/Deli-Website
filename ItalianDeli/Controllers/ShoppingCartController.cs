using ItalianDeli.Models;
using ItalianDeli.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItalianDeli.Controllers
{
    public class ShoppingCartController : Controller
    {

        ApplicationDbContext storeDB = new ApplicationDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            HttpContext.Session["CartTotal"] = cart.GetTotal();
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            // Retrieve the item from the database
            var addedItem = storeDB.Items
                .Single(item => item.ID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            int count = cart.AddToCart(addedItem);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(addedItem.Name) +
                    " has been added to your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = count,
                DeleteId = id
            };
            HttpContext.Session["CartTotal"] = cart.GetTotal();
            return Json(results);

            // Go back to the main store page for more shopping
           // return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            if (quantity == 0)
            {
                return RemoveFromCart(id);
            }
            else if (quantity > 0)
            {
                string itemName = storeDB.Items
                    .Single(item => item.ID == id).Name;

                int itemCount = cart.UpdateCart(id, quantity);

                // Set up our ViewModel
                var results = new ShoppingCartUpdateViewModel
                {
                    Message = Server.HtmlEncode(itemName) +
                              " has updated in your shopping cart.",
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
                HttpContext.Session["CartTotal"] = cart.GetTotal();
                return Json(results);
            }
            else
            {
                var results = new ShoppingCartUpdateViewModel
                {
                    Message = Server.HtmlEncode("Please enter a quantity greater than or equal to 0"),
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = cart.GetItemCount(id),
                    DeleteId = id
                };
                HttpContext.Session["CartTotal"] = cart.GetTotal();
                return Json(results);
            }
        }


        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the item to display confirmation

            // Get the name of the album to display confirmation
            string itemName = storeDB.Items
                .Single(item => item.ID == id).Name;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "One (1) "+ Server.HtmlEncode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            HttpContext.Session["CartTotal"] = cart.GetTotal();
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}