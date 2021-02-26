using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Demo.Models
{
    public class Order//Shopping Chart
    {
        private readonly MovieStoreDBContext context;
        public Order()
        {

        }
        public Order(MovieStoreDBContext _context)
        {
            context = _context;
        }

        [Key]
        public int ID { get; set; }

        public decimal TotalPrice { get; set; }

        //public List<OrderedMovie> OrderList { get; set; }

        public int CustomerID { get; set; }

        public virtual Customer Customer { get; set; }


        public virtual ICollection<OrderedMovie> MovieOrder { get; set; }
    }
}
