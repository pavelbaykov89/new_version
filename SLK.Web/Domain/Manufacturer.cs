using System.Collections.Generic;

namespace Slk.Domain.Core
{
    public class Manufacturer
    {
        protected Manufacturer() { }       

        public Manufacturer(string name)
        {
            Name = name;
        }

        public long ID { get; protected set; }

        public string Name { get; protected set; }

        public string ImagePath { get; protected set; }

        public int DisplayOrder { get; protected set; }

        public bool Published { get; protected set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
