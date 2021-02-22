using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Demo.Controllers
{
    public class MoviesController : Controller
    {
        Movie movie;

        private async Task LoadMovie(int id)
        {
            movie = await MovieLoadApi.LoadApi(id);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MovieDetails(int id)
        {
            await LoadMovie(id);
            return View(movie);
        }
    }
}
