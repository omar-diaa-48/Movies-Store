using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Demo.Models
{
    public class MovieOrder
    {
        public int MovieID { get; set; }
        public int OrderID { get; set; }
        public Movie Movie { get; set; }
        public Order Order { get; set; }
    }
}
