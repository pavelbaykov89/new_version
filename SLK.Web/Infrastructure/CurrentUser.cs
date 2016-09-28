using Microsoft.AspNet.Identity;
using SLK.DataLayer;
using SLK.Web.Models;
using System.Security.Principal;

namespace SLK.Web.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity _identity;
        private readonly ApplicationDbContext _context;

        private ApplicationUser _user;

        public CurrentUser(IIdentity identity, ApplicationDbContext context)
        {
            _identity = identity;
            _context = context;
        }

        public ApplicationUser User
        {
            get { return _user ?? (_user = _context.Users.Find(_identity.GetUserId())); }
        }
    }
}