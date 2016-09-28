using Microsoft.Web.Mvc;
using SLK.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Infrastructure
{
    [CategorySelectListPopulator,
    ManufacturerSelectListPopulator]
    public abstract class SLKController : Controller
    {
        protected ActionResult RedirectToAction<TController>(
            Expression<Action<TController>> action)
            where TController : Controller
        {
            return ControllerExtensions.RedirectToAction(this, action);
        }
    }
}