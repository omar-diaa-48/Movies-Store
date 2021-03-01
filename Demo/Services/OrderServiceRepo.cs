//using Demo.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TMDbLib.Objects.Search;

//namespace Demo.Services
//{
//    public class OrderServiceRepo : IOrderRepository
//    {
//        private readonly MovieStoreDBContext context;

//        public OrderServiceRepo(MovieStoreDBContext _context)
//        {
//            context = _context;
//        }
//        public void AddToCart(SearchMovie movie)
//        {
//            //check if customer add the same item in shoppingcart before 
//            OrderedMovie orderedMovie = context.OrderedMovies.SingleOrDefault(m => m.MovieID == movie.Id /*&& m.ID == CartId*/);//shopping
//            if (orderedMovie == null)
//            {
//                orderedMovie = new OrderedMovie()
//                {
                    
//                    MovieID = movie.Id,
//                    CountItems = 1,
//                    Price = 20,
//                    Title = "ahmed",
//                    OrderID=2
//                };
//                context.OrderedMovies.Add(orderedMovie);
//            }
//            else
//            {
//                orderedMovie.CountItems++;
//            }
//            //add ordered movie to ordersmovie table
//            context.SaveChanges();

//        }
//        public int RemoveFromCart(SearchMovie movie, int CartId)
//        {
//            var localAmount = 0;
//            OrderedMovie orderedMovie = context.OrderedMovies.SingleOrDefault(m => m.MovieID == movie.Id && m.ID == CartId);//shopping

//            if (orderedMovie != null)
//            {
//                if (orderedMovie.CountItems > 1)
//                {
//                    orderedMovie.CountItems--;
//                    //current count of this item in db
//                    localAmount = orderedMovie.CountItems;
//                }
//                else
//                {
//                    context.OrderedMovies.Remove(orderedMovie);
//                }

//            }
//            context.SaveChanges();
//            return localAmount;
//        }
//        public void ClearCart(int CartId)
//        {
//            var cartMovies = context.OrderedMovies.Where(c => c.ID == CartId);
//            context.OrderedMovies.RemoveRange(cartMovies);
//            context.SaveChanges();
//        }

//        public decimal GetShoppingCartTotal(int CartId)
//        {
//            // consider price 10 for all movies
//            var totalPrice = context.OrderedMovies.Where(c => c.ID == CartId)
//                                  .Select(c => c.CountItems * 10)
//                                  .Sum();
//            return totalPrice;
//        }

//        public static Order Getorder(IServiceProvider services)
//        {
//            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
//            var context = services.GetService<MovieStoreDBContext>();
//            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
//            //set key value pair for session
//            session.SetString("CartId", cartId);
//            var status = int.TryParse(cartId, out int orderId);
//            return new Order(context) { ID = orderId };
//            // need to edit
//        }

//        public List<OrderedMovie> GetShoppingCartItems(int CartId)
//        { 
//            var OrderMovies = context.OrderedMovies.Where(c => c.ID == CartId).ToList();
//            return OrderMovies;
//        }
//    }
//}
