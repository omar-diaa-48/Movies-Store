using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class OrderedMovie   //items
    {
        private decimal tax;
        private decimal price;

        [Key]
        public int ID { get; set; } //Shopping Cart Item ID

        public string Title { get; set; }
        //public int CountItems { get; set; }//amount
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                tax = (price * 3) / 100;
            }
        }
        public decimal Tax { get => tax; }
        public string OrderId { get; set; }//Shoppingcart ID 
        public int MovieId { get; set; }
        public int Amount { get; set; }

        public virtual Order Order { get; set; }
    }
}
