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
        public ActionResult Index(int? page)
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
                    movie.CastSimpleFromMovie(BLLmovie);
                    movies.Add(movie);
                }
                #endregion
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
            var repo = new MovieRepository();
            var movie = new MovieViewModel();
            var movieFromDB = new BLL.Movie();
            movieFromDB = repo.GetMovieByIDShortDetails(imdbID);
            movie.CastFromMovie(movieFromDB);
            return View(movie);
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
        


    }
}