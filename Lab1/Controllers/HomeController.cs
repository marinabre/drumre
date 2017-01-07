﻿using Projekt.App_Start;
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
using BLL.DummyHelpers;

namespace Projekt.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {            
            if (User.Identity.IsAuthenticated)
            {
                var person = PersonRepository.GetPersonByEmail(User.Identity.Name, true);                
                var movies = new List<MovieViewModel>();
                var BLLmovies = new List<BLL.Movie>();
                #region Getting recommended movies here
                BLLmovies = Recommender.Recommend(person).ToList();
                //BLLmovies = MovieProvider.RecommendMovies();
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
            var search = new SearchViewModel();
            ViewBag.genres = new List<SelectListItem>();
            var genresFromDB = MovieRepository.GetAllGenres();
            foreach (string genre in genresFromDB)
            {
                var selectListItem = new SelectListItem();
                selectListItem.Text = genre;
                selectListItem.Value = genre;
                ViewBag.genres.Add(selectListItem);
            }
            return View(search);
        }

        [HttpPost]
        public ActionResult Search(List<string> Genres, string Directors, string Actors, int YearFrom, int YearTo, double IMDBRatingFrom, double IMDBRatingTo, int TomatoRatingFrom, int TomatoRatingTo, int MetascoreRatingFrom, int MetascoreRatingTo, int FBSharesFrom, int FBSharesTo, int FBLikesFrom, int FBLikesTo, int RuntimeFrom, int RuntimeTo)
        {
            if (Genres == null)
            {
                Genres = new List<string>();
            }
            var filter = new BLL.Models.Filter(Genres);
            var movies = new List<MovieViewModel>();

            filter.Actors = new List<string>();
            if (Actors != null && Actors.Length > 0)
            {
                string[] rawActors = Actors.Split(',');
                foreach (string actor in rawActors)
                {
                    actor.Trim();
                    filter.Actors.Add(actor);
                }
            }

            if (Directors != null && Directors.Length > 0)
            {
                filter.Directors = new List<string>();
                string[] rawDirectors = Directors.Split(',');
                foreach (string director in rawDirectors)
                {
                    director.Trim();
                    filter.Directors.Add(director);
                }
            }

            if (RuntimeFrom == -1)
            {
                filter.RuntimeFrom = null;
            }
            else
            {
                filter.RuntimeFrom = RuntimeFrom;
            }

            if (RuntimeTo == -1)
            {
                filter.RuntimeTo = null;
            }
            else
            {
                filter.RuntimeTo = RuntimeTo;
            }

            if (FBLikesFrom == -1)
            {
                filter.FBLikesFrom = null;
            }
            else
            {
                filter.FBLikesFrom = FBLikesFrom;
            }

            if (FBLikesTo == -1)
            {
                filter.FBLikesTo = null;
            }
            else
            {
                filter.FBLikesTo = FBLikesTo;
            }

            if (FBSharesFrom == -1)
            {
                filter.FBSharesFrom = null;
            }
            else
            {
                filter.FBSharesFrom = FBSharesFrom;
            }

            if (FBSharesTo == -1)
            {
                filter.FBSharesTo = null;
            }
            else
            {
                filter.FBSharesTo = FBSharesTo;
            }

            if (IMDBRatingFrom == -1)
            {
                filter.IMDBRatingFrom = null;
            }
            else
            {
                filter.IMDBRatingFrom = IMDBRatingFrom;
            }

            if (IMDBRatingTo == -1)
            {
                filter.IMDBRatingTo = null;
            }
            else
            {
                filter.IMDBRatingTo = IMDBRatingTo;
            }

            if (MetascoreRatingFrom == -1)
            {
                filter.MetascoreRatingFrom = null;
            }
            else
            {
                filter.MetascoreRatingFrom = MetascoreRatingFrom;
            }

            if (MetascoreRatingTo == -1)
            {
                filter.MetascoreRatingTo = null;
            }
            else
            {
                filter.MetascoreRatingTo = MetascoreRatingTo;
            }

            if (TomatoRatingFrom == -1)
            {
                filter.TomatoRatingFrom = null;
            }
            else
            {
                filter.TomatoRatingFrom = TomatoRatingFrom;
            }

            if (TomatoRatingTo == -1)
            {
                filter.TomatoRatingTo = null;
            }
            else
            {
                filter.TomatoRatingTo = TomatoRatingTo;
            }

            if (YearFrom == -1)
            {
                filter.YearFrom = null;
            }
            else
            {
                filter.YearFrom = YearFrom;
            }

            if (YearTo == -1)
            {
                filter.YearTo = null;
            }
            else
            {
                filter.YearTo = YearTo;
            }

            // Search comes here
            List<BLL.Movie> moviesFromDB = Recommender.FilterResults(null, filter).ToList();
            foreach (var movieFromDB in moviesFromDB)
            {
                var movie = new MovieViewModel();
                movie.CastSearchFromMovie(movieFromDB);
                movies.Add(movie);
            }

            return View("SearchResults", movies);            
        }

        public ActionResult Recommend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recommend(int Genres, int Actors, int Directors, bool Gender, int MaxAgeDifference, int MinimalFriendsTogether, int MinimalMoviesTogether, bool GenderComm, int MaxAgeDifferenceComm, int MinimalFriendsTogetherComm, int MinimalMoviesTogetherComm, List<string> Movies, List<string> Friends)
        {
            return View("RecommendResults");
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