using SLK.DataLayer;
using SLK.Web.Infrastructure.ModelMetadata.Filters;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    /// <summary>
    /// Marker attribute name should starts with "Populate"(for Editor templates engine)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateUsersListAttribute : Attribute
    {

    }

    public class UserListPopulatorAttribute : DropdownPopulatorAttribute
    {
        public UserListPopulatorAttribute() : base(typeof(PopulateUsersListAttribute))
        { }

        public ApplicationDbContext Context { get; set; }

        protected override SelectListItem[] Populate()
        {
            return Context.DomainUsers
                .Where(u => !u.Deleted)
                .Select(u => new SelectListItem()
                {
                    Text = u.Email,
                    Value = u.ID.ToString(),
                })
                .ToArray();
        }
    }
}