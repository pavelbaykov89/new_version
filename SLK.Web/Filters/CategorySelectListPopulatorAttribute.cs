using SLK.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    public class CategorySelectListPopulatorAttribute : ActionFilterAttribute
    {
        public ApplicationDbContext Context { get; set; }

        private SelectListItem[] GetAvailableCategories()
        {
            return Context.Categories.Select(c => 
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ID.ToString()
                }).ToArray();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;

            if (viewResult != null && viewResult.Model is IHaveCategorySelectList)
            {
                ((IHaveCategorySelectList)viewResult.Model).AvailableCategories
                    = GetAvailableCategories();
            }
        }
    }

    public interface IHaveCategorySelectList
    {
        SelectListItem[] AvailableCategories { get; set; }
    }
}