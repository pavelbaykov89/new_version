using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Services;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
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

        #region Users Table Functionality

        // GET: Users List
        public ActionResult Table()
        {
            ViewBag.UserMenuActive = "active open";
            ViewBag.UserActive = "active open";
            ViewBag.Title = "Users";

            var model = new UserListViewModel();            
            model.AddNewForm = new AddNewUserModel();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
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

        // POST: Add ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(AddNewUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model).WithWarning("Some fields are invalid!");
            }

            // ToDo Add adding user

            return Json(new { success = true });
        }

        // GET: Edit ShopType Form
        public ActionResult Edit(int id)
        {
            var model = _context.DomainUsers
                .ProjectTo<AddNewUserModel>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AddNewUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
            }

            // ToDo Updating user

            return Json(new { success = true });
        }

        // GET: Delete ShopType
        public ActionResult Delete(int id)
        {
            var user = _context.DomainUsers.Find(id);
            _context.DomainUsers.Remove(user);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}