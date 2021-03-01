using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Demo.Controllers
{
    public class MoviesController : Controller
    {
        SearchContainer<SearchMovie> popularMovies, upComingMovies, topRatedMovies, nowPlayingMovies;
        Movie movie;

        private async Task LoadListOfMovies()
        {
            popularMovies = await MovieListLoadApi.LoadApi("POPULAR");
            upComingMovies = await MovieListLoadApi.LoadApi("COMING SOON");
            topRatedMovies = await MovieListLoadApi.LoadApi("TOP RATED");
            nowPlayingMovies = await MovieListLoadApi.LoadApi("NOW PLAYING");
        }

        private async Task LoadMovie(int id)
        {
            movie = await MovieLoadApi.LoadApi(id);
        }

        public async Task<IActionResult> MovieDetails(int id)
        {
            await LoadMovie(id);
            return View(movie);
        }

        public async Task<IActionResult> Search(string condition, string search)
        {
            await LoadListOfMovies();

            var resultViewModel = new SearchResultMovieModelView();
            var returnViewModel = new SearchResultMovieModelView();

            InitializeModel(resultViewModel);

            if(search == null || string.IsNullOrWhiteSpace(search))
            {
                return View(resultViewModel);
            }


            if(condition == "Title")
            {
                foreach (var item in resultViewModel.ResultList.Results)
                    if (item.Title.ToLower().Contains(search.ToLower()))
                        if (!returnViewModel.ResultList.Results.Any(m=>m.Title == item.Title))
                            returnViewModel.ResultList.Results.Add(item);

            }
            else if(condition == "Vote")
            {
                if(double.TryParse(search, out double vote))
                {
                    if (vote >= 10 || vote <= 0)
                        vote = 0;

                    foreach (var item in resultViewModel.ResultList.Results)
                        if (item.VoteAverage >= vote)
                            if (!returnViewModel.ResultList.Results.Contains(item))
                                returnViewModel.ResultList.Results.Add(item);
                }
            }

            returnViewModel.ViewTitle = "Search Result";

            return View(returnViewModel);
        }

        public async Task<IActionResult> ListMoviesByCategory(string condition)
        {
            await LoadListOfMovies();

            var viewModel = new SearchResultMovieModelView();

            switch (condition)
            {
                case "Latest":
                    foreach (var item in nowPlayingMovies.Results)
                        viewModel.ResultList.Results.Add(item);
                    viewModel.ViewTitle = "Latest Movies";
                    break;
                case "Popular":
                    foreach (var item in topRatedMovies.Results)
                        viewModel.ResultList.Results.Add(item);
                    viewModel.ViewTitle = "Popular Movies";
                    break;
                case "Soon":
                    foreach (var item in upComingMovies.Results)
                        viewModel.ResultList.Results.Add(item);
                    viewModel.ViewTitle = "Coming Soon Movies";
                    break;
                default:
                    break;
            }
            return View("Search", viewModel);
        }

        public async Task<IActionResult> ListMoviesByGenre(int genre)
        {
            await LoadListOfMovies();

            var resultViewModel = new SearchResultMovieModelView();
            var returnViewModel = new SearchResultMovieModelView();

            InitializeModel(resultViewModel);

            foreach (var item in resultViewModel.ResultList.Results)
                if (item.GenreIds.Contains(genre))
                    returnViewModel.ResultList.Results.Add(item);

            returnViewModel.ViewTitle = "By Genre";

            return View("Search", returnViewModel);
        }

        public async Task<IActionResult> ListAllMovies()
        {
            await LoadListOfMovies();

            var returnViewModel = new SearchResultMovieModelView();

            InitializeModel(returnViewModel);

            returnViewModel.ViewTitle = "All Movies";

            return View("Search", returnViewModel);
        }

        private void InitializeModel(SearchResultMovieModelView model)
        {
            foreach (var item in popularMovies.Results)
                model.ResultList.Results.Add(item);
            foreach (var item in upComingMovies.Results)
                model.ResultList.Results.Add(item);
            foreach (var item in topRatedMovies.Results)
                model.ResultList.Results.Add(item);
            foreach (var item in nowPlayingMovies.Results)
                model.ResultList.Results.Add(item);
        }
    }
}
