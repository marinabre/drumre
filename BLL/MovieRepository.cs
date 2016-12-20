using OMDbSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class MovieRepository
    {
        Baza baza = new Baza();

        //za dohvat omdb podataka o filmu, ako se flag stavi na true, znači da se updatea postojeći film
        public Movie OMDbData(Movie movie, bool refresh = false)
        {
            OMDbClient omdb = new OMDbClient(true);
            Item omdbResult = omdb.GetItemByID(movie.IMDbId).Result;
            if (omdbResult.imdbID != null)
            {
                movie.Rated = omdbResult.Rated;
                movie.Awards = omdbResult.Awards;
                movie.Country = omdbResult.Country;
                movie.Language = omdbResult.Language;

                if (omdbResult.Metascore != "N/A")
                {
                    movie.Metascore = Int32.Parse(omdbResult.Metascore);
                }
                if (omdbResult.tomatoFresh != "N/A")
                {
                    movie.TomatoFresh = Int32.Parse(omdbResult.tomatoFresh);
                }
                if (omdbResult.tomatoRotten != "N/A")
                {
                    movie.TomatoRotten = Int32.Parse(omdbResult.tomatoRotten);
                }
                if (omdbResult.tomatoRating != "N/A")
                {
                    movie.TomatoRating = Decimal.Parse(omdbResult.tomatoRating);
                }
                if (omdbResult.tomatoReviews != "N/A")
                {
                    movie.TomatoReviews = Int32.Parse(omdbResult.tomatoReviews);
                }
                if (omdbResult.tomatoUserMeter != "N/A")
                {
                    movie.TomatoUserMeter = Int32.Parse(omdbResult.tomatoUserMeter);
                }
                if (omdbResult.tomatoUserRating != "N/A")
                {
                    movie.TomatoUserRating = Decimal.Parse(omdbResult.tomatoUserRating);
                }
                if (omdbResult.tomatoUserReviews != "N/A")
                {
                    movie.TomatoUserReviews = Int32.Parse(omdbResult.tomatoUserReviews);
                }
                if (refresh)
                {
                    baza.updateMovie(movie);
                }
            }
            return movie;
        }



    }
}
