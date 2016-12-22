using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using MongoDB.Bson.Serialization.Conventions;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task Apokalipsa()
        {
            //await Recommender.getSimilar("tt0133093");
        }
    }
}
