using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Billing
{
    public class Bootstrapper : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Type the order ID and then press Enter to publish the CC decline event");
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                int orderID = 0;
                if (int.TryParse(line, out orderID))
                {

                    Bus.Publish<RecurringPaymentDeclineEvent>(m =>
                    {
                        m.OrderID = orderID;
                    });
                    Console.WriteLine("Published RecurringPaymentDeclineEvent with orderId {0}", orderID);
                }
            }
        }

        public void Stop()
        {
        }
    }
}
