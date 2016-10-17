using System;

namespace SLK.Domain.Core
{
    public class ProductNote
    {
        protected ProductNote() { }

        public int ID { get; protected set; }

        public int UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public int ProductInShopID { get; protected set; }

        public virtual ProductInShop ProductInShop { get; protected set; }

        public string Note { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
    }
}
