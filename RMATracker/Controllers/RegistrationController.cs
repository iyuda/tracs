using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMATracker.Models;
using DataLayer;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Net.Http;

namespace RMATracker.Controllers
{
    public class RegistrationController : Controller
    {

        public ActionResult Index(string reg_no = null)
        {
            RegistrationsModel reg_model;
            if (reg_no == null)
            {
                reg_model = new RegistrationsModel();
                ViewBag.SubTitle = "";
            }
            else { 
                reg_model = new RegistrationsModel(reg_no);
                ViewBag.SubTitle = " #" + reg_no;
            }
            
            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = this.ControllerContext.RouteData.Values["action"];
            else
                ViewBag.ViewType = id;

            reg_model.ViewType = ViewBag.ViewType;
            ViewBag.MenuTitle = "Registration";
            ViewBag.Title = "Registration";
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View(reg_model);
        }



        private void SaveImages(RegistrationsModel model)
        {
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var newFileName = "";
                var imagePath = "";
                //WebImage image = WebImage.GetImageFromRequest("Images[" + i + "]._ImageBody");
                WebImage image = WebImage.GetImageFromRequest("upload-file-"+i);
                if (image != null)
                {
                    newFileName = Guid.NewGuid().ToString() + "!" + Path.GetFileName(image.FileName);
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "images"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "images");
                    imagePath = @"/images/" + newFileName;
                    image.Save(@"~\" + imagePath);
                    model.RegistrationParts[i].image_location = imagePath;
                }
                else
                    model.RegistrationParts[i].image_location = null;
            }
        }
        
        public ActionResult SaveRegistrationData(RegistrationsModel model)
        {
           
            if (Request.Files.Count > 0)
            {
                SaveImages(model);
            }
            
            if (model.Save())
            {
                if (model.SendEmail() == "OK")
                {
                    return new RedirectResult("https://parabit.com/thank-you");
                }
                else
                    return View("Error");
            }
            else
                return View("Error");
        }
        
    }
    
   
}