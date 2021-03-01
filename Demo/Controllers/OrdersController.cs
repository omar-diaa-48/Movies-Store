﻿using Demo.Models;
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
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class OrdersController : Controller
    {
        Movie movie;
        private readonly MovieStoreDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static PayPalCheckoutSdk.Orders.Order createOrderResult;
        private readonly Order _order;

        SearchContainer<SearchMovie> popularMovies, upComingMovies, topRatedMovies, nowPlayingMovies;

        public OrdersController(MovieStoreDBContext context,
                                    UserManager<ApplicationUser> userManager,
                                    Order order)
        {
            _context = context;
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
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel();

            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            if(customer != null)
            {
                var order = _context.Orders.FirstOrDefault(o => o.CustomerID == customer.Id);

                order.OrderedMovies = _context.OrderedMovies.Where(o => o.OrderId == order.OrderId).ToList();

                shoppingCartViewModel.Order = order;
                shoppingCartViewModel.OrderTotal = order.GetShoppingCartTotal();
            }
            else
            {
                _order.OrderedMovies = _order.GetShoppingCartItems(customer.Id);//edit

                shoppingCartViewModel = new ShoppingCartViewModel()
                {
                    Order = _order,
                    OrderTotal = _order.GetShoppingCartTotal()

                };
            }


            return View(shoppingCartViewModel);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int movieId)
        {
            var flag = false;

            await LoadMovie(movieId);

            var customer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (movie != null)
            {
                if(customer != null)
                {
                    var orderedMovie = MovieToOrderedMovie(movie);
                    var order = _context.Orders.Include(o => o.OrderedMovies).FirstOrDefault(o => o.CustomerID == customer.Id);

                    foreach (var item in order.OrderedMovies)
                        if (item.MovieId == orderedMovie.MovieId)
                        {
                            item.Amount++;
                            await _context.SaveChangesAsync();
                            flag = true;
                        }


                    if (orderedMovie != null && order != null && !flag)
                    {
                        orderedMovie.OrderId = order.OrderId;
                        _context.OrderedMovies.Add(orderedMovie);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {

                }
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

        public OrderedMovie MovieToOrderedMovie(Movie movie)
        {
            return new OrderedMovie
            {
                MovieId = movie.Id,
                Title = movie.Title,
                Amount = 1,
                Price = 10,
            };
        }

        public void Checkout(string orderId)
        {
            var order = _context.Orders.Include(o => o.OrderedMovies).FirstOrDefault(o => o.OrderId == orderId);

            if(order != null)
                BuyMovies(order);
        }

        public void BuyMovies(Demo.Models.Order order)
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
