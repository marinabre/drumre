using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class SimpleMovieViewModel
    {
        public string htmlClass { get; set; }
        public string Title { get; set; }
        public string PosterURL { get; set; }
        public string IMDBID { get; set; }

        public void CastSimpleFromMovie(BLL.Movie movie)
        {
            Title = movie.Title;
            IMDBID = movie.IMDbId;
            if (movie.PosterPath == "" || movie.PosterPath == null)
            {
                PosterURL = null;
            }
            else
            {
                PosterURL = "http://image.tmdb.org/t/p/w185" + movie.PosterPath;
            }
        }

        public void CastSimpleFromMovie(BLL.FBMovie movie)
        {
            Title = movie.Title;
            IMDBID = movie.IMDbID;

            var movieRepo = new BLL.MovieRepository();
            var movieFromDB = movieRepo.GetMovieByID(IMDBID);
            if (movieFromDB.PosterPath == "" || movieFromDB.PosterPath == null)
            {
                PosterURL = null;
            }
            else
            {
                PosterURL = "http://image.tmdb.org/t/p/w185" + movieFromDB.PosterPath;
            }
        }
    }
}