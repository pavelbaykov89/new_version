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
using SLK.Services.FileStorage;

namespace SLK.Web.Controllers
{
    public class ShopController : SLKController
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilesRepository _filesRepo;

        public ShopController(ApplicationDbContext context, IFilesRepository filesRepo)
        {
            _filesRepo = filesRepo;
            _context = context;
        }

        public ActionResult Table()
        {
            ViewBag.ShopMenuActive = "active open";
            ViewBag.ShopActive = "active open";
            ViewBag.Title = "Shops";

            var model = new ShopListViewModel();            
            //model.AddNewForm = new AddEditShopForm();
            //model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Shop";
            //model.Editable = true;

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

        public ActionResult New()
        {
            ViewBag.ShopMenuActive = "active open";
            ViewBag.ShopActive = "active open";

            var model = new AddEditShopForm();
            _filesRepo.BuildUrl("dasdadas");
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Created shop")]
        public ActionResult New(AddEditShopForm model)
        {
            ViewBag.ShopMenuActive = "active open";
            ViewBag.ShopActive = "active open";

            if (!ModelState.IsValid)
            {
                return View(model)
                    //this extension breaks default MVC validation 
                   //.WithWarning("Some fields are invalid!")
                   ;
            }

            var shop = new Shop();
            //TODO - это нужно сделать automapper'ом
            shop.Name = model.MainTab.Name;
            shop.ShortDescription = model.MainTab.ShortDescription;
            shop.FullDescription = model.MainTab.FullDescription;
            shop.Address = model.MainTab.Address;
            shop.Phone = model.MainTab.Phone;
            shop.Email = model.MainTab.Email;
            shop.IsKosher = model.MainTab.IsKosher;
            shop.SeoUrl = model.MainTab.SeoUrl;
            shop.Theme = model.MainTab.Theme;
            shop.OwnerID = model.MainTab.OwnerID.Value;

            if (model.MainTab.Image != null)
            {
                var imageId = _filesRepo.Create(model.MainTab.Image.InputStream, model.MainTab.Image.FileName, new[] { "jpg", "png", "jpeg", "bmp" });
                shop.ImagePath = _filesRepo.BuildUrl(imageId);
                shop.HasImage = true;
            }
            if (model.MainTab.Logo != null)
            {
                var logoId = _filesRepo.Create(model.MainTab.Logo.InputStream, model.MainTab.Logo.FileName, new[] { "jpg", "png", "jpeg", "bmp" });
                shop.LogoPath = _filesRepo.BuildUrl(logoId);
            }
            if (model.MainTab.Favicon != null)
            {
                var favId = _filesRepo.Create(model.MainTab.Favicon.InputStream, model.MainTab.Favicon.FileName, new[] { "jpg", "png", "jpeg", "ico" });
                shop.FaviconPath = _filesRepo.BuildUrl(favId);
            }

            _context.Shops.Add(shop);
            _context.SaveChanges();

            return RedirectToAction<ShopController>(c => c.Table());
        }

        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Shops
                .ProjectTo<AddEditShopForm>()
                .SingleOrDefault(p => p.ID == id);
            //model.AddOrEditUrl = Url.Action("Edit");

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