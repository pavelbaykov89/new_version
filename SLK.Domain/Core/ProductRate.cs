using System;

namespace SLK.Domain.Core
{
    public class ProductRate
    {
        protected ProductRate() { }

        public int ID { get; protected set; }

        public int UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public int ProductInShopID { get; protected set; }

        public virtual ProductInShop ProductInShop { get; protected set; }

        public decimal Rate { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
    }
}
