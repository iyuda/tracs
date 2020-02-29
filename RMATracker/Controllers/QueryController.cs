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
    public class QueryController : Controller
    {

       
        public ActionResult RmaForm(string case_no=null)
        {

            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = "";
            else
                ViewBag.ViewType = id;

            ViewBag.MenuTitle = "Query RMA / Change Status";
            ViewBag.Title = "RMA Query";
            ViewBag.CaseNo = case_no;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            //if (case_no != null)
            //{
            //    RMATrackerViewModel rma = new RMATrackerViewModel(case_no);
            //    if (rma.Form != null)
            //    {
            //        bool is_rma_empty = rma.RmaBase == null;
            //        if (!is_rma_empty) is_rma_empty = rma.RmaBase.id == null;
            //        return View(rma);
            //    }
            //    return View();
            //}
            return View();
        }
        public ActionResult Survey(string survey_id = null)
        {
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            SurveyModel survey_model;
            if (survey_id == null)
            {
                return View("Error");
            }
            
            survey_model = new SurveyModel(survey_id);
            ViewBag.QueryMode = true;
            ViewBag.SubTitle = " #" + survey_id;
            

            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = "";
            else
                ViewBag.ViewType = id;

            ViewBag.MenuTitle = "Query Survey";
            ViewBag.Title = "Survey ";
            ViewBag.SubTitle = " #" + survey_id;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;

            return View("~/Views/Survey/Index.cshtml", survey_model);
        }

        [HttpGet]
        public PartialViewResult GetSearch(string case_no)
        {

            RMATrackerViewModel rma = new RMATrackerViewModel(case_no);
            if (rma.Form!=null)
                ViewBag.ViewType = rma.Form.form_name;

            bool is_rma_empty = rma.RmaBase == null;
            if(!is_rma_empty)  is_rma_empty = rma.RmaBase.id == null;
            ViewBag.QueryMode = !is_rma_empty;
            return PartialView("~/Views/RmaForm/Index.cshtml", rma);
        }
       
        public ActionResult UpdateStatus(RMATrackerViewModel model)
        {
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            string status = "";
            if (model.RmaBase.Update(new string[] { "status_id" }, new string[] { "id" }))
            {
                foreach (RmaParts part in model.RmaParts)
                {
                    part.status_id = model.RmaBase.status_id.ToString();
                    part.Update(new string[] { "status_id", "additional_notes" }, new string[] { "id" });
                }
                status = model.SendStatusChangeEmail(model.RmaBase._status);
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
      
         public ContentResult UpdatePartStatus(ReturnedPart model)
        {
            return null;
        }
    }
    
   
}