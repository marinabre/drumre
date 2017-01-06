using Projekt.App_Start;
using Projekt.Models;
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
using PagedList;

namespace Projekt.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index(int? page, List<MovieViewModel> movies = null)
        {            
            if (User.Identity.IsAuthenticated)
            {
                var person = new Person();
                if (movies == null)
                {
                    movies = new List<MovieViewModel>();
                    var BLLmovies = new List<BLL.Movie>();
                    #region Getting recommended movies here
                    BLLmovies = Recommender.Recommend(person).ToList();
                    foreach (var BLLmovie in BLLmovies)
                    {
                        if (BLLmovie.Title == "") continue;
                        var movie = new MovieViewModel();
                        movie.CastSimpleFromMovie(BLLmovie);
                        movies.Add(movie);
                    }
                    #endregion
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View("HomeLoggedIn", movies.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View();
            }            
        }

        public ActionResult Details(string imdbID)
        {
            var movie = new MovieViewModel();
            var movieFromDB = new BLL.Movie();
            movieFromDB = MovieRepository.GetMovieByID(imdbID);
            movie.CastFromMovie(movieFromDB);
            return View(movie);
        }

        public ActionResult Search()
        {
            var search = new SearchViewModel();
            return View(search);
        }

        [HttpPost]
        public ActionResult Search(string Actors, int YearFrom, int YearTo, decimal IMDBRatingFrom, decimal IMDBRatingTo, decimal TomatoRatingFrom, decimal TomatoRatingTo, decimal MetascoreRatingFrom, decimal MetascoreRatingTo, int FBSharesFrom, int FBSharesTo, int FBLikesFrom, int FBLikesTo)
        {
            var filter = new BLL.Models.Filter();
            var movies = new List<MovieViewModel>();

            filter.Actors = new List<string>();
            string[] rawActors = Actors.Split(',');
            foreach (string actor in rawActors)
            {
                actor.Trim();
                filter.Actors.Add(actor);
            }

            filter.FBLikesFrom = FBLikesFrom;
            filter.FBLikesTo = FBLikesTo;
            filter.FBSharesFrom = FBSharesFrom;
            filter.FBSharesTo = FBSharesTo;
            filter.IMDBRatingFrom = IMDBRatingFrom;
            filter.IMDBRatingTo = IMDBRatingTo;
            filter.MetascoreRatingFrom = MetascoreRatingFrom;
            filter.MetascoreRatingTo = MetascoreRatingTo;
            filter.TomatoRatingFrom = TomatoRatingFrom;
            filter.TomatoRatingTo = TomatoRatingTo;
            filter.YearFrom = YearFrom;
            filter.YearTo = YearTo;

            // Search comes here

            return RedirectToAction("Index", movies);
        }

        public ActionResult Recommend()
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
    }
}