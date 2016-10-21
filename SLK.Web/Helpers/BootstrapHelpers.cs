using Microsoft.Web.Mvc.Html;
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SLK.Web.Helpers
{
    public static class BootstrapHelpers
    {
        public static IHtmlString BootstrapLabelFor<TModel, TProp>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel,TProp>> property)
        {
            return helper.LabelFor(property, new
            {
                @class = "col-md-3 control-label"
            });
        }

        public static IHtmlString BootstrapLabelFor(
            this HtmlHelper helper,
            string propertyName)
        {
            return helper.Label(propertyName, new
            {
                @class = "col-md-3 control-label"
            });
        }
    }
}