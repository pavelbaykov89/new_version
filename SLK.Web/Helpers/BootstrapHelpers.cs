using Microsoft.Web.Mvc.Html;
using System;
using System.Linq;
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
            Expression<Func<TModel,TProp>> property,
            string colMd)
        {
            return LabelHelper(helper, ModelMetadata.FromLambdaExpression(property, helper.ViewData), ExpressionHelper.GetExpressionText(property),
                "col-md-" + colMd + "  control-label");
            //return helper.LabelFor(property, new
            //{
            //    @class = "col-md-3 control-label"
            //});
        }

        public static IHtmlString BootstrapLabelFor(
            this HtmlHelper helper,
            string propertyName,
            string colMd)
        {
            return LabelHelper(helper, ModelMetadata.FromStringExpression(propertyName, helper.ViewData), ExpressionHelper.GetExpressionText(propertyName),
                "col-md-" + colMd + " control-label");
            //return helper.Label(propertyName, new
            //{
            //    @class = "col-md-3 control-label"
            //});
        }

        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string cssClass)
        {
            var isNullable = metadata.IsNullableValueType || !metadata.ModelType.IsValueType;
            var propertyName = htmlFieldName.Split('.').Last();
            var label = new TagBuilder("label");
            label.Attributes["for"] = TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName));
            label.Attributes["class"] = cssClass;
            label.InnerHtml = string.Format(
                "{0}{1}",
                metadata.DisplayName,
                isNullable && metadata.IsRequired ? "*" : "&nbsp;"
            );
            return MvcHtmlString.Create(label.ToString());
        }
    }
}
