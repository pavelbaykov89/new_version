﻿using Knoema.Localization;
using SLK.Web.Infrastructure;
using System.Globalization;
using System.Web.Mvc;

namespace SLK.Web.Controllers
{
    public class HomeController : SLKController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Lang(string culture)
        {
            LocalizationManager.Instance.SetCulture(new CultureInfo(culture));
            return new RedirectResult(Request.UrlReferrer.ToString());
        }
    }
}