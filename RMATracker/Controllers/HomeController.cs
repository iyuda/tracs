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
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Web.Caching;
using System.Web.WebPages;
using System.Web.Routing;

namespace RMATracker.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Login(string Email=null)
        {

            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            ViewBag.NoLayout = true;
            ViewBag.TempPassword = !String.IsNullOrEmpty(TempData["TempPassword"]?.ToString());
            ViewBag.ErrorMsg = TempData["ErrorMsg"];
            ViewBag.LoginFailed = (ViewBag.ErrorMsg??"") !="";
            if (Email == null)
                Email = TempData["Email"]?.ToString();
            if (Email == null)
                return View();
            else
            {
                string Password = TempData["Password"]?.ToString();
                return View(new LoginModel() { Email = Email, Password = Password });
            }
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return RedirectToAction("Login", "Home");

        }
        public void ClearApplicationCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }
        }
        public dynamic ValidateLogin(LoginModel model)
        {
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            string postData = JsonConvert.SerializeObject(model);
            
            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_ValidateLogin, "POST", data: postData);

            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = (string)response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                string message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                if (message == "") message = strResponseMsg;
                ViewBag.ErrorMsg = message;
                ViewBag.LoginFailed = true;
                return View("Login", model);

                
            }
            else
            {
                try
                {
                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        JToken jsonToken = data?[0];
                        User user = jsonToken?.ToObject<User>();
                        user.id = int.Parse(jsonToken?["UserId"].ToString());
                        string passwordTypeId = jsonToken?["PasswordTypeId"]?.ToString();
                        string token = (JsonMaker.GetJsonExtract(strResponseMsg, "$.data[0].token") ?? "").ToString();
                        Session["Token"] = token;

                        switch (passwordTypeId)
                        {
                            case "1":
                                Session["UserId"] = user.id?.ToString();
                                List<UserProfile> userProfile = new List<UserProfile>();
                                if (RMAFormController.GetReturnAddresses(Session["UserId"].ToString(), Session["Token"].ToString()).Count == 0)
                                {
                                    ViewBag.PopupAddress = true;
                                    return View("Login", model);
                                }
                                else
                                    return View("PasswordReset");
                            case "2":
                                Session["UserId"] = user.id?.ToString();
                                return View("PasswordReset");
                            case "5":
                                TempData["Email"] = jsonToken?["Email"]?.ToString();
                                TempData["UserId"] = user.id?.ToString();
                                return RedirectToAction("VerifyCodeGet", "Home");
                            default:
                                

                                Session["FirmId"] = jsonToken?["FirmId"]?.ToString();
                                Session["FirmName"] = jsonToken?["FirmName"]?.ToString();
                                Session["FirmTypeId"] = jsonToken?["FirmTypeId"]?.ToString();
                                Session["Email"] = jsonToken?["Email"]?.ToString();
                                Session["Phone"] = jsonToken?["Phone"]?.ToString();
                                Session["UserId"] = user.id?.ToString();
                                Session["UserName"] = user.Name?.ToString();
                                Session["Password"] = model.Password;
                                Session["User"] = user;
                                if ((Session["FirmTypeId"] ?? "").ToString().ToLower()!="3")
                                    
                                {
                                    ViewBag.NoLayout = true;
                                    return RedirectToAction("GetRMAForm", "Rmaform");
                                }
                                else
                                {
                                    ViewBag.Title = (Session["FirmName"] ?? "").ToString();
                                    return RedirectToAction("Securitas", "Home");
                                    //return View("~/Views/Home/Securitas.cshtml");
                                }

                                
                        }
                    }
                    else
                    {
                        string message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        ViewBag.ErrorMsg = message;
                        ViewBag.LoginFailed = true;
                        ViewBag.NoLayout = true;
                        return View("Login", model);
                        
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    ViewBag.ErrorMsg = "There was an error processing your request.  Please try again later.";
                    ViewBag.LoginFailed = true;
                    ViewBag.NoLayout = true;
                    return View();
                }
                return View();
            }
        }
       
        public ActionResult UserProfileGet()
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
            if ((State.fullList?.Count??0) == 0) State.fullList = GetStates();
            Firm.fullList = GetFirms();
            string UserId = Session["UserId"]?.ToString();
            if (String.IsNullOrEmpty(UserId)) return null;
            string Token = Session["Token"]?.ToString();
            UserProfile userModel = GetUserProfile(UserId, Token);
            if (userModel.ProfileCountryId != null || userModel.ProfileCountryId != 0)
                State.fullList = GetStatesByCountry(userModel.ProfileCountryId.ToString());
            userModel.UserId = UserId;
            userModel.Password = Session["Password"]?.ToString();
            ViewBag.NoLayout = true;
            ViewBag.RmaErrors = Session["RmaErrors"];
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("UserProfile", userModel);

        }
        public ActionResult PasswordChangeGet(string CurrentPassword)
        {
            ViewBag.NoLayout = true;
            ViewBag.CurrentPassword = CurrentPassword;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("PasswordChange");
        }
        [HttpPost]
        //[RequestSwitcher("desktop", "mobile")]
        public ActionResult Login(LoginModel model)
        {
            
            if (ModelState.IsValid)
            {
                ActionResult response = ValidateLogin(model);
                if (!((Session["FirmName"]??"").ToString().ToLower().Contains("securitas")))
                    ViewBag.NoLayout = true;
                return response;

            }
            ViewBag.NoLayout = true;
            ViewBag.LoginFailed = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View(model);
        }


       
        [HttpPost]
        public ActionResult ValidateEmailUpdate()
        {
            string email = Request["email"];
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> { { "Email", email }});
            
            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_ValidateEmail, "POST", data: postData);


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(message))
                    message = "There was an error processing your request";
                return Json(new { status = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Email"] = email;
                TempData["TempPassword"] = true;
                TempData["Password"] = "";
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, message = "test"  }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VerifyCodeGet()
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
            ViewBag.Email = TempData["Email"];
            ViewBag.UserId = TempData["UserId"];
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("VerifyCode");
        }

        [HttpPost]
        public ActionResult VerifyCodeUpdate()
        {
            string UserId = Request["UserId"];
            string VerificationCode = Request["VerificationCode"];
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> { { "UserId", UserId }, { "VerificationCode", VerificationCode } });
            TempData["Email"] = Request["Email"];

            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_VerifyCode, "POST", data: postData);


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {

                var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString(); //JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(message))
                    message = "There was an error processing your request";
                return Json(new { status = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Content(strResponseMsg, "application/json");;
            }



        }

        public ActionResult ResendCode()
        {
            string UserId = Request["UserId"];
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> { { "UserId", UserId } });

            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_ResendCode, "GET", parameters: new List<string> { UserId });


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(message))
                    message = "There was an error processing your request";
                return Json(new { status = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Content(strResponseMsg, "application/json"); ;
            }



        }
        [HttpPost]
        public ActionResult UserRegistrationUpdate(UserRegistration model)
        {
            
            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Registration, "POST", data: JsonConvert.SerializeObject(model));


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(message))
                    message = "There was an error processing your request";
                return Json(new { status = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Email"] = model.Email;
                TempData["Password"] = model.Password;
                TempData["UserId"] = JObject.Parse(strResponseMsg)?["data"]?[0]["UserId"]?.ToString();
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, message = "test"  }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UserProfileUpdate(UserProfile model)
        {
            bool apiError = false;
            try
            {
                var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_UpdateProfile, "PUT", data: JsonConvert.SerializeObject(model), WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;
                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                    if (int.Parse(status) == 1)
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        JToken jsonToken = data?[0];
                        User user = jsonToken?.ToObject<User>();
                        user.id = int.Parse(jsonToken?["UserId"].ToString());


                        Session["UserId"] = user.id?.ToString();
                        Session["UserName"] = user.Name?.ToString();
                        Session["User"] = user;

                        
                        Session["FirmTypeId"] = jsonToken?["FirmTypeId"]?.ToString();

                        Session["Email"] = model.Email;
                        Session["Phone"] = model.Phone;
                        Session["FirmName"] = user.FirmName;
                        Session["FirmId"] = model.FirmId;

                    }
                    return Content(strResponseMsg, "application/json");
                    //return Json(new { status = 0, message = "test"  }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = 0, message = Logger.GetProperErrorMessage(ex.Message, apiError) }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult UpdateDefaultAddress(UserProfile model)
        {
            bool apiError = false;
            try
                {

                    if (model.UserId == null)
                    model.UserId = Session["UserId"]?.ToString();
                var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_UpdateReturnAddress, "PUT", data: JsonConvert.SerializeObject(model), WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;
                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                    return Content(strResponseMsg, "application/json");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = 0, message = Logger.GetProperErrorMessage(ex.Message, apiError) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ValidateEmailGet()
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
            ViewBag.NoLayout = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("ValidateEmail");
        }

        public ActionResult UserRegistrationGet()
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            ClearApplicationCache();
            if ((Country.fullList?.Count ?? 0) == 0) Country.fullList = Country.GetCountries();
            //if ((State.fullList?.Count ?? 0) == 0) State.fullList = GetStates();
            Firm.fullList = GetFirms();
            ViewBag.NoLayout = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("UserRegistration");
        }

        public ActionResult PasswordResetGet()
        {
            Response.Cache.SetNoStore();
            ViewBag.NoLayout = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("PasswordReset");
        }
        public ActionResult UserProfileAddressGet()
        {
            //if ((State.fullList?.Count??0) == 0) State.fullList = GetStates();

            string UserId = Session["UserId"]?.ToString();
            
            string Token = Session["Token"]?.ToString();
            UserProfile userModel = GetUserProfile(UserId, Token);
            userModel.UserId = UserId;
            ViewBag.NoLayout = true;
            ViewBag.RmaErrors = Session["RmaErrors"];
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("_UserProfileAddress", userModel);

        }
        public ActionResult PasswordResetUpdate()
        {
            bool apiError = false;
            try {
                string postData = JsonConvert.SerializeObject(new Dictionary<string, string> {
                                                    { "UserId", Session["UserId"]?.ToString() },
                                                    { "NewPassword", Request["Password"] }});

                var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_ResetPassword, "POST", data: postData, WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;
                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    return Content(strResponseMsg, "application/json");

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = 0, message = Logger.GetProperErrorMessage(ex.Message, apiError) }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PasswordChangeUpdate()
        {
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> {
                                                { "UserId", Session["UserId"]?.ToString() },
                                                { "CurrentPassword", Request["txtCurrentPassword"] },
                                                { "NewPassword", Request["txtNewPassword"] }});

            var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_ChangePassword, "POST", data: postData, WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                if (String.IsNullOrEmpty(message))
                    message = "There was an error processing your request";
                return Json(new { status = 0, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                if (int.Parse(status)==1) Session["Password"] = Request["password-txt"];
                return Content(strResponseMsg, "application/json");

            }
        }
        public ActionResult AppBanner() {
                return View("_AppBanner");

    }
    public ActionResult UserDashBoard()
        {
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
       
        public ActionResult Securitas()
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
            Session["RmaErrors"] = "";
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            UserProfile  userModel = HomeController.GetUserProfile(Session["UserId"].ToString(), Session["Token"].ToString());
            if (userModel.FirmTypeId != "3")
            {

                return RedirectToAction("GetRmaForm", "RmaForm");
            }
            ViewBag.Title = "RMA Request - Securitas Bank Options";
            ViewBag.Securitas = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View();
        }
        public List<Firm> GetFirms()
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
			Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Firms, "GET");

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    string message = null;
                    try
                    {
                        message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    }
                    catch
                    {

                    }
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        //List<Firm> firms = data.ToObject<List<Firm>>();
                        List<Firm> firms = new List<Firm>();
                        foreach (JToken item in data)
                        {
                            Firm firm = item.ToObject<Firm>();
                            firm.id = int.Parse(item["FirmId"].ToString());
                            firms.Add(firm);
                        }
                        return firms;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        ModelState.AddModelError("", message);

                        apiError = true;
                        throw new Exception(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError); ;
                Logger.LogException(ex); return new List<Firm>();
            }
        }

        public JsonResult GetStatesByCountryJson(string countryId)
        {
            var states = GetStatesByCountry(countryId);
            SelectList objStatesList = new SelectList(states, "StateId", "Name", 0);
            return Json(objStatesList, JsonRequestBehavior.AllowGet);
        }

        public List<State> GetStatesByCountry(string countryId)
        {
            
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_States, parameters: new List<string> { countryId }, method: "GET");

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        List<State> states = new List<State>();
                        foreach (JToken item in data)
                        {
                            states.Add(item.ToObject<State>());
                        }
                        return states;
                        
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        apiError = true;
                        throw new ApplicationException(message);
                    }
                }
            }

            catch (WebException ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<State>();
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<State>();
            }

        }

        public List<State> GetStates()
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
			    Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_States, "GET");

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        List<State> states = new List<State>();
                        foreach (JToken item in data)
                        {
                            states.Add(item.ToObject<State>());
                        }
                        return states;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        apiError = true;
                        throw new ApplicationException(message);
                    }
                }
            }

            catch (WebException ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<State>();
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<State>();
            }

        }
        public static int GetDefaultAddress(string UserId, ref List<UserProfile> Users, string Token)
        {

            List<ReturnAddress> lstReturnAddresses = RMAFormController.GetReturnAddresses(UserId, Token);
            if (lstReturnAddresses.Count == 0)
                lstReturnAddresses.Add(new ReturnAddress());
            ReturnAddress defAddress = lstReturnAddresses.FirstOrDefault(x => x.IsDefault) ?? lstReturnAddresses[0];
            
            Users[0].ReturnAddressId = defAddress.ReturnAddressId;
            Users[0].ProfileStateId = defAddress.ReturnStateId;
            Users[0].ProfileCountryId = defAddress.ReturnCountryId;
            Users[0].StateName = defAddress.StateName;
            Users[0].StreetAddress = defAddress.Street;
            Users[0].City = defAddress.City;
            Users[0].Zip = defAddress.ZipCode;
            Users[0].UserId = UserId;
            return lstReturnAddresses.Count;

                

        }

        public static UserProfile GetUserProfile(string UserId, string Token)
        {
            UserProfile userModel = (UserProfile)System.Web.HttpContext.Current.Items["UserProfile"];
            if (userModel != null)
            {
                return userModel;
            }

            bool apiError = false;
            try
            {
                
                var userInfo = ClientDetection.GetUserEnvironment(new HttpRequestWrapper(System.Web.HttpContext.Current.Request));
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_UserProfile, "GET", parameters: new List<string> { UserId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {

                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        List<UserProfile> userProfiles = new List<UserProfile>();
                        foreach (JToken item in data)
                        {
                            UserProfile userProfile = item.ToObject<UserProfile>();
                            userProfile.UserId = UserId;
                            userProfile.Phone = userProfile?.Phone?.Replace(" ", "");
                            try
                            {
                                userProfile.Phone = String.Format("{0:(###)###-####}", Int64.Parse(new String(userProfile.Phone.Where(Char.IsDigit).ToArray())));
                            }
                            catch
                            {
                            }
                            userProfile.IsSubcontractor = userProfile.PrimecontractorId != null;
                            userProfiles.Add(userProfile);
                        }
                        GetDefaultAddress(UserId, ref userProfiles, Token);
                        if (userProfiles.Count == 0)
                            userProfiles.Add(new UserProfile());
                        System.Web.HttpContext.Current.Items["UserProfile"] = userProfiles[0];
                        return userProfiles[0];

                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        System.Web.HttpContext.Current.Session["RmaErrors"] = Logger.GetProperErrorMessage(message, apiError);
                        apiError = true;
                        throw new WebException(message);
                    }
                }
            }
            catch (WebException ex)
            {
                System.Web.HttpContext.Current.Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new UserProfile();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new UserProfile();
            }

        }

         public ActionResult KendoAutoComplete()
        {
            ViewBag.NoLayout = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("~/Views/Shared/KendoAutoComplete.cshtml");
        }

        public int? GetStateId(string name)
        {
            var userInfo = ClientDetection.GetUserEnvironment(Request);
            Tuple<HttpResponseMessage, string> stateResponse = UrlRequests.UrlRequest(userInfo, Constants.EP_States, "GET");
            JArray data = (JArray)JsonMaker.GetJsonExtract(stateResponse.Item2, "$.data");
            foreach (var item in data)
            {
                var state = item.ToObject<State>();
                if (state.Name.Equals(name))
                {
                    return state.StateId;
                }
            }

            return 0;
        }
    }
    
}
