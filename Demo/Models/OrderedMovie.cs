using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class OrderedMovie
    {
        public int ID { get; set; }
        public int MovieID { get; set; }
        public string Title { get; set; }
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
        public int OrderID { get; set; }
    }
}
