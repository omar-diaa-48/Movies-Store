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

        public async Task<IActionResult> BuyMovies()
        {
            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            var order = new Demo.Models.Order
            {
                CustomerID = customer.Id,
                Customer = customer,
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

            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View("SignUp");
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser AppUser, string Password)
        {
            var newUser = new ApplicationUser
            {
                UserName = AppUser.UserName,
                Email = AppUser.Email,
                FirstName = AppUser.FirstName,
                LastName = AppUser.LastName,
                PhoneNumber = AppUser.PhoneNumber,
                Address = AppUser.Address,
                Gender = AppUser.Gender,
                BirthDate = AppUser.BirthDate
            };

            var result = await _userManager.CreateAsync(newUser, Password);

            if (result.Succeeded)
            {
                //Sign in here 
                var signInResult = await _singInManager.PasswordSignInAsync(newUser, Password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return RedirectToAction("SignUp");
        }

        public async Task<IActionResult> LogOut()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
