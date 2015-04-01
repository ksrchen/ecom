using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecom.web.Models
{
    public class CheckoutRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }

        public int shoppingCardId { get; set; }
        public string creditCardToken { get; set; }
        public string creditCardType { get; set; }
        public string creditCardExpirationMonth { get; set; }
        public string creditCardExpirationYear { get; set; }
        public string creditCardNumber { get; set; }

    }
}