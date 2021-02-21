using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        static List<Movie> Movies = new List<Movie>
        {
            new Movie {Name="Ibrahim Elabyad", Genre=Genre.Action},
            new Movie {Name="Heen Maysara", Genre=Genre.Action},
            new Movie {Name="Elens we elgen", Genre=Genre.Action},
            new Movie {Name="Ala gamb yasta", Genre=Genre.Action},
            new Movie {Name="Debug", Genre=Genre.Action},
            new Movie {Name="Nemo", Genre=Genre.Action},
            new Movie {Name="Amazon", Genre=Genre.Family}
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(Movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
