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

namespace RMATracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login(string Email=null)
        {
            ViewBag.NoLayout = true;
            if (Email == null)
                Email = TempData["Email"]?.ToString();
            if (Email == null)
                return View();
            else
                return View(new LoginModel() { Email = Email });
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
 
        public dynamic ValidateLogin(LoginModel model)
        {

            string postData = JsonConvert.SerializeObject(model);
            //DataLayerTests.WebApiTests w = new DataLayerTests.WebApiTests();
            //w.Initialize();
            //    w.Get();
            //dynamic response = UrlRequests.UrlRequest_test(1, "ValidateLogin", "POST", data: postData);
            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_ValidateLogin, "POST", data: postData);

            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = (string)response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                string message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                if (message == "") message = strResponseMsg;
                ViewBag.ErrorMsg = message;
                ViewBag.LoginFailed = true;
                return View("Login", model);

                //return Json(new { status=0, message = strResponseMsg }, JsonRequestBehavior.AllowGet);
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
                            case "2":
                                Session["UserId"] = user.id?.ToString();
                                return View("PasswordReset");
                            default:
                                //user.LoadRMARelations(Session["Token"]?.ToString());

                                Session["FirmId"] = jsonToken?["FirmId"]?.ToString();
                                Session["FirmName"] = jsonToken?["FirmName"]?.ToString();
                                Session["FirmType"] = jsonToken?["FirmType"]?.ToString();
                                Session["Email"] = jsonToken?["Email"]?.ToString();
                                Session["Phone"] = jsonToken?["Phone"]?.ToString();                                
                                Session["UserId"] = user.id?.ToString();
                                Session["UserName"] = user.Name?.ToString();
                                Session["Password"] = model.Password;
                                Session["User"] = user;
                                

                                if (!(Session["FirmType"] ?? "").ToString().ToLower().Contains("securitas"))
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

                                //return GetUserProfile(user);
                                //                                return RedirectToAction("Index", "Rmaform");
                        }
                    }
                    else
                    {
                        string message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        ViewBag.ErrorMsg = message;
                        ViewBag.LoginFailed = true;
                        ViewBag.NoLayout = true;
                        return View("Login", model);
                        //                    return Json(new { Success = false, LoginError = message }, JsonRequestBehavior.AllowGet);
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
            string UserId = Session["UserId"]?.ToString();
            if (String.IsNullOrEmpty(UserId)) return null;
            string Token = Session["Token"]?.ToString();
            UserProfile userModel = GetUserProfile(UserId, Token);
            userModel.UserId = UserId;
            userModel.Password = Session["Password"]?.ToString();
            ViewBag.NoLayout = true;
            ViewBag.RmaErrors = Session["RmaErrors"];
            return View("UserProfile", userModel);

        }
        public ActionResult PasswordChangeGet(string CurrentPassword)
        {
            ViewBag.NoLayout = true;
            ViewBag.CurrentPassword = CurrentPassword;
            return View("PasswordChange");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            State.fullList = GetStates();
            if (ModelState.IsValid)
            {
                ActionResult response = ValidateLogin(model);
                if (!((Session["FirmName"]??"").ToString().ToLower().Contains("securitas")))
                    ViewBag.NoLayout = true;
                return response;

            }
            ViewBag.NoLayout = true;
            ViewBag.LoginFailed = true;
            return View(model);
        }


       
        [HttpPost]
        public ActionResult ValidateEmailUpdate()
        {
            string email = Request["email"];
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> { { "Email", email }});
            
            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_ValidateEmail, "POST", data: postData);


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, Message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, Message = "test"  }, JsonRequestBehavior.AllowGet);
            }



        }
        [HttpPost]
        public ActionResult UserRegistrationUpdate(UserRegistration model)
        {
            
            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_Registration, "POST", data: JsonConvert.SerializeObject(model));


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Email"] = model.Email;
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, Message = "test"  }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UserProfileUpdate(UserProfile model)
        {
            //string postData = JsonConvert.SerializeObject(new Dictionary<string, string> {
            //                                    { "UserId", Session["UserId"]?.ToString() },
            //                                    { "Name", Request["Name"] },
            //                                    { "Phone", Request["Phone"] },
            //                                    { "Email", Request["Email"] },
            //                                    { "FirmName", Request["FirmName"] }});

            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_UpdateProfile, "PUT", data: JsonConvert.SerializeObject(model), WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                if (int.Parse(status) == 1)
                {
                    return RedirectToAction("Securitas", "Home");
                    JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                    JToken jsonToken = data?[0];

                    Session["Email"] = jsonToken?["FirmName"]?.ToString(); //Request["Email"];
                    Session["Phone"] = jsonToken?["Phone"]?.ToString(); //Request["Phone"];
                    Session["FirmName"] = jsonToken?["FirmName"]?.ToString(); //Request["FirmName"];
                    Session["FirmId"] = jsonToken?["FirmId"]?.ToString(); //JObject.Parse(strResponseMsg)?["data"]?[0]["FirmId"]?.ToString();
                    Session["UserName"] = Request["Name"];
                }
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, Message = "test"  }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdateDefaultAddress(UserProfile model)
        {
            //model.ProfileStateId = GetStateId(model.StateName);
            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_UpdateReturnAddress, "PUT", data: JsonConvert.SerializeObject(model), WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });

            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                return Content(strResponseMsg, "application/json");
            }

        }

        public ActionResult ValidateEmailGet()
        {
            ViewBag.NoLayout = true;
            return View("ValidateEmail");
        }
        public ActionResult UserRegistrationGet()
        {
            ViewBag.NoLayout = true;
            return View("UserRegistration");
        }
        public ActionResult PasswordResetGet()
        {
            ViewBag.NoLayout = true;
            return View("PasswordReset");
        }
        public ActionResult PasswordResetUpdate()
        {
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> {
                                                { "UserId", Session["UserId"]?.ToString() },
                                                { "NewPassword", Request["Password"] }});

            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_ResetPassword, "POST", data: postData, WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, Message = "test"  }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PasswordChangeUpdate()
        {
            string postData = JsonConvert.SerializeObject(new Dictionary<string, string> {
                                                { "UserId", Session["UserId"]?.ToString() },
                                                { "CurrentPassword", Request["txtCurrentPassword"] },
                                                { "NewPassword", Request["txtNewPassword"] }});

            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_ChangePassword, "POST", data: postData, WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });


            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;
            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { status = 0, message = "There was an error processing your request" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "0";
                if (int.Parse(status)==1) @Session["Password"] = Request["txtNewPassword"];
                return Content(strResponseMsg, "application/json");
                //return Json(new { status = 0, Message = "test"  }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserDashBoard()
        {
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
            return View();
        }
        public List<State> GetStates()
        {
            try
            {

                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_States, "GET");

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("There was an error processing your request.  Please try again later.");
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
                        throw new ApplicationException(message);
                    }
                }
            }

            catch (WebException ex)
            {
                Session["RmaErrors"] = ex.Message;
                Logger.LogException(ex); return new List<State>();
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = "There was an error processing your request.  Please try again later.";
                Logger.LogException(ex); return new List<State>();
            }

        }
        public void GetDefaultAddress(string UserId, ref List<UserProfile> Users, string Token)
        {
            Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_GetReturnAddress, "GET", parameters: new List<string> { UserId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

            HttpResponseMessage objResponseMsg = response.Item1;
            string strResponseMsg = response.Item2;

            if (objResponseMsg.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException();
            }
            else
            {

                var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                if (status.ToString() == "1")
                {
                    JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                    // The first element is guarenteed to be the default address because of the 
                    //  the ordering in the database
                    ReturnAddress retAddress = data[0].ToObject<ReturnAddress>();
                    Users[0].ReturnAddressId = retAddress.ReturnAddressId;
                    Users[0].ProfileStateId = retAddress.ReturnStateId;
                    Users[0].StateName = retAddress.StateName;
                    Users[0].StreetAddress = JsonMaker.GetJsonExtract(data[0].ToString(), "$.Street").ToString(); //retAddress.Street;
                    Users[0].City = retAddress.City;
                    Users[0].Zip = JsonMaker.GetJsonExtract(data[0].ToString(), "$.ZipCode").ToString();// retAddress.ZipCode;
                    Users[0].UserId = UserId;
                }else
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                    throw new WebException(message);
                }
            }
        }

        public UserProfile GetUserProfile(string UserId, string Token)
        {
            try
            {
                //string actionParams = String.Format("BankAddress/{0}/{1}", UserId, Token);
                //dynamic response = UrlRequests.UrlRequest_test(5, actionParams, "GET");
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(Constants.EP_UserProfile, "GET", parameters: new List<string> { UserId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException();
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
                            userProfiles.Add(userProfile);
                        }
                        GetDefaultAddress(UserId, ref userProfiles, Token);
                        if (userProfiles.Count > 0)
                            userProfiles.Add(new UserProfile());
                        return userProfiles[0];

                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        throw new WebException(message);
                    }
                }
            }
            catch (WebException ex)
            {
                Session["RmaErrors"] = ex.Message;
                Logger.LogException(ex); return new UserProfile();
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = "There was an error processing your request.  Please try again later.";
                Logger.LogException(ex); return new UserProfile();
            }

        }

         public ActionResult KendoAutoComplete()
        {
            //ViewBag.NoLayout = true;
            return View("~/Views/Shared/KendoAutoComplete.cshtml");
        }

        public int? GetStateId(string name)
        {
            Tuple<HttpResponseMessage, string> stateResponse = UrlRequests.UrlRequest(Constants.EP_States, "GET");
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
