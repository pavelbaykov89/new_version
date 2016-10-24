using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Web.Models;
using SLK.Web.Models.UserModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public ActionResult Table()
        {
            ViewBag.UserMenuActive = "active open";
            ViewBag.UserActive = "active open";

            return View(new UserListViewModel());
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var users = _context.DomainUsers
                    .Where(u => !u.Deleted)
                    .ProjectTo<UserListViewModel>();
                       
            var userNameFilter = Convert.ToString(Request["UserName"]);
            var emailFilter = Convert.ToString(Request["Email"]);
            var firstNameFilter = Convert.ToString(Request["FirstName"]);
            var lastNameFilter = Convert.ToString(Request["LastName"]);
            var linkedFilter = Convert.ToString(Request["LinkedToShopName"]);
            var creationFilter = Convert.ToString(Request["CreationDate"]);
                        
            if (!string.IsNullOrEmpty(userNameFilter))
            {
                users = users.Where(u => u.UserName.Contains(userNameFilter));
            }

            if (!string.IsNullOrEmpty(emailFilter))
            {
                users = users.Where(u => u.Email.Contains(emailFilter));
            }

            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                users = users.Where(u => u.FirstName.Contains(firstNameFilter));
            }

            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                users = users.Where(u => u.LastName.Contains(lastNameFilter));
            }

            if (!string.IsNullOrEmpty(linkedFilter))
            {
                users = users.Where(u => u.LinkedToShopName.Contains(linkedFilter));
            }

            if (!string.IsNullOrEmpty(creationFilter))
            {
                //users = users.Where(u => u.CreationDate.Contains(creationFilter));
            }
                        
            string ordering = "";
            int ind = 0;

            while (Request[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request[$"order[{ind}][column]"]);
                var sortDirection = Request[$"order[{ind}][dir]"];

                ordering += sortColumnIndex == 0 ? "UserName" :
                            sortColumnIndex == 1 ? "Email" :
                            sortColumnIndex == 2 ? "FirstName" :
                            sortColumnIndex == 3 ? "LastName" :
                            sortColumnIndex == 4 ? "LinkedToShopName" :
                            sortColumnIndex == 5 ? "CreationDate" : "";

                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                users = users.OrderBy(ordering).AsQueryable();
            }

            var count = users.Count();

            users = users
                .Skip(param.start)
                .Take(param.length);

            var totalCount = _context.DomainUsers.Where(u => !u.Deleted).Count();

            return Json(new
            {
                draw = param.draw,
                recordsTotal = totalCount,
                recordsFiltered = count,
                data = users.ToArray()
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}