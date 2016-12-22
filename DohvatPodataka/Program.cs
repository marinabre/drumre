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
using TVDBSharp;

namespace DohvatPodataka
{
    public class Program
    {
        Baza baza = new Baza();
        MovieRepository repo = new MovieRepository();
        static int Main(string[] args)
        {
            var prog = new Program();
            //Task.Run(async () =>
            //{
            //var movies = await prog.IMDB();
            var tvShowa = prog.thetvdb("marina");
           // }).GetAwaiter().GetResult();
            Console.WriteLine(tvShowa.First().Id +" " + tvShowa.First().ImdbId + " "+ tvShowa.First().Name);
            Console.ReadLine();
            return 0;
        }

        public async Task<List<BLL.Movie>> IMDB()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            OMDbClient omdb = new OMDbClient(true);

            List<BLL.Movie> rez = new List<BLL.Movie>();
            // 6 328 656
            for (int j = 1001; j <= 2000; j += 50)
            {
                for (int i = j; i < j + 50; i++)
                {
                    TMDbLib.Objects.Movies.Movie pom = new TMDbLib.Objects.Movies.Movie();
                    try
                    {
                        pom = await IMDB.GetMovieAsync(i, MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Similar | MovieMethods.Reviews | MovieMethods.Keywords);

                    }
                    catch
                    {
                        baza.saveMovies(rez);
                        return rez;
                    }

                    if (pom.ImdbId != null && pom.ImdbId.Length > 0)
                    {
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
                        rez.Add(newMovie);
                    }
                }
                if (rez != null)
                {
                    baza.saveMovies(rez);
                    rez = new List<BLL.Movie>();
                }

            }
            return rez;
        }


        ////thetvdb.com API KEY: BDA5FCAB219B7E8E
        public List<TVDBSharp.Models.Show> thetvdb(string name)
        {
            var tvdb = new TVDB("BDA5FCAB219B7E8E");
            var results = tvdb.Search("tt0944947");
          //  var shows = new List<BazaPodataka.TVShow>();
            //tvdb.Search("", Int32.MaxValue);
            return results;
        }
    }
}
