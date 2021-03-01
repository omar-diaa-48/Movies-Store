using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using Demo.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class Order//Shopping Chart
    {
        private readonly MovieStoreDBContext _appDbContext;

        private Order(MovieStoreDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Order()
        {

        }

        [Key]
        public string OrderId { get; set; }

        public string CustomerID { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderedMovie> OrderedMovies { get; set; }









        public static Order Getorder(IServiceProvider services)
        {

            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MovieStoreDBContext>();


            string cartId = "";
            bool NewCartNeeded;

            if (session.GetString("CartId") == null)
            {
                NewCartNeeded = true;
                cartId = Guid.NewGuid().ToString();
                session.SetString("CartId", cartId);
            }
            else
            {
                NewCartNeeded = false;
                cartId = session.GetString("CartId");
            }
            //  string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();



            if (NewCartNeeded)
            {

                var addedcart = new Order(context) { OrderId = cartId };

                context.Orders.Add(addedcart);
                context.SaveChanges();
                return addedcart;
            }
            else
            {
                var ExistingCart = context.Orders.FirstOrDefault(s => s.OrderId == cartId);
                return ExistingCart;
            }


            //  return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Movie movie)
        {
            int mymovieid = movie.Id;

            //var myordermovie =
            //        _appDbContext.OrderedMovies.Include(s => s.Order).SingleOrDefault(
            //            s => s.MovieId == mymovieid && s.Order.OrderId == OrderId);

            var myordermovie = MovieToOrderedMovie(movie);

            if (OrderedMovies.Any(o => o.Title == myordermovie.Title))
                OrderedMovies.FirstOrDefault(o => o.Title == myordermovie.Title).Amount++;
            else
                OrderedMovies.Add(myordermovie);

            //var shoppingCartItem =
            //        _appDbContext.ShoppingCartItems.SingleOrDefault(
            //            s => s.Movie.Id == movie.Id && s.ShoppingCartId == ShoppingCartId);

            //if (myordermovie == null)
            //{
            //    myordermovie = new OrderedMovie
            //    {
            //        OrderId = OrderId,
            //        MovieId = mymovieid,
            //        //Movie = movie,
            //        Amount = 1
            //    };

            //    _appDbContext.OrderedMovies.Add(myordermovie);
            //}
            //else
            //{
            //    myordermovie.Amount++;
            //}
            //_appDbContext.SaveChanges();
        }

        public int RemoveFromCart(SearchMovie movie)
        {
            //int mymovieid = movie.Id;
            //var shoppingCartItem =
            //        _appDbContext.OrderedMovies.SingleOrDefault(
            //            s => s.MovieId == mymovieid && s.OrderId == OrderId);
            //var shoppingCartItem =
            //        _appDbContext.ShoppingCartItems.SingleOrDefault(
            //            s => s.Movie.Id == movie.Id && s.ShoppingCartId == ShoppingCartId);

            if (OrderedMovies.Any(o => o.MovieId == movie.Id))
                OrderedMovies.FirstOrDefault(o => o.MovieId == movie.Id).Amount--;
            else
                OrderedMovies.Remove(OrderedMovies.FirstOrDefault(o => o.MovieId == movie.Id));

            //if (shoppingCartItem != null)
            //{
            //    if (shoppingCartItem.Amount > 1)
            //    {
            //        shoppingCartItem.Amount--;
            //        localAmount = shoppingCartItem.Amount;
            //    }
            //    else
            //    {
            //        _appDbContext.OrderedMovies.Remove(shoppingCartItem);
            //    }
            //}

            //_appDbContext.SaveChanges();

            return OrderedMovies.FirstOrDefault(o=>o.MovieId == movie.Id).Amount;
        }

        public List<OrderedMovie> GetShoppingCartItems()
        {
            return OrderedMovies ??
                   (OrderedMovies =
                       _appDbContext.OrderedMovies.Where(c => c.OrderId == OrderId)
                           //.Include(s => s.movititle)
                           .ToList());
        }

        private OrderedMovie MovieToOrderedMovie(Movie movie)
        {
            return new OrderedMovie
            {
                MovieId = movie.Id,
                Title = movie.Title,
                Amount = 1,
                Price = 10,
            };
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .OrderedMovies
                .Where(cart => cart.OrderId == OrderId);

            _appDbContext.OrderedMovies.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.OrderedMovies.Where(c => c.OrderId == OrderId)
                .Select(c => 10 * c.Amount).Sum();
            return total;
        }

        public virtual ApplicationUser Customer { get; set; }

    }
}