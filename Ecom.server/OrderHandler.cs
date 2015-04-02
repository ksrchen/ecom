using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.server
{

    public class OrderHandler : IHandleMessages<CreateOrder>
    {
        IBus _bus;

        public OrderHandler(IBus bus)
        {
            _bus = bus;
        }
        public void Handle(CreateOrder message)
        {
            Console.WriteLine("Got create order message");

            using (var db = new Ecom.Model.ecomEntities())
            {
                db.Orders.Add(message.Order);
                db.SaveChanges();
            }
            Console.WriteLine("Order {0} created", message.Order.OrderID);

        }


    }
}
