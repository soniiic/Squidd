using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Squidd.Commander.Web.Startup))]
namespace Squidd.Commander.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
