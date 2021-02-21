using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class Movie
    {

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; } 
        public string DirectorName { get; set; }
        public string MainActorName { get; set; }

        public virtual ICollection<MovieOrder> MovieOrder { get; set; }
    }

    public enum Genre
    {
        Action,
        Comedy,
        Thirller,
        Horror,
        Drama,
        Romance,
        Kids,
        Family
    }
}
