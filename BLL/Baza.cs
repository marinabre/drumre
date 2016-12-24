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
        const string connectionString = "mongodb://marina:marina@aws-eu-central-1-portal.0.dblayer.com:15324";
        public IMongoDatabase db { get; set; }
        public Baza()
        {
            db = MongoInstance.GetDatabase;
        }
        public async void saveMovies(List<Movie> newObjects)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
            //collection.InsertMany(newObjects);
            await collection.InsertManyAsync(newObjects);
        }
        public void updateMovie(Movie updatedMovie)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");

            collection.ReplaceOne(p => p.Title == updatedMovie.IMDbId,
                        updatedMovie,
                        new UpdateOptions { IsUpsert = true });
        }
        public async void saveTVShows(List<TVShow> newObjects)
        {
            var baza = new Baza();
            baza.db = MongoInstance.GetDatabase;
            try
            {
                IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");
                //collection.InsertMany(newObjects);
                await collection.InsertManyAsync(newObjects);
            }catch
            {
                var client = new MongoClient("mongodb://ana:anaana@aws-eu-central-1-portal.0.dblayer.com:15324,aws-eu-central-1-portal.1.dblayer.com:15324");
                baza.db = client.GetDatabase("projekt");
                IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");
                //collection.InsertMany(newObjects);
                await collection.InsertManyAsync(newObjects);
            }
        }
        public void updateTVShow(TVShow updatedTVShow)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");

            collection.ReplaceOne(p => p.IMDbId == updatedTVShow.IMDbId,
                        updatedTVShow,
                        new UpdateOptions { IsUpsert = true });
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
