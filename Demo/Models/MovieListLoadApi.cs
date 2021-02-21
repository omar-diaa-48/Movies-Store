using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
namespace Demo.Models
{
    public class MovieListLoadApi
    {
        public static async Task<SearchContainer<Movie>> LoadApi()
        {
            string url = $"https://api.themoviedb.org/3/movie/upcoming?api_key=774138a66b45c3a757f0402c916b6966&language=en-US&page=1";


            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    SearchContainer<Movie> MoviesList = await response.Content.ReadAsAsync<SearchContainer<Movie>>();
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
