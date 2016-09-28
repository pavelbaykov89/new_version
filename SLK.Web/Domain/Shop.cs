using System;
using System.Collections.Generic;
using System.Device.Location;

namespace Slk.Domain.Core
{
    public class Shop
    {
        protected Shop() { }

        public long ID { get; protected set; }

        public string Name { get; protected set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public long OwnerID { get; protected set; }
                
        public virtual User Owner { get; protected set; }

        public long ShopTypeID { get; protected set; }

        public virtual ShopType ShopType { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public int DisplayOrder { get; protected set; }

        public bool HasImage { get; protected set; }

        public string ImagePath { get; protected set; }

        public string LogoPath { get; protected set; }

        public string Address { get; protected set; }

        public GeoCoordinate Coordinate { get; protected set; }

        public string Phone { get; protected set; }

        public string Phone2 { get; protected set; }

        public string Email { get; protected set; }

        public bool IsKosher { get; protected set; }

        public bool IsShipEnabled { get; protected set; }

        public bool Active { get; protected set; }

        public virtual ICollection<UserAddressSearch> UserAddressSearchs { get; set; } = new List<UserAddressSearch>();

        public virtual ICollection<User> UsersSubsForNews { get; set; } = new List<User>();

        public virtual ICollection<User> UsersSubsForSocial { get; set; } = new List<User>();

        public virtual ICollection<ShopShipTime> ShopShipTimes { get; set; } = new List<ShopShipTime>();

        public virtual ICollection<ShopWorkTime> ShopWorkTimes { get; set; } = new List<ShopWorkTime>();

        public virtual ICollection<ShopRate> ShopRates { get; set; } = new List<ShopRate>();

        public virtual ICollection<ProductSKUMap> ProductSKUMaps { get; set; } = new List<ProductSKUMap>();

        public virtual ICollection<ProductInShop> Products { get; set; } = new List<ProductInShop>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<User> LinkedToShop { get; set; } = new List<User>();
    }
}
