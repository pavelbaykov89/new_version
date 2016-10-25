using SLK.Domain.Core;
using SLK.DataLayer;
using SLK.Web.Infrastructure.Tasks;
using System.Linq;
using System;

namespace SLK.Web.App_Start
{
    public class SeedData : IRunAtStartup
    {
        private readonly ApplicationDbContext _context;

        public SeedData(ApplicationDbContext context)
        {   
            _context = context;
        }


        private  ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                if(_userManager == null)
                    _userManager = ApplicationUserManager.Create(null, _context);
                return _userManager;
            }
        }

        public void Execute()
        {
            if (!_context.Users.Any())
            {
                //var user = _context.Users.Add(new ApplicationUser
                //{
                //    Email = "test@test.test",
                //    UserName = "TestUser"                    
                //});
                
                var user = new ApplicationUser { UserName = "a@slk.co.il", Email = "a@slk.co.il" };
                var result = UserManager.CreateAsync(user, "dina1212").Result;
                if (result.Succeeded)
                {                
                    var domainUser = new User(user.Id, "a@slk.co.il", "a@slk.co.il");
                    domainUser.FirstName = "Main";
                    domainUser.LastName = "Admin";
                    domainUser.FollowSLKNews = true;

                    _context.DomainUsers.Add(domainUser);
                    _context.SaveChanges();
                }

            }

            var category = _context.Categories.FirstOrDefault() ??
                        _context.Categories.Add(new Category("Food"));

            var manuf1 = _context.Manufacturers.FirstOrDefault() ??
                        _context.Manufacturers.Add(new Manufacturer("Alpine cows"));

            var manuf2 = _context.Manufacturers.FirstOrDefault(m => m.Name == "Bread Galaxy") ??
                        _context.Manufacturers.Add(new Manufacturer("Bread Galaxy"));

            var manuf3 = _context.Manufacturers.FirstOrDefault(m => m.Name == "Snikers") ??
                        _context.Manufacturers.Add(new Manufacturer("Snikers"));

            var product1 = _context.Products.FirstOrDefault() ??
                        _context.Products.Add(new Product("Milk", category, manuf1, "Milk 3.2%", "Alpine Cows Milk 3.2%", "324231554443", "/Content/images/34747426476.png"));

            var product2 = _context.Products.FirstOrDefault(p => p.Name == "Bread") ??
                        _context.Products.Add(new Product("Bread", category, manuf2, "Softly bread", "The best bread in the world", "464323454523", ""));

            var product3 = _context.Products.FirstOrDefault(p => p.Name == "Chocolate") ??
                        _context.Products.Add(new Product("Chocolate", category, manuf3, "Milk Chocolate", "Snikers milk chocolate", "047323432182", null));

            _context.SaveChanges();
        }
    }
}