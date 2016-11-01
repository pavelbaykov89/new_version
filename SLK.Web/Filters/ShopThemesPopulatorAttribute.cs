using SLK.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateShopThemesAttribute : Attribute
    {

    }

    public class ShopThemesPopulatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null && viewResult.Model != null)
            {
                var property = viewResult.Model.GetType().GetProperties().FirstOrDefault(
                    prop => IsDefined(prop, typeof(PopulateShopThemesAttribute)));
                if (property != null)
                {
                    viewResult.ViewData["SimpleDropdownList_" + property.Name] = new SelectListItem[]
                    {
                        new SelectListItem()
                        {
                            Value = "SlkOrigin",
                            Text = "SlkOrigin"
                        },
                        new SelectListItem()
                        {
                            Value = "Supertlv",
                            Text = "Supertlv"
                        },
                    };
                }
            }
        }
    }
}