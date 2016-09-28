using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SLK.Web.Startup))]
namespace SLK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
