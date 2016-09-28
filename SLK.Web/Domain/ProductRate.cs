using System;

namespace Slk.Domain.Core
{
    public class ProductRate
    {
        protected ProductRate() { }

        public long ID { get; protected set; }

        public long UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public long ProductInShopID { get; protected set; }

        public virtual ProductInShop ProductInShop { get; protected set; }

        public decimal Rate { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
    }
}
