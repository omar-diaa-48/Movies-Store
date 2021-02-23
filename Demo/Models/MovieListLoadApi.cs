using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Demo.Models
{
    public class MovieListLoadApi
    {
        public static async Task<SearchContainer<SearchMovie>> LoadApi(string required)
        {
            string url = "";
            switch (required)
            {
                case "POPULAR":
                    url = $"https://api.themoviedb.org/3/movie/popular?api_key=774138a66b45c3a757f0402c916b6966&language=en-US";
                    break;
                case "COMING SOON":
                    url = $"https://api.themoviedb.org/3/movie/upcoming?api_key=774138a66b45c3a757f0402c916b6966&language=en-US";
                    break;
                case "TOP RATED":
                    url = $"https://api.themoviedb.org/3/movie/top_rated?api_key=774138a66b45c3a757f0402c916b6966&language=en-US";
                    break;
                case "NOW PLAYING":
                    url = $"https://api.themoviedb.org/3/movie/now_playing?api_key=774138a66b45c3a757f0402c916b6966&language=en-US";
                    break;
                default:
                    break;
            }


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    SearchContainer<SearchMovie> MoviesList = await response.Content.ReadAsAsync<SearchContainer<SearchMovie>>();
                    return MoviesList;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            };
        }
    }
}
