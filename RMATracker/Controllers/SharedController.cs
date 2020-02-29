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
    public class SharedController : Controller
    {

        public ActionResult rmaform(string case_no =null)
        {
            //Retrieve new rma from DB
            RMATrackerViewModel rma;
            if (case_no == null)
                rma = new RMATrackerViewModel();
            else
                rma = new RMATrackerViewModel(case_no);
            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = this.ControllerContext.RouteData.Values["action"];
            else
                ViewBag.ViewType = id;

            //string[] PathArray=Request.Path.Split('/');
            //ViewBag.ViewType = PathArray[PathArray.Length-1];
            
            List<Forms> forms = DataHelper.GetQueryList<Forms>("select * from forms ");
            Forms form = forms.FirstOrDefault(f => f.form_name == ViewBag.ViewType);
            if (form != null)
                ViewBag.Title = "RMA " + form.form_title;
            else if (id == null)
                ViewBag.Title = "RMA Form Selection";
            else
                ViewBag.Title = "RMA " + ViewBag.ViewType;


            rma.ViewType = ViewBag.ViewType;
            rma.IsQuery = false;
            return View("rma-form",rma);
        }
    
        public ActionResult Query()
        {

            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = "";
            else
                ViewBag.ViewType = id;

            ViewBag.MenuTitle = "Query RMA / Change Status";
            ViewBag.Title = "RMA Query";
            return View();
        }
        public ActionResult Registration()
        {
            RegistrationsModel rma = new RegistrationsModel();
            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = this.ControllerContext.RouteData.Values["action"];
            else
                ViewBag.ViewType = id;

            rma.ViewType = ViewBag.ViewType;
            ViewBag.MenuTitle = "Registration";
            ViewBag.Title = "Registration";
            return View(rma);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "http://parabit.com/contact/";

            return View();
        }

        private void SaveImages(dynamic model)
        {
            for (var i = 1; i <= Request.Files.Count; i++)
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
                    model.Parts[i-1].image_path = imagePath;
                }
                else
                    model.Parts[i-1].image_path = null;
            }
        }
        public ActionResult SaveRMAData(RMATrackerViewModel model)
        {
            //if (submit=="UpdateStatus")
            //{
            //    return UpdateStatus(model);
            //}
            if (Request.Files.Count > 0)
            {
                SaveImages(model);
            }
            model.RMABase.status_id = 1;
            foreach(Parts part in model.Parts)
            {
                part.status_id = "1";
            }
            if (model.Save())
            {
                if (model.SendRequestEmail() == "OK")
                {
                    model.RMABase.UpdateStatus(1);
                    return new RedirectResult("https://parabit.com/thank-you");
                }
                else
                    return View("Error");
            }
            else
                return View("Error");
        }
        public ActionResult SaveRegistrationData(RegistrationsModel model)
        {
            //if (submit=="UpdateStatus")
            //{
            //    return UpdateStatus(model);
            //}
            if (Request.Files.Count > 0)
            {
                SaveImages(model);
            }
            
            if (model.Save())
            {
                if (model.SendRegistrationEmail() == "OK")
                {
                    return new RedirectResult("https://parabit.com/thank-you");
                }
                else
                    return View("Error");
            }
            else
                return View("Error");
        }
        [HttpGet]
        public PartialViewResult GetSearch(string case_no)
        {

            RMATrackerViewModel rma = new RMATrackerViewModel(case_no);
            if (rma.Form!=null)
                ViewBag.ViewType = rma.Form.form_name;

            bool is_rma_empty = rma.RMABase == null;
            if(!is_rma_empty)  is_rma_empty = rma.RMABase.id == null;
            ViewBag.QueryMode = !is_rma_empty;
            return PartialView("~/Views/Shared/rma-form.cshtml", rma);
        }
        //[HttpPost]
        //public PartialViewResult UpdateStatus([System.Web.Http.FromBody] RMATrackerViewModel model, int PartIndex)
        //{
        //    //if (model.Parts.UpdateStatus())
        //    //{ }

        //    if (model.Parts[PartIndex].Update(new string[] { "status" }, new string[] { "id" }))
        //    {
        //        ViewBag.SubmitValue = "Status Updated";
        //    }
        //    return PartialView("~/Views/Shared/Index.cshtml");
        //}

        //public ContentResult UpdateStatus(string status_id, string case_no)
        //{
        //    string action_query = String.Format("update RmaBase set status_id = '{0}' where id = {1}", status_id, part_id);
        //    dynamic result = DataHelper.ActionQuery(action_query);
        //    bool bool_result;
        //    if (Boolean.TryParse(result.ToString(), out bool_result))
        //    {
        //        RMATrackerViewModel rma = new RMATrackerViewModel(case_no);
        //        if (rma.Parts != null)
        //        {
        //            Parts part = rma.Parts.FirstOrDefault(p => p.id.ToString() == part_id);
        //            string email_status = rma.SendStatusChangeEmail(part.seq_no);
        //            return (Content(email_status));
        //        }
        //        else
        //            return (Content("Not Found"));
        //    }
        //    else
        //    {
        //        //HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.NotFound);
        //        //response_msg.Content = new StringContent(result, Encoding.UTF8, "application/json");
        //        //content = ResponseString;
        //        return (Content(result));
        //    }
        //}
        public ActionResult UpdateStatus(RMATrackerViewModel model)
        {
            string status = "";
            if (model.RMABase.Update(new string[] { "status_id" }, new string[] { "id" }))
            {
                foreach (Parts part in model.Parts)
                {
                    part.status_id = model.RMABase.status_id.ToString();
                    part.Update(new string[] { "status_id", "additional_notes" }, new string[] { "id" });
                }
                status = model.SendStatusChangeEmail(model.RMABase._status);
                if (status == "OK")
                {
                    return RedirectToAction("Query");
                }
                else
                {
                    ViewBag.Error = status;
                    return View("Error");
                }
            }
            else
            {
                ViewBag.Error = status;
                return View("Error");
            }
        }
      
         public ContentResult UpdatePartStatus(RMATrackerViewModel model)
        {
            try
            {
                Parts part = model.Parts[model.CurrentPartIndex - 1];
                part.Update(new string[] { "status_id", "additional_notes" }, new string[] { "id" });
                string email_status = model.SendStatusChangeEmail(part._status, model.CurrentPartIndex);
                return (Content(email_status));
            }
            catch (Exception ex)
            {
                return (Content(ex.Message));
            }
        }
    }
    //public class rmaparabittechController : SharedController
    //{

    //}
    //public class rmaboaController : SharedController
    //{

    //}
    //public class rmasecuritasgeneralController : SharedController
    //{

    //}
    //public class rmaparabitsubController : SharedController
    //{

    //}
    //public class rmageneralController : SharedController
    //{

   // }
   
}