using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Deli_Website.Startup))]
namespace Deli_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
