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
           // int movies = 0;
            List<TVShow> shows = new List<TVShow>();
            Task.Run(async () =>
            {
              //  movies = await prog.IMDB();
                shows = await prog.IMDB_shows();
                num = shows.Count();

            }).GetAwaiter().GetResult();
            Console.WriteLine("Povučeno je " + num + "serija\n");
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
            for (int j = 64852; j <= 624852; j += 100)
            {
                for (int i = j; i < j+100; i ++)
                {
                    TMDbLib.Objects.Movies.Movie pom = new TMDbLib.Objects.Movies.Movie();
                    try
                    {
                        pom = await IMDB.GetMovieAsync(i, MovieMethods.Credits | MovieMethods.Similar | MovieMethods.Videos | MovieMethods.Reviews | MovieMethods.Keywords);
                        if (pom.ImdbId != "" && pom.ImdbId != null && pom.ImdbId.Length > 0)
                        {
                            var newMovie = new BLL.Movie
                            {
                                IMDbId = pom.ImdbId,
                                Id = pom.Id,
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
                            newMovie = movieRepo.OMDbData(newMovie);
                            newMovie = movieRepo.SubtitleData(newMovie);
                            rez.Add(newMovie);
                        }
                    }
                    catch
                    {
                        if (rez != null && rez.Count > 0)
                        {
                            baza.saveMovies(rez);
                            rez = new List<BLL.Movie>();
                        }
                    }                 
                }
                if (rez.Count > 0)
                {
                    baza.saveMovies(rez);
                    rez = new List<BLL.Movie>();
                }
               
                counter = j;
            }
            return counter;
        }

        public async Task<List<BLL.TVShow>> IMDB_shows()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");

            List<BLL.TVShow> rez = new List<BLL.TVShow>();
            for (int j = 67376; j < 107376; j += 100)
            {
                for (int i = j; i < j + 100; i++)
                {
                    TMDbLib.Objects.TvShows.TvShow pom = new TMDbLib.Objects.TvShows.TvShow();
                    try
                    {
                        pom = await IMDB.GetTvShowAsync(i, TMDbLib.Objects.TvShows.TvShowMethods.Credits | TMDbLib.Objects.TvShows.TvShowMethods.Similar | TMDbLib.Objects.TvShows.TvShowMethods.Videos | TMDbLib.Objects.TvShows.TvShowMethods.ContentRatings | TMDbLib.Objects.TvShows.TvShowMethods.ExternalIds);

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
                    catch
                    {
                        if (rez != null && rez.Count > 0)
                        {
                            baza.saveTVShows(rez);
                            rez = new List<BLL.TVShow>();
                        }
                    }


                }
                if (rez != null && rez.Count > 0)
                {
                    baza.saveTVShows(rez);
                    rez = new List<BLL.TVShow>();
                }
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
