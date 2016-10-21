using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Web.Models;
using SLK.Web.Models.ShopModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace SLK.Web.Controllers
{
    public class ShopTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopTypeController(ApplicationDbContext context)
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
            ViewBag.ShopTypeMenuActive = "active open";
            ViewBag.ShopTypeActive = "active open";

            var model = _context.ShopTypes
               .ProjectTo<ShopTypeListViewModel>()
               .FirstOrDefault();

            return View(model);
        }

        public ActionResult List(jQueryDataTableParamModel param)
        {
            var items = _context.ShopTypes
                .ProjectTo<ShopTypeListViewModel>();

            int displayOrderFilterInteger;

            var nameFilter = Convert.ToString(Request["Name"]);
            var displayOrderFilter = (Int32.TryParse(Request["DisplayOrder"], out displayOrderFilterInteger)) ? "y" : "";

            if (!string.IsNullOrEmpty(nameFilter))
            {
                items = items.Where(p => p.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(displayOrderFilter))
            {
                items = items.Where(p => p.DisplayOrder == displayOrderFilterInteger);
            }

            string ordering = "";
            int ind = 0;

            while (Request[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request[$"order[{ind}][column]"]);
                var sortDirection = Request[$"order[{ind}][dir]"];

                ordering += sortColumnIndex == 0 ? "Name" :
                            sortColumnIndex == 1 ? "DisplayOrder" : "";

                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                items = items.OrderBy(ordering).AsQueryable();
            }

            var count = items.Count();

            items = items
                .Skip(param.start)
                .Take(param.length);

            var totalCount = _context.ShopTypes.Count();

            return Json(new
            {
                draw = param.draw,
                recordsTotal = totalCount,
                recordsFiltered = count,
                data = items.ToArray()
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}