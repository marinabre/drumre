using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TMDbLib.Objects.Movies;

namespace BLL
{
    public class Baza
    {
        public async void saveMovies(List<Movie> newObjects)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
            //collection.InsertMany(newObjects);
            try
            {
                await collection.InsertManyAsync(newObjects);
            }
            catch(MongoDB.Driver.MongoConnectionException e)
            {
                db = MongoInstance.Reconnect;
                IMongoCollection<Movie> collection2 = db.GetCollection<Movie>("movies");
                await collection.InsertManyAsync(newObjects);
            }
        }
        public void updateMovie(Movie updatedMovie)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");

            collection.ReplaceOne(p => p.Title == updatedMovie.IMDbId,
                        updatedMovie,
                        new UpdateOptions { IsUpsert = true });
        }
        public void saveTVShows(List<TVShow> newObjects)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");
            collection.InsertMany(newObjects);
            // await collection.InsertManyAsync(newObjects);
        }
        public void updateTVShow(TVShow updatedTVShow)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");

            collection.ReplaceOne(p => p.IMDbId == updatedTVShow.IMDbId,
                        updatedTVShow,
                        new UpdateOptions { IsUpsert = true });
        }

        public async Task<List<TMDbLib.Objects.Movies.Movie>> dohvatiIzStareLokalne(int Id)
        {
            var _client = new MongoClient();
            var _database = _client.GetDatabase("Lab1-v3");
            var filter = Builders<TMDbLib.Objects.Movies.Movie>.Filter.AnyGte("_id", Id) & Builders<TMDbLib.Objects.Movies.Movie>.Filter.AnyLt("_id", Id + 200);
            var collection = _database.GetCollection<TMDbLib.Objects.Movies.Movie>("movies");
            var result = await collection.Find(filter).ToListAsync();
            return result;
        }

        public List<TVShow> GetTVShows(string showName, int numberOfResults, int skip = 0)
        {
            var db = MongoInstance.GetDatabase;
            var collection = db.GetCollection<TVShow>("shows");
            var filter = Builders<TVShow>.Filter.Where(x => x.Name == showName);
            var emisije = collection.Find(filter).Limit(numberOfResults).Skip(skip).ToList();
            //var filter = Builders<TVShow>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));
                        
            return emisije;
        }
        //public void unesiEmisije(List<TVShow> newObjects)
        //{
        //    var collection = _database.GetCollection<TVShow>("shows");
        //    collection.InsertMany(newObjects);
        //}




        //ima više smisla kada se unose filmovi koji se pretražuju preko abecede
        //da ne pizdi zbog duplića
        // var models = new WriteModel<BsonDocument>[newObjects.Count];
        //// use ReplaceOneModel with property IsUpsert set to true to upsert whole documents
        //for (var i = 0; i < newObjects.Count; i++)
        //{
        //    var bsonDoc = newObjects[i].ToBsonDocument();
        //    models[i] = new ReplaceOneModel<BsonDocument>(new BsonDocument("_id", newObjects[i].Id), bsonDoc) { IsUpsert = true };
        //};
        //await collection.BulkWriteAsync(models);
    }
}
