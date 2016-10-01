using MusicStore1.Context;
using MusicStore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore1.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoredb storeDB = new MusicStoredb();
        const string PromoCode = "Free";
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        //POST:/Checkout/AddressAndPayment
        [HttpPost]

        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                if(string.Equals(values["PromoCode"],PromoCode,
                    StringComparison.OrdinalIgnoreCase)==false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //SaveOrder
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();

                    //Process The Order
                    var Cart = ShoppingCart.GetCart(this.HttpContext);
                        Cart.CreateOrder(order);
                        return RedirectToAction("Complete", new { id = order.OrderID });
                }
            }
            catch
            {
                //Invalid-Redisplay with errors
                return View(order);
            }
        }

        //Get:Checkout/Complete
        public ActionResult Complete(int id)
        {
            //Validate customer Own this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderID == id && o.Username == User.Identity.Name);
            if(isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
	}
}