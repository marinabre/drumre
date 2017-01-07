using Facebook;
using Google.Apis.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using OMDbSharp;
using OSDBnet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace BLL
{
    public class MovieRepository
    {
        Baza baza = new Baza();

        //za dohvat omdb podataka o filmu, ako se flag stavi na true, znači da se updatea postojeći film
        public Movie OMDbData(Movie movie, bool refresh = false)
        {
            OMDbClient omdb = new OMDbClient(true);
            try
            {
                Item omdbResult = omdb.GetItemByID(movie.IMDbId).Result;
                if (omdbResult.imdbID != null)
                {
                    movie.Rated = omdbResult.Rated;
                    movie.Awards = omdbResult.Awards;
                    movie.Country = omdbResult.Country;
                    movie.Language = omdbResult.Language;

                    if (omdbResult.Metascore != "N/A")
                    {
                        movie.Metascore = Int32.Parse(omdbResult.Metascore);
                    }
                    if (omdbResult.tomatoFresh != "N/A")
                    {
                        movie.TomatoFresh = Int32.Parse(omdbResult.tomatoFresh);
                    }
                    if (omdbResult.tomatoRotten != "N/A")
                    {
                        movie.TomatoRotten = Int32.Parse(omdbResult.tomatoRotten);
                    }
                    if (omdbResult.tomatoRating != "N/A")
                    {
                        movie.TomatoRating = Decimal.Parse(omdbResult.tomatoRating);
                    }
                    if (omdbResult.tomatoReviews != "N/A")
                    {
                        movie.TomatoReviews = Int32.Parse(omdbResult.tomatoReviews);
                    }
                    if (omdbResult.tomatoUserMeter != "N/A")
                    {
                        movie.TomatoUserMeter = Int32.Parse(omdbResult.tomatoUserMeter);
                    }
                    if (omdbResult.tomatoUserRating != "N/A")
                    {
                        movie.TomatoUserRating = Decimal.Parse(omdbResult.tomatoUserRating);
                    }
                    if (omdbResult.tomatoUserReviews != "N/A")
                    {
                        movie.TomatoUserReviews = Int32.Parse(omdbResult.tomatoUserReviews);
                    }
                    if (refresh)
                    {
                        baza.updateMovie(movie);
                    }
                }
            }
            catch { }
            return movie;
        }

        public Movie SubtitleData(Movie movie, bool refresh = false)
        {
            try
            {
                var osdb = Osdb.Login("eng", "FileBot");
                var subtitles = osdb.SearchSubtitlesFromImdb("eng", movie.IMDbId.Substring(2));
                if (subtitles.Count > 0)
                {
                    movie.MovieSubtitle = subtitles.First();
                    if (refresh)
                    {
                        baza.updateMovie(movie);
                    }

                }
                return movie;
            }
            catch (Exception e)
            {                
                return movie;
            }
        }

        public void FBData (string IMDbId, bool refresh = false)
        {
            //var claimsforUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            //var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            //var fb = new FacebookClient(access_token);
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1279180495466644",
                client_secret = "780860b63672ff262370699538722160",
                grant_type = "client_credentials"
            });
            String imdbURL = "http://www.imdb.com/title/" + IMDbId;
            //var parameters = new FacebookBatchParameter[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    String imdbURL = "http://www.imdb.com/title/" + IMDbIds[i];
            //    parameters[i] = new FacebookBatchParameter(HttpMethod.Get, "?fields=og_object{ likes.limit(0).summary(true), engagement, title, id, image }, share &ids=" + imdbURL);
            //}
            try
            {
                //dynamic Info = fb.Batch(parameters);
                //for (int i = 1; i < 11; i++)
                //{
                //    var FBLikes = Info[i].og_object.engagement.count;
                //    var FBShares = Info[i].share.share_count;
                //    baza.updateMovieFB(IMDbIds[i-1], FBShares, FBLikes);
                //}
                dynamic Info = fb.Get("?fields=og_object{ likes.limit(0).summary(true), engagement, title, id, image }, share &ids=" + imdbURL);
                Info = GetProperty(Info, imdbURL);
                //movie.Title = Info.og_object.title;
                var FBLikes = Info.og_object.engagement.count;
                var FBShares = Info.share.share_count;
                baza.updateMovieFB(IMDbId, FBShares, FBLikes);

            }
            catch
            {
                System.Threading.Thread.Sleep(10000);
                dynamic Info = fb.Get("?fields=og_object{ likes.limit(0).summary(true), engagement, title, id, image }, share &ids=" + imdbURL);
                Info = GetProperty(Info, imdbURL);
                //movie.Title = Info.og_object.title;
                var FBLikes = Info.og_object.engagement.count;
                var FBShares = Info.share.share_count;
                baza.updateMovieFB(IMDbId, FBShares, FBLikes);

            }
         }

        public static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }

        public Movie GetMovieByID (string imdbID)
        {
            return baza.GetMovieByID(imdbID);
        }
        public Movie GetMovieByIDShortDetails(string imdbID)
        {
            return baza.GetMovieByID(imdbID, true);
        }

        public Movie GetMovieByTitle(string title)
        {
            return baza.GetMovieByTitle(title);
        }

        public static async Task<IList<Movie>> GetMoviesByFB(IList<FBMovie> fbMovies)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var builder = Builders<Movie>.Filter;
            if (fbMovies.Count > 0)
            {
                var name = fbMovies.ElementAt(0).Title;
                var filter = builder.Eq("Title", name);

                for (int i = 1; i < fbMovies.Count; i++)
                {
                    var name2 = fbMovies.ElementAt(i).Title;
                    filter = filter | builder.Eq("Title", name2);
                }
                return await movies.Find(filter).ToListAsync();
            }
            return null;
        }

        public async Task<Movie> GetMovieByTitleFromAPI(string title)
        {
            MovieRepository repo = new MovieRepository();
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            var search = await IMDB.SearchMovieAsync(title);
            var pom = await IMDB.GetMovieAsync(search.Results[0].Id, MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Similar | MovieMethods.Reviews | MovieMethods.Keywords);

            var newMovie = new BLL.Movie
            {
                IMDbId = pom.ImdbId,
                Title = pom.Title,
                Runtime = pom.Runtime,
                Credits = pom.Credits,
                Genres = pom.Genres,
                Keywords = pom.Keywords,
                Overview = pom.Overview,
                Popularity = pom.Popularity,
                PosterPath = pom.PosterPath,
                ReleaseDate = pom.ReleaseDate,
                Reviews = pom.Reviews,
                Similar = pom.Similar,
                Status = pom.Status,
                Videos = pom.Videos,
                VoteAverage = pom.VoteAverage,
                VoteCount = pom.VoteCount
            };
            newMovie = repo.OMDbData(newMovie);
            newMovie = repo.SubtitleData(newMovie);
            return newMovie;

        }
        public async Task<Movie> GetMovieByIdFromAPI(string id)
        {
            MovieRepository repo = new MovieRepository();
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            var pom = await IMDB.GetMovieAsync(id, MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Similar | MovieMethods.Reviews | MovieMethods.Keywords);

            var newMovie = new BLL.Movie
            {
                IMDbId = pom.ImdbId,
                Title = pom.Title,
                Runtime = pom.Runtime,
                Credits = pom.Credits,
                Genres = pom.Genres,
                Keywords = pom.Keywords,
                Overview = pom.Overview,
                Popularity = pom.Popularity,
                PosterPath = pom.PosterPath,
                ReleaseDate = pom.ReleaseDate,
                Reviews = pom.Reviews,
                Similar = pom.Similar,
                Status = pom.Status,
                Videos = pom.Videos,
                VoteAverage = pom.VoteAverage,
                VoteCount = pom.VoteCount
            };           
            newMovie = repo.OMDbData(newMovie);
            newMovie = repo.SubtitleData(newMovie);            
            return newMovie;
        }

        public static string GetYTURLForMovie(Movie movie)
        {
            // http://www.youtube.com/embed/6vQgHBugHxA?autoplay=0
            string url = "";
            if (movie.Videos.Results.Count > 0)
            {
                foreach (var result in movie.Videos.Results)
                {
                    if (result.Site == "YouTube")
                    {
                        url = "http://www.youtube.com/embed/" + result.Key + "?autoplay=0";
                    }
                }
            }
            if (url == "")
            {
                var youtubeService = new Google.Apis.YouTube.v3.YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = ConfigurationManager.AppSettings["APIKey"],
                    ApplicationName = "DruMreLab1"
                });

                var searchListRequest = youtubeService.Search.List("id, snippet");
                searchListRequest.Q = movie.Title; // Replace with your search term.
                searchListRequest.MaxResults = 1;

                //you need to create the service object earlier
                var request = youtubeService.Videos.List("statistics,snippet");

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = searchListRequest.Execute();

                List<string> videos = new List<string>();
                //List<string> channels = new List<string>();
                //List<string> playlists = new List<string>();

                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            request.Id = searchResult.Id.VideoId;
                            var response = request.Execute();
                            foreach (var video in response.Items)
                            {
                                if (video.Statistics != null)
                                {
                                    var count = video.Statistics.ViewCount;
                                }
                            }
                            videos.Add("http://www.youtube.com/embed/" + searchResult.Id.VideoId + "?autoplay=0");
                            break;

                        case "youtube#channel":
                            //channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                            break;

                        case "youtube#playlist":
                            //playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                            break;
                    }

                    //Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
                    //Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
                    //Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
                    return videos[0];
                }
            }
            return url;                     
        }

        public static List<string> GetAllGenres()
        {
            var db = MongoInstance.GetDatabase;
            var genresColl = db.GetCollection<BsonDocument>("genres");
            var filter = new BsonDocument();
            var genres = genresColl.Find(filter).Limit(50).ToList();
            var result = new List<string>();
            foreach (var genre in genres)
            {
                result.Add(genre["Name"].ToString());
            }
            return result;
        }
    }
}
