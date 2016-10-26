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

            return View(new ShopListViewModel());
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var shops = _context.Shops
                    .ProjectTo<ShopListViewModel>();

            int displayOrderFilterInteger;

            var nameFilter = Convert.ToString(Request["Store name"]);
            var phoneFilter = Convert.ToString(Request["Phone"]);
            var cellularFilter = Convert.ToString(Request["Cellular"]);
            var emailFilter = Convert.ToString(Request["Orders delivery email"]);
            var kosherFilter = Convert.ToString(Request["Kosher"]);
            var activeFilter = Convert.ToString(Request["Active"]);
            var displayOrderFilter = (Int32.TryParse(Request["Store importance"], out displayOrderFilterInteger)) ? "y" : "";
            var domainFilter = Convert.ToString(Request["Domain address extension"]);

            shops = shops
                .WhereIfText(nameFilter, p => p.Name.Contains(nameFilter))
                .WhereIfText(phoneFilter, p => p.Phone.Contains(phoneFilter))
                .WhereIfText(cellularFilter, p => p.Phone2.Contains(cellularFilter))
                .WhereIfText(emailFilter, p => p.Email.Contains(emailFilter))
                .WhereIf(!string.IsNullOrEmpty(kosherFilter) && kosherFilter != "any", p => p.IsKosher == (kosherFilter == "true" ? true : false))
                .WhereIf(!string.IsNullOrEmpty(activeFilter) && activeFilter != "any", p => p.Active == (activeFilter == "true" ? true : false))
                .WhereIfText(displayOrderFilter, p => p.DisplayOrder == displayOrderFilterInteger)
                .WhereIfText(domainFilter, p => p.SeoUrl.Contains(domainFilter));
           

            string ordering = "";
            int ind = 0;

            while (Request[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request[$"order[{ind}][column]"]);
                var sortDirection = Request[$"order[{ind}][dir]"];

                ordering += sortColumnIndex == 0 ? "Name" :
                            sortColumnIndex == 1 ? "DisplayOrder" :
                            sortColumnIndex == 2 ? "Phone" :
                            sortColumnIndex == 3 ? "Phone2" :
                            sortColumnIndex == 4 ? "Email" :
                            sortColumnIndex == 5 ? "IsKosher" :
                            sortColumnIndex == 6 ? "Active" :
                            sortColumnIndex == 8 ? "SeoUrl" : "";


                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                shops = shops.OrderBy(ordering).AsQueryable();
            }

            var count = shops.Count();

            shops = shops
                .Skip(param.start)
                .Take(param.length);

            var totalCount = _context.Products.Where(p => !p.Deleted).Count();

            return Json(new
            {
                draw = param.draw,
                recordsTotal = totalCount,
                recordsFiltered = count,
                data = shops.ToArray()
            },
             JsonRequestBehavior.AllowGet);
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
            //TODO - может быть это можно сделать автомаппером
            shop.Name = model.Name;
            shop.ShortDescription = model.ShortDescription;
            shop.FullDescription = model.FullDescription;
            shop.DisplayOrder = model.DisplayOrder;
            shop.HasImage = model.HasImage;
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