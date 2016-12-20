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

namespace DohvatPodataka
{
    public class Program
    {
        Baza baza = new Baza();
        MovieRepository repo = new MovieRepository();
        static int Main(string[] args)
        {
            var prog = new Program();
            Task.Run(async () =>
            {
                var movies = await prog.IMDB();
            }).GetAwaiter().GetResult();
            return 0;
        }

        public async Task<List<BLL.Movie>> IMDB()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            OMDbClient omdb = new OMDbClient(true);
            var osdb = Osdb.Login("eng", "FileBot");

            List<BLL.Movie> rez = new List<BLL.Movie>();
            // 6 328 656
            for (int j = 101; j <= 1000; j += 50)
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
                        baza.spremiFilmove(rez);
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
                        try
                        {
                            var subtitles = osdb.SearchSubtitlesFromImdb("eng", newMovie.IMDbId.Substring(2));
                            if (subtitles.Count > 0)
                            {
                                newMovie.MovieSubtitle = subtitles.First();
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        rez.Add(newMovie);
                    }
                }
                if (rez != null)
                {
                    baza.spremiFilmove(rez);
                    rez = null;
                }

            }
            return rez;
        }



    }
}
