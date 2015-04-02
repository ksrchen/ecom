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
            using (var db = new Ecom.Model.ecomEntities())
            {
                var query = from item in db.ShoppingCarts.Include("Product") select item;
                ViewBag.Subtotal = query.Count() > 0 ? query.Sum(p => p.Product.Price) : 0;
                return View("index", query.ToList());
            }
        }

        public ActionResult Remove(int id)
        {
            using (var db = new Ecom.Model.ecomEntities())
            {
                var item = db.ShoppingCarts.FirstOrDefault(p => p.ShoppingCartID == id);
                if (item !=null)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                return Index();
            }
        }
    }
}