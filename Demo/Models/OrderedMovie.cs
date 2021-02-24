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
        public decimal Price { get; set; }
        public int OrderID { get; set; }
    }
}
