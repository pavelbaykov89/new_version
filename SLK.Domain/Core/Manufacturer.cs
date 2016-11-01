using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class Manufacturer
    {
        protected Manufacturer() { }       

        public Manufacturer(string name)
        {
            Name = name;
        }

        public int ID { get; protected set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public bool Published { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
