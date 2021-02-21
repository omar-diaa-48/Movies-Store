﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.TvShows.Cast;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    public class ClientTvShowTests : TestBase
    {
        private static Dictionary<TvShowMethods, Func<TvShow, object>> _methods;

        public ClientTvShowTests()
        {
            _methods = new Dictionary<TvShowMethods, Func<TvShow, object>>
            {
                [TvShowMethods.Credits] = tvShow => tvShow.Credits,
                [TvShowMethods.Images] = tvShow => tvShow.Images,
                [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
                [TvShowMethods.ContentRatings] = tvShow => tvShow.ContentRatings,
                [TvShowMethods.AlternativeTitles] = tvShow => tvShow.AlternativeTitles,
                [TvShowMethods.Keywords] = tvShow => tvShow.Keywords,
                [TvShowMethods.Changes] = tvShow => tvShow.Changes,
                [TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates,
                [TvShowMethods.Recommendations] = tvShow => tvShow.Recommendations,
                [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
                [TvShowMethods.WatchProviders] = tvShow => tvShow.WatchProviders
            };
        }

        [Fact]
        public void TestTvShowExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / known_for", " / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / images", " / keywords", " / similar", " / translations", " / videos", " / genre_ids", " / recommendations");

            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.BreakingBad).Result;

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public void TestTvShowExtrasAll()
        {
            IgnoreMissingJson(" / id");
            IgnoreMissingJson(" / genre_ids", " / known_for", " / similar", " / translations", " / videos", "alternative_titles / id", "content_ratings / id", "credits / id", "external_ids / id", "keywords / id", " / recommendations");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.BreakingBad, combinedEnum).Result;

            TestBreakingBadBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        [Fact]
        public void TestTvShowSeparateExtrasCredits()
        {
            Credits credits = Config.Client.GetTvShowCreditsAsync(IdHelper.BreakingBad).Result;

            Assert.NotNull(credits);
            Assert.NotNull(credits.Cast);
            Assert.Equal(IdHelper.BreakingBad, credits.Id);

            Cast castPerson = credits.Cast[0];
            Assert.Equal("Walter White", castPerson.Character);
            Assert.Equal("52542282760ee313280017f9", castPerson.CreditId);
            Assert.Equal(17419, castPerson.Id);
            Assert.Equal("Bryan Cranston", castPerson.Name);
            Assert.NotNull(castPerson.ProfilePath);
            Assert.Equal(0, castPerson.Order);

            Assert.NotNull(credits.Crew);

            Crew crewPerson = credits.Crew.FirstOrDefault(s => s.Id == 66633);
            Assert.NotNull(crewPerson);

            Assert.Equal(66633, crewPerson.Id);
            Assert.Equal("52542287760ee31328001af1", crewPerson.CreditId);
            Assert.Equal("Production", crewPerson.Department);
            Assert.Equal("Vince Gilligan", crewPerson.Name);
            Assert.Equal("Executive Producer", crewPerson.Job);
            Assert.NotNull(crewPerson.ProfilePath);
        }

        [Fact]
        public void TestTvShowSeparateExtrasExternalIds()
        {
            ExternalIdsTvShow externalIds = Config.Client.GetTvShowExternalIdsAsync(IdHelper.GameOfThrones).Result;

            Assert.NotNull(externalIds);
            Assert.Equal(1399, externalIds.Id);
            Assert.Equal("/en/game_of_thrones", externalIds.FreebaseId);
            Assert.Equal("/m/0524b41", externalIds.FreebaseMid);
            Assert.Equal("tt0944947", externalIds.ImdbId);
            Assert.Equal("24493", externalIds.TvrageId);
            Assert.Equal("121361", externalIds.TvdbId);
            Assert.Equal("GameOfThrones", externalIds.FacebookId);
            Assert.Equal("GameOfThrones", externalIds.TwitterId);
            Assert.Equal("gameofthrones", externalIds.InstagramId);
        }

        [Fact]
        public void TestTvShowSeparateExtrasContentRatings()
        {
            ResultContainer<ContentRating> contentRatings = Config.Client.GetTvShowContentRatingsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(contentRatings);
            Assert.Equal(IdHelper.BreakingBad, contentRatings.Id);

            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.NotNull(contentRating);
            Assert.Equal("TV-MA", contentRating.Rating);
        }

        [Fact]
        public void TestTvShowSeparateExtrasAlternativeTitles()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = Config.Client.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(alternativeTitles);
            Assert.Equal(IdHelper.BreakingBad, alternativeTitles.Id);

            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("IT"));
            Assert.NotNull(alternativeTitle);
            Assert.Equal("Breaking Bad - Reazioni collaterali", alternativeTitle.Title);
        }

        [Fact]
        public void TestTvShowSeparateExtrasKeywords()
        {
            ResultContainer<Keyword> keywords = Config.Client.GetTvShowKeywordsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(keywords);
            Assert.Equal(IdHelper.BreakingBad, keywords.Id);

            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id == 41525);
            Assert.NotNull(keyword);
            Assert.Equal("high school teacher", keyword.Name);
        }

        [Fact]
        public void TestTvShowSeparateExtrasTranslations()
        {
            TranslationsContainerTv translations = Config.Client.GetTvShowTranslationsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(translations);
            Assert.Equal(IdHelper.BreakingBad, translations.Id);

            Translation translation = translations.Translations.FirstOrDefault(r => r.Iso_639_1 == "hr");
            Assert.NotNull(translation);
            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public void TestTvShowSeparateExtrasVideos()
        {
            ResultContainer<Video> videos = Config.Client.GetTvShowVideosAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(videos);
            Assert.Equal(IdHelper.BreakingBad, videos.Id);

            Video video = videos.Results.FirstOrDefault(r => r.Id == "5759db2fc3a3683e7c003df7");
            Assert.NotNull(video);

            Assert.Equal("5759db2fc3a3683e7c003df7", video.Id);
            Assert.Equal("en", video.Iso_639_1);
            Assert.Equal("XZ8daibM3AE", video.Key);
            Assert.Equal("Trailer", video.Name);
            Assert.Equal("YouTube", video.Site);
            Assert.Equal(720, video.Size);
            Assert.Equal("Trailer", video.Type);
        }

        [Fact]
        public void TestTvShowSeparateExtrasAccountState()
        {
            IgnoreMissingJson(" / id", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", " / videos", " / recommendations");

            // Test the custom parsing code for Account State rating
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            TvShow show = Config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            if (show.AccountStates == null || !show.AccountStates.Rating.HasValue)
            {
                Config.Client.TvShowSetRatingAsync(IdHelper.BigBangTheory, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                show = Config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            }

            Assert.NotNull(show.AccountStates);
            Assert.True(show.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(show.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public void TestTvShowSeparateExtrasImages()
        {
            ImagesWithId images = Config.Client.GetTvShowImagesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Backdrops);
            Assert.NotNull(images.Posters);
        }

        [Fact]
        public void TestTvShowGetImagesWithImageLanguage()
        {
            ImagesWithId resp = Config.Client.GetTvShowImagesAsync(IdHelper.BreakingBad, language: "en-US", includeImageLanguage: "en").Result;

            Assert.True(resp.Backdrops.Count > 0);
            Assert.True(resp.Posters.Count > 0);
        }

        [Fact]
        public void TestTvShowGetTvShowWithImageLanguage()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / content_ratings", " / genre_ids", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            TvShow resp = Config.Client.GetTvShowAsync(IdHelper.BreakingBad, language: "en-US", includeImageLanguage: "en", extraMethods: TvShowMethods.Images).Result;

            Assert.True(resp.Images.Backdrops.Count > 0);
            Assert.True(resp.Images.Backdrops.All(b => b.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
            Assert.True(resp.Images.Posters.Count > 0);
            Assert.True(resp.Images.Posters.All(p => p.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
        }

        private void TestBreakingBadBaseProperties(TvShow tvShow)
        {
            Assert.NotNull(tvShow);
            Assert.Equal("Breaking Bad", tvShow.Name);
            Assert.Equal("Breaking Bad", tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.Homepage);
            Assert.Equal(new DateTime(2008, 01, 19), tvShow.FirstAirDate);
            Assert.Equal(new DateTime(2013, 9, 29), tvShow.LastAirDate);
            Assert.Equal(false, tvShow.InProduction);
            Assert.Equal("Ended", tvShow.Status);
            Assert.Equal("Scripted", tvShow.Type);
            Assert.Equal("en", tvShow.OriginalLanguage);

            Assert.NotNull(tvShow.ProductionCompanies);
            Assert.Equal(3, tvShow.ProductionCompanies.Count);
            Assert.Equal(2605, tvShow.ProductionCompanies[0].Id);
            Assert.Equal("Gran Via Productions", tvShow.ProductionCompanies[0].Name);

            Assert.NotNull(tvShow.CreatedBy);
            Assert.Equal(1, tvShow.CreatedBy.Count);
            Assert.Equal(66633, tvShow.CreatedBy[0].Id);
            Assert.Equal("Vince Gilligan", tvShow.CreatedBy[0].Name);

            Assert.NotNull(tvShow.EpisodeRunTime);
            Assert.Equal(2, tvShow.EpisodeRunTime.Count);

            Assert.NotNull(tvShow.Genres);
            Assert.Equal(18, tvShow.Genres[0].Id);
            Assert.Equal("Drama", tvShow.Genres[0].Name);

            Assert.NotNull(tvShow.Languages);
            Assert.Equal("en", tvShow.Languages[0]);

            Assert.Null(tvShow.NextEpisodeToAir);

            Assert.NotNull(tvShow.LastEpisodeToAir);
            Assert.Equal(62161, tvShow.LastEpisodeToAir.Id);

            Assert.NotNull(tvShow.Networks);
            Assert.Equal(1, tvShow.Networks.Count);
            Assert.Equal(174, tvShow.Networks[0].Id);
            Assert.Equal("AMC", tvShow.Networks[0].Name);

            Assert.NotNull(tvShow.OriginCountry);
            Assert.Equal(1, tvShow.OriginCountry.Count);
            Assert.Equal("US", tvShow.OriginCountry[0]);

            Assert.NotNull(tvShow.Seasons);
            Assert.Equal(6, tvShow.Seasons.Count);
            Assert.Equal(0, tvShow.Seasons[0].SeasonNumber);
            Assert.Equal(1, tvShow.Seasons[1].SeasonNumber);

            Assert.Equal(62, tvShow.NumberOfEpisodes);
            Assert.Equal(5, tvShow.NumberOfSeasons);

            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);

            Assert.NotEqual(0, tvShow.Popularity);
            Assert.NotEqual(0, tvShow.VoteAverage);
            Assert.NotEqual(0, tvShow.VoteAverage);
        }

        [Fact]
        public void TestTvShowPopular()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.GetTvShowPopularAsync(i).Result);

            SearchContainer<SearchTv> result = Config.Client.GetTvShowPopularAsync().Sync();
            Assert.NotNull(result.Results[0].Id);
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public void TestTvShowSeasonCount()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", " / videos", " / recommendations");

            TvShow tvShow = Config.Client.GetTvShowAsync(1668).Result;
            Assert.Equal(tvShow.Seasons[1].EpisodeCount, 24);
        }

        [Fact]
        public void TestTvShowVideos()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", "videos / id", " / recommendations");

            TvShow tvShow = Config.Client.GetTvShowAsync(1668, TvShowMethods.Videos).Result;
            Assert.NotNull(tvShow.Videos);
            Assert.NotNull(tvShow.Videos.Results);
            Assert.NotNull(tvShow.Videos.Results[0]);

            Assert.Equal("552e1b53c3a3686c4e00207b", tvShow.Videos.Results[0].Id);
            Assert.Equal("en", tvShow.Videos.Results[0].Iso_639_1);
            Assert.Equal("lGTOru7pwL8", tvShow.Videos.Results[0].Key);
            Assert.Equal("Friends - Opening", tvShow.Videos.Results[0].Name);
            Assert.Equal("YouTube", tvShow.Videos.Results[0].Site);
            Assert.Equal(360, tvShow.Videos.Results[0].Size);
            Assert.Equal("Opening Credits", tvShow.Videos.Results[0].Type);
        }

        [Fact]
        public void TestTvShowGetMovieWatchProviders()
        {
            SingleResultContainer<Dictionary<string, WatchProviders>> resp = Config.Client.GetTvShowWatchProvidersAsync(IdHelper.GameOfThrones).Result;

            Assert.NotNull(resp);

            Dictionary<string, WatchProviders> watchProvidersByRegion = resp.Results;
            Assert.NotNull(watchProvidersByRegion);

            // Not making further assertions since this data is highly dynamic.
        }

        [Fact]
        public void TestTvShowTranslations()
        {
            TranslationsContainerTv translations = Config.Client.GetTvShowTranslationsAsync(1668).Result;

            Assert.Equal(1668, translations.Id);
            Translation translation = translations.Translations.SingleOrDefault(s => s.Iso_639_1 == "hr");
            Assert.NotNull(translation);

            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public void TestTvShowSimilars()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainer<SearchTv> tvShow = Config.Client.GetTvShowSimilarAsync(1668).Result;

            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.NotNull(item);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath),
                "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(1100, item.Id);
            Assert.Equal("How I Met Your Mother", item.OriginalName);
            Assert.Equal(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath),
                "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.True(item.Popularity > 0);
            Assert.Equal("How I Met Your Mother", item.Name);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Equal(1, item.OriginCountry.Count);
            Assert.True(item.OriginCountry.Contains("US"));
        }

        [Fact]
        public void TestTvShowRecommendations()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type", "results[array] / popularity");

            SearchContainer<SearchTv> tvShow = Config.Client.GetTvShowRecommendationsAsync(1668).Result;

            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.NotNull(item);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(1100, item.Id);
            Assert.Equal("How I Met Your Mother", item.OriginalName);
            Assert.Equal(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.Equal("How I Met Your Mother", item.Name);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Equal(1, item.OriginCountry.Count);
            Assert.True(item.OriginCountry.Contains("US"));
        }

        [Fact]
        public void TestTvShowTopRated()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            // This test might fail with inconsistent information from the pages due to a caching problem in the API.
            // Comment from the Developer of the API
            // That would be caused from different pages getting cached at different times.
            // Since top rated only pulls TV shows with 2 or more votes right now this will be something that happens until we have a lot more ratings.
            // It's the single biggest missing data right now and there's no way around it until we get more people using the TV data.
            // And as we get more ratings I increase that limit so we get more accurate results.
            TestHelpers.SearchPages(i => Config.Client.GetTvShowTopRatedAsync(i).Result);

            SearchContainer<SearchTv> result = Config.Client.GetTvShowTopRatedAsync().Sync();
            Assert.NotNull(result.Results[0].Id);
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public void TestTvShowLatest()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / similar", " / translations", " / videos", " / recommendations");

            TvShow tvShow = Config.Client.GetLatestTvShowAsync().Sync();

            Assert.NotNull(tvShow);
        }

        [Fact]
        public void TestTvShowReviews()
        {
            TestHelpers.SearchPages(i => Config.Client.GetTvShowReviewsAsync(IdHelper.BreakingBad, page: i).Result);
        }

        [Fact]
        public void TestTvShowLists()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / created_by", " / credits", " / external_ids", " / genres", " / images", " / in_production", " / keywords", " / languages", " / networks", " / production_companies", " / seasons", " / similar", " / translations", " / videos", "results[array] / media_type", " / recommendations");

            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                TestHelpers.SearchPages(i => Config.Client.GetTvShowListAsync(type, i).Result);
            }
        }

        [Fact]
        public void TestTvShowAccountStateFavoriteSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the favourite
            if (accountState.Favorite)
                Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT favourited
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS favourited
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public void TestTvShowAccountStateWatchlistSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the watchlist
            if (accountState.Watchlist)
                Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT watchlisted
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS watchlisted
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public void TestTvShowAccountStateRatingSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingBadRating()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingOutOfBounds()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 10.5).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingLowerBoundsTest()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 0).Result);
        }

        [Fact]
        public void TestTvShowSetRatingUserSession()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the episode is NOT rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the episode IS rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5).Result);

            // Try changing it back to the previous rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 8).Result);
        }

        [Fact]
        public void TestTvShowMissing()
        {
            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.MissingID).Result;

            Assert.Null(tvShow);
        }
    }
}