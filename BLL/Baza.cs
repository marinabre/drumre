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
        //mongo instance Anin je u app_start folderu na main projektu, odlučiti kaj s tim
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public Baza()
        {
            if (_client == null)
            {
                _client = new MongoClient();
                _database = _client.GetDatabase("projekt");
            }
        }

        public async void spremiFilmove(List<TMDbLib.Objects.Movies.Movie> newObjects)
        {
            var collection = _database.GetCollection<TMDbLib.Objects.Movies.Movie>("movies");
            await collection.InsertManyAsync(newObjects);

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

        //public List<TVShow> dohvatiEmisije(string name)
        //{
        //    var filter = Builders<TVShow>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));
        //    var emisije = _database.GetCollection<TVShow>("shows").Find(filter).ToList();
        //    return emisije;
        //}
        //public void unesiEmisije(List<TVShow> newObjects)
        //{
        //    var collection = _database.GetCollection<TVShow>("shows");
        //    collection.InsertMany(newObjects);
        //}

    }
}
