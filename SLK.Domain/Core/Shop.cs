using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Device.Location;

namespace SLK.Domain.Core
{
    public class Shop
    {
        public Shop()
        {
            Active = true;
            IsKosher = true;
            IsShipEnabled = true;
            CreationDate = DateTime.Now;
        }

        public int ID { get; protected set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public int OwnerID { get; set; }
                
        public virtual User Owner { get; set; }

        public List<int> ShopTypeID { get; protected set; }

        public virtual List<ShopType> ShopType { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public int DisplayOrder { get; set; }

        public bool HasImage { get; set; }

        public string ImagePath { get; set; }

        public string LogoPath { get; set; }

        public string Address { get; set; }

        //public GeoCoordinate Coordinate { get; set; }
        public DbGeography Coordinate { get; set; }

        public string Phone { get; set; }

        public string Phone2 { get; set; }

        public string Email { get; set; }

        public bool IsKosher { get; set; }

        public bool IsShipEnabled { get; set; }

        public bool Active { get; set; }

        public string SeoUrl { get; set; }

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
