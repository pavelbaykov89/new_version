using System;

namespace SLK.Domain.Core
{
    public class ProductComment
    {
        protected ProductComment() { }

        public long ID { get; protected set; }

        public long UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public long ProductInShopID { get; protected set; }

        public virtual ProductInShop ProductInShop { get; protected set; }

        public string Title { get; protected set; }

        public string Comment { get; protected set; }

        public DateTime CreatedOn { get; protected set; }

        public bool Approved { get; protected set; }
    }
}
