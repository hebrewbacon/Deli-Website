using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using ItalianDeli.Models;
using ItalianDeli.ViewModels;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ItalianDeli.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var orders = from o in db.Orders
                        select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.FirstName);
                    break;
                case "Price":
                    orders = orders.OrderBy(s => s.Total);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(s => s.Total);
                    break;
                default:  // Name ascending 
                    orders = orders.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));

            //return View(await db.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            var orderDetails = db.OrderDetails.Where(x => x.OrderId == id );

            order.OrderDetails = await orderDetails.ToListAsync();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Update/2
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Order order = await db.Orders.FindAsync(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                //update the order status
                if (order.OrderStatus == Common.Status.Cooking)
                {
                    order.OrderStatus = Common.Status.ReadyForPickup;
                }
                else if (order.OrderStatus == Common.Status.ReadyForPickup)
                {
                    order.OrderStatus = Common.Status.Complete;
                }
                order.CreditCard = "123456789"; //hardcoding this until I fix it later
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Class: {0}, Property: {1}, Error: {2}", validationErrors.Entry.Entity.GetType().FullName,
                                      validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            //query the db again and return the list of orders wrapped in the order viewmodel
            List<Order> orders = db.Orders.Where(x => x.OrderStatus == 0).ToList();

            foreach (var order in orders)
            {
                var orderDetails = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                order.OrderDetails = orderDetails;
            }
            var orderList = new Orders
            {
                Order = orders
            };

            return View(orderList);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult CookOrders()
        {
            //query the db again and return the list of orders wrapped in the order viewmodel
            List<Order> orders = db.Orders.Where(x => x.OrderStatus == 0).ToList();

            foreach (var order in orders)
            {
                var orderDetails = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                order.OrderDetails = orderDetails;
            }
            var orderList = new Orders
            {
                Order = orders
            };

            return View(orderList);
        }

        public ActionResult DeliveryOrders()
        {
            //query the db again and return the list of orders wrapped in the order viewmodel
            List<Order> orders = db.Orders.Where(x => (int)x.OrderStatus == 1).ToList();

            foreach (var order in orders)
            {
                var orderDetails = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                order.OrderDetails = orderDetails;
            }
            var orderList = new Orders
            {
                Order = orders
            };

            return View(orderList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
