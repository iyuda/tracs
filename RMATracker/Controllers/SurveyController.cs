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
using System.Net.Mime;

namespace RMATracker.Controllers
{
    public class SurveyController : Controller
    {


        public ActionResult Index(string survey_id = null)
        {
            SurveyModel survey_model;
            if (survey_id == null)
            {
                survey_model = new SurveyModel();
                ViewBag.SubTitle = "";
            }
            else
            {
                survey_model = new SurveyModel(survey_id);
                ViewBag.SubTitle = " #" + survey_id;
            }

            var id = this.ControllerContext.RouteData.Values["id"];
            if (id == null)
                ViewBag.ViewType = this.ControllerContext.RouteData.Values["action"];
            else
                ViewBag.ViewType = id;

            ViewBag.MenuTitle = "Survey";
            ViewBag.Title = "Survey";

            //bool is_survey_empty = survey_model.Survey == null;
            //if (!is_survey_empty) is_survey_empty = survey_model.Survey.id == null;
            

            return View(survey_model);
        }
        private string GetPropertyValue(SurveyModel Model, string className, string propertyName, string Index = null)
        {
            dynamic WorkObject;
            switch (className)
            {
                case "SurveyIntegrityChecks":
                    WorkObject = Model.SurveyIntegrityChecks;
                    break;
                case "SurveyObstructions":
                    WorkObject = Model.SurveyObstructions;
                    break;
                case "SurveyReaderArrivalStates":
                    WorkObject = Model.SurveyReaderArrivalStates;
                    break;
                case "SurveySiteInspection":
                    WorkObject = Model.SurveySiteInspection;
                    break;
                default:
                    WorkObject = Model.Survey;
                    break;
            }
            if (!String.IsNullOrEmpty(Index))
            {
                WorkObject = WorkObject[Int32.Parse(Index.ToString())];
            }
            return WorkObject?.GetType().GetProperty(propertyName.ToString())?.GetValue(WorkObject, null)?.ToString();
        }
        
        //private void SaveImages(SurveyModel model)
        //{
        //    for (var i = 1; i <= Request.Files.Count; i++)
        //    {
        //        var imagePath = "";
        //        //WebImage image = WebImage.GetImageFromRequest("Images[" + i + "]._ImageBody");
        //        WebImage image = WebImage.GetImageFromRequest("upload-file-" + i);
        //        if (image != null)
        //        {
        //            //string className = model.ImagePathsMap[i - 1].Split('@')[0];
        //            //string index = model.ImagePathsMap[i - 1].Split('@')[1].Trim();
        //            //string propertyName = model.ImagePathsMap[i - 1].Split('@')[2];

        //            //imagePath = GetPropertyValue(model, className, propertyName, index);
        //            //Guid.NewGuid().ToString() + "!" + Path.GetFileName(image.FileName);
        //            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "images"))
        //                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "images");
        //            //imagePath = @"/images/" + newFileName;
        //            image.Save(@"~\" + imagePath);
        //            //model.RegistrationParts[i - 1].image_path = imagePath;
        //        }
        //        //else
        //        //model.RegistrationParts[i - 1].image_path = null;
        //    }
        //}
        public ActionResult SaveSurveyData(SurveyModel model)
        {

            //if (Request.Files.Count > 0)
            //{
            //    SaveImages(model);
            //}

            if (model.Save(Request))
            {
                //var fileStreamResult = ToWord(model);
                string url = Request.Url.AbsoluteUri.Replace("/" + this.ControllerContext.RouteData.Values["controller"], "/Query").Replace("/"+ this.ControllerContext.RouteData.Values["action"], String.Format("/survey?survey_id={0}", model.Survey.id));
                if (model.SendEmail(url) == "OK")
                {
                    return new RedirectResult("https://parabit.com/thank-you");
                }
                else
                    return View("Error");
            }
            else
                return View("Error");
        }

        public FileResult ToWord(SurveyModel model)
        {
            string html = Utilities.ViewToString(this, "~/Views/Survey/Index.cshtml", model);
            //return Content(html, "application/msword");
            MemoryStream htmlStream = new MemoryStream();
            
            StreamWriter writer = new StreamWriter(htmlStream);
            writer.Write(html);
            writer.Flush();
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=filename.doc");
            Response.ContentType = "application/force-download";
            Response.BinaryWrite(htmlStream.ToArray());
            Response.End();
            htmlStream.Position = 0;
            return File(htmlStream, "application/msword", Server.UrlEncode("d:\\temp\\filename1.pdf"));
        }

        


    }
    
   
}