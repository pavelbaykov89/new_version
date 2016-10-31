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

        #region Product Table Functionality
        
        // GET: Product List
        public ActionResult Table()        
        {
            ViewBag.ProductMenuActive = "active open";
            ViewBag.ProductActive = "active open";
            
            ViewBag.Shops = new List<string>{ "supertlv", "nehama", "superyuda" };

            var model = new ProductsListViewModel();
            model.AddNewForm = new AddEditProductForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Product";
            model.Editable = true;
            model.Popup = true;

            ViewBag.Title = "Products";            

            return View(model);
        }

        // Ajax: Products by filters
        public ActionResult List()
        {   
            var result = PopulateService.PopulateByFilters(
                _context.Products.Where(p => !p.Deleted).ProjectTo<ProductsListViewModel>(),
                Request.Params,
                typeof(ProductsListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Add Product
        [HttpPost, ValidateAntiForgeryToken, Log("Created product")]
        public ActionResult New(AddEditProductForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model).WithWarning("Some fields are invalid!");
            }

            var catID = Convert.ToInt32(model.CategoryID);

            var manID = Convert.ToInt32(model.ManufacturerID);

            var product = new Product(model.Name, _context.Categories.FirstOrDefault(c => c.ID == catID),
                _context.Manufacturers.FirstOrDefault(m => m.ID == manID), 
                model.ShortDescription, model.FullDescription, model.SKU, model.Image);

            product.Components = model.Components;
            product.ProductMeasure = _context.Measuries.FirstOrDefault(m=> m.Name == model.ContentUnitMeasureName);
            product.DisplayOrder = model.DisplayOrder;
            product.IsKosher = model.IsKosher;
            //product.IsVegan = model.IsVegan;
            product.KosherType = model.KosherType;            
            product.UnitsPerPackage = model.UnitsPerPackage;

            _context.Products.Add(product);

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Edit Product Form
        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Products
                .ProjectTo<AddEditProductForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");
            
            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update Product
        [HttpPost, Log("Product changed")]
        public ActionResult Edit(AddEditProductForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
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

            return Json(new { success = true });
        }

        // GET: Delete Product
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

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Import/Export Functionality
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
        #endregion

        #region Tasks Functionality
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
        #endregion
    }
}