using System.Collections.Generic;

namespace Slk.Domain.Core
{
    public class Category
    {
        protected Category() { }

        public Category(string name)         
        {
            Name = name;
        }

        public long ID { get; protected set; }

        public string Name { get; protected set; }

        public long? ParentCategoryID { get; protected set; }

        public virtual Category ParentCategory { get; protected set; }

        public bool HasImage { get; protected set; }

        public string ImagePath { get; protected set; }

        public int DisplayOrder { get; protected set; }

        public bool Published { get; protected set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<Category> Childs { get; set; } = new List<Category>();
    }
}
