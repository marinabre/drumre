using Lab1.App_Start;
using Lab1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using BLL;

namespace Lab1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        BLL.Baza baza = new BLL.Baza();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                #region Getting recommended movies here
                var movies = new List<MovieViewModel>();
                var BLLmovies = BLL.DummyHelpers.MovieProvider.RecommendMovies();
                foreach (var BLLmovie in BLLmovies)
                {
                    if (BLLmovie.Title == "") continue;
                    var movie = new MovieViewModel();
                    movie.CastFromMovie(BLLmovie);
                    movies.Add(movie);
                }
                #endregion
                return View("HomeLoggedIn", movies);
            }
            else
            {
                return View();
            }            
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        // public async System.Threading.Tasks.Task<ActionResult> Contact()
        public ActionResult Contact()
        {
            //za pozivanje dohvata iz imdba
            //return View(await IMDB2());
            return View();

        }
        public async System.Threading.Tasks.Task<List<TMDbLib.Objects.Movies.Movie>> IMDB2()
        {
            var IMDB = new TMDbClient("2c54085e8a7f520650d65cb78a48555a");
            //var results2 = await IMDB.GetMovieAsync(47964, MovieMethods.Credits | MovieMethods.Videos);
            List<TMDbLib.Objects.Movies.Movie> rez = new List<TMDbLib.Objects.Movies.Movie>();
            // 6 328 656
            for (int j = 1; j <= 10000; j += 1000)
            {
                for (int i = j; i <= j + 1000; i++)
                {
                    TMDbLib.Objects.Movies.Movie pom = new TMDbLib.Objects.Movies.Movie();
                    try
                    {
                        pom = await IMDB.GetMovieAsync(i, MovieMethods.Credits | MovieMethods.Videos);

                    }
                    catch
                    {
                        baza.spremiFilmove(rez);
                        rez = null;
                    }

                    if (pom.ImdbId != null)
                    {
                        rez.Add(pom);
                    }
                }
                if (rez != null)
                {
                    baza.spremiFilmove(rez);
                }

            }
            return rez;

        }
      

        ////thetvdb.com API KEY: BDA5FCAB219B7E8E
        //public List<BazaPodataka.TVShow> thetvdb()
        //{
        //    var tvdb = new TVDB("BDA5FCAB219B7E8E");
        //    var results = tvdb.Search(name);
        //    var shows = new List<BazaPodataka.TVShow>();
        //    tvdb.Search("", Int32.MaxValue);

        //    foreach (TVDBSharp.Models.Show item in results)
        //    {
        //        shows.Add(new BazaPodataka.TVShow(item.ImdbId, item.Name, item.Rating ?? 0, item.Description, item.Actors));
        //    }

        //    baza.unesiEmisije(shows);
        //    return shows;
        //}



    }
}