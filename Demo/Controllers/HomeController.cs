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
using TMDbLib.Objects.Search;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        SearchContainer<SearchMovie> popularMovies, upComingMovies, topRatedMovies, nowPlayingMovies;

        private readonly ILogger<HomeController> _logger;

        public  HomeController(ILogger<HomeController> logger)
        {

            ApiHelper.InitialClient();

            _logger = logger;
        }

        private async Task LoadListOfMovies()
        {
            popularMovies = await MovieListLoadApi.LoadApi("POPULAR");
            upComingMovies = await MovieListLoadApi.LoadApi("COMING SOON");
            topRatedMovies = await MovieListLoadApi.LoadApi("TOP RATED");
            nowPlayingMovies = await MovieListLoadApi.LoadApi("NOW PLAYING");
        }


        public async Task<IActionResult> Index()
        {
            await LoadListOfMovies();
            var viewModel = new MoviesListsViewModel
            {
                PopularMovies = popularMovies,
                UpComingMovies = upComingMovies,
                TopRatedMovies = topRatedMovies,
                NowPlayingMovies = nowPlayingMovies
            };
            return View(viewModel);
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
