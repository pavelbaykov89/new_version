using System;
using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class ProductInShop
    {
        protected ProductInShop() { }

        public long ID { get; protected set; }

        public long ProductID { get; protected set; }

        public virtual Product Product { get; protected set; }

        public long ShopID { get; protected set; }

        public virtual Shop Shop { get; protected set; }

        public decimal Price { get; protected set; }

        public decimal Quantity { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public decimal PricebyUnit { get; protected set; }

        public decimal MaxCartQuantity { get; protected set; }

        public int QuantityType { get; protected set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public virtual ICollection<ProductRate> ProductRates { get; set; } = new List<ProductRate>();

        public virtual ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();

        public virtual ICollection<ProductComment> ProductComments { get; set; } = new List<ProductComment>();
    }
}
