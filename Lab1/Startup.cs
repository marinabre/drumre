using Projekt.Models;
using Microsoft.Owin;
using MongoDB.Driver;
using Owin;
using MongoDB.Bson.Serialization.Conventions;

[assembly: OwinStartupAttribute(typeof(Projekt.Startup))]
namespace Projekt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("ignore extra elements", pack, t => true);
            ConfigureAuth(app);
        }
    }
}
