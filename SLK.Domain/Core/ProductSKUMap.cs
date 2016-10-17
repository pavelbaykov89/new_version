namespace SLK.Domain.Core
{
    public class ProductSKUMap
    {
        protected ProductSKUMap() { }

        public int ID { get; protected set; }

        public string ShortSKU { get; protected set; }

        public int ProductID { get; protected set; }

        public virtual Product Product { get; protected set; }

        public int ShopID { get; protected set; }

        public virtual Shop Shop { get; protected set; }

        public decimal Price { get; protected set; }

        public decimal Quantity { get; protected set; }

        public string ImportProductName { get; protected set; }

        public bool Imported { get; protected set; }
    }
}
