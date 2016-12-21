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
        }
    }
}
