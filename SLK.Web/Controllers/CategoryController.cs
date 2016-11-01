using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services;
using SLK.Web.Filters;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
using SLK.Web.Models;
using SLK.Web.Models.CategoryModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class CategoryController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        #region Categories Table Functionality

        // GET: Categories List
        public ActionResult Table()
        {
            ViewBag.CategoryMenuActive = "active open";
            ViewBag.CategoryActive = "active open";
            ViewBag.Title = "Categories";

            var model = new CategoryListViewModel();         
            model.AddNewForm = new AddEditCategoryForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Category";
            model.Editable = true;
            model.Popup = true;

            return View("~/Views/Shared/Table.cshtml", model);
        }

        // Ajax: Categories by filters
        public ActionResult List()
        {
            var result = PopulateService.PopulateByFilters(
                _context.Categories.ProjectTo<CategoryListViewModel>(),
                Request.Params,
                typeof(CategoryListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Add Category
        [HttpPost, ValidateAntiForgeryToken, Log("Created category")]
        public ActionResult New(AddEditCategoryForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model).WithWarning("Some fields are invalid!");
            }

            var category = new Category(model.Name);
            category.ParentCategoryID = Convert.ToInt32(model.CategoryID);
            category.ImagePath = model.ImagePath;
            category.DisplayOrder = model.DisplayOrder;
            category.Published = model.Published;

            _context.Categories.Add(category);

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Edit Category Form
        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Categories
                .ProjectTo<AddEditCategoryForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update Category
        [HttpPost, Log("Product changed")]
        public ActionResult Edit(AddEditCategoryForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
            }

            var category = _context.Categories.SingleOrDefault(i => i.ID == model.ID);

            if (category == null)
            {
                return JsonError("Cannot find the product specified.");
            }
                       
            category.ParentCategoryID = Convert.ToInt32(model.CategoryID);
            category.ImagePath = model.ImagePath;
            category.DisplayOrder = model.DisplayOrder;
            category.Published = model.Published;

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Delete Category
        [Log("Deleted product {id}")]
        public ActionResult Delete(long id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return RedirectToAction<CategoryController>(c => c.Table())
                    .WithError("Unable to find the product.  Maybe it was deleted?");
            }

            _context.Categories.Remove(category);

            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}