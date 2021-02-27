using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class OrderedMovie   //items
    {
        [Key]
        public int ID { get; set; } //Shopping Cart Item ID
        public int MovieID { get; set; }
        public string Title { get; set; }
        public  int CountItems { get; set; }//amount
        private decimal price;
        public decimal Price { 
            get=>price;
            set
            {
                price = value;
                tax = (price * 3) / 100;
            }
        }
        private decimal tax;
        public decimal Tax { get=>tax;}
        public int OrderID { get; set; }//Shoppingcart ID 
    }
}
