using System;
using System.Device.Location;

namespace SLK.Domain.Core
{
    public class UserAddressSearch
    {
        protected UserAddressSearch() { }

        public long ID { get; protected set; }

        public long UserID { get; protected set; }

        public virtual User User { get; protected set; }

        public string Address { get; protected set; }

        public GeoCoordinate Coordinate { get; protected set; }

        public long ShopID { get; protected set; }

        public virtual Shop Shop { get; protected set; }

        public DateTime CreatedOn { get; protected set; }

        public string PageUrl { get; protected set; }

        public string RefererUrl { get; protected set; }

        public string WrotenAddress { get; protected set; }

        public long ShopTypeID { get; protected set; }

        public virtual ShopType ShopType { get; protected set; }
    }
}
