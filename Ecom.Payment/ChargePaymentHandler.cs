using Ecom.messages;
using NServiceBus;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Payment
{
    public class ChargePaymentSagaData : ContainSagaData
    {
        [Unique]
        public int OrderID { get; set; }
        public int RetryCount { get; set; }
    }

    public class RetryCharge 
    {
        public int OrderID { get; set; }
        public string OriginalReasonCode { get; set; }
    }

    public class ChargePaymentHandler : Saga<ChargePaymentSagaData>,
        IAmStartedByMessages<ChargePayment>,
        IHandleTimeouts<RetryCharge>
    {
        IBus _bus;

        public ChargePaymentHandler(IBus bus)
        {
            _bus = bus;
        }
        public void Handle(ChargePayment message)
        {
            Console.WriteLine("Got charge payment message");
            Data.OrderID = message.OrderID;
            Data.RetryCount = 0;

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

                    int Code = int.Parse(response.ReasonCode);
                    if (Code >= 200 && Code < 300)  // decline: going to retry the next day
                    {
                        RequestTimeout<RetryCharge>(TimeSpan.FromSeconds(5), new RetryCharge { OrderID = order.OrderID, OriginalReasonCode = response.ReasonCode });
                    }
                    else
                    {
                        _bus.Reply(new PaymentDeclined { OrderID = message.OrderID, ReasonCode = response.ReasonCode });
                        MarkAsComplete();
                    }
                }
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ChargePaymentSagaData> mapper)
        {
            mapper.ConfigureMapping<ChargePayment>(s => s.OrderID).ToSaga(m => m.OrderID);
            mapper.ConfigureMapping<RetryCharge>(s => s.OrderID).ToSaga(m => m.OrderID);
        }

        public void Timeout(RetryCharge state)
        {
            if (Data.RetryCount++ < 3)
            {
                Console.WriteLine("payment retrying to charge {0}", Data.RetryCount);
                RequestTimeout<RetryCharge>(TimeSpan.FromSeconds(5), state);
            }
            else
            {
                Console.WriteLine("payment declined {0}, reason code {1}", state.OrderID, state.OriginalReasonCode);
                ReplyToOriginator(new PaymentDeclined { OrderID = state.OrderID, ReasonCode = state.OriginalReasonCode });
                MarkAsComplete();
            }
        }
    }
}
