using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SLK.Web.ProductModels
{
    public class EditProductForm : IMapFrom<Product>
    {
        [HiddenInput]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
                
        public string ShortDescription { get; set; }
              
        public string FullDescription { get; set; }

        [Required]
        public string SKU { get; set; }
        
        public string ImagePath { get; set; }

        [DataType("Integer")]
        public int DisplayOrder { get; set; }

        [Required, Display(Name = "Category")]
        public string CategoryID { get; set; }

        [Required, Display(Name = "Manufacturer")]
        public string ProductManufacturerID { get; set; }        

        public bool IsKosher { get; set; }

        public string KosherType { get; set; }

        public string Components { get; set; }

        public decimal MeasureUnitStep { get; set; }

        public decimal UnitsPerPackage { get; set; }

        public string ContentUnitMeasureName { get; set; }
    }
}