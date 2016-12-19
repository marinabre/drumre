using Projekt.Models;
using Microsoft.Owin;
using MongoDB.Driver;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projekt.Startup))]
namespace Projekt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //MongoClient client = new MongoClient();
            //var db = client.GetDatabase("Projekt");
            //var collection = db.GetCollection<Person>("Person");

            //Person person = new Person
            //{
            //    Name = "Banana",
            //    Surname = "Nas",
            //    Birthday = System.DateTime.Now,
            //};

            //collection.InsertOne(person);
        }
    }
}
