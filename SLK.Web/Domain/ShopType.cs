using System.Collections.Generic;

namespace Slk.Domain.Core
{
    public class ShopType
    {
        protected ShopType() { }

        public long ID { get; protected set; }

        public string Name { get; protected set; }

        public int DisplayOrder { get; protected set; }

        public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
    }
}
