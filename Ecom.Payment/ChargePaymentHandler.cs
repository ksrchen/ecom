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
                var order = db.Orders.FirstOrDefault(p => p.OrderID == message.OrderID);


                var response = PaymentGateway.Charge(order);
                if (response.Status)
                {
                    Console.WriteLine("Charged order {0}, trx id {1}", message.OrderID, response.TransactionId);
                    _bus.Reply(new RecievedPayment { OrderID = message.OrderID, TransactionID = response.TransactionId });
                }
                else
                {
                    Console.WriteLine("payment declined {0}, reason code {1}", message.OrderID, response.ReasonCode);
                    _bus.Reply(new PaymentDeclined { OrderID = message.OrderID, ReasonCode = response.ReasonCode });
                }
            }
        }
    }
}
