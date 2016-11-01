using SLK.Web.Infrastructure.ModelMetadata.Filters;
using System;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    /// <summary>
    /// Marker attribute name should starts with "Populate"(for Editor templates engine)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateShopThemesAttribute : Attribute
    {

    }

    public class ShopThemesPopulatorAttribute : DropdownPopulatorAttribute
    {
        public ShopThemesPopulatorAttribute() : base(typeof(PopulateShopThemesAttribute))
        { }
        
        protected override SelectListItem[] Populate()
        {
            return new SelectListItem[]
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