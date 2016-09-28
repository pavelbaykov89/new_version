using StructureMap;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SLK.Web.Infrastructure
{
    public class StructureMapFilterProvider : FilterAttributeFilterProvider
    {
        private readonly Func<IContainer> _containerFactory;

        public StructureMapFilterProvider(Func<IContainer> containerFactory)
        {
            _containerFactory = containerFactory;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            var container = _containerFactory();

            foreach (var filter in filters)
            {
                container.BuildUp(filter.Instance);
                yield return filter;
            }
        }
    }
}