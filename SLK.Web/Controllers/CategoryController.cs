using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
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
            var categories = _context.Categories                    
                .ProjectTo<CategoryListViewModel>();

            int displayOrderFilterInteger;

            var nameFilter = Convert.ToString(Request["Name"]);
            var parentFilter = Convert.ToString(Request["ParentCategoryName"]);
            var hasImageFilter = Convert.ToString(Request["HasImage"]);
            var imagePathFilter = Convert.ToString(Request["ImagePath"]);                         
            var displayOrderFilter = (Int32.TryParse(Request["DisplayOrder"], out displayOrderFilterInteger)) ? "y" : "";
            var publishedFilter = Convert.ToString(Request["Published"]);

            if (!string.IsNullOrEmpty(nameFilter))
            {
                categories = categories.Where(p => p.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(parentFilter))
            {
                categories = categories.Where(p => p.ParentCategoryName.Contains(parentFilter));
            }

            if (!string.IsNullOrEmpty(hasImageFilter) && hasImageFilter != "any")
            {
                bool flag = hasImageFilter == "true" ? true : false;
                categories = categories.Where(p => p.HasImage == flag);
            }

            if (!string.IsNullOrEmpty(imagePathFilter))
            {
                categories = categories.Where(p => p.ImagePath.Contains(imagePathFilter));
            }
            
            if (!string.IsNullOrEmpty(displayOrderFilter))
            {
                categories = categories.Where(p => p.DisplayOrder == displayOrderFilterInteger);
            }

            if (!string.IsNullOrEmpty(publishedFilter) && publishedFilter != "any")
            {
                bool flag = publishedFilter == "true" ? true : false;
                categories = categories.Where(p => p.Published == flag);
            }

            string ordering = "";
            int ind = 0;

            while (Request[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(Request[$"order[{ind}][column]"]);
                var sortDirection = Request[$"order[{ind}][dir]"];

                ordering += sortColumnIndex == 0 ? "Name" :
                            sortColumnIndex == 1 ? "ParentCategoryName" :
                            sortColumnIndex == 3 ? "ImagePath" :
                            sortColumnIndex == 4 ? "DisplayOrder" : "";


                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                categories = categories.OrderBy(ordering).AsQueryable();
            }

            var count = categories.Count();

            categories = categories
                .Skip(param.start)
                .Take(param.length);

            var totalCount = _context.Categories.Count();

            return Json(new
            {
                draw = param.draw,
                recordsTotal = totalCount,
                recordsFiltered = count,
                data = categories.ToArray()
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}