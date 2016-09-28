using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Web.Models;
using StructureMap;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace SLK.Web.Infrastructure.StructureMapRegistry
{
    public class MvcRegistry : Registry
    {
        public MvcRegistry()
        {
            For<BundleCollection>().Use(BundleTable.Bundles);
            For<RouteCollection>().Use(RouteTable.Routes);
            For<IIdentity>().Use(() => HttpContext.Current.User.Identity);
            For<HttpSessionStateBase>()
                .Use(() => new HttpSessionStateWrapper(HttpContext.Current.Session));
            For<HttpContextBase>()
                .Use(() => new HttpContextWrapper(HttpContext.Current));
            For<HttpServerUtilityBase>()
                .Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));

            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            For<DbContext>().Use<ApplicationDbContext>();
            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}