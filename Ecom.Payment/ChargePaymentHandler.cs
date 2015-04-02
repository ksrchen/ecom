using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Payment
{
    public class ChargePaymentHandler : IHandleMessages<Ecom.messages.ChargePayment>
    {
        IBus _bus;

        public ChargePaymentHandler(IBus bus)
        {
            _bus = bus;
        }
        public void Handle(Ecom.messages.ChargePayment message)
        {
            Console.WriteLine("Got charge payment message");

            using (var db = new Ecom.Model.ecomEntities())
            {
                var order = db.Orders.FirstOrDefault(p => p.OrderID == message.Order.OrderID);

                var transactionId = PaymentGateway.Charge(order);
                Console.WriteLine("Charged order {0}, trx id {1}", message.Order.OrderID, transactionId);

                _bus.Reply(new RecievedPayment { OrderID = message.Order.OrderID, TransactionID = transactionId });
            }
        }


    }

    
}
