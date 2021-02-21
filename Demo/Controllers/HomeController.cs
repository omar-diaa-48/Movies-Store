using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        SearchContainer<Movie>  movies;

        private async Task LoadListOfMovies()
        {
            movies = await MovieListLoadApi.LoadApi();
        }

        private readonly ILogger<HomeController> _logger;

        public  HomeController(ILogger<HomeController> logger)
        {

            ApiHelper.InitialClient();

            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await LoadListOfMovies();
            return View(movies);
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
