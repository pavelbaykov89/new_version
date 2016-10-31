using SLK.DataLayer;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PopulateUsersListAttribute : Attribute
    {

    }

    public class UserListPopulatorAttribute : ActionFilterAttribute
    {
        public ApplicationDbContext Context { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null && viewResult.Model != null) {
                var hasUserListProperties = viewResult.Model.GetType().GetProperties().Any(
                    prop => IsDefined(prop, typeof(PopulateUsersListAttribute)));
                if (hasUserListProperties)
                {
                    var users = Context.DomainUsers.Where(u => !u.Deleted).ToArray();
                    viewResult.ViewBag.Users = users.Select(u => new SelectListItem()
                    {
                        Text = u.Email,
                        Value = u.ID.ToString(),
                    })
                    .ToArray();
                }
            }
        }
    }
}