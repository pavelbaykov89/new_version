using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services;
using SLK.Services.Task;
using SLK.Web.Filters;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
using SLK.Web.Models;
using SLK.Web.Models.ProductModels;
using SLK.Web.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class ProductController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public ActionResult Table()        
        {
            ViewBag.ProductMenuActive = "active open";
            ViewBag.ProductActive = "active open";
            
            ViewBag.Shops = new List<string>{ "supertlv", "nehama", "superyuda" };

            var model = new ProductsListViewModel();
            model.AddNewForm = null;
            model.EditUrl = Url.Action("Edit");
            model.DeleteUrl = Url.Action("Delete");

            ViewBag.Title = "Products";
            ViewBag.Controller = "Product";

            return View(model);
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {   
            var result = PopulateService.PopulateByFilters<ProductsListViewModel>(
                _context.Products.Where(p => !p.Deleted).ProjectTo<ProductsListViewModel>(),
                Request.Params,
                typeof(ProductsListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);

           
        }

        public ActionResult New()
        {
            var model = new NewProductForm();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Created product")]
        public ActionResult New(NewProductForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model).WithWarning("Some fields are invalid!");
            }

            var catID = Convert.ToInt32(model.CategoryID);

            var manID = Convert.ToInt32(model.ManufacturerID);

            var product = new Product(model.Name, _context.Categories.FirstOrDefault(c => c.ID == catID),
                _context.Manufacturers.FirstOrDefault(m => m.ID == manID), 
                model.ShortDescription, model.FullDescription, model.SKU, model.ImagePath);

            product.Components = model.Components;
            product.ProductMeasure = _context.Measuries.FirstOrDefault(m=> m.Name == model.ContentUnitMeasureName);
            product.DisplayOrder = model.DisplayOrder;
            product.IsKosher = model.IsKosher;
            //product.IsVegan = model.IsVegan;
            product.KosherType = model.KosherType;            
            product.UnitsPerPackage = model.UnitsPerPackage;

            _context.Products.Add(product);

            _context.SaveChanges();

            return RedirectToAction<ProductController>(c => c.Table())
                .WithSuccess("Product created!");
        }

        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Products
                .ProjectTo<EditProductForm>()
                //.First();
                .SingleOrDefault(p => p.ID == id);
            
            if (model == null)
            {
                return RedirectToAction<ProductController>(c => c.Table())
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            //return Json(model, JsonRequestBehavior.AllowGet);
            return View(model);
        }

        [HttpPost, Log("Product changed")]
        public ActionResult Edit(EditProductForm model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var product = _context.Products.SingleOrDefault(i => i.ID == model.ID);

            if (product == null)
            {
                return JsonError("Cannot find the product specified.");
            }

            product.Components = model.Components;
            product.ProductMeasure = _context.Measuries.FirstOrDefault(m => m.Name == model.ContentUnitMeasureName);
            product.DisplayOrder = model.DisplayOrder;
            product.IsKosher = model.IsKosher;         
            product.KosherType = model.KosherType;            
            product.UnitsPerPackage = model.UnitsPerPackage;

            _context.SaveChanges();

            return RedirectToAction<ProductController>(c => c.Table()).WithSuccess("Product updated!");
        }
   
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase attachment)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            if (attachment == null)
            {
                return JsonError("Cannot find the product specified.");
            }
                        
            var fullname = Server.MapPath("~/App_Data/Temp/") + attachment.FileName;

            attachment.SaveAs(fullname);

            var task = new TaskDescription("Products importing from", attachment.FileName);

            TaskManager.AddTask(task);

            Task.Factory.StartNew(() => 
            {
                ProductsImportService.ImportProductsFromExcelFile(fullname, new ApplicationDbContext(), task);
            });

            return RedirectToAction<ProductController>(c => c.Table()).WithSuccess("Products file uploaded!");
        }
               
        public ActionResult Download()
        {
            var pictureFilter = Convert.ToString(Request["picture"]);
            var categoryFilter = Convert.ToString(Request["category"]);
            var anyShop = true;
            var shopFilter = new Dictionary<string, bool>();

            foreach (var shop in new List<string> { "supertlv", "nehama", "superyuda" })
            {
                var value = (Request[shop] != null);

                if (anyShop) anyShop = !value;

                shopFilter.Add(shop, value);
            }


            var fileName = "Products_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".xlsx";
            
            var context = new ApplicationDbContext();

            return File(ProductsExportService.ExportProductsToExcelFile(
                        typeof(ProductExportModel).GetProperties(),
                        context.Products.Where(p => !p.Deleted).ProjectTo<ProductExportModel>(),
                        pictureFilter, categoryFilter, anyShop, shopFilter),
                        "application/xlsx",
                        fileName);
        }

        [Log("Deleted product {id}")]
        public ActionResult Delete(long id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction<ProductController>(c => c.Table())
                    .WithError("Unable to find the product.  Maybe it was deleted?");
            }

            product.Delete();

            _context.SaveChanges();

            return RedirectToAction<ProductController>(c => c.Table())
                .WithSuccess("Product deleted!");
        }
        
        [HttpGet]
        public ActionResult FilesCount()
        {
            var files = System.IO.Directory.EnumerateFiles(Server.MapPath("~/FilesToDownload/")).Select(f => f.Substring(f.LastIndexOf('\\') + 1));

            return Json(new
            {
                count = files.Count(),
                files = files
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TasksCount()
        {
            return Json(new
            {
                count = TaskManager.GetTasksCount()
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ActiveTasks()
        {
            var tasks = TaskManager.GetTasks();

            return Json(new
            {
                count = tasks.Count,
                tasks = tasks.ToArray()
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}