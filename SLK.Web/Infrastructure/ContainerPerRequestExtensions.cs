using StructureMap;
using System.Web;

namespace SLK.Web.Infrastructure
{
    public static class ContainerPerRequestExtensions
    {
        public static IContainer GetContainer(this HttpContextBase context)
        {
            return (IContainer)HttpContext.Current.Items["_Container"]
                ?? IoC.Container;
        }
    }
}