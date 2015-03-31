using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.messages
{
    public class CreateShoppingCart : ICommand
    {
        public ShoppingCart cart { get; set; }
    }
}
