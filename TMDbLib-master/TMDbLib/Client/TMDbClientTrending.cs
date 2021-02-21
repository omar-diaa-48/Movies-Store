﻿using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<SearchContainer<SearchMovie>> GetTrendingMoviesAsync(TimeWindow timeWindow, int page = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("trending/movie/{time_window}");
            req.AddUrlSegment("time_window", timeWindow.ToString());

            if (page >= 1)
                req.AddQueryString("page", page.ToString());

            RestResponse<SearchContainer<SearchMovie>> resp = await req.ExecuteGet<SearchContainer<SearchMovie>>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<SearchTv>> GetTrendingTvAsync(TimeWindow timeWindow, int page = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("trending/tv/{time_window}");
            req.AddUrlSegment("time_window", timeWindow.ToString());

            if (page >= 1)
                req.AddQueryString("page", page.ToString());

            RestResponse<SearchContainer<SearchTv>> resp = await req.ExecuteGet<SearchContainer<SearchTv>>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<SearchPerson>> GetTrendingPeopleAsync(TimeWindow timeWindow, int page = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("trending/person/{time_window}");
            req.AddUrlSegment("time_window", timeWindow.ToString());

            if (page >= 1)
                req.AddQueryString("page", page.ToString());

            RestResponse<SearchContainer<SearchPerson>> resp = await req.ExecuteGet<SearchContainer<SearchPerson>>(cancellationToken).ConfigureAwait(false);

            return resp;
        }
    }
}