using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.messages
{
    public class ChargePayment : ICommand
    {
        public Ecom.Model.Order Order { get; set; }
    }
}
