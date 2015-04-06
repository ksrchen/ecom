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
        IHandleMessages<RecievedPayment>,
        IHandleMessages<PaymentDeclined>,
        IHandleMessages<FulfillmentComplete>
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

                Data.OrderID = message.Order.OrderID;

                _bus.Send("Ecom.Payment", new ChargePayment { OrderID = message.Order.OrderID });
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
            _bus.Send("Ecom.Fulfillment", new RequestFulfillment { OrderID = message.OrderID });
            _bus.Send("Ecom.Billing", new CreateInvoice { OrderID = message.OrderID });

        }

        public void Handle(PaymentDeclined message)
        {
            Console.WriteLine("Got Payment declined. order {0}", message.OrderID);
            using (var db = new Ecom.Model.ecomEntities())
            {
                var order = db.Orders.FirstOrDefault(p => p.OrderID == message.OrderID);
                if (order != null)
                {
                    order.OrderStatusID = 5;
                    db.SaveChanges();
                }
            }
            MarkAsComplete();

        }

        public void Handle(FulfillmentComplete message)
        {

            Console.WriteLine("Got FulfillmentComplete order {0}", message.OrderID);

           
            using (var db = new Ecom.Model.ecomEntities())
            {
                var order = db.Orders.FirstOrDefault(p => p.OrderID == message.OrderID);
                if (order != null)
                {
                    order.OrderStatusID = 6;
                    db.SaveChanges();
                }
            }
            MarkAsComplete();
        }


        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<CreateOrder>(s => s.Order.OrderID).ToSaga(m => m.OrderID);
            mapper.ConfigureMapping<RecievedPayment>(s => s.OrderID).ToSaga(m => m.OrderID);
            mapper.ConfigureMapping<PaymentDeclined>(s => s.OrderID).ToSaga(m => m.OrderID);

        }

       
    }
}
