using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services;
using SLK.Web.Filters;
using SLK.Web.Infrastructure;
using SLK.Web.Infrastructure.Alerts;
using SLK.Web.Models;
using SLK.Web.Models.ManufacturerModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class ManufacturerController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ManufacturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Manufacturers Table Functionality

        // GET: Manufacturers List
        public ActionResult Table()
        {
            ViewBag.ManufacturerMenuActive = "active open";
            ViewBag.ManufacturerActive = "active open";
            ViewBag.Title = "Manufacturers";
            
            var model = new ManufacturerListViewModel();
            model.AddNewForm = new AddEditManufacturerForm();
            model.AddNewForm.AddOrEditUrl = Url.Action("New");
            model.ControllerName = "Manufacturer";
            model.Editable = true;
            model.Popup = true;

            ViewBag.Title = "Manufacturers";


            return View("~/Views/Shared/Table.cshtml", model);
        }

        // Ajax: Manufacturers by filters
        public ActionResult List()
        {
            var result = PopulateService.PopulateByFilters<ManufacturerListViewModel>(
                _context.Manufacturers.ProjectTo<ManufacturerListViewModel>(),
                Request.Params,
                typeof(ManufacturerListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Add Manufacturer
        [HttpPost, ValidateAntiForgeryToken, Log("Created product")]
        public ActionResult New(AddEditManufacturerForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model).WithWarning("Some fields are invalid!");
            }

            var manufacturer = new Manufacturer(model.Name);

            manufacturer.ImagePath = model.ImagePath;
            manufacturer.DisplayOrder = model.DisplayOrder;
            manufacturer.Published = model.Published;

            _context.Manufacturers.Add(manufacturer);

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Edit Manufacturer Form
        [Log("Editing product {id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Manufacturers
                .ProjectTo<AddEditManufacturerForm>()
                .SingleOrDefault(p => p.ID == id);
            model.AddOrEditUrl = Url.Action("Edit");

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        // POST: Update Manufacturer
        [HttpPost, Log("Product changed")]
        public ActionResult Edit(AddEditManufacturerForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
            }

            var manufacturer = _context.Manufacturers.SingleOrDefault(i => i.ID == model.ID);

            if (manufacturer == null)
            {
                return JsonError("Cannot find the product specified.");
            }

            manufacturer.Name = model.Name;
            manufacturer.ImagePath = model.ImagePath;
            manufacturer.DisplayOrder = model.DisplayOrder;
            manufacturer.Published = model.Published;

            _context.SaveChanges();

            return Json(new { success = true });
        }

        // GET: Delete Manufacturer
        [Log("Deleted product {id}")]
        public ActionResult Delete(long id)
        {
            var manufacturer = _context.Manufacturers.Find(id);

            if (manufacturer == null)
            {
                return RedirectToAction<ManufacturerController>(c => c.Table())
                    .WithError("Unable to find the product.  Maybe it was deleted?");
            }

            _context.Manufacturers.Remove(manufacturer);

            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}