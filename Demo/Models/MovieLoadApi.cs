using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;
namespace Demo.Models
{
    public class MovieLoadApi
    {
        public static async Task<Movie> LoadApi(int movieID = 0)
        {
            string url = $" https://api.themoviedb.org/3/movie/{movieID}?api_key=774138a66b45c3a757f0402c916b6966&language=en-US";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    Movie movie = await response.Content.ReadAsAsync<Movie>();
                    return movie;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            };
        }
    }
}
