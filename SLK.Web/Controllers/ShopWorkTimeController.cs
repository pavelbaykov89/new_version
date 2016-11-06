using SLK.DataLayer;
using SLK.Web.Infrastructure;
using SLK.Web.Models.TimesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class ShopWorkTimeController : SLKController
    {
        private readonly ApplicationDbContext _context;

        public ShopWorkTimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var model = new AddEditTimesModel[7];

            for (int i = 0; i < 7; ++i)
            {
                model[i] = new AddEditTimesModel();
            }
                       
            ViewBag.Title = "Shop Work Times";
            return PartialView("~/Views/Shared/TimesView.cshtml", model);
        }
    }
}