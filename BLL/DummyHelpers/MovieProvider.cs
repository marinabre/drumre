using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DummyHelpers
{
    public class MovieProvider
    {
        public static List<Movie> RecommendMovies()
        {
            var movies = new List<Movie>();

            IMongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("Lab1-v4");
            var collection = db.GetCollection<BsonDocument>("Movies");

            var filter = new BsonDocument();
            var res = collection.Find(filter).ToList();

            var rnd = new Random();

            foreach (var movieDB in res)
            {
                var movie = new Movie();
                if (movieDB["Director"].BsonType != BsonType.Null)
                {
                    movie.Director = movieDB["Director"].AsString;
                }
                else
                {
                    movie.Director = "";
                }
                if (movieDB["Country"].BsonType != BsonType.Null)
                {
                    movie.Country = movieDB["Country"].AsString;
                }
                else
                {
                    movie.Country = "";
                }
                if (movieDB["Genre"].BsonType != BsonType.Null)
                {
                    movie.Genre = movieDB["Genre"].AsString;
                }
                else
                {
                    movie.Genre = "";
                }
                if (movieDB["imdbRating"].BsonType != BsonType.Null)
                {
                    if (movieDB["imdbRating"].AsString != "N/A")
                    {
                        movie.IMDBRating = Convert.ToDecimal(movieDB["imdbRating"].AsString);
                    }                    
                }
                else
                {
                    movie.IMDBRating = 0;
                }
                if (movieDB["Metascore"].BsonType != BsonType.Null)
                {
                    if (movieDB["Metascore"].AsString != "N/A") 
                    {
                        movie.Metascore = Convert.ToDecimal(movieDB["Metascore"].AsString);
                    }                    
                }
                else
                {
                    movie.Metascore = 0;
                }
                if (movieDB["Runtime"].BsonType != BsonType.Null)
                {
                    movie.Runtime = movieDB["Runtime"].AsString;
                }
                else
                {
                    movie.Runtime = "";
                }
                if (movieDB["Title"].BsonType != BsonType.Null)
                {
                    movie.Title = movieDB["Title"].AsString;
                }
                else
                {
                    movie.Title = "";
                }
                if (movieDB["tomatoRating"].BsonType != BsonType.Null)
                {
                    if (movieDB["tomatoRating"].AsString != "N/A")
                    {
                        movie.TomatoRating = Convert.ToDecimal(movieDB["tomatoRating"].AsString);
                    }                    
                }
                else
                {
                    movie.TomatoRating = 0;
                }
                if (movieDB["Writer"].BsonType != BsonType.Null)
                {
                    movie.Writer = movieDB["Writer"].AsString;
                }
                else
                {
                    movie.Writer = "";
                }
                if (movieDB["Year"].BsonType != BsonType.Null)
                {
                    movie.Year = movieDB["Year"].AsString;
                }
                else
                {
                    movie.Year = "";
                }
                movie.Likes = rnd.Next();                              
                movie.Shares = rnd.Next();                                                                
                movies.Add(movie);
            }

            return movies;
        }
    }
}
