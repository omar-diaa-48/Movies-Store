using Demo.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private static PayPalCheckoutSdk.Orders.Order createOrderResult;

        public UserController(UserManager<ApplicationUser> userManager, 
                                SignInManager<ApplicationUser> signInMManager)
        {
            _userManager = userManager;
            _singInManager = signInMManager;

        }
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

        public IActionResult Approved()
        {
            var captureOrderResponse = CaptureOrderSample.CaptureOrder(createOrderResult.Id, true).Result;
            var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            return RedirectToAction("Index", new { Controller = "Home" });
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string password)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                var signInResult = await _singInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult Register()
        {
            return View("SignUp");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string UserName, string Password)
        {
            var user = new ApplicationUser
            {
                UserName = UserName,
            };
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                //Sign in here 
                var signInResult = await _singInManager.PasswordSignInAsync(user, Password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> LogOut()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
