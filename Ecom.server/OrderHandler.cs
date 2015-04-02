using Ecom.messages;
using NServiceBus;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ecom.Model;

namespace Ecom.server
{
    public class OrderSagaData : ContainSagaData
    {
        [Unique]
        public int OrderID { get; set; }
    }

    public class OrderHandler : 
        Saga<OrderSagaData>,
        IAmStartedByMessages<CreateOrder>,
        IHandleMessages<CreateOrder>,
        IHandleMessages<RecievedPayment>

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
                db.Entry(message.Order).State = System.Data.Entity.EntityState.Detached;

                Console.WriteLine("Order {0} created", message.Order.OrderID);

                _bus.Send("Ecom.Payment", new ChargePayment { Order = message.Order });
            }
        }


        public void Handle(RecievedPayment message)
        {
            Console.WriteLine("Got Payment Recieved. order {0}", message.OrderID);
            using (var db = new Ecom.Model.ecomEntities())
            {
                var order = db.Orders.FirstOrDefault(p => p.OrderID == message.OrderID);
                if (order !=  null)
                {
                    order.OrderStatusID = 2;
                    db.SaveChanges();
                }
            }

        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<CreateOrder>(s => s.Order.OrderID).ToSaga(m => m.OrderID);
            mapper.ConfigureMapping<RecievedPayment>(s => s.OrderID).ToSaga(m => m.OrderID);

        }
    }
}
