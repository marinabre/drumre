using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using MongoDB.Bson.Serialization.Conventions;

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
    }
}
