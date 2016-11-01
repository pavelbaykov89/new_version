using SLK.Web.Helpers;
using System;
using System.Web.Mvc;

namespace SLK.Web.Infrastructure.ModelMetadata.Filters
{
    public abstract class DropdownPopulatorAttribute : ActionFilterAttribute
    {
        private readonly Type _markerAttributeType;

        protected DropdownPopulatorAttribute(Type markerAttributeType)
        {
            _markerAttributeType = markerAttributeType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResultBase;
            if (viewResult != null && viewResult.Model != null)
            {
                var hasProperty = ReflectionHelper.HasPropertyWithAttribute(viewResult.Model, _markerAttributeType);
                if (hasProperty)
                {
                    viewResult.ViewData[_markerAttributeType.Name] = Populate();
                }
            }
        }

        protected abstract SelectListItem[] Populate();
    }
}