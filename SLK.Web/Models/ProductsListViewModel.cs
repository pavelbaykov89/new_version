using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models
{
    public class ProductsListViewModel : ListModel, IMapFrom<Product>
    {        
        [HiddenInput]
        public int ID { get; set; }

        public string Name { get; set; }

        public string SKU { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Manufacturer")]
        public string ProductManufacturerName { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        [DisplayName("Has Image")]
        public bool HasImage { get; set; }

        public int DisplayOrder { get; set; }

    }
}