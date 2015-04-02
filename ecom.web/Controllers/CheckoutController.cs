using ecom.web.Models;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ecom.web.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            using (var db = new Ecom.Model.ecomEntities())
            {
                var query = from item in db.ShoppingCarts.Include("Product") select item;
                ViewBag.OrderTotal = query.Count()>0 ? query.Sum(p => p.Product.Price) : 0;
            }
            return View("Index");
            
        }

        public ActionResult PlaceOrder([FromBody] CheckoutRequest req )
        {
            try
            {
                BusConfiguration configuration = new BusConfiguration();
                configuration.UsePersistence<InMemoryPersistence>();
                ISendOnlyBus bus = Bus.CreateSendOnly(configuration);
                bus.Send("Ecom.Server", new Ecom.messages.CreateOrder() {Order = Convert(req)});
                clearCart();
                return View();
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("", exp.Message);
                return Index();
            }
        }
        private void clearCart()
        {
            using (var db = new Ecom.Model.ecomEntities())
            {
                foreach (var i in db.ShoppingCarts)
                {
                    db.Entry(i).State = System.Data.Entity.EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }

        private Ecom.Model.Order Convert(CheckoutRequest req)
        {
            using (var db = new Ecom.Model.ecomEntities())
            {
                var shoppingCartItems = (from d in  db.ShoppingCarts.Include("Product") select d).ToList();
                if (shoppingCartItems.Count() <= 0)
                {
                    throw new Exception("No shopping cart items found during checkout");
                }

                var order = new Ecom.Model.Order()
                {
                    OrderDate = DateTime.Now,
                    UserID = 1,
                    OrderStatusID = 1,
                    TotalPrice = shoppingCartItems.Sum(p=>p.Product.Price),

                    CreditCard = new Ecom.Model.CreditCard
                    {
                        Active = true,
                        Contact = new Ecom.Model.Contact
                        {
                            FirstName = req.firstName,
                            LastName = req.lastName,
                            Active = true,
                            City = req.city,
                            CountryCode = req.country,
                            Email = req.email,
                            PhoneNumber = req.phone,
                            PostalCode = req.postalCode,
                            StateCode = req.state,
                            StreetLine1 = req.addressLine1,
                            StreetLine2 = req.addressLine2,
                        },
                        Token = req.creditCardToken,
                        ExiprationYear = int.Parse(req.creditCardExpirationYear),
                        ExpirationMonth = int.Parse(req.creditCardExpirationMonth),
                        Last4Digits = req.creditCardNumber.Substring(req.creditCardNumber.Length - 4),
                        Type = req.creditCardType,
                        UserID = 1,
                    },

                };
                foreach (var item in shoppingCartItems)
                {
                    order.OrderLines.Add(new Ecom.Model.OrderLine
                    {
                        Discount =0,
                        ProductID = item.ProductID,
                       // Product = item.Product,
                        Quantity = item.Quantity,
                        TaxAmount = 0,
                        TaxRate =0 ,
                        UnitPrice = item.Product.Price,                       
                    });
                }

                return order;
            }
        }
    }
}