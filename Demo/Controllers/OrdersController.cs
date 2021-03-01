using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Microsoft.AspNetCore.Identity;

namespace Demo.Controllers
{
    public class OrdersController : Controller
    {
        Movie movie;
        private readonly MovieStoreDBContext _moviestroedbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private static PayPalCheckoutSdk.Orders.Order createOrderResult;
        private readonly Order _order;

        SearchContainer<SearchMovie> popularMovies, upComingMovies, topRatedMovies, nowPlayingMovies;

        public OrdersController(MovieStoreDBContext moviestroedbcontext,
                                    UserManager<ApplicationUser> userManager,
                                    Order order)
        {
            _moviestroedbcontext = moviestroedbcontext;
            _userManager = userManager;
            _order = order;
        }

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

        public async Task<IActionResult> Index()//int CartID
        {
            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            _order.OrderedMovies = _order.GetShoppingCartItems(customer.Id);//edit

            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                Order = _order,
                OrderTotal = _order.GetShoppingCartTotal()

            };
            return View(shoppingCartViewModel);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int movieId)
        {
            await LoadMovie(movieId);

            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (movie != null)
                _order.AddToCart(movie, customer.Id);

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
                _order.RemoveFromCart(selectedmovie);
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

        public void BuyMovies( Demo.Models.Order order)
        {

            var createOrderResponse = CreateOrderSample.CreateOrder(order, true).Result;
            createOrderResult = createOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            Redirect(createOrderResult.Links[1].Href);
        }

        public IActionResult Approved()
        {
            var captureOrderResponse = CaptureOrderSample.CaptureOrder(createOrderResult.Id, true).Result;
            var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();
            return RedirectToAction("Index", new { Controller = "Home" });
        }
    }
}
