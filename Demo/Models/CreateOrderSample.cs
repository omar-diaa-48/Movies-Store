using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

namespace Demo.Models
{
    public class CreateOrderSample
    {
        private static OrderRequest BuildRequestBody(PurchaseUnitRequest PUR)
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",

                ApplicationContext = new ApplicationContext
                {
                    BrandName = "Movie Store",
                    LandingPage = "BILLING",
                    CancelUrl = "https://localhost:44308/Home/Error",
                    ReturnUrl = "https://localhost:44308/User/aproved",
                    UserAction = "CONTINUE",
                    ShippingPreference = "SET_PROVIDED_ADDRESS"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>{PUR}
            };

            return orderRequest;
        }
        public async static Task<HttpResponse> CreateOrder(Demo.Models.Order order , bool debug = false)
        {
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(BuildRequestBody(ConvertOrderToItems(order)));
            var response = await PayPalClient.client().Execute(request);
            return response;
        }
        private static PurchaseUnitRequest ConvertOrderToItems(Demo.Models.Order order)
        {
            List<Item> items = new List<Item>();
            decimal totalTax=0;
            foreach(var movie in order.OrderedMovies)
            {
                Item i = new Item()
                {
                    Name = movie.Title,
                    Description = "Movie",
                    Sku = "sku" + movie.ID.ToString(),
                    UnitAmount = new Money
                    {
                        CurrencyCode = "USD",
                        Value = String.Format("{0:0.00}", movie.Price)
                    },
                    Tax = new Money
                    {
                        CurrencyCode = "USD",
                        Value = String.Format("{0:0.00}", movie.Tax)
                    },
                    Quantity = "1",
                    Category = "DIGITAL_GOODS"
                };
                totalTax += movie.Tax;
                items.Add(i);
            }
            return new PurchaseUnitRequest
            {
                ReferenceId = "PUHF",
                Description = "Entertainment",
                CustomId = "CUST-EntertainmentMovies",
                SoftDescriptor = "EntertainmentMovies",
                AmountWithBreakdown = new AmountWithBreakdown
                {
                    
                    CurrencyCode = "USD",
                    Value = String.Format("{0:0.00}",(order.TotalPrice + totalTax + 5)),
                    AmountBreakdown = new AmountBreakdown
                    {
                        ItemTotal = new Money
                        {
                            CurrencyCode = "USD",
                            Value = String.Format("{0:0.00}",order.TotalPrice)
                        },
                        Shipping = new Money
                        {
                            CurrencyCode = "USD",
                            Value = "5.00"
                        },
                        Handling = new Money
                        {
                            CurrencyCode = "USD",
                            Value = "0.00"
                        },
                        TaxTotal = new Money
                        {
                            CurrencyCode = "USD",
                            Value = String.Format("{0:0.00}",totalTax.ToString())
                        },
                        ShippingDiscount = new Money
                        {
                            CurrencyCode = "USD",
                            Value = "0.00"
                        }
                    }
                },
                Items = items,
                ShippingDetail = new ShippingDetail
                {
                    Name = new Name
                    {
                        FullName = "Omar Gamal"
                    },
                    AddressPortable = new AddressPortable
                    {
                        AddressLine1 = "123 Townsend St",
                        AddressLine2 = "Floor 6",
                        AdminArea2 = "San Francisco",
                        AdminArea1 = "CA",
                        PostalCode = "94107",
                        CountryCode = "US"
                    }
                }
            };
        }    
    }
}
