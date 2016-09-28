using Microsoft.AspNet.Identity.EntityFramework;
using Slk.Domain.Core;
using SLK.Web.Domain;
using SLK.Web.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SLK.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Childs)
                .WithOptional(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryID);
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<LogAction> Logs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ContentUnitMeasure> Measuries { get; set; }

        public DbSet<ContentUnitMeasureMap> MeasureMaps { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<ProductInShop> ProductInShops { get; set; }

        public DbSet<ProductNote> ProductNotes { get; set; }

        public DbSet<ProductRate> ProductRates { get; set; }

        public DbSet<ProductSKUMap> ProductSKUMaps { get; set; }

        public DbSet<Role> DomainRoles { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<ShopRate> ShopRates { get; set; }

        public DbSet<ShopShipTime> ShopShipTimes { get; set; }

        public DbSet<ShopType> ShopTypes { get; set; }

        public DbSet<ShopWorkTime> ShopWorkTimes { get; set; }

        public DbSet<User> DomainUsers { get; set; }

        public DbSet<UserAddressSearch> UserAddressSearchs { get; set; }
    }
}