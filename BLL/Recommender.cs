using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace BLL
{
    public class Recommender
    {
        public static decimal calculateActorSimilarity (Movie movieA, Movie movieB)
        {
            IList<Cast> castA = movieA.Credits.Cast;
            IList<Cast> castB = movieB.Credits.Cast;

            decimal intersection = castA.Select(a => a.Name).Intersect(castB.Select(b => b.Name)).Count();
            //decimal joined = castA.Count() + castB.Count();

            return intersection;
        }

        public static decimal calculateDirectorSimilarity(Movie movieA, Movie movieB)
        {
            IList<Crew> directorsA = movieA.Credits.Crew.Where(x => x.Job == "Director").ToList();
            IList<Crew> directorsB = movieB.Credits.Crew.Where(x => x.Job == "Director").ToList();

            decimal intersection = directorsA.Select(a => a.Name).Intersect(directorsB.Select(b => b.Name)).Count();
            //decimal joined = directorsA.Count() + directorsB.Count();

            return intersection;
        }

        public static decimal calculateGenreSimilarity(Movie movieA, Movie movieB)
        {
            IList<Genre> genresA = movieA.Genres;
            IList<Genre> genresB = movieB.Genres;

            decimal intersection = genresA.Intersect(genresB).Count();
            decimal joined = genresA.Count() + genresB.Count();

            return 2 * intersection / joined;
        }

        public static MovieSimilarity getSimilarity (Movie movieA, Movie movieB)
        {
            var db = MongoInstance.GetDatabase;
            var similar = db.GetCollection<MovieSimilarity>("similar");
            var builder = Builders<MovieSimilarity>.Filter;
            Movie first = MovieSimilarity.getFirst(movieA, movieB);
            Movie second = MovieSimilarity.getSecond(movieA, movieB);
            var filter = builder.Eq("movieA", first.IMDbId) | builder.Eq("movieB", second.IMDbId);
            var result = similar.Find(filter);

            //ako je u bazi:
            if (result.Count() > 0)
            {
                return result.First();
            }
            else
            {
                //ako nije u bazi:
                MovieSimilarity newSimilarity = new MovieSimilarity(movieA, movieB);
                similar.InsertOne(newSimilarity);
                return newSimilarity;
            } 
        }

        public static IFindFluent<Movie,Movie> SimilarByGenres (IList<Genre> Genres)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection <Movie> ("movies");
            var filter = GenresFilter(Genres);
            if (filter != null)
                return movies.Find(filter);  
            return null;
        }

        public static IFindFluent<Movie, Movie> SimilarByActors(IList<String> Actors)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var filter = ActorsFilter(Actors);
            if (filter != null)
                return movies.Find(filter);
            return null;
        }

        public static IFindFluent<Movie, Movie> SimilarByDirectors(IList<String> Directors)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var filter = DirectorsFilter(Directors);
            if (filter != null)
                return movies.Find(filter);
            return null;
        }

        public static FilterDefinition<Movie> GenresFilter(IList<Genre> Genres)
        {
            var builder = Builders<Movie>.Filter;
            if (Genres.Count > 0)
            {
                var name = Genres.ElementAt(0).Name;
                var filter = builder.ElemMatch(x => x.Genres, x => x.Name == name);

                for (int i = 1; i < Genres.Count; i++)
                {
                    var name2 = Genres.ElementAt(i).Name;
                    filter = filter | builder.ElemMatch(x => x.Genres, x => x.Name == name2);
                }
                return filter;
            }
            return null;
        }

        public static FilterDefinition<Movie> ActorsFilter(IList<String> Actors)
        {
            var builder = Builders<Movie>.Filter;
            if (Actors.Count > 0)
            {
                var name = Actors.ElementAt(0);
                var filter = builder.ElemMatch(x => x.Credits.Cast, x => x.Name == name);

                for (int i = 1; i < Actors.Count; i++)
                {
                    var name2 = Actors.ElementAt(i);
                    filter = filter | builder.ElemMatch(x => x.Credits.Cast, x => x.Name == name2);
                }
                return filter;
            }
            return null;
        }

        public static FilterDefinition<Movie> DirectorsFilter(IList<String> Directors)
        {
            var builder = Builders<Movie>.Filter;
            if (Directors.Count > 0)
            {
                var name = Directors.ElementAt(0);
                var filter = builder.ElemMatch(x => x.Credits.Crew, x => x.Name == name);

                for (int i = 1; i < Directors.Count; i++)
                {
                    var name2 = Directors.ElementAt(i);
                    filter = filter | builder.ElemMatch(x => x.Credits.Crew, x => x.Name == name2);
                }
                return filter;
            }
            return null;
        }


        public static void MatchProfile (Profile profile, int genre, int actor, int director)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");

            var genres = profile.TopGenres(3);
            var actors = profile.TopActors(5);
            var directors = profile.TopDirectors(5);

            var genreFilter = GenresFilter(genres);
            var actorsFilter = ActorsFilter(actors);
            var directorsFilter = DirectorsFilter(directors);

            var filter = genreFilter & actorsFilter & directorsFilter;
            movies.Find(filter);
            
        }
    }
}
