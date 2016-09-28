using SLK.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    public class ManufacturerSelectListPopulatorAttribute : ActionFilterAttribute
    {
        public ApplicationDbContext Context { get; set; }

        private SelectListItem[] GetAvailableManufacturers()
        {
            return Context.Manufacturers.Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.ID.ToString()
                }).ToArray();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;

            if (viewResult != null && viewResult.Model is IHaveManufacturerSelectList)
            {
                ((IHaveManufacturerSelectList)viewResult.Model).AvailableManufacturers
                    = GetAvailableManufacturers();
            }
        }
    }

    public interface IHaveManufacturerSelectList
    {
        SelectListItem[] AvailableManufacturers { get; set; }
    }
}