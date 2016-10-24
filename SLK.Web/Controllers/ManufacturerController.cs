using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
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
            
            return View(new ManufacturerListViewModel());
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var manufacturers = _context.Manufacturers
                .ProjectTo<ManufacturerListViewModel>();

            int displayOrderFilterInteger;

            var nameFilter = Convert.ToString(Request["Name"]);        
            var imagePathFilter = Convert.ToString(Request["ImagePath"]);
            var displayOrderFilter = (Int32.TryParse(Request["DisplayOrder"], out displayOrderFilterInteger)) ? "y" : "";
            var publishedFilter = Convert.ToString(Request["Published"]);

            if (!string.IsNullOrEmpty(nameFilter))
            {
                manufacturers = manufacturers.Where(p => p.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(imagePathFilter))
            {
                manufacturers = manufacturers.Where(p => p.ImagePath.Contains(imagePathFilter));
            }

            if (!string.IsNullOrEmpty(displayOrderFilter))
            {
                manufacturers = manufacturers.Where(p => p.DisplayOrder == displayOrderFilterInteger);
            }

            if (!string.IsNullOrEmpty(publishedFilter) && publishedFilter != "any")
            {
                bool flag = publishedFilter == "true" ? true : false;
                manufacturers = manufacturers.Where(p => p.Published == flag);
            }

            string ordering = "";
            int ind = 0;

            while (Request[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request[$"order[{ind}][column]"]);
                var sortDirection = Request[$"order[{ind}][dir]"];

                ordering += sortColumnIndex == 0 ? "Name" :                            
                            sortColumnIndex == 1 ? "ImagePath" :
                            sortColumnIndex == 2 ? "DisplayOrder" : "";


                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                manufacturers = manufacturers.OrderBy(ordering).AsQueryable();
            }

            var count = manufacturers.Count();

            manufacturers = manufacturers
                .Skip(param.start)
                .Take(param.length);

            var totalCount = _context.Categories.Count();

            return Json(new
            {
                draw = param.draw,
                recordsTotal = totalCount,
                recordsFiltered = count,
                data = manufacturers.ToArray()
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}