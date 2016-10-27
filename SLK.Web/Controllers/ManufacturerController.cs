using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Services;
using SLK.Web.Models;
using SLK.Web.Models.ManufacturerModels;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManufacturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manufacturer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Manufacturer
        public ActionResult Table()
        {
            ViewBag.ManufacturerMenuActive = "active open";
            ViewBag.ManufacturerActive = "active open";

            var model = new ManufacturerListViewModel();
            model.AddNewForm = null;
            model.EditUrl = Url.Action("Edit");
            model.DeleteUrl = Url.Action("Delete");

            ViewBag.Title = "Manufacturers";
            ViewBag.Controller = "Manufacturer";

            return View("~/Views/Shared/Table.cshtml", model);
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters<ManufacturerListViewModel>(
                _context.Manufacturers.ProjectTo<ManufacturerListViewModel>(),
                Request.Params,
                typeof(ManufacturerListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}