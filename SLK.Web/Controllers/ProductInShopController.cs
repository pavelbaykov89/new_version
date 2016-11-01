using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Services;
using SLK.Services.Task;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
using SLK.Web.Models.ProductInShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class ProductInShopController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ProductInShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Users Table Functionality

        // GET: Users List
        public ActionResult Table()
        {
            ViewBag.ProductMenuActive = "active open";
            ViewBag.ProductInShopActive = "active open";
            ViewBag.Title = "Products In Shop";

            var model = new ProductInShopListViewModel();
            model.AddNewForm = new AddEditProductInShopForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "ProductInShop";
            model.Editable = true;
            model.Popup = true;

            //return View("~/Views/Shared/Table.cshtml", model);
            return View(model);
        }

        // Ajax: Users by filters
        public ActionResult List()
        {
            var result = PopulateService.PopulateByFilters(
                _context.ProductInShops.ProjectTo<ProductInShopListViewModel>(),
                Request.Params,
                typeof(ProductInShopListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            foreach (var user in result.data)
            {
                user.StringCreationDate = user.CreationDate.ToString("dd/MM/yyyy");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New()
        {
            var model = new AddEditProductInShopForm();
           
            return View(model);
        }

        // POST: Add ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(AddEditProductInShopForm model)
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
            var model = _context.ProductInShops
                .ProjectTo<AddEditProductInShopForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update ShopType
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditProductInShopForm model)
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
            var productInShop = _context.ProductInShops.Find(id);
            _context.ProductInShops.Remove(productInShop);
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

            var task = new TaskDescription("ProductsInShop importing from", attachment.FileName);

            TaskManager.AddTask(task);

            Task.Factory.StartNew(() =>
            {
                ProductsInShopImportService.ImportProductsFromExcelFile(fullname, new ApplicationDbContext(), task);
            });

            return RedirectToAction<ProductInShopController>(c => c.Table()).WithSuccess("Products file uploaded!");
        }        
        #endregion
    }
}