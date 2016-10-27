using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Web.Models;
using SLK.Web.Models.ShopTypeModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using SLK.Domain.Core;
using SLK.Web.Infrastructure;
using SLK.Services;
using SLK.Web.Infrastructure.Alerts;

namespace SLK.Web.Controllers
{
    public class ShopTypeController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ShopTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region ShopTypes Table Functionality
        
        // GET: ShopTypes List
        public ActionResult Table()
        {
            ViewBag.ShopTypeMenuActive = "active open";
            ViewBag.ShopTypeActive = "active open";
            
            var model = new ShopTypeListViewModel();
            model.AddNewForm = new AddEditShopTypeForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "ShopType";
            model.Editable = true;
            model.Popup = true;

            ViewBag.Title = "Shop Types";            

            return View("~/Views/Shared/Table.cshtml", model);
        }

        // Ajax: ShopTypes by filters
        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters<ShopTypeListViewModel>(
                _context.ShopTypes.ProjectTo<ShopTypeListViewModel>(),
                Request.Params,
                typeof(ShopTypeListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        // POST: Add ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(AddEditShopTypeForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model).WithWarning("Some fields are invalid!");
            }

            var newType = new ShopType();
            newType.Name = model.Name;
            newType.DisplayOrder = model.DisplayOrder;
            _context.ShopTypes.Add(newType);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Edit ShopType Form
        public ActionResult Edit(int id)
        {
            var model = _context.ShopTypes
                .ProjectTo<AddEditShopTypeForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditShopTypeForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
            }

            var shopType = _context.ShopTypes.Find(model.ID.Value);
            shopType.Name = model.Name;
            shopType.DisplayOrder = model.DisplayOrder;

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Delete ShopType
        public ActionResult Delete(int id)
        {
            var shopType = _context.ShopTypes.Find(id);
            _context.ShopTypes.Remove(shopType);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}