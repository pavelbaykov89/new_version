using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Services;
using SLK.Web.Infrastructure;
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

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        // GET: Category
        public ActionResult Table()
        {
            ViewBag.CategoryMenuActive = "active open";
            ViewBag.CategoryActive = "active open";
             
            return View(new CategoryListViewModel());
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var result = PopulateService.PopulateByFilters(
                _context.Categories.ProjectTo<CategoryListViewModel>(),
                Request.Params,
                typeof(CategoryListViewModel).GetProperties().Where(p => !p.GetCustomAttributes(false).Any(a => a is HiddenInputAttribute)).ToArray());

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}