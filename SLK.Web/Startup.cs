using Microsoft.Owin;
using Owin;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Tasks;

[assembly: OwinStartupAttribute(typeof(SLK.Web.Startup))]
namespace SLK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            using (var container = IoC.Container.GetNestedContainer())
            {
                foreach (var task in container.GetAllInstances<IRunAtStartup>())
                {
                    task.Execute();
                }
            }
        }
    }
}
