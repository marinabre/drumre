﻿using System;
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
        public void spremiFilmove(List<Movie> newObjects)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
            collection.InsertMany(newObjects);
            // await collection.InsertManyAsync(newObjects);
        }
        public void updateMovie(Movie updatedMovie)
        {
            var db = MongoInstance.GetDatabase;
            IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");

            collection.ReplaceOne(p => p.Title == updatedMovie.IMDbId,
                        updatedMovie,
                        new UpdateOptions { IsUpsert = true });
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