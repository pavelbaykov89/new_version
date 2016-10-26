using AutoMapper.QueryableExtensions;
using SLK.DataLayer;
using SLK.Web.Models;
using SLK.Web.Models.ShopModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using SLK.Domain.Core;

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
            var model = new ShopTypeListViewModel();
            model.AddNewForm = new AddEditShopTypeForm(Url.Action("AddNew"));            
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

            model = model ?? new ShopTypeListViewModel();
            model.AddNewForm = new AddEditShopTypeForm(Url.Action("AddNew"));
            model.EditUrl = Url.Action("Edit");
            model.DeleteUrl = Url.Action("Delete");

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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddNew(AddEditShopTypeForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/AddNewPopup.cshtml", model);
            }

            var newType = new ShopType();
            newType.Name = model.Name;
            newType.DisplayOrder = model.DisplayOrder;
            _context.ShopTypes.Add(newType);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult Edit(int id)
        {
            var shopType = _context.ShopTypes.Find(id);
            var model = new AddEditShopTypeForm(Url.Action("Edit"), id);
            model.Name = shopType.Name;
            model.DisplayOrder = shopType.DisplayOrder;

            return PartialView("~/Views/Shared/EditPopup.cshtml", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AddEditShopTypeForm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Shared/EditPopup.cshtml", model);
            }

            var shopType = _context.ShopTypes.Find(model.ID.Value);
            shopType.Name = model.Name;
            shopType.DisplayOrder = model.DisplayOrder;

            _context.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult Delete(int id)
        {
            var shopType = _context.ShopTypes.Find(id);
            _context.ShopTypes.Remove(shopType);
            _context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}