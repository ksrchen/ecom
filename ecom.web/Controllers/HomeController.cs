using System.Web.Mvc;
using System.Linq;
namespace ecom.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new ecom.web.Models.ecomEntities())
            {               
                var query = from item in db.Products select item;
                return View("index", query.ToList());
            }
           
        }

        public ActionResult Add2Cart(int id)
        {
            using (var db = new ecom.web.Models.ecomEntities())
            {
                db.ShoppingCarts.Add(new Models.ShoppingCart
                {
                    ProductID = id,
                    UserID = 1,
                    Quantity = 1
                    
                });
                db.SaveChanges();
                return Index();
            }
        }
    }
}