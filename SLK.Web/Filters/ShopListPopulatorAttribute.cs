using SLK.DataLayer;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateShopsListAttribute : Attribute
    {
    }

    public class ShopListPopulatorAttribute : ActionFilterAttribute
    {
        public ApplicationDbContext Context { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null && viewResult.Model != null)
            {
                var hasShopListProperties = viewResult.Model.GetType().GetProperties().Any(
                    prop => IsDefined(prop, typeof(PopulateShopsListAttribute)));
                if (hasShopListProperties)
                {
                    var shops = Context.Shops.ToArray();
                    viewResult.ViewBag.Shops = shops.Select(s => new SelectListItem()
                    {
                        Text = s.Name,
                        Value = s.ID.ToString(),
                    })
                    .ToArray();
                }
            }
        }
    }
}