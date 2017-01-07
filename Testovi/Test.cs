using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using MongoDB.Bson.Serialization.Conventions;
using System.Threading.Tasks;
using System.Collections.Generic;
using static BLL.Recommender;
using MongoDB.Driver;
using System.Linq;
using BLL.Models;

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

        //[TestMethod]
        //public void Matrix()
        //{
        //    MovieRepository m= new MovieRepository();
        //    Movie matrix = m.GetMovieByID("tt0133093");
        //    MovieRepository.FBData(matrix);
        //    Assert.AreEqual(0, matrix.FBShares);
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
        //    Person person = PersonRepository.GetPersonByName("Marina");
        //    person.Profile = new Profile(person);
        //    var db = MongoInstance.GetDatabase;
        //    var persons = db.GetCollection<Person>("testPerson");
        //    persons.ReplaceOne(p => p.Email == person.Email,
        //        person,
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
        //    //var db = MongoInstance.GetDatabase;
        //    //db.DropCollection("results");
        //    //var collection = db.GetCollection<Movie>("results");
        //    Person Ana = PersonRepository.GetPersonByName("Tena");

        //    Ana.Profile.FavouriteActors.Add("Keanu Reeves", 1);


        //    IList<Movie> rec = Recommender.RecommendMoviesFromProfile(Ana.Profile, 4, 5, 10);
        //    //foreach (Movie m in rec)
        //    //    collection.InsertOne(m);
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

        //[TestMethod]
        //public void BuildInesProfiles()
        //{
        //    Person Ines = PersonRepository.GetPersonByName("Ines");
        //    PersonRepository.BuildProfile(Ines);
        //}

        //[TestMethod]
        //public void MakeProfile()
        //{
        //    Person Ana = PersonRepository.GetPersonByName("Ana");
        //    var profile = new Profile(Ana);
        //}

        //[TestMethod]
        //public void Filter()
        //{
        //    Person Ana = PersonRepository.GetPersonByName("Ana");
        //    var profile = new Profile(Ana);
        //    IList<string> genres = new List<string>();
        //    genres.Add("Action");
        //    genres.Add("Drama");
        //    Filter f = new Filter(genres);

        //    var res = FilterResults(profile.LikedMovies, f);
        //    Assert.AreEqual(0, res.Count);
        //    //Assert.AreNotEqual(profile.LikedMovies.Count, res.Count);

        //}

        //[TestMethod]
        //public void GetPersonByMail()
        //{
        //    Person Ana = PersonRepository.GetPersonByEmail("žlabr", true);
        //    Assert.IsNotNull(Ana.Profile);
        //}


        //[TestMethod]
        //public void TestNullYear()
        //{
        //    Person Ana = PersonRepository.GetPersonByName("Ana");
        //    var profile = new Profile(Ana);
        //    IList<string> genres = new List<string>();

        //    genres.Add("Action");
        //    //genres.Add("Drama");
        //    Filter f = new Filter(genres);
        //    f.YearFrom = 1999;
        //    f.IMDBRatingFrom = 7;

        //    var res = FilterResults(profile.LikedMovies, f);
        //    Assert.AreEqual(0, res.Count);
        //    //Assert.AreNotEqual(profile.LikedMovies.Count, res.Count);

        //}

        //[TestMethod]
        //public void BuildInesProfiles()
        //{
        //    Person Ines = PersonRepository.GetPersonByName("Ines");
        //    PersonRepository.BuildProfile(Ines);
        //}

        //[TestMethod]
        //public void TestNullYear()
        //{
        //    Person Ana = PersonRepository.GetPersonByName("Ana");
        //    var profile = new Profile(Ana);
        //    IList<string> actors = new List<string>();

        //    actors.Add("Keanu Reeves");
        //    //genres.Add("Drama");
        //    Filter f = new Filter();
        //    f.Actors = actors;
        //    //f.YearFrom = 1999;
        //    //f.IMDBRatingFrom = 7;

        //    var res = FilterResults(new List<Movie> (), f);
        //    Assert.AreEqual(0, res.Count);
        //    //Assert.AreNotEqual(profile.LikedMovies.Count, res.Count);

        //}

        //[TestMethod]
        //public void SearchEmpty()
        //{
        //    var res = MovieRepository.SearchFilter(null);
        //    Assert.AreEqual(0, res.Count);
        //}

        //[TestMethod]
        //public void Search()
        //{
        //    //isprobajte ovdi sve:
        //    Filter filter = new Filter();
        //    filter.Actors = new List<String>();
        //    filter.Actors.Add("Keanu Reeves");
        //    filter.Actors.Add("Al Pacino");
        //    //filter.YearFrom = 2011;
        //    //filter.IMDBRatingFrom = 9;


        //    var res = MovieRepository.SearchFilter(filter);
        //    Assert.AreEqual(0, res.Count());
        //}

        //[TestMethod]
        //public void meh()
        //{
        //    PersonRepository.meh();
        //}

        [TestMethod]
        public void deleteProfile()
        {
            Person p = PersonRepository.GetPersonByName("Ines");
            PersonRepository.DeleteProfile(p);
            p = PersonRepository.GetPersonByName("Ana");
            PersonRepository.DeleteProfile(p);
        }

    }
}
