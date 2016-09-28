using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class Product
    {
        protected Product() { }

        public Product(string name, Category category, Manufacturer manufacturer, string shortDesc, string fullDesc, string sku, string imagePath)        
        {
            Name = name;
            Category = category;
            Manufacturer = manufacturer;
            ShortDescription = shortDesc;
            FullDescription = fullDesc;
            SKU = sku;
            ImagePath = imagePath;

            HasImage = imagePath?.Length > 0;
            Deleted = false;
        }

        public long ID { get; protected set; }

        public string SKU { get; protected set; }

        public string Name { get; protected set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public bool HasImage { get; protected set; }

        public string ImagePath { get; protected set; }

        public int DisplayOrder { get; set; }

        public long CategoryID { get; protected set; }

        public virtual Category Category { get; protected set; }

        public long ManufacturerID { get; protected set; }
        
        public virtual Manufacturer Manufacturer { get; protected set; }

        public bool IsVegan { get; set; }

        public bool IsKosher { get; set; }

        public string KosherType { get; set; }

        public string Components { get; set; }

        //public long ProductMeasureID { get; protected set; }

        public decimal MeasureUnitStep { get; set; }

        public decimal UnitsPerPackage { get; set; }

        public long? ContentUnitMeasureID { get; }

        public virtual ContentUnitMeasure ContentUnitMeasure { get; set; }

        public bool Deleted { get; protected set; }

        public virtual ICollection<ProductSKUMap> ProductSKUMaps { get; set; } = new List<ProductSKUMap>();
        
        public virtual ICollection<ProductInShop> ProductInShops { get; set; } = new List<ProductInShop>();

        public void Delete()
        {
            Deleted = true;
        }
    }
}
