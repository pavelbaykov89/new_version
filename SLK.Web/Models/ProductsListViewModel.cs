using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLK.Web.Models
{
    public class ProductsListViewModel : IMapFrom<Product>
    {
        public long ID { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public bool HasImage { get; set; }
        
        public string CategoryName { get; set; }

        public string ManufacturerName { get; set; }
    }
}