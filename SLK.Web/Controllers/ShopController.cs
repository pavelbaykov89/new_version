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
            model.AddNewForm = new AddEditShopForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Shop";
            model.Editable = true;

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
            var model = new AddEditShopForm();
            _filesRepo.BuildUrl("dasdadas");
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Created shop")]
        public ActionResult New(AddEditShopForm model)
        {            
            if (!ModelState.IsValid)
            {
                return View(model)
                    //this extension breaks default MVC validation 
                   //.WithWarning("Some fields are invalid!")
                   ;
            }

            var shop = new Shop();
            //TODO - это нужно сделать automapper'ом
            shop.Name = model.Name;
            shop.ShortDescription = model.ShortDescription;
            shop.FullDescription = model.FullDescription;
            shop.Address = model.Address;
            shop.Phone = model.Phone;
            shop.Email = model.Email;
            shop.IsKosher = model.IsKosher;
            shop.SeoUrl = model.SeoUrl;
            shop.OwnerID = model.OwnerID.Value;

            if(model.Image != null)
            {
                var imageId = _filesRepo.Create(model.Image.InputStream, model.Image.FileName, new[] { "jpg", "png", "jpeg", "bmp" });
                shop.ImagePath = _filesRepo.BuildUrl(imageId);
                shop.HasImage = true;
            }
            if (model.Logo != null)
            {
                var logoId = _filesRepo.Create(model.Logo.InputStream, model.Logo.FileName, new[] { "jpg", "png", "jpeg", "bmp" });
                shop.LogoPath = _filesRepo.BuildUrl(logoId);
            }
            if (model.Favicon != null)
            {
                var favId = _filesRepo.Create(model.Favicon.InputStream, model.Favicon.FileName, new[] { "jpg", "png", "jpeg", "ico" });
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