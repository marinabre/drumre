using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var movies = new List<MovieViewModel>();
                var BLLmovies = BLL.DummyHelpers.MovieProvider.RecommendMovies();
                foreach (var BLLmovie in BLLmovies)
                {
                    var movie = new MovieViewModel();
                    movie.CastFromMovie(BLLmovie);
                    movies.Add(movie);
                }
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}