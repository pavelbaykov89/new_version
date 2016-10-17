namespace SLK.Domain.Core
{
    public class ContentUnitMeasureMap
    {
        protected ContentUnitMeasureMap() { }

        public int ID { get; protected set; }

        public int ContentUnitMeasureID { get; protected set; }

        public virtual ContentUnitMeasure ContentUnitMeasure { get; protected set; }

        public string Name { get; protected set; }

        public string Synonymous { get; set; }

        public decimal Multiplicator { get; protected set; }
    }
}
