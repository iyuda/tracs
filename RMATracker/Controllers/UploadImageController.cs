using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMATracker.Controllers
{
    public class UploadImageController : Controller
    {
        // GET: UploadImage
        public ActionResult UploadImage()
        {
            ViewBag.ViewType = "asdfdas";
            return View();
        }
    }
}