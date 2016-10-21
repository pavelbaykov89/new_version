using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SLK.Web.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            var images = Directory.EnumerateFiles(Server.MapPath("~/Content/ImportedProductImages/"))
                .Where(i => Path.GetExtension(i) == ".png" || Path.GetExtension(i) == ".jpg" || Path.GetExtension(i) == ".jpeg")
                .Select(i => i = "../Content/ImportedProductImages/" + Path.GetFileName(i));

            return View(images);
        }
    }
}