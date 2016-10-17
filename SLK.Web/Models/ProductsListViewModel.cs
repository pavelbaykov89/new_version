using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SLK.Web.Models
{
    public class ProductsListViewModel : IMapFrom<Product>
    {        
        public int ID { get; set; }

        public string Name { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string SKU { get; set; }

        [DisplayName("Manufacturer")]
        public string ProductManufacturerName { get; set; }

        [DisplayName("Image?")]
        public bool HasImage { get; set; }
    }
}