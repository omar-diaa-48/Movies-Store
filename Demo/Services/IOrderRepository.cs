using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace Demo.Services
{
    public interface IOrderRepository
    {
        public void AddToCart(SearchMovie movie);
        public int RemoveFromCart(SearchMovie movie, int CartId);
        public void ClearCart(int CartId);
        public decimal GetShoppingCartTotal(int CartId);
        public List<OrderedMovie> GetShoppingCartItems(int CartId);

    }
}
