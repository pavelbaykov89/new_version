using SLK.Web.Utilities;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Helpers
{
    public static class JsonHtmlHelpers
    {
        public static IHtmlString JsonFor<T>(this HtmlHelper helper, T obj)
        {
            return helper.Raw(obj.ToJson());
        }
    }
}