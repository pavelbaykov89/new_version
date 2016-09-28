namespace SLK.Domain.Core
{
    public class OrderItem
    {
        protected OrderItem() { }

        public long ID { get; protected set; }

        public long OrderID { get; protected set; }

        public virtual Order Order { get; protected set; }

        public long ProductInShopID { get; protected set; }
        
        public virtual ProductInShop ProductInShop { get; protected set; }

        public decimal Price { get; protected set; }

        public decimal Quantity { get; protected set; }

        public decimal DiscountAmount { get; protected set; }

        public string DiscountDescription { get; set; }

        public string ItemComment { get; set; }

        public int Status { get; set; }
    }
}
