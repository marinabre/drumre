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
            IMongoDatabase db = null;
            //collection.InsertMany(newObjects);
            try
            {
                db = MongoInstance.GetDatabase;
                IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
                await collection.InsertManyAsync(newObjects);
            }
            catch(MongoDB.Driver.MongoConnectionException e)
            {
                db = MongoInstance.Reconnect;
                IMongoCollection<Movie> collection = db.GetCollection<Movie>("movies");
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
        public async void saveTVShows(List<TVShow> newObjects)
        {
            var db = MongoInstance.GetDatabase;
            try
            {
                IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");
                //collection.InsertMany(newObjects);
                await collection.InsertManyAsync(newObjects);
            }
            catch (MongoDB.Driver.MongoConnectionException e)
            {
                db = MongoInstance.Reconnect;
                IMongoCollection<TVShow> collection = db.GetCollection<TVShow>("shows");
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
        public Movie GetMovieByID(string imdbID, bool shortDetails = false)
        {
            IMongoDatabase db = null;
            IMongoCollection<Movie> movies = null;
            Movie result = new Movie();
            ProjectionDefinition<Movie> project = null;
            try
            {
                db = MongoInstance.GetDatabase;
                movies = db.GetCollection<Movie>("movies");
                if (shortDetails)
                {
                    project = Builders<Movie>.Projection.Slice((StringFieldDefinition<Movie>)("Credits.Cast"), 0, 5).Slice((StringFieldDefinition<Movie>)("Credits.Crew"), 0, 5);
                    //BsonDocument shortenDetails = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>("{ 'Credits.Cast': {$slice: 5 }, 'Credits.Crew': {$slice: 5 } }");
                    result = movies.Find(p => p.IMDbId == imdbID).Project<Movie>(project).First();
                    //db.getCollection('movies').find( {}, { "Credits.Cast": {$slice: 5 }, "Credits.Crew": {$slice: 5 } } );
                }
                else
                {
                    result = movies.Find(p => p.IMDbId == imdbID).First();
                }
                return result;
            }
            catch (MongoDB.Driver.MongoConnectionException)
            {
                db = MongoInstance.Reconnect;
                movies = db.GetCollection<Movie>("movies");
                if (shortDetails)
                {
                    result = movies.Find(p => p.IMDbId == imdbID).Project<Movie>(project).First();
                }
                else
                {
                    result = movies.Find(p => p.IMDbId == imdbID).First();
                }
                return result;
            }

        }

        public Movie GetMovieByTitle(string title)
        {
            IMongoDatabase db = null;
            IMongoCollection<Movie> movies = null;
            try
            {
                db = MongoInstance.GetDatabase;
                movies = db.GetCollection<Movie>("movies");
                var result = movies.Find(p => p.Title == title);
                if (result.Count() > 0)
                    return result.First();
                return null;
            }
            catch (MongoDB.Driver.MongoConnectionException)
            {
                db = MongoInstance.Reconnect;
                movies = db.GetCollection<Movie>("movies");
                var result = movies.Find(p => p.Title == title);
                if (result.Count() > 0)
                    return result.First();
                return null;
            }
        }


        public void updateMovieFB(string imdbId, long shares, long likes)
        {
            IMongoDatabase db = null;
            IMongoCollection<Movie> movies = null;
            var filter = Builders<Movie>.Filter.Eq("IMDbId", imdbId);
            var update = Builders<Movie>.Update
                .Set("FBShares", shares).Set("FBLikes", likes);
            try {
                db = MongoInstance.GetDatabase;
                movies = db.GetCollection<Movie>("movies");
                movies.UpdateOneAsync(filter, update);
            }
            catch (MongoDB.Driver.MongoConnectionException)
            {
                db = MongoInstance.Reconnect;
                movies = db.GetCollection<Movie>("movies");
                movies.UpdateOneAsync(filter, update);
            }
        }

        public List<string> GetMoviesIMDbId(int skip, int limit)
        {
            IMongoDatabase db = null;
            IMongoCollection<Movie> collection = null;
            List<BsonDocument> results = new List<BsonDocument>();
            try
            {
                db = MongoInstance.GetDatabase;
                collection = db.GetCollection<Movie>("movies");
                results = collection.Find(Builders<Movie>.Filter.Empty).Project(Builders<Movie>.Projection.Include(m => m.IMDbId).Exclude(m => m.Id)).Skip(skip).Limit(limit).ToList();
            }
            catch (MongoDB.Driver.MongoConnectionException)
            {
                db = MongoInstance.Reconnect;
                collection = db.GetCollection<Movie>("movies");
                results = collection.Find(Builders<Movie>.Filter.Empty).Project(Builders<Movie>.Projection.Include(m => m.IMDbId).Exclude(m => m.Id)).Skip(skip).Limit(limit).ToList();
            }
            var imdbIds = new List<string>();
           foreach (var item in results)
            {
                BsonValue value;
                item.TryGetValue("IMDbId", out value);
                imdbIds.Add(value.AsString);
            }
            return imdbIds;
        }

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
