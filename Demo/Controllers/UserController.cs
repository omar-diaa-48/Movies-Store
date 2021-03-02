using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace Demo.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MovieStoreDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IAuthorizationService _AuthorizationService;

        //private static PayPalCheckoutSdk.Orders.Order createOrderResult;

        public UserController(RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInMManager,
                                MovieStoreDBContext context,
                                IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInMManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> UserDetails()
        {
            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (customer == null)
                return RedirectToAction("Index", "Home");

            return View(customer);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);

            if (customer == null)
                return RedirectToAction("Index", "Home");

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser customer)
        {
            var customerToBeUpdated = await _userManager.FindByIdAsync(id);

            if (customerToBeUpdated == null)
                return RedirectToAction("Index", "Home");


            customerToBeUpdated.FirstName = customer.FirstName;
            customerToBeUpdated.LastName = customer.LastName;
            customerToBeUpdated.Email = customer.Email;
            customerToBeUpdated.Address = customer.Address;
            customerToBeUpdated.BirthDate = customer.BirthDate;
            customerToBeUpdated.Gender = customer.Gender;
            customerToBeUpdated.PhoneNumber = customer.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(customerToBeUpdated);

            if (updateResult.Succeeded)
                return RedirectToAction("Index", "Home");


            return View(customer);
        }

        public async Task<IActionResult> UpdatePassword(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);

            if (customer == null)
                return RedirectToAction("Edit", new { id = id });

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string id, string oldPassword, string newPassword, ApplicationUser customer)
        {
            var customerToBeUpdated = await _userManager.FindByIdAsync(customer.Id);

            if (customerToBeUpdated == null)
                return RedirectToAction("Edit", new { id = id });

            var changePasswordResult = await _userManager.ChangePasswordAsync(customerToBeUpdated, oldPassword, newPassword);

            if (changePasswordResult.Succeeded)
            {
                return RedirectToAction("Edit", new { id = id });
            }

            return RedirectToAction("UpdatePassword", new { id = id });
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            var ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ViewBag.ExternalLogins = ExternalLogins;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string password)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
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
        public async Task<IActionResult> Register(ApplicationUser AppUser, string Password, string adminRole)
        {
            IdentityResult roleResult = null;
            IdentityResult roleAssignResult = null;
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

            if (result.Succeeded )
            {

                var order = new Demo.Models.Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    CustomerID = newUser.Id
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                if (adminRole == "on")
                {
                    if (!await _roleManager.RoleExistsAsync("adminRole"))
                    {
                        roleResult = await _roleManager.CreateAsync(new IdentityRole("adminRole"));
                    }

                    if (roleResult?.Succeeded ?? true)
                    {
                        roleAssignResult = await _userManager.AddToRoleAsync(newUser, "adminRole");
                    }
                }

                if (result.Succeeded && (roleResult == null || roleResult.Succeeded))
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    var link = Url.Action(nameof(VerifyEmail), "User", new { userID = newUser.Id, code }, Request.Scheme, Request.Host.ToString());

                    await _emailService.SendAsync(newUser.Email, "email verify", $"<p>Please verify your email by clicking &nbsp; <a style = \"color: red; font-size: 30px;\" href = \"{link}\" > Here </a></p>", true);

                    return RedirectToAction("EmailVerification");
                }
                //var signInResult = await _signInManager.PasswordSignInAsync(newUser, Password, false, false);

                //if (signInResult.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home", new { area = "" });
                //}

            }

            return RedirectToAction("SignUp");
        }
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string userID, string code)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
