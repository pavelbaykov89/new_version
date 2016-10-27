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
            ViewBag.Controller = "Shop";

            return View("~/Views/Shared/Table.cshtml", new ShopListViewModel());
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

            return View(model);
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

            return RedirectToAction<ShopController>(c => c.Table())
                .WithSuccess("Product created!");
        }

        //[Log("Editing product {id}")]
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var model = _context.Products
        //        .ProjectTo<EditProductForm>()
        //        //.First();
        //        .SingleOrDefault(p => p.ID == id);

        //    if (model == null)
        //    {
        //        return RedirectToAction<ProductController>(c => c.Table())
        //            .WithError("Unable to find the issue.  Maybe it was deleted?");
        //    }

        //    //return Json(model, JsonRequestBehavior.AllowGet);
        //    return View(model);
        //}

        //[HttpPost, Log("Product changed")]
        //public ActionResult Edit(EditProductForm model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return JsonValidationError();
        //    }

        //    var product = _context.Products.SingleOrDefault(i => i.ID == model.ID);

        //    if (product == null)
        //    {
        //        return JsonError("Cannot find the product specified.");
        //    }

        //    product.Components = model.Components;
        //    product.ProductMeasure = _context.Measuries.FirstOrDefault(m => m.Name == model.ContentUnitMeasureName);
        //    product.DisplayOrder = model.DisplayOrder;
        //    product.IsKosher = model.IsKosher;
        //    product.KosherType = model.KosherType;
        //    product.MeasureUnitStep = model.MeasureUnitStep;
        //    product.UnitsPerPackage = model.UnitsPerPackage;

        //    _context.SaveChanges();

        //    return RedirectToAction<ProductController>(c => c.Table()).WithSuccess("Product updated!");
        //}

        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase attachment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return JsonValidationError();
        //    }

        //    if (attachment == null)
        //    {
        //        return JsonError("Cannot find the product specified.");
        //    }

        //    var fullname = Server.MapPath("~/App_Data/Temp/") + attachment.FileName;

        //    attachment.SaveAs(fullname);

        //    var task = new TaskDescription("Products importing from", attachment.FileName);

        //    TaskManager.AddTask(task);

        //    Task.Factory.StartNew(() =>
        //    {
        //        ProductsImportService.ImportProductsFromExcelFile(fullname, new ApplicationDbContext(), task);
        //    });

        //    return RedirectToAction<ProductController>(c => c.Table()).WithSuccess("Products file uploaded!");
        //}

        //public ActionResult Download()
        //{
        //    var fileName = "Products_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".xlsx";

        //    var context = new ApplicationDbContext();

        //    return File(ProductsExportService.ExportProductsToExcelFile(
        //                typeof(ProductExportModel).GetProperties(),
        //                context.Products.ProjectTo<ProductExportModel>()),
        //                "application/xlsx",
        //                fileName);
        //}

        //[Log("Deleted product {id}")]
        //public ActionResult Delete(long id)
        //{
        //    var product = _context.Products.Find(id);

        //    if (product == null)
        //    {
        //        return RedirectToAction<ProductController>(c => c.Table())
        //            .WithError("Unable to find the product.  Maybe it was deleted?");
        //    }

        //    product.Delete();

        //    _context.SaveChanges();

        //    return RedirectToAction<ProductController>(c => c.Table())
        //        .WithSuccess("Product deleted!");
        //}
    }
}