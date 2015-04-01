using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecom.web.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            using (var db = new ecom.web.Models.ecomEntities())
            {
                var query = from item in db.ShoppingCarts.Include("Product") select item;
                ViewBag.OrderTotal = query.Sum(p => p.Product.Price);
            }
            return View();
            
        }

        public ActionResult PlaceOrder()
        {
            return View();
        }
    }
}