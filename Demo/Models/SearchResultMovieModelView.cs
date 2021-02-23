using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Demo.Models
{
    public class SearchResultMovieModelView 
    {
        public SearchResultMovieModelView()
        {
            ResultList = new SearchContainer<SearchMovie>();
            ResultList.Results = new List<SearchMovie>();
        }
        public SearchContainer<SearchMovie> ResultList { get; set; }
    }
}
