using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace BLL
{
    class Recommender
    {
        public decimal calculateActorSimilarity (Movie movieA, Movie movieB)
        {
            IList<Cast> castA = movieA.Credits.Cast;
            IList<Cast> castB = movieB.Credits.Cast;

            decimal intersection = castA.Select(a => a.Name).Intersect(castB.Select(b => b.Name)).Count();
            //decimal joined = castA.Count() + castB.Count();

            return intersection;
        }

        public decimal calculateDirectorSimilarity(Movie movieA, Movie movieB)
        {
            IList<Crew> directorsA = movieA.Credits.Crew.Where(x => x.Job == "Director").ToList();
            IList<Crew> directorsB = movieB.Credits.Crew.Where(x => x.Job == "Director").ToList();

            decimal intersection = directorsA.Select(a => a.Name).Intersect(directorsB.Select(b => b.Name)).Count();
            //decimal joined = directorsA.Count() + directorsB.Count();

            return intersection;
        }

        public decimal calculateGenreSimilarity(Movie movieA, Movie movieB)
        {
            IList<Genre> genresA = movieA.Genres;
            IList<Genre> genresB = movieB.Genres;

            decimal intersection = genresA.Intersect(genresB).Count();
            decimal joined = genresA.Count() + genresB.Count();

            return 2 * intersection / joined;
        }



        public MovieSimilarity getSimilarity(Movie movieA, Movie movieB)
        {
            MovieSimilarity result = new MovieSimilarity();
            result.movieA = movieA.IMDbId;
            result.movieB = movieB.IMDbId;
            result.actorSimilarity = calculateActorSimilarity(movieA, movieB);
            result.directorSimilarity = calculateDirectorSimilarity(movieA, movieB);
            result.genreSimilarity = calculateGenreSimilarity(movieA, movieB);
            return result;
        }

    }
}
