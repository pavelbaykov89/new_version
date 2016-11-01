using SLK.DataLayer;
using SLK.Web.Infrastructure.ModelMetadata.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    /// <summary>
    /// Marker attribute name should starts with "Populate"(for Editor templates engine)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateManufacturersAttribute : Attribute
    {

    }

    public class ManufacturerSelectListPopulatorAttribute : DropdownPopulatorAttribute
    {
        public ManufacturerSelectListPopulatorAttribute() : base(typeof(PopulateManufacturersAttribute))
        { }

        public ApplicationDbContext Context { get; set; }

        protected override SelectListItem[] Populate()
        {
            return Context.Manufacturers.Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.ID.ToString()
                }).ToArray();
        }
    }
}