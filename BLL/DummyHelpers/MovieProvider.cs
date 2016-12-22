
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

            //IMongoClient client = new MongoClient();
            IMongoDatabase db = MongoInstance.GetDatabase;
            var collection = db.GetCollection<Movie>("movies");

            var filter = new BsonDocument();
            var res = collection.Find(filter).Limit(50).ToList();
                                                                      
            return res;
        }
    }
}
