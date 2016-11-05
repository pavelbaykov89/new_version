using System;
using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class ProductInShop
    {
        public ProductInShop() { }

        public int ID { get; protected set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        public int ShopID { get; set; }

        public virtual Shop Shop { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IncludeVAT { get; set; }

        public bool IncludeInShippingPrice { get; set; }

        public decimal PricebyUnit { get; set; }

        public decimal MaxCartQuantity { get; set; }

        public int QuantityType { get; set; }

        public string ProductOptions { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public virtual ICollection<ProductRate> ProductRates { get; set; } = new List<ProductRate>();

        public virtual ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();

        public virtual ICollection<ProductComment> ProductComments { get; set; } = new List<ProductComment>();
    }
}
