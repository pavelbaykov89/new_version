using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class ShopType
    {
        public ShopType() { }

        public int ID { get; protected set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
    }
}
