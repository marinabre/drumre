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

        [TestMethod]
        public void dohvatFilma()
        {
            //tt0317219
            Movie result = MovieRepository.GetMovieByID("tt0317219");
            Assert.AreEqual("tt0317219", result.IMDbId);
        }

        [TestMethod]
        public void usporedba()
        //tt0133093 - The Matrix
        //tt0118971 - Devil's Advocate
        {
            Movie result = MovieRepository.GetMovieByID("tt0133093");
            Assert.AreEqual("tt0133093", result.IMDbId);
            Movie result2 = MovieRepository.GetMovieByID("tt0118971");
            Assert.AreEqual("tt0118971", result2.IMDbId);

            MovieSimilarity sim = Recommender.getSimilarity(result, result2);
            Assert.AreNotEqual(0, sim.actorSimilarity);
        }

        //[TestMethod]
        //public async Task Apokalipsa()
        //{
        //    //await Recommender.getSimilar("tt0133093");
        //}

        [TestMethod]
        public void MatchMaking()
        {
            Person Ines = PersonRepository.GetPersonByName("Ines");
            Person Marina = PersonRepository.GetPersonByName("Marina");
            Assert.IsNotNull(Ines);
            Assert.IsNotNull(Marina);
            var db = MongoInstance.GetDatabase;
            var matches = db.GetCollection<Match>("Match");
            matches.InsertOne(new Match(Ines, Marina));
        }

        //[TestMethod]
        public void GenreSimilarityMatrix()
        {
            Movie matrix = MovieRepository.GetMovieByID("tt0133093");
            //IList<Movie> similar = Recommender.SimilarByGenres(matrix, 100).Result;
            var similar = Recommender.SimilarByGenres(matrix.Genres);
            Assert.AreEqual(1, similar.Count());
        }

        [TestMethod]
        public void ActorSimilarityMatrix()
        {
            IList < String > actors = new List<String>();
            actors.Add("Keanu Reeves");
            actors.Add("Sigourney Weaver");
            actors.Add("Goran Visnjic");


            var similar = Recommender.SimilarByActors(actors);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void ProfileBuild()
        {
            Person Ana = PersonRepository.GetPersonByName("Ana");
            Ana.Profile = new Profile(Ana);
            var db = MongoInstance.GetDatabase;
            var persons = db.GetCollection<Person>("testPerson");
            persons.ReplaceOne(p => p.Email == Ana.Email,
                Ana,
                new UpdateOptions { IsUpsert = true });
            Assert.AreEqual(1, 2);
        }

        [TestMethod]
        public void TopActors()
        {
            Person Ana = PersonRepository.GetPersonByName("Ana", "testPerson");
            var top = Ana.Profile.TopActors(5);
            Assert.AreEqual(1, top.Count);
        }

        [TestMethod]
        public void MatchProfile()
        {
            var db = MongoInstance.GetDatabase;
            var collection = db.GetCollection<Movie>("results");
            Person Ana = PersonRepository.GetPersonByName("Ana", "testPerson");
            
            IList<Movie> rec = Recommender.MatchProfile(Ana.Profile, 3, 5, 5);
            foreach (Movie m in rec)
                collection.InsertOne(m);
            Assert.AreEqual(1, rec.Count);
        }

    }
}
