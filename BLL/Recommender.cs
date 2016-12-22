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

        public static async Task getSimilar(String imdbID)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var similar = db.GetCollection<MovieSimilarity>("similar");
            Movie movie = movies.Find(m => m.IMDbId == imdbID).First();

            //var cursor = movies.FindAsync(new BsonDocument());
            ////foreach (Movie m in )
            ////{
            ////    getSimilarity(movie, m);
            ////}
            //cursor.ForEachAsync 
            //await cursor.ForEachAsync(movie => getSimilarity(movie, m));

            using (var cursor = await movies.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var m in batch)
                    {
                        getSimilarity(movie, m);
                    }
                }
            }
        }
    }
}
