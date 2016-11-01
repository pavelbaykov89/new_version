using SLK.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateProductsListAttribute : Attribute
    {
    }

    public class ProductListPopulatorAttribute : ActionFilterAttribute
    {
        public ApplicationDbContext Context { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null && viewResult.Model != null)
            {
                var hasProductListProperties = viewResult.Model.GetType().GetProperties().Any(
                    prop => IsDefined(prop, typeof(PopulateProductsListAttribute)));
                if (hasProductListProperties)
                {
                    var products = Context.Products.ToArray();
                    viewResult.ViewBag.Products = products.Select(p => new SelectListItem()
                    {
                        Text = p.Name,
                        Value = p.ID.ToString(),
                    })
                    .ToArray();
                }
            }
        }
    }
}