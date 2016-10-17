using System;
using System.Collections.Generic;

namespace SLK.Domain.Core
{
    public class User
    {
        #region Constructors

        protected User() { }

        public User(string identityID, string email, string password)
            : this()
        {
            IdentityID = identityID;
            Email = email;
            Password = password;

            CreationDate = DateTime.Now;
 
            //AddRole(Role.DefaultRole);
        }
        #endregion

        #region Properties

        public int ID { get; protected set; }

        public string IdentityID { get; protected set; }

        public string Email { get; protected set; }

        public string Password { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool FollowSLKNews { get; set; }

        public long? LinkedToShopID { get; protected set; }

        public virtual Shop LinkedToShop { get; protected set; }

        public bool Deleted { get; set; }
        
        public virtual ICollection<Shop> ShopsSubsForNews { get; set; } = new List<Shop>();

        public virtual ICollection<Shop> ShopsSubsForSocial { get; set; } = new List<Shop>();

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        public virtual ICollection<UserAddressSearch> SearchAddresses { get; set; } = new List<UserAddressSearch>();

        public virtual ICollection<ShopRate> ShopRates { get; set; } = new List<ShopRate>();

        public virtual ICollection<ProductRate> ProductRates { get; set; } = new List<ProductRate>();

        public virtual ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();

        public virtual ICollection<ProductComment> ProductComments { get; set; } = new List<ProductComment>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        #endregion

        #region Member functions

        public void AddRole(Role role)
        {
            if (Roles.Contains(role))
                return;
 
            Roles.Add(role);
        }
        #endregion
    }
}
