using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.server
{
    public class ShoppingCartHandler : IHandleMessages<CreateShoppingCart>
    {
        IBus _bus;

        public ShoppingCartHandler (IBus bus)
        {
            _bus = bus;
        }
        public void Handle(CreateShoppingCart message)
        {
            Console.WriteLine("Got CreateShoppingCart message");
        }

       
    }
}
