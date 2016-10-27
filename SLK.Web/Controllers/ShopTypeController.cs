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

namespace SLK.Web.Controllers
{
    public class ShopTypeController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ShopTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Table()
        {
            ViewBag.ShopTypeMenuActive = "active open";
            ViewBag.ShopTypeActive = "active open";

            var model = _context.ShopTypes
               .ProjectTo<ShopTypeListViewModel>()
               .FirstOrDefault();

            model = model ?? new ShopTypeListViewModel();
            model.AddNewForm = new AddEditShopTypeForm(Url.Action("AddNew"));
            model.EditUrl = Url.Action("Edit");
            model.DeleteUrl = Url.Action("Delete");
            
            ViewBag.Title = "Shop Types";
            ViewBag.Controller = "ShopType";

            return View("~/Views/Shared/Table.cshtml", model);
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters<ShopTypeListViewModel>(
                _context.ShopTypes.ProjectTo<ShopTypeListViewModel>(),
                Request.Params,
                typeof(ShopTypeListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddNew(AddEditShopTypeForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model);
            }

            var newType = new ShopType();
            newType.Name = model.Name;
            newType.DisplayOrder = model.DisplayOrder;
            _context.ShopTypes.Add(newType);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult Edit(int id)
        {
            var shopType = _context.ShopTypes.Find(id);
            var model = new AddEditShopTypeForm(Url.Action("Edit"), id);
            model.Name = shopType.Name;
            model.DisplayOrder = shopType.DisplayOrder;

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

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

        public ActionResult Delete(int id)
        {
            var shopType = _context.ShopTypes.Find(id);
            _context.ShopTypes.Remove(shopType);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}