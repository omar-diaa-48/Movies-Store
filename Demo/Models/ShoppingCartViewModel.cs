using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ShoppingCartViewModel
    {
        public Order ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
    }
}
