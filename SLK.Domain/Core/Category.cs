using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class Category
    {
        protected Category() { }

        public Category(string name)         
        {
            Name = name;
        }

        public int ID { get; protected set; }

        public string Name { get; protected set; }

        public int? ParentCategoryID { get; set; }

        public virtual Category ParentCategory { get; set; }
        
        public string ImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public bool Published { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<Category> Childs { get; set; } = new List<Category>();
    }
}
