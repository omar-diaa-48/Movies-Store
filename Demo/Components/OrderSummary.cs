using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models;
namespace Demo.Components
{
    public class OrderSummary : ViewComponent
    {
        private readonly Order _order;
        public OrderSummary(Order order)
        {
            _order = order;
        }

        public IViewComponentResult Invoke(string id)
        {
            var items = _order.GetShoppingCartItems();
            _order.OrderedMovies = items;
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                Order = _order,
                OrderTotal = _order.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }
    }
}
