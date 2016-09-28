using AutoMapper.QueryableExtensions;
using Microsoft.Web.Mvc;
using Slk.Domain.Core;
using SLK.Web.Data;
using SLK.Web.Filters;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
using SLK.Web.Models;
using System;
using System.Linq;
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
        public ActionResult Index()
        {
            var model = _context.Products
                .Where(p => !p.Deleted)
                .ProjectTo<ProductsListViewModel>();
                
            return View(model);
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

            var catID = Convert.ToInt64(model.CategoryID);

            var manID = Convert.ToInt64(model.ManufacturerID);

            var product = new Product(model.Name, _context.Categories.FirstOrDefault(c => c.ID == catID),
                _context.Manufacturers.FirstOrDefault(m => m.ID == manID), 
                model.ShortDescription, model.FullDescription, model.SKU, model.ImagePath);

            product.Components = model.Components;
            product.ContentUnitMeasure = _context.Measuries.FirstOrDefault(m=> m.Name == model.ContentUnitMeasureName);
            product.DisplayOrder = model.DisplayOrder;
            product.IsKosher = model.IsKosher;
            product.IsVegan = model.IsVegan;
            product.KosherType = model.KosherType;
            product.MeasureUnitStep = model.MeasureUnitStep;
            product.UnitsPerPackage = model.UnitsPerPackage;

            _context.Products.Add(product);

            _context.SaveChanges();

            return RedirectToAction<ProductController>(c => c.Index())
                .WithSuccess("Product created!");
        }

        [Log("Editing product {id}")]
        public ActionResult Edit(long id)
        {
            var model = _context.Products
                .ProjectTo<EditProductForm>()
                .SingleOrDefault(p => p.ID == id);
            
            if (model == null)
            {
                return RedirectToAction<ProductController>(c => c.Index())
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            return View(model);
        }

        [HttpPost, Log("Product changed")]
        public ActionResult Edit(EditProductForm model)
        {
            if (!ModelState.IsValid)
            {
                //return JsonValidationError();
            }

            var product = _context.Products.SingleOrDefault(i => i.ID == model.ID);

            if (product == null)
            {
                //return JsonError("Cannot find the issue specified.");
            }

            product.Components = model.Components;
            product.ContentUnitMeasure = _context.Measuries.FirstOrDefault(m => m.Name == model.ContentUnitMeasureName);
            product.DisplayOrder = model.DisplayOrder;
            product.IsKosher = model.IsKosher;
            product.IsVegan = model.IsVegan;
            product.KosherType = model.KosherType;
            product.MeasureUnitStep = model.MeasureUnitStep;
            product.UnitsPerPackage = model.UnitsPerPackage;

            _context.SaveChanges();

            return RedirectToAction<ProductController>(c => c.Index()).WithSuccess("Product updated!");
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Deleted product {id}")]
        public ActionResult Delete(long id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction<HomeController>(c => c.Index())
                    .WithError("Unable to find the product.  Maybe it was deleted?");
            }

            product.Delete();

            _context.SaveChanges();

            return RedirectToAction<HomeController>(c => c.Index())
                .WithSuccess("Product deleted!");
        }
    }
}