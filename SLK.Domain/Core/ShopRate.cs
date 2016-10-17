using System;

namespace SLK.Domain.Core
{
    public class ShopRate
    {
        protected ShopRate() { }

        public int ID { get; protected set; }

        public int UserID {get; protected set; }

        public virtual User User { get; protected set; }

        public int ShopID { get; protected set; }
        
        public virtual Shop Shop { get; protected set; }

        public decimal Rate { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
    }
}
