using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Payment
{
    public class PaymentGateway
    {

        public static PS.models.TransactionResponse Charge(Ecom.Model.Order order)
        {
            using (var client = GetClient(0))
            {
                var request = new PS.models.TransactionRequest
                {
                    TransactionType = PS.models.TransactionTypes.Sales,
                    PaymentToken = order.CreditCard.Token,
                    CreditCardType = order.CreditCard.Type,
                    Currency = "USD",
                    Amount = Convert2Curreny(order.TotalPrice),
                    TaxAmount = 0,
                    ReferenceNumber = "IID:" + order.OrderStatusID
                };

                FillInLevel2n3Data(request, order);

                var result = client.PostAsJsonAsync<PS.models.TransactionRequest>("api/transaction", request).Result;
                result.EnsureSuccessStatusCode();

                var formatters = new List<MediaTypeFormatter>() {
                    new JsonMediaTypeFormatter(),
                };

                var resp = result.Content.ReadAsAsync<PS.models.TransactionResponse>(formatters).Result;
                return resp;
            };

        }


        private static void FillInLevel2n3Data(PS.models.TransactionRequest request, Ecom.Model.Order order)
        {
            request.CustomerPO = order.OrderID.ToString();
            request.ShipTo = new PS.models.Contact
            {
                FirstName = order.CreditCard.Contact.FirstName,
                LastName = order.CreditCard.Contact.LastName,
                StreetLine1 = order.CreditCard.Contact.StreetLine1,
                StreetLine2 = order.CreditCard.Contact.StreetLine2,
                City = order.CreditCard.Contact.City,
                State = order.CreditCard.Contact.StateCode,
                PostalCode = order.CreditCard.Contact.PostalCode,
                Country = order.CreditCard.Contact.CountryCode,
                EmailAddress = order.CreditCard.Contact.Email,
            };

            request.LineItems = new List<PS.models.LineItem>();
            if (order != null)
            {
                foreach (var p in order.OrderLines)
                {
                   
                        var product = p.Product;
                        request.LineItems.Add(new PS.models.LineItem
                        {
                            CommodityCode = product.GLCode,
                            ProductCode = product.GLCode,
                            ProductDescription = product.Name,
                            ProductName = product.Name,
                            Quantity = (int)p.Quantity,
                            UnitPrice = Convert2Curreny(p.UnitPrice),
                            ProductSKU = product.GLCode,
                            TaxRate = p.TaxRate,
                            TaxAmount = Convert2Curreny(p.TaxAmount),
                        });
                   
                }
            }
        }
        private static int Convert2Curreny(decimal amount)
        {
            return (int)(Math.Round(amount, 2) * 100);
        }

        private static HttpClient GetClient(int companyId)
        {
            string login = @"LOOPNET\kchen";
            string password = @"1X)TS$CJHY";
            string baseUrl = @"https://mpps.maint.qa.loopnet.com";

            var parts = login.Split(new char[] { '\\' });
            var domain = parts[0];
            var userName = parts[1];

            var client = new HttpClient(
                new HttpClientHandler
                {
                    Credentials = new NetworkCredential(userName, password, domain)
                }
                    );

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        

    }
}
