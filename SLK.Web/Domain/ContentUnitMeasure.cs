using System.Collections.Generic;

namespace Slk.Domain.Core
{
    public class ContentUnitMeasure
    {
        protected ContentUnitMeasure() { }

        public long ID { get; protected set; }

        public string Name { get; protected set; }

        public string DisplayName { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<ContentUnitMeasureMap> ContentUnitMeasureMaps { get; set; } = new List<ContentUnitMeasureMap>();
    }
}
