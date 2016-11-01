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
    public class PopulateCategoriesAttribute : Attribute
    {

    }


    public class CategorySelectListPopulatorAttribute : DropdownPopulatorAttribute
    {
        public CategorySelectListPopulatorAttribute() : base(typeof(PopulateCategoriesAttribute))
        { }

        public ApplicationDbContext Context { get; set; }

        protected override SelectListItem[] Populate()
        {
            return Context.Categories.Select(c =>
                 new SelectListItem
                 {
                     Text = c.Name,
                     Value = c.ID.ToString()
                 }).ToArray();
        }       
    }
}