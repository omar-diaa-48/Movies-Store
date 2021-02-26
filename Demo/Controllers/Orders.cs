using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Demo.Controllers
{
    public class Orders : Controller
    {
        private readonly MovieStoreDBContext _moviestroedbcontext;
        private readonly Order _order;
        private readonly OrderServiceRepo _OrderServiceRepo;
        SearchContainer<SearchMovie> popularMovies, upComingMovies, topRatedMovies, nowPlayingMovies;

        public Orders(MovieStoreDBContext moviestroedbcontext, Order order)
        {
            this._moviestroedbcontext = moviestroedbcontext;
            this._order = order;
            _OrderServiceRepo = new OrderServiceRepo(moviestroedbcontext);
        }
        private async Task LoadListOfMovies()
        {
            popularMovies = await MovieListLoadApi.LoadApi("POPULAR");
            upComingMovies = await MovieListLoadApi.LoadApi("COMING SOON");
            topRatedMovies = await MovieListLoadApi.LoadApi("TOP RATED");
            nowPlayingMovies = await MovieListLoadApi.LoadApi("NOW PLAYING");
        }

        public IActionResult Index()//int CartID
        {
            var items = _OrderServiceRepo.GetShoppingCartItems(1);//edit
            _order.MovieOrder = items;
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ShoppingCart = _order,
                ShoppingCartTotal = _OrderServiceRepo.GetShoppingCartTotal(1)

            };
            return View(shoppingCartViewModel);
        }
        public async Task<RedirectToActionResult> AddToShoppingCart(int movieId)
        {
            await LoadListOfMovies();
            var searchResultModel = new SearchResultMovieModelView();
            InitializeModel(searchResultModel);
            var selectedmovie = searchResultModel.ResultList.Results.FirstOrDefault(e => e.Id == movieId);
            if (selectedmovie != null)
            {
                _OrderServiceRepo.AddToCart(selectedmovie);
            }
            return RedirectToAction("Index");
        }


        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int movieId)
        {
            await LoadListOfMovies();
            var searchResultModel = new SearchResultMovieModelView();
            InitializeModel(searchResultModel);
            var selectedmovie = searchResultModel.ResultList.Results.FirstOrDefault(e => e.Id == movieId);
            if (selectedmovie != null)
            {
                _OrderServiceRepo.RemoveFromCart(selectedmovie, 1);
            }
            return RedirectToAction("Index");
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
