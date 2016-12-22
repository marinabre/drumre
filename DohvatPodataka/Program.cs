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
        MovieRepository repo = new MovieRepository();
        static int Main(string[] args)
        {
            var prog = new Program();
            int movies = 0;
            Task.Run(async () =>
            {
                movies = await prog.IMDB();

            }).GetAwaiter().GetResult();
            Console.WriteLine("Povučeno je " + movies + "filmova\n");
            Console.ReadLine();
            return 0;
        }

        public async Task<int> IMDB()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            OMDbClient omdb = new OMDbClient(true);
            int counter = 0;
            List<BLL.Movie> rez = new List<BLL.Movie>();
            // 6 328 656
            //for (int j = 10001; j <= 10500; j += 50)
            //{
            //    for (int i = j; i < j + 50; i++)
            //    {
            //        TMDbLib.Objects.Movies.Movie pom = new TMDbLib.Objects.Movies.Movie();
            //        try
            //        {
            //            pom = await IMDB.GetMovieAsync(i, MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Similar | MovieMethods.Reviews | MovieMethods.Keywords);

            //        }
            //        catch
            //        {
            //            if (rez != null && rez.Count > 0)
            //            {
            //                baza.spremiFilmove(rez);
            //            }                        
            //            return i;
            //        }

            //        if (pom.ImdbId != null && pom.ImdbId.Length > 0)
            //        {
            //            var newMovie = new BLL.Movie
            //            {
            //                IMDbId = pom.ImdbId,
            //                Title = pom.Title,
            //                Runtime = pom.Runtime,
            //                Credits = pom.Credits,
            //                Genres = pom.Genres,
            //                Keywords = pom.Keywords,
            //                Overview = pom.Overview,
            //                Popularity = pom.Popularity,
            //                PosterPath = pom.PosterPath,
            //                ReleaseDate = pom.ReleaseDate,
            //                Reviews = pom.Reviews,
            //                Similar = pom.Similar,
            //                Status = pom.Status,
            //                Videos = pom.Videos,
            //                VoteAverage = pom.VoteAverage,
            //                VoteCount = pom.VoteCount
            //            };

            //            newMovie = repo.OMDbData(newMovie);
            //            newMovie = repo.SubtitleData(newMovie);
            //            rez.Add(newMovie);
            //        }
            //    }
            //    if (rez != null && rez.Count > 0)
            //    {
            //        baza.spremiFilmove(rez);
            //        rez = new List<BLL.Movie>();
            //    }
            //    counter = j+49;
            //}

            for (int i = 30946; i <= 39946; i+=500)
            {
                var oldMovies = await baza.dohvatiIzStareLokalne(i);
                foreach (var item in oldMovies)
                {
                    SearchContainer<TMDbLib.Objects.Reviews.ReviewBase> reviews = null;
                    SearchContainer<TMDbLib.Objects.Search.SearchMovie> similar = null; 
                    if (item.ImdbId != "" && item.ImdbId != null)
                    {
                         similar = await IMDB.GetMovieSimilarAsync(Int32.Parse(item.ImdbId.Substring(2)));
                         reviews = await IMDB.GetMovieReviewsAsync(Int32.Parse(item.ImdbId.Substring(2)));
                    }

                    var newMovie = new BLL.Movie
                    {
                        IMDbId = item.ImdbId,
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
                    newMovie = repo.OMDbData(newMovie);
                    newMovie = repo.SubtitleData(newMovie);
                    rez.Add(newMovie);
                }
                baza.spremiFilmove(rez);
                rez = new List<BLL.Movie>();
                counter = i;

            }

            return counter;
        }



    }
}
