using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Domain.Extensions;
using SLK.Web.Infrastructure;
using SLK.Web.Models;
using SLK.Web.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using SLK.Web.Filters;
using SLK.Web.Infrastructure.Alerts;
using SLK.Domain.Core;
using SLK.Services;

namespace SLK.Web.Controllers
{
    public class ShopController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Table()
        {
            ViewBag.ShopMenuActive = "active open";
            ViewBag.ShopActive = "active open";
            ViewBag.Title = "Shops";

            var model = new ShopListViewModel();            
            model.AddNewForm = new AddEditShopForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Shop";
            model.Editable = true;
            model.Popup = true;

            return View("~/Views/Shared/Table.cshtml", model);
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters<ShopListViewModel>(
                _context.Shops.ProjectTo<ShopListViewModel>(),
                Request.Params,
                typeof(ShopListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost, ValidateAntiForgeryToken, Log("Created shop")]
        public ActionResult New(AddEditShopForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model).WithWarning("Some fields are invalid!");
            }

            var shop = new Shop();
            //TODO - это нужно сделать automapper'ом
            shop.Name = model.Name;
            shop.ShortDescription = model.ShortDescription;
            shop.FullDescription = model.FullDescription;
            shop.DisplayOrder = model.DisplayOrder;
            shop.HasImage = !string.IsNullOrEmpty(model.ImagePath);
            shop.ImagePath = model.ImagePath;
            shop.LogoPath = model.LogoPath;
            shop.Address = model.Address;
            shop.Phone = model.Phone;
            shop.Phone2 = model.Phone2;
            shop.Email = model.Email;
            shop.IsKosher = model.IsKosher;
            shop.IsShipEnabled = model.IsShipEnabled;
            shop.Active = model.Active;
            shop.SeoUrl = model.SeoUrl;
            shop.Owner = _context.DomainUsers.First();

            _context.Shops.Add(shop);

            _context.SaveChanges();

            return Json(new { success = true });
        }

        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Shops
                .ProjectTo<AddEditShopForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        [HttpPost, Log("Product changed")]
        public ActionResult Edit(AddEditShopForm model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var shop = _context.Shops.SingleOrDefault(i => i.ID == model.ID);

            if (shop == null)
            {
                return JsonError("Cannot find the product specified.");
            }

            // ToDo Update Shop

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Delete Shop
        public ActionResult Delete(int id)
        {
            var shop = _context.Shops.Find(id);
            _context.Shops.Remove(shop);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}