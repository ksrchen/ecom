using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.server
{
    public class RecurringPaymentDeclinedHandler : IHandleMessages<RecurringPaymentDeclineEvent>
    {
        IBus _bus;

        public RecurringPaymentDeclinedHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(RecurringPaymentDeclineEvent message)
        {
            Console.WriteLine("Got RecurringPaymentDeclineEvent message");
        }
    }
}
