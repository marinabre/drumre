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
                    if (BLLmovie.Title == "" || BLLmovie.PosterPath == null || BLLmovie.PosterPath == "") continue;
                    var movie = new MovieViewModel();
                    movie.CastSimpleFromMovie(BLLmovie);
                    movies.Add(movie);
                }
                #endregion
                int pageSize = 9;
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

        public ActionResult SearchPaging(int? page, BLL.Models.Filter filter)
        {
            var movies = new List<MovieViewModel>();
            // Search comes here
            List<BLL.Movie> moviesFromDB = MovieRepository.SearchFilter(filter);
            foreach (var movieFromDB in moviesFromDB)
            {
                var movie = new MovieViewModel();
                if (movieFromDB.PosterPath == null || movieFromDB.PosterPath == "")
                {
                    continue;
                }
                movie.CastSearchFromMovie(movieFromDB);
                movies.Add(movie);
            }

            ViewBag.filter = filter;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View("SearchResults", movies.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Search(List<string> Genres, string Directors, string Actors, int YearFrom, int YearTo, string IMDBRatingFrom, string IMDBRatingTo, int TomatoRatingFrom, int TomatoRatingTo, int MetascoreRatingFrom, int MetascoreRatingTo, int FBSharesFrom, int FBSharesTo, int FBLikesFrom, int FBLikesTo, int RuntimeFrom, int RuntimeTo)
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
                    string fixedActor = actor.Trim();
                    filter.Actors.Add(fixedActor);
                }
            }

            if (Directors != null && Directors.Length > 0)
            {
                filter.Directors = new List<string>();
                string[] rawDirectors = Directors.Split(',');
                foreach (string director in rawDirectors)
                {
                    string fixedDirector = director.Trim();
                    filter.Directors.Add(fixedDirector);
                }
            }

            if (RuntimeFrom == MinMaxConstants.RuntimeMin)
            {
                filter.RuntimeFrom = null;
            }
            else
            {
                filter.RuntimeFrom = RuntimeFrom;
            }

            if (RuntimeTo == MinMaxConstants.RuntimeMax)
            {
                filter.RuntimeTo = null;
            }
            else
            {
                filter.RuntimeTo = RuntimeTo;
            }

            if (FBLikesFrom == MinMaxConstants.FBLikesMin)
            {
                filter.FBLikesFrom = null;
            }
            else
            {
                filter.FBLikesFrom = FBLikesFrom;
            }

            if (FBLikesTo == MinMaxConstants.FBLikesMax)
            {
                filter.FBLikesTo = null;
            }
            else
            {
                filter.FBLikesTo = FBLikesTo;
            }

            if (FBSharesFrom == MinMaxConstants.FBSharesMin)
            {
                filter.FBSharesFrom = null;
            }
            else
            {
                filter.FBSharesFrom = FBSharesFrom;
            }

            if (FBSharesTo == MinMaxConstants.FBSharesMax)
            {
                filter.FBSharesTo = null;
            }
            else
            {
                filter.FBSharesTo = FBSharesTo;
            }

            if (IMDBRatingFrom == MinMaxConstants.IMDBRatingMin.ToString())
            {
                filter.IMDBRatingFrom = null;
            }
            else
            {
                filter.IMDBRatingFrom = Convert.ToDouble(IMDBRatingFrom);
            }

            if (IMDBRatingTo == MinMaxConstants.IMDBRatingMax.ToString())
            {
                filter.IMDBRatingTo = null;
            }
            else
            {
                filter.IMDBRatingTo = Convert.ToDouble(IMDBRatingTo);
            }

            if (MetascoreRatingFrom == MinMaxConstants.MetascoreMin)
            {
                filter.MetascoreRatingFrom = null;
            }
            else
            {
                filter.MetascoreRatingFrom = MetascoreRatingFrom;
            }

            if (MetascoreRatingTo == MinMaxConstants.MetascoreMax)
            {
                filter.MetascoreRatingTo = null;
            }
            else
            {
                filter.MetascoreRatingTo = MetascoreRatingTo;
            }

            if (TomatoRatingFrom == MinMaxConstants.TomatoMin)
            {
                filter.TomatoRatingFrom = null;
            }
            else
            {
                filter.TomatoRatingFrom = TomatoRatingFrom;
            }

            if (TomatoRatingTo == MinMaxConstants.TomatoMax)
            {
                filter.TomatoRatingTo = null;
            }
            else
            {
                filter.TomatoRatingTo = TomatoRatingTo;
            }

            if (YearFrom == MinMaxConstants.YearMin)
            {
                filter.YearFrom = null;
            }
            else
            {
                filter.YearFrom = YearFrom;
            }

            if (YearTo == MinMaxConstants.YearMax)
            {
                filter.YearTo = null;
            }
            else
            {
                filter.YearTo = YearTo;
            }

            // Search comes here
            List<BLL.Movie> moviesFromDB = MovieRepository.SearchFilter(filter);
            int i = 1;
            int j = 1;
            foreach (var movieFromDB in moviesFromDB)
            {
                var movie = new MovieViewModel();
                if (movieFromDB.PosterPath == null || movieFromDB.PosterPath == "")
                {
                    continue;
                }
                movie.CastSearchFromMovie(movieFromDB);
                if (j > 9)
                {
                    i++;
                    j = 1;
                }
                else
                {                    
                    j++;
                }
                movie.htmlClass = i.ToString();
                movies.Add(movie);
            }
            
            return View("SearchResults", movies);            
        }

        public ActionResult Recommend()
        {
            var recommend = new RecommendViewModel();
            ViewBag.genres = new List<SelectListItem>();
            var genresFromDB = MovieRepository.GetAllGenres();
            foreach (string genre in genresFromDB)
            {
                var selectListItem = new SelectListItem();
                selectListItem.Text = genre;
                selectListItem.Value = genre;
                ViewBag.genres.Add(selectListItem);
            }
            return View(recommend);
        }

        [HttpPost]
        public ActionResult Recommend(bool Profile, bool Friends, bool Community, int SGenres, 
            int Actors, int Directors, bool Gender, int MaxAgeDifference, int MinimalFriendsTogether, 
            int MinimalMoviesTogether, bool GenderComm, int MaxAgeDifferenceComm, 
            int MinimalFriendsTogetherComm, int MinimalMoviesTogetherComm, List<string> Genres, 
            string FilterDirectors, string FilterActors, int YearFrom, int YearTo, string IMDBRatingFrom, 
            string IMDBRatingTo, int TomatoRatingFrom, int TomatoRatingTo, int MetascoreRatingFrom, 
            int MetascoreRatingTo, int FBSharesFrom, int FBSharesTo, int FBLikesFrom, int FBLikesTo, 
            int RuntimeFrom, int RuntimeTo, bool Filter)
        {
            var person = PersonRepository.GetPersonByEmail(User.Identity.Name, true);
            var recMovies = Recommender.Recommend(person, Profile, Friends, Community, SGenres, Actors, Directors, Gender, MaxAgeDifference, MinimalFriendsTogether, MinimalMoviesTogether, GenderComm, MaxAgeDifferenceComm, MinimalFriendsTogetherComm, MinimalMoviesTogetherComm);
            if (Filter)
            {
                if (Genres == null)
                {
                    Genres = new List<string>();
                }
                var filter = new BLL.Models.Filter(Genres);
                filter.Actors = new List<string>();
                if (FilterActors != null && FilterActors.Length > 0)
                {
                    string[] rawActors = FilterActors.Split(',');
                    foreach (string actor in rawActors)
                    {
                        string fixedActor = actor.Trim();
                        filter.Actors.Add(fixedActor);
                    }
                }

                if (FilterDirectors != null && FilterDirectors.Length > 0)
                {
                    filter.Directors = new List<string>();
                    string[] rawDirectors = FilterDirectors.Split(',');
                    foreach (string director in rawDirectors)
                    {
                        string fixedDirector = director.Trim();
                        filter.Directors.Add(fixedDirector);
                    }
                }

                if (RuntimeFrom == MinMaxConstants.RuntimeMin)
                {
                    filter.RuntimeFrom = null;
                }
                else
                {
                    filter.RuntimeFrom = RuntimeFrom;
                }

                if (RuntimeTo == MinMaxConstants.RuntimeMax)
                {
                    filter.RuntimeTo = null;
                }
                else
                {
                    filter.RuntimeTo = RuntimeTo;
                }

                if (FBLikesFrom == MinMaxConstants.FBLikesMin)
                {
                    filter.FBLikesFrom = null;
                }
                else
                {
                    filter.FBLikesFrom = FBLikesFrom;
                }

                if (FBLikesTo == MinMaxConstants.FBLikesMax)
                {
                    filter.FBLikesTo = null;
                }
                else
                {
                    filter.FBLikesTo = FBLikesTo;
                }

                if (FBSharesFrom == MinMaxConstants.FBSharesMin)
                {
                    filter.FBSharesFrom = null;
                }
                else
                {
                    filter.FBSharesFrom = FBSharesFrom;
                }

                if (FBSharesTo == MinMaxConstants.FBSharesMax)
                {
                    filter.FBSharesTo = null;
                }
                else
                {
                    filter.FBSharesTo = FBSharesTo;
                }

                if (IMDBRatingFrom == MinMaxConstants.IMDBRatingMin.ToString())
                {
                    filter.IMDBRatingFrom = null;
                }
                else
                {
                    filter.IMDBRatingFrom = Convert.ToDouble(IMDBRatingFrom);
                }

                if (IMDBRatingTo == MinMaxConstants.IMDBRatingMax.ToString())
                {
                    filter.IMDBRatingTo = null;
                }
                else
                {
                    filter.IMDBRatingTo = Convert.ToDouble(IMDBRatingTo);
                }

                if (MetascoreRatingFrom == MinMaxConstants.MetascoreMin)
                {
                    filter.MetascoreRatingFrom = null;
                }
                else
                {
                    filter.MetascoreRatingFrom = MetascoreRatingFrom;
                }

                if (MetascoreRatingTo == MinMaxConstants.MetascoreMax)
                {
                    filter.MetascoreRatingTo = null;
                }
                else
                {
                    filter.MetascoreRatingTo = MetascoreRatingTo;
                }

                if (TomatoRatingFrom == MinMaxConstants.TomatoMin)
                {
                    filter.TomatoRatingFrom = null;
                }
                else
                {
                    filter.TomatoRatingFrom = TomatoRatingFrom;
                }

                if (TomatoRatingTo == MinMaxConstants.TomatoMax)
                {
                    filter.TomatoRatingTo = null;
                }
                else
                {
                    filter.TomatoRatingTo = TomatoRatingTo;
                }

                if (YearFrom == MinMaxConstants.YearMin)
                {
                    filter.YearFrom = null;
                }
                else
                {
                    filter.YearFrom = YearFrom;
                }

                if (YearTo == MinMaxConstants.YearMax)
                {
                    filter.YearTo = null;
                }
                else
                {
                    filter.YearTo = YearTo;
                }
                recMovies = Recommender.FilterResults(recMovies, filter);
            }
            var movies = new List<SimpleMovieViewModel>();
            int i = 1;
            int j = 1;
            foreach (var recmovie in recMovies)
            {
                var movie = new SimpleMovieViewModel();
                movie.CastSimpleFromMovie(recmovie);                
                if (j > 9)
                {
                    i++;
                    j = 1;
                }
                else
                {
                    j++;
                }
                movie.htmlClass = i.ToString();
                movies.Add(movie);
            }
            return View("RecommendResults", movies);
        }

        public ActionResult SubtitleDownloadLink(string imdbID)
        {            
            try
            {
                string url = MovieRepository.GetSubtitleDownloadLink(imdbID).ToString();
                if (url != null)
                {
                    return Redirect(url);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadDuck", "Something went wrong with the subtitle download link.");
            }            
        }

        public ActionResult SubtitlePageLink(string imdbID)
        {
            
            try
            {
                string url = MovieRepository.GetSubtitlePageLink(imdbID).ToString();
                if (url != null)
                {
                    return Redirect(url);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("BadDuck", "Something went wrong with the subtitle page link.");
            }
        }

        public ActionResult BadDuck(string message)
        {
            return View(message);
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