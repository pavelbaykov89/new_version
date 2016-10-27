using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Services;
using SLK.Web.Infrastructure;
using SLK.Web.Models;
using SLK.Web.Models.UserModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class UserController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users List
        public ActionResult Table()
        {
            ViewBag.UserMenuActive = "active open";
            ViewBag.UserActive = "active open";
            ViewBag.Title = "Users";

            var model = new UserListViewModel();            
            model.AddNewForm = null;
            //model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "User";
            model.Editable = true;
            model.Popup = true;

            return View("~/Views/Shared/Table.cshtml", model);
        }

        // Ajax: Users by filters
        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters<UserListViewModel>(
                _context.DomainUsers.Where(p => !p.Deleted).ProjectTo<UserListViewModel>(),
                Request.Params,
                typeof(UserListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            foreach (var user in result.data)
            {
                user.StringCreationDate = user.CreationDate.ToString("dd/MM/yyyy");
            }

            return Json(result, JsonRequestBehavior.AllowGet);            
        }
    }
}