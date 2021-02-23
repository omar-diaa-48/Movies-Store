using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Demo.Models
{
    public class MoviesListsViewModel
    {
        public SearchContainer<SearchMovie> PopularMovies { get; set; }
        public SearchContainer<SearchMovie> UpComingMovies { get; set; }
        public SearchContainer<SearchMovie> TopRatedMovies { get; set; }
        public SearchContainer<SearchMovie> NowPlayingMovies { get; set; }
    }
}
