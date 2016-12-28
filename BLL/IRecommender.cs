using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    interface IRecommender
    {
        IList<Movie> HistoryMovieRecommendation(int limitMovies);
        IList<Movie> FriendsMovieRecommendation(int limitFriends, int limitMovies);
        IList<Movie> PersonsMovieRecommendation(int limitPersons, int limitMovies);
        IList<Movie> HistorySimilar();

    }
}
