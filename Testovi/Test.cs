using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using MongoDB.Bson.Serialization.Conventions;
using System.Threading.Tasks;
using System.Collections.Generic;
using static BLL.Recommender;
using MongoDB.Driver;
using System.Linq;

namespace Testovi
{
    [TestClass]
    public class Test
    {
        [TestInitialize()]
        public void Initialize()
        {
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("ignore extra elements", pack, t => true);
            
        }


        //[TestMethod]
        //public void MatchMaking()
        //{
        //    Person Ines = PersonRepository.GetPersonByName("Ines");
        //    Person Marina = PersonRepository.GetPersonByName("Marina");
        //    Assert.IsNotNull(Ines);
        //    Assert.IsNotNull(Marina);
        //    var db = MongoInstance.GetDatabase;
        //    var matches = db.GetCollection<Match>("Match");
        //    matches.InsertOne(new Match(Ines, Marina));
        //}

        ////[TestMethod]
        //public void GenreSimilarityMatrix()
        //{
        //    Movie matrix = MovieRepository.GetMovieByID("tt0133093");
        //    //IList<Movie> similar = Recommender.SimilarByGenres(matrix, 100).Result;
        //    var similar = Recommender.SimilarByGenres(matrix.Genres);
        //    Assert.AreEqual(1, similar.Count());
        //}

        //[TestMethod]
        //public void ActorSimilarityMatrix()
        //{
        //    IList < String > actors = new List<String>();
        //    actors.Add("Keanu Reeves");
        //    actors.Add("Sigourney Weaver");
        //    actors.Add("Goran Visnjic");

        //    var similar = Recommender.SimilarByActors(actors);
        //    Assert.AreEqual(1, 1);
        //}

        //[TestMethod]
        //public void ProfileBuild()
        //{
        //    Person Ines = PersonRepository.GetPersonByName("Ines");
        //    Ines.Profile = new Profile(Ines);
        //    var db = MongoInstance.GetDatabase;
        //    var persons = db.GetCollection<Person>("testPerson");
        //    persons.ReplaceOne(p => p.Email == Ines.Email,
        //        Ines,
        //        new UpdateOptions { IsUpsert = true });
        //    Assert.AreEqual(1, 2);
        //}

        //[TestMethod]
        //public void TopActors()
        //{
        //    Person Ana = PersonRepository.GetPersonByName("Ana", "testPerson");
        //    var top = Ana.Profile.TopActors(5);
        //    Assert.AreEqual(1, top.Count);
        //}

        //[TestMethod]
        //public void RecommendFromProfile()
        //{
        //    var db = MongoInstance.GetDatabase;
        //    db.DropCollection("results");
        //    var collection = db.GetCollection<Movie>("results");
        //    Person Ana = PersonRepository.GetPersonByName("Ana", "testPerson");

        //    IList<Movie> rec = Recommender.RecommendMoviesFromProfile(Ana.Profile, 4, 5, 10);
        //    foreach (Movie m in rec)
        //        collection.InsertOne(m);
        //    Assert.AreEqual(1, rec.Count);
        //}

        //[TestMethod]
        //public void RecommendFromFriends()
        //{
        //    var db = MongoInstance.GetDatabase;
        //    var collection = db.GetCollection<Movie>("results");
        //    Person Ines = PersonRepository.GetPersonByName("Ines");

        //    IList<Movie> rec = Recommender.RecommendFromFriends(Ines, false, -1, 0, 0);
        //    foreach (Movie m in rec)
        //        collection.InsertOne(m);
        //    Assert.AreEqual(1, rec.Count);
        //}

        //[TestMethod]
        //public void RecommendFromEverybody()
        //{
        //    var db = MongoInstance.GetDatabase;
        //    var collection = db.GetCollection<Movie>("results");
        //    Person Ines = PersonRepository.GetPersonByName("Ines");

        //    IList<Movie> rec = Recommender.RecommendMoviesFromEverybody(Ines, false, -1, 0, 0);
        //    foreach (Movie m in rec)
        //        collection.InsertOne(m);
        //    Assert.AreEqual(1, rec.Count);
        //}


        //[TestMethod]
        //public void BuildProfiles()
        //{
        //    PersonRepository.BuildAllProfiles();
        //}

        [TestMethod]
        public void MakeProfile()
        {
            Person Ana = PersonRepository.GetPersonByName("Ana");
            var profile = new Profile(Ana);
        }

    }
}
