using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Elvencurse2.Website.Startup))]
namespace Elvencurse2.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
