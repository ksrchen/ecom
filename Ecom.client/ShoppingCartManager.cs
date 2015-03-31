using Ecom.messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.client
{
    public class ShoppingCartManager : IWantToRunWhenBusStartsAndStops
    {
        IBus bus;

        public ShoppingCartManager(IBus bus)
        {
            this.bus = bus;
        }


        public void Start()
        {
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                Guid id = Guid.NewGuid();

                var message = new CreateShoppingCart();
                bus.Send("Ecom.server", message);

                Console.WriteLine("Send a new PlaceOrder message with id: {0}", id.ToString("N"));
            }

        }

        public void Stop()
        {
            
        }
    }
}
