using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.messages
{
    public class FulfillmentComplete : IMessage
    {
        public int OrderID { get; set; }
    }
}
