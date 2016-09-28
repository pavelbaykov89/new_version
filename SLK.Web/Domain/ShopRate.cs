using System;

namespace Slk.Domain.Core
{
    public class ShopRate
    {
        protected ShopRate() { }

        public long ID { get; protected set; }

        public long UserID {get; protected set; }

        public virtual User User { get; protected set; }

        public long ShopID { get; protected set; }
        
        public virtual Shop Shop { get; protected set; }

        public decimal Rate { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
    }
}
