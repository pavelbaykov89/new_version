using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class ContentUnitMeasure
    {
        protected ContentUnitMeasure() { }

        public int ID { get; protected set; }

        public string Name { get; protected set; }

        public string DisplayName { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<ContentUnitMeasureMap> ContentUnitMeasureMaps { get; set; } = new List<ContentUnitMeasureMap>();
    }
}
