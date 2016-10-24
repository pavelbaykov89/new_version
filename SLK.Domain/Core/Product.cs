using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SLK.Domain.Core
{
    public class Product
    {
        public Product() { }

        public Product(string name, Category category, Manufacturer manufacturer, string shortDesc, string fullDesc, string sku, string imagePath)        
        {
            Name = name;
            Category = category;
            Manufacturer = manufacturer;
            ShortDescription = shortDesc;
            FullDescription = fullDesc;
            SKU = sku;
            Image = imagePath;

            HasImage = imagePath?.Length > 0;
            Deleted = false;

            MeasureUnitStep = 0;
            UnitsPerPackage = 0;
        }

        public int ID { get; protected set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }       

        public string Image { get; protected set; }

        public decimal Rate { get; set; }
        
        public int RateCount { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

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

        public bool HasImage { get; protected set; }

        public int ManufacturerID { get; protected set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public string Brand { get; set; }
        
        public int DisplayOrder { get; set; }

        public bool Deleted { get; set; }
        
        public int? ProductMeasureID { get; set; }

        public virtual ContentUnitMeasure ProductMeasure { get; set; }

        public string MadeCountry { get; set; }

        [MaxLength(4000)]
        public string SeoDescription { get; set; }
        
        [MaxLength(4000)]
        public string SeoKeywords { get; set; }

        //public int? ContentUnitMeasureID { get; set; }

        //public ContentUnitMeasureMap ContentUnitMeasure { get; set; }

        public decimal ContentUnitPriceMultiplicator { get; set; }

        // New property
        //public bool IsVegan { get; set; }       

        // Links
        public virtual ICollection<ProductSKUMap> ProductSKUMaps { get; set; } = new List<ProductSKUMap>();
        
        public virtual ICollection<ProductInShop> ProductInShops { get; set; } = new List<ProductInShop>();

        public List<ProductInShop> ProductShops { get; set; }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
