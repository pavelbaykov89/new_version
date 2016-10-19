using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;

namespace SLK.Web.Models.ProductModels
{
    public class ProductExportModel : IMapFrom<Product>
    {
        public string SKU { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string Image { get; set; }

        public decimal Rate { get; set; }

        public int RateCount { get; set; }
        
        public string CategoryName { get; set; }

        public bool NoTax { get; set; }

        public bool IsFeaturedTop { get; set; }

        public bool IsFeaturedLeft { get; set; }

        public bool IsKosher { get; set; }

        public string KosherType { get; set; }

        public string Capacity { get; set; }

        public string MeasureUnit { get; set; }

        public decimal MeasureUnitStep { get; set; }

        public bool SoldByWeight { get; set; }

        public decimal UnitsPerPackage { get; set; }

        public string ProductShopOptions { get; set; }

        public string Components { get; set; }

        public bool IgnoreOnImport { get; set; }

        public bool HasImage { get; set; }
                
        public string ProductManufacturerName { get; set; }

        public int DisplayOrder { get; set; }

        public bool Deleted { get; set; }
        
        public string ProductMeasureDisplayName { get; set; }

        public string MadeCountry { get; set; }
                
        public string SeoDescription { get; set; }
                
        public string SeoKeywords { get; set; }
        
        public decimal ContentUnitPriceMultiplicator { get; set; }
    }
}