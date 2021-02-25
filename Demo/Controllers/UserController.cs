using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Demo.Controllers
{
    public class UserController : Controller
    {
        private static PayPalCheckoutSdk.Orders.Order createOrderResult;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult BuyMovies()
        {
            Demo.Models.Order order = new Demo.Models.Order
            {
                CustomerID = 1,
                Customer = new Customer()
                {
                    Address = "Focak many",
                    ID = 1,
                },
                ID = 1,
                MovieOrder = new List<OrderedMovie>() {
                    new OrderedMovie() {
                        ID = 1,
                        MovieID = 1,
                        OrderID = 1,
                        Price = 50,
                        Title = "Car1"
                    },
                    new OrderedMovie() {
                        ID = 2,
                        MovieID = 2,
                        OrderID = 1,
                        Price = 60,
                        Title = "Car2"
                    },
                    new OrderedMovie() {
                        ID = 3,
                        MovieID = 5,
                        OrderID = 1,
                        Price = 100,
                        Title = "Tent"
                    }
                },
                TotalPrice = 210
            };
            var createOrderResponse = CreateOrderSample.CreateOrder(order, true).Result;
            createOrderResult = createOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            return Redirect(createOrderResult.Links[1].Href);
        }
        public IActionResult aproved()
        {
            var captureOrderResponse = CaptureOrderSample.CaptureOrder(createOrderResult.Id, true).Result;
            var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            return RedirectToAction("Index", new { Controller = "Home" });
        }
    }
}
