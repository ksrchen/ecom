using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecom.web.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            using (var db = new ecom.web.Models.ecomEntities())
            {
                var query = from item in db.ShoppingCarts.Include("Product") select item;
                ViewBag.Subtotal = query.Sum(p => p.Product.Price);
                return View("index", query.ToList());
            }
        }
    }
}