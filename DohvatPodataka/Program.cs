using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib;
using BLL;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using OMDbSharp;
using OSDBnet;
using TMDbLib.Objects.General;

namespace DohvatPodataka
{
    public class Program
    {
        Baza baza = new Baza();
        MovieRepository movieRepo = new MovieRepository();
        ShowRepository showRepo = new ShowRepository();
        static int Main(string[] args)
        {
            var num = 0;
            var prog = new Program();
            int movies = 0;
           // List<TVShow> shows = new List<TVShow>();
            Task.Run(async () =>
            {
                movies = await prog.IMDB();
                //shows = await prog.IMDB_shows("mje");

            }).GetAwaiter().GetResult();
            Console.WriteLine("Povučeno je " + movies + "filmova\n");
           // Console.WriteLine(shows.Count);
            Console.ReadLine();

            return num;
        }

        public async Task<int> IMDB()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            OMDbClient omdb = new OMDbClient(true);
            int counter = 0;
            List<BLL.Movie> rez = new List<BLL.Movie>();
            // 54001
            for (int i = 25356; i <= 54001; i += 200)
            {
                var oldMovies = await baza.dohvatiIzStareLokalne(i);
                foreach (var item in oldMovies)
                {
                    SearchContainer<TMDbLib.Objects.Reviews.ReviewBase> reviews = null;
                    SearchContainer<TMDbLib.Objects.Search.SearchMovie> similar = null;
                    if (item.ImdbId != "" && item.ImdbId != null && item.ImdbId.Length > 0)
                    {
                        similar = await IMDB.GetMovieSimilarAsync(Int32.Parse(item.ImdbId.Substring(2)));
                        reviews = await IMDB.GetMovieReviewsAsync(Int32.Parse(item.ImdbId.Substring(2)));

                        var newMovie = new BLL.Movie
                        {
                            IMDbId = item.ImdbId,
                            Id = item.Id,
                            Title = item.Title,
                            Runtime = item.Runtime,
                            Credits = item.Credits,
                            Genres = item.Genres,
                            Keywords = item.Keywords,
                            Overview = item.Overview,
                            Popularity = item.Popularity,
                            PosterPath = item.PosterPath,
                            ReleaseDate = item.ReleaseDate,
                            Reviews = reviews,
                            Similar = similar,
                            Status = item.Status,
                            Videos = item.Videos,
                            VoteAverage = item.VoteAverage,
                            VoteCount = item.VoteCount
                        };
                        newMovie = movieRepo.OMDbData(newMovie);
                        newMovie = movieRepo.SubtitleData(newMovie);
                        rez.Add(newMovie);
                    }
                }
                if (rez.Count > 0)
                {
                    baza.saveMovies(rez);
                }
                rez = new List<BLL.Movie>();
                counter = i;
            }
            return counter;
        }

        public async Task<List<BLL.TVShow>> IMDB_shows(string mje)
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");

            List<BLL.TVShow> rez = new List<BLL.TVShow>();
                for (int i = 251; i <= 350; i++)
                {
                    TMDbLib.Objects.TvShows.TvShow pom = new TMDbLib.Objects.TvShows.TvShow();
                    try
                    {
                        pom = await IMDB.GetTvShowAsync(i, TMDbLib.Objects.TvShows.TvShowMethods.Credits | TMDbLib.Objects.TvShows.TvShowMethods.Similar | TMDbLib.Objects.TvShows.TvShowMethods.Videos | TMDbLib.Objects.TvShows.TvShowMethods.ContentRatings | TMDbLib.Objects.TvShows.TvShowMethods.ExternalIds);

                    }
                    catch
                    {
                        baza.saveTVShows(rez);
                        return rez;
                    }

                    if (pom.ExternalIds != null && pom.ExternalIds.ImdbId != null && pom.ExternalIds.ImdbId.Length > 0)
                    {
                        var newTVShow = new BLL.TVShow
                        {
                            Id = pom.Id,
                            IMDbId = pom.ExternalIds.ImdbId,
                            Name = pom.Name,
                            NumberOfEpisodes = pom.NumberOfEpisodes,
                            Credits = pom.Credits,
                            Genres = pom.Genres,
                            NumberOfSeasons = pom.NumberOfSeasons,
                            Overview = pom.Overview,
                            ContentRatings = pom.ContentRatings,
                            PosterPath = pom.PosterPath,
                            Similar = pom.Similar,
                            Videos = pom.Videos,
                            VoteAverage = pom.VoteAverage
                        };

                        newTVShow = showRepo.OMDbData(newTVShow);
                        rez.Add(newTVShow);
                    }
                }
                if (rez != null)
                {
                    baza.saveTVShows(rez);
                    rez = new List<BLL.TVShow>();
                }
            return rez;
        }
        ////thetvdb.com API KEY: BDA5FCAB219B7E8E
        //public List<TVDBSharp.Models.Show> thetvdb(string name)
        //{
        //    var tvdb = new TVDB("BDA5FCAB219B7E8E");
        //    var results = tvdb.Search("tt0944947");
        //  //  var shows = new List<BazaPodataka.TVShow>();
        //    //tvdb.Search("", Int32.MaxValue);
        //    return results;
        //}
    }
}
