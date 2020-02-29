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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;
using System.Web.Script.Serialization;
using System.Collections;


namespace RMATracker.Controllers
{
    public class RMAFormController : Controller
    {

        public ActionResult TabsView()
        {

            User user = (User)Session["User"];
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return View("TabsView", user);
        }

        //[NoCache]
        public ActionResult GetRMAForm(int? BankIncludeType = null)
        {

            try
            {

                Session["RmaErrors"] = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                Response.AppendHeader("Pragma", "no-cache");
                Response.AppendHeader("Expires", "0");


                if (Session["Token"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                string Token = Session["Token"]?.ToString();
                User user = (User)Session["User"];
                UserProfile userModel = HomeController.GetUserProfile(user?.id.ToString(), Token);



                if (userModel.FirmTypeId == "3" && BankIncludeType == null)
                {
                    return RedirectToAction("Securitas", "Home");
                }

                RMAModel rmaModel = new RMAModel(Token, user, userModel?.PrimecontractorName);

                rmaModel.FirmTypeId = int.Parse(userModel?.FirmTypeId ?? "0");
                rmaModel.BankIncludeType = BankIncludeType;
                rmaModel.rma = new RMA();
                rmaModel.rma.UserId = user.id;
                rmaModel.rma.Token = Token;

                rmaModel.rma.FirmId = int.Parse(userModel?.FirmId ?? "0");
                rmaModel.rma.LoadRelations(Token);

                LoadRMARelations(rmaModel.rma.Token, rmaModel.rma.FirmId, rmaModel.rma.UserId);

                var id = this.ControllerContext.RouteData.Values["id"];
                if (id != null)
                    ViewBag.ViewType = id;

                else
                    ViewBag.ViewType = userModel.FirmName;


                ViewBag.Title = "RMA Request - " + ViewBag.ViewType;
                ViewBag.BankIncludeType = BankIncludeType;
                if (BankIncludeType == 1)
                    ViewBag.Title += " - Bank Of America";
                if (BankIncludeType == 2)
                    ViewBag.Title += " - General";

                //ViewBag.FirmName = TempData["FirmName"];
                //ViewBag.NoLayout = true;
                ViewBag.RmaErrors = Session["RmaErrors"];
                ViewBag.IsMobile = Request.Browser.IsMobileDevice;
                if (Session["Token"] == null || userModel.UserId == null)
                {
                    TempData["ErrorMsg"] = ViewBag.RmaErrors;
                    return RedirectToAction("Login", "Home");
                }
                return View("RMAParent", rmaModel);
            }
            catch (Exception ex)
            {
                RMATracker.Logger.LogException(ex);
                ViewBag.RmaErrors = "Please try again later";
                ViewBag.IsMobile = Request.Browser.IsMobileDevice;
                return View("RMAParent", new RMAModel());
            }
        }

        public ActionResult GetCompanyBranches(int company_id)
        {

            try
            {
                if (company_id == -1)
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                string Token = Session["Token"]?.ToString();

                dynamic returnValues = GetCompanyAddresses(company_id.ToString(), Token, Request);
                if (returnValues.GetType().Name != "JsonResult")
                {
                    SelectList objCompanyBranchList = new SelectList(returnValues, "Id", "FullAddress", 0);
                    return Json(objCompanyBranchList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(returnValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                RMATracker.Logger.LogException(ex);
                return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetReturnAddressFromId(int ReturnAddressId)
        {
            try
            {
                if (ReturnAddressId == -1)
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                string Token = Session["Token"]?.ToString();
                string UserId = Session["UserId"]?.ToString();
                string strFullAddress = GetReturnAddresses(UserId, Token)?.FirstOrDefault(x => x.id == ReturnAddressId)?.FullAddress;
                return Json(strFullAddress, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                RMATracker.Logger.LogException(ex);
                return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReturnAddressListViewGet()
        {
            List<ReturnAddress> objAddresses = new List<ReturnAddress>();
            try
            {

                string Token = Session["Token"]?.ToString();
                string UserId = Session["UserId"]?.ToString();
                objAddresses = GetReturnAddresses(UserId, Token);  //Bank.fullList.FirstOrDefault(m => m.id == company_id)?.companyBranches?.ToList();

            }
            catch (Exception ex)
            {
                RMATracker.Logger.LogException(ex);
            }
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("_ReturnAddressListView", objAddresses);
        }

        public ActionResult Index(string case_no = null)
        {

            RMAModel rma;
            if (case_no == null)
                rma = new RMAModel();
            else
                rma = new RMAModel(case_no);

            var id = this.ControllerContext.RouteData.Values["id"];
            if (id != null)
                ViewBag.ViewType = id;

            else
                ViewBag.ViewType = this.ControllerContext.RouteData.Values["action"];


            if (id == null)
                ViewBag.Title = "RMA Entry";
            else
                ViewBag.Title = "RMA Request - " + ViewBag.ViewType;



            rma.IsQuery = false;
            return View(rma);
        }
        public ActionResult ReturnAddressEntry(int index, bool DynamicAdd = false)
        {
            ViewData["index"] = index;
            ViewBag.DynamicAdd = DynamicAdd;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_ReturnAddressEntryView.cshtml");
        }
        public ActionResult MediaEntry(int index, bool DynamicAdd = false, string AcceptTypes = "Images")
        {
            ViewData["index"] = index;
            ViewBag.DynamicAdd = DynamicAdd;
            ViewBag.AcceptTypes = AcceptTypes;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_MediaEntryView.cshtml");
        }
        public ActionResult RemoveMedia(int index)
        {
            ViewData["index"] = index;
            ViewBag.IsRemoved = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_MediaEntryView.cshtml");
        }

        public ActionResult PartEntry(int Index, bool DynamicAdd = false)
        {
            ViewData["index"] = Index;
            ViewBag.DynamicAdd = DynamicAdd;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_PartEntryView.cshtml");
        }
        public ActionResult RemovePart(int index)
        {
            ViewData["index"] = index;
            ViewBag.IsRemoved = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_PartEntryView.cshtml");
        }

        public ActionResult FaultEntry(int index, bool DynamicAdd = false)
        {
            ViewData["index"] = index;
            ViewBag.DynamicAdd = DynamicAdd;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_FaultEntryView.cshtml");
        }
        public ActionResult RemoveFault(int index)
        {
            ViewData["index"] = index;
            ViewBag.IsRemoved = true;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            ViewBag.IsMobile = Request.Browser.IsMobileDevice;
            return PartialView("~/Views/RmaForm/Singles/_FaultEntryView.cshtml");
        }

        public JsonResult SubmitReturnAddress(ReturnAddress model)
        {
            bool apiError = false;
            try
            {
                var response = PostAddress<ReturnAddress>(model, Constants.EP_AddReturnAddress);
                var responseFix = response.Replace("ReturnAddressId", "id").Replace("address_id", "id");
                var ReturnAddressId = (JToken)JObject.Parse(responseFix)?["data"]?[0]["id"];
                var ReturnAddresses = GetReturnAddresses(Session["UserId"]?.ToString(), Session["Token"]?.ToString());

                return Json(new
                {
                    status = ((JToken)JObject.Parse(responseFix)?["status"])?.ToString(),
                    message = Logger.GetProperErrorMessage(((JToken)JObject.Parse(responseFix)?["message"])?.ToString(), true),
                    data = new
                    {
                        IsDefault = model.IsDefault,
                        ReturnAddressId = ReturnAddressId?.ToString(),
                        FullAddress = ReturnAddresses?.FirstOrDefault(x => x.id.ToString() == ReturnAddressId?.ToString())?.FullAddress
                        //FullAddress = String.Format("{0} {1}, {2} {3}", model.Street, model.City, model.ZipCode)
                    },
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                //var strError = (new JObject(new JProperty("status", 0), new JProperty("message", "There was an error processing your request.  Please try again later."))).ToString();
                return Json(new
                {
                    status = "0",
                    message = Logger.GetProperErrorMessage(ex.Message, apiError)
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SubmitCompanyBranch(CompanyBranch model)
        {
            try
            {
                var response = PostAddress<CompanyBranch>(model, Constants.EP_AddCompanyAddress);
                return Json(new
                {
                    status = ((JToken)JObject.Parse(response)?["status"])?.ToString(),
                    message = Logger.GetProperErrorMessage(((JToken)JObject.Parse(response)?["message"])?.ToString(), true),
                    data = new
                    {
                        SiteId = ((JToken)JObject.Parse(response)?["data"]?[0]["SiteId"])?.ToString()
                    },
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                var strError = (new JObject(new JProperty("status", 0), new JProperty("message", "There was an error processing your request.  Please try again later."))).ToString();
                return Json(new
                {
                    status = "0",
                    message = Logger.GetProperErrorMessage(ex.Message, false)
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SubmitCompanyDetails(CompanyDetails model)
        {
            try
            {
                var response = PostAddress<CompanyDetails>(model, Constants.EP_AddCompanyDetails);
                return Json(new
                {
                    status = ((JToken)JObject.Parse(response)?["status"])?.ToString(),
                    message = Logger.GetProperErrorMessage(((JToken)JObject.Parse(response)?["message"])?.ToString(), true),

                    data = new
                    {
                        CompanyId = ((JToken)JObject.Parse(response)?["data"]?[0]["CompanyId"])?.ToString(),
                        SiteId = ((JToken)JObject.Parse(response)?["data"]?[0]["SiteId"])?.ToString()
                    },

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                var strError = (new JObject(new JProperty("status", 0), new JProperty("message", "There was an error processing your request.  Please try again later."))).ToString();
                return Json(new
                {
                    status = "0",
                    message = Logger.GetProperErrorMessage(ex.Message, false)
                }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult submitRMA(RMAModel model)
        {
            System.Diagnostics.Stopwatch submitTimer = new Stopwatch();
            submitTimer.Start();
            model.rma.Token = Session["Token"]?.ToString();
            foreach (RMAMedia media in model.rma.RMAMedias)
            {
                HttpPostedFileBase file = null;
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (String.IsNullOrEmpty(Request.Files[i].FileName) ||  !Request.Files.GetKey(i).ToString().Contains("upload-file"))
                        continue;
                    if (Request.Files[i].FileName == media.Filename)
                        file = Request.Files[i];
                }

                if (file != null)
                {

                    // compress file if possible


                    string MediaFolder = (bool)media.isVideo ? "/Work Files/Videos" : "/Work Files/Images";
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + MediaFolder))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + MediaFolder);

                    var binDirectoryPath = Server.MapPath("~/bin");
                    string origFilePath = Server.MapPath("~/" + MediaFolder + "/" + file.FileName); // String.Format("~\\{0}\\{1}", MediaFolder, file.FileName);
                    string workFile = origFilePath;
                    string newFilePath = Server.MapPath("~/" + MediaFolder + "/compressed." + file.FileName);  //String.Format("~\\{0}\\{1}", MediaFolder, "compressed." + file.FileName);
                    Logger.LogActivity("Media File Path: " + origFilePath);
                    try
                    {
                        file.SaveAs(origFilePath);
                        if (System.IO.File.Exists(binDirectoryPath + "\\ffmpeg.exe"))
                        {

                            if (System.IO.File.Exists(newFilePath))
                                try
                                {
                                    System.IO.File.Delete(newFilePath);
                                }
                                catch { }
                            Logger.LogActivity("Compressing File: " + origFilePath + " to " + newFilePath);
                            Process process = new Process();
                            process.StartInfo.FileName = binDirectoryPath + "\\ffmpeg.exe";
                            string videoOptions = String.Format("-vf {0} -c:v libx264 -preset fast -c:a aac", origFilePath.ToLower().EndsWith("mov") ? "transpose=1" : "scale=640:360");

                            string Arguments = (bool)media.isVideo ? videoOptions : "-b 1000000";
                            process.StartInfo.Arguments = String.Format("-y -i \"{0}\" {1} \"{2}\"", origFilePath, Arguments, newFilePath);
                            process.StartInfo.ErrorDialog = true;
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.Start();
                            System.Diagnostics.Stopwatch timer = new Stopwatch();
                            timer.Start();
                            while (!process.HasExited && process.Responding)
                            {
                                Thread.Sleep(100);
                                if (timer.Elapsed.TotalSeconds > 100)
                                    break;
                            }
                            timer.Stop();
                        }
                        workFile = System.IO.File.Exists(newFilePath) ? newFilePath : origFilePath;
                        var stream = System.IO.File.Open(workFile, FileMode.Open); // (Stream)new StreamReader(filePath);
                        var sr = new BinaryReader(stream);
                        var buffer = sr.ReadBytes((int)stream.Length);
                        media.Content = Convert.ToBase64String(buffer);
                        stream.Close();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, AdditionalMsg: "Error compressing media file " + file.FileName);
                        var strError = (new JObject(new JProperty("status", 0), new JProperty("message", "There was an error processing your request.  Please try again later."))).ToString();

                    }

                    Logger.LogActivity("Work Media File Path: " + workFile);


                    if (System.IO.File.Exists(workFile))
                        try
                        {
                            System.IO.File.Delete(workFile);
                        }
                        catch { }
                }
            }

            var response = PostRMA(model.rma);
            Logger.LogAction("Submit elapsed time: " + submitTimer.Elapsed + System.Environment.NewLine, "Activity");
            return Content(response, "application/json");


        }





        public static List<ReturnAddress> GetReturnAddresses(string UserId, string Token = null)
        {
            List<ReturnAddress> lstReturnAddresses = (List<ReturnAddress>)System.Web.HttpContext.Current.Items["ReturnAddresses"];
            if (lstReturnAddresses != null)
            {
                return lstReturnAddresses;
            }

            bool apiError = false;
            try
            {
                if (Token == null)
                    Token = System.Web.HttpContext.Current.Session["Token"]?.ToString();

                var userInfo = ClientDetection.GetUserEnvironment(new HttpRequestWrapper(System.Web.HttpContext.Current.Request));
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_GetReturnAddress, "GET", parameters: new List<string> { UserId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<ReturnAddress> ReturnAddresses = new List<ReturnAddress>();
                        foreach (JToken item in data)
                        {
                            ReturnAddress returnAddress = item.ToObject<ReturnAddress>();
                            returnAddress.id = int.Parse(item["ReturnAddressId"].ToString());
                            returnAddress.IsDefault = (bool)item["IsDefault"];
                            returnAddress.Street = item["Street"].ToString();
                            returnAddress.ZipCode = item["ZipCode"].ToString();
                            if (!ReturnAddresses.Exists(i => i.id == returnAddress.id))
                                ReturnAddresses.Add(returnAddress);
                        }
                        System.Web.HttpContext.Current.Items["ReturnAddresses"] = ReturnAddresses;
                        return ReturnAddresses;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();

                        Logger.LogActivity(message);
                        apiError = true;
                        throw new Exception(message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<ReturnAddress>();
            }
        }





        public List<Bank> GetBanks(string FirmId, string Token)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Companies, "GET", parameters: new List<string> { FirmId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<Bank> banks = new List<Bank>();
                        foreach (JToken item in data)
                        {
                            Bank bank = item.ToObject<Bank>();
                            bank.id = int.Parse(item["CompanyId"].ToString());
                            //bank.companyBranches = GetCompanyAddresses(bank.id.ToString(), Token);
                            banks.Add(bank);
                        }
                        return banks;
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
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex);
                return new List<Bank>();
            }
        }


        public dynamic GetCompanyAddresses(string CompanyId, string Token = null, HttpRequestBase Request = null)
        {
            bool apiError = false;
            try
            {

                if (Token == null)
                    Token = Session["Token"]?.ToString();
                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_CompanyAddresses, "GET", parameters: new List<string> { CompanyId }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<CompanyBranch> companyBranches = new List<CompanyBranch>();
                        foreach (JToken item in data)
                        {
                            CompanyBranch companyBranch = item.ToObject<CompanyBranch>();
                            companyBranch.id = int.Parse(item["SiteId"].ToString());
                            companyBranches.Add(companyBranch);
                        }

                        return companyBranches;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        apiError = true;
                        throw new ApplicationException(message);

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string message = Logger.GetProperErrorMessage(ex.Message, apiError);

                return new List<CompanyBranch>() { new CompanyBranch() { id = -1, FullAddress = message } };
                //return Json(new { status = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public List<DispatchReason> GetDispatchReasons(string Token)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_DispatchReason, "GET", WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<DispatchReason> dispatchReasons = new List<DispatchReason>();
                        foreach (JToken item in data)
                        {
                            DispatchReason dispatchReason = item.ToObject<DispatchReason>();
                            dispatchReason.id = int.Parse(item["DispatchReasonId"].ToString());
                            dispatchReasons.Add(dispatchReason);

                        }

                        return dispatchReasons;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        apiError = true;
                        throw new Exception(message);

                    }
                }
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<DispatchReason>();
            }
        }
        public List<CreditReason> GetCreditReason(string Token)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_CreditReason, "GET", WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<CreditReason> CreditReasons = new List<CreditReason>();
                        foreach (JToken item in data)
                        {
                            CreditReason creditReason = item.ToObject<CreditReason>();
                            creditReason.id = int.Parse(item["CreditReasonId"].ToString());
                            CreditReasons.Add(creditReason);
                        }
                        return CreditReasons;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        apiError = true;
                        throw new Exception(message);

                    }
                }
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<CreditReason>();
            }
        }
        public List<Fault> GetFaults(string Token)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Faults, "GET", WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<Fault> faults = new List<Fault>();
                        foreach (JToken item in data)
                        {
                            Fault fault = item.ToObject<Fault>();
                            fault.id = int.Parse(item["FaultId"].ToString());
                            faults.Add(fault);
                        }
                        return faults;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();

                        apiError = true;
                        throw new Exception(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<Fault>();
            }
        }

        public List<PartType> GetPartTypes(string Token)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Parts, "GET", WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
                        List<PartType> partTypes = new List<PartType>();
                        foreach (JToken item in data)
                        {
                            PartType partType = item.ToObject<PartType>();
                            //partType.id = int.Parse(item["PartId"].ToString());
                            partTypes.Add(partType);
                        }
                        return partTypes;
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
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex); return new List<PartType>();
            }
        }

        public ActionResult GetPartDetails(string SerialNumber)
        {
            bool apiError = false;
            try
            {

                string Token = Session["Token"]?.ToString();
                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_PartDetails, "GET", parameters: new List<string> { SerialNumber }, WebHeaders: new WebHeaderCollection() { { "Token", Token } });

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;
                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();  //var message = JObject.Parse(strResponseMsg)?["message"]?.ToString() ?? "";
                    apiError = true;
                    throw new ApplicationException(message);

                    //var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "";
                    //if (status.ToString() == "2" || String.IsNullOrEmpty(message))
                    //    message = "Please try again later.";
                    //return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var status = JObject.Parse(strResponseMsg)?["status"]?.ToString() ?? "";
                    if (status.ToString() == "2")
                        return Json(new { status = 2, message = "Please try again later." }, JsonRequestBehavior.AllowGet);
                    else
                        return Content(strResponseMsg, "application/json"); ;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = 2, message = Logger.GetProperErrorMessage(ex.Message, apiError) }, JsonRequestBehavior.AllowGet);

            }
        }
        public List<State> GetStates(string Token = null)
        {
            bool apiError = false;
            try
            {

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_States, "GET", WebHeaders: new WebHeaderCollection() { { "Token", Token } });

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
            catch (Exception ex)
            {
                Session["RmaErrors"] = Logger.GetProperErrorMessage(ex.Message, apiError);
                Logger.LogException(ex);
                return new List<State>();
            }

        }

        public string PostAddress<T>(T model, string Endpoint)
        {
            bool apiError = false;
            try
            {
                if (model.GetType().Name != "CompanyBranch")
                {
                    model.GetType().GetProperties().FirstOrDefault(p => p.Name == "UserId").SetValue(model, Session["UserId"]);
                }

                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Endpoint, "POST", data: JsonConvert.SerializeObject(model), WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });
                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                    //throw new Exception(message);
                    return strResponseMsg;
                }
            }
            catch (Exception ex)
            {

                Logger.LogException(ex);
                return (new JObject(new JProperty("status", 0), new JProperty("message", Logger.GetProperErrorMessage(ex.Message, apiError)))).ToString();
            }


        }
        public string GetPartDescription(string partNumber)
        {
            return PartType.fullList.FirstOrDefault(x => x.PartNumber.ToString() == partNumber)?.PartDescription;
        }
        public string GetPartNumber(string partDescription)
        {
            return PartType.fullList.FirstOrDefault(x => x.PartNumber.ToString() == partDescription)?.PartNumber;
        }
        public List<RMAType> GetRMATypes(string Token)
        {
            return new List<RMAType>() { new RMAType() { id = 2, Description = "Service Request" }, new RMAType() { id = 1, Description = "Credit Request" } };

        }
        public string PostRMA(RMA model)
        {
            bool apiError = false;
            try
            {
                //http://www.toontricks.com/2018/10/tutorial-javascriptserializerdeserializ.html
                var jsonSerializer = new JavaScriptSerializer();
                jsonSerializer.MaxJsonLength = 130000000;
                List<RMA> lstRMa = new List<RMA>() { model };
                var selectFields = lstRMa.Select(x => new {
                    VendorCall = x.TrackingNumber,
                    ParabitCallDate = x.ServiceCallDate,
                    RMAType = x.RMATypeId ?? -9999999,
                    CreditReason = x.CreditReasonId,
                    x.ReturnAddressId,
                    SiteId = x.CompanyBranchId ?? -9999999,
                    CompanyId = x.CompanyId ?? -9999999,
                    x.UserId,
                    DispatchReason = x.dispatchReason.id,
                    DispatchDescription = x.dispatchReason.Description,
                    Faults = x.rel_RMA_Faults?.Where(m => (m.IsRemoved ?? false) == false)?.Select(f => new { FaultId = f.id, AdditionalInfo = f.Observations, StepsUndertaken = f.StepsUndertaken }),
                    Parts = x.returnedParts?.Where(m => (m.IsRemoved ?? false) == false)?.Select(r => new {
                        r.SerialNumber,
                        PartNumber =
                                              ((!String.IsNullOrEmpty(r.SerialNumber) && String.IsNullOrEmpty(r.PartNumber))
                                                  ||
                                              (!String.IsNullOrEmpty(r.SerialNumber) && !String.IsNullOrEmpty(r.PartNumber) && !(r.PartNumber ?? "").Contains("!#$"))
                                              ?
                                              "Other"
                                              :
                                              (
                                                  String.IsNullOrEmpty(r.SerialNumber) && !(r.PartNumber ?? "").Contains("!#$")
                                                  ?
                                                  "Non Serialized"
                                                  :
                                                  r.PartNumber?.Split("!#$".ToCharArray())[0]
                                              )),
                        PartDescription =
                                              !String.IsNullOrEmpty(r.PartNumber) && !(r.PartNumber ?? "").Contains("!#$")
                                              ?
                                              r.PartNumber?.Split("!#$".ToCharArray())[0] + " - " + r.PartDescription?.Split("!#$".ToCharArray())?[0]
                                              :
                                              r.PartDescription?.Split("!#$".ToCharArray())?[0]
                    }),
                    Media = x.RMAMedias?.Where(m => (m.IsRemoved ?? false) == false && String.IsNullOrEmpty(m.Filename) == false)?.Select(m => new { m.Filename, RMAMedia = m.Content, IsVideo = m.isVideo }),
                    ParabitCalled = x.parabitCall.WasParabitCalled ? 1 : 0,
                    DateCalled = x.parabitCall.CallDate,
                    ParabitDispatch = x.parabitCall.DispatchNumber
                }).ToList()[0];
                var data = JsonConvert.DeserializeObject(jsonSerializer.Serialize(selectFields)).ToString().Replace("null,", "\"\",").Replace(": null", ": \"\"").Replace("-9999999", "null");
                var userInfo = ClientDetection.GetUserEnvironment(Request);
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_SubmitRMA, "POST", data: data, WebHeaders: new WebHeaderCollection() { { "Token", Session["Token"]?.ToString() } });

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                    apiError = true;
                    throw new ApplicationException(message);
                }
                else
                {
                    return strResponseMsg;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string message = Logger.GetProperErrorMessage(ex.Message, apiError);
                return (new JObject(new JProperty("status", 0), new JProperty("message", message))).ToString();
            }
        }
        public dynamic LoadRMARelations(string Token, int? FirmId, int? UserId)

        {
            Logger.LogAction("Getting state list", "Activity");
            if (State.fullList?.Count == 0) State.fullList = GetStates();
            Logger.LogAction("Getting dispatch reasons", "Activity");
            DispatchReason.fullList = GetDispatchReasons(Token);
            Logger.LogAction("Getting credit reasons", "Activity");
            CreditReason.fullList = GetCreditReason(Token);
            Logger.LogAction("Getting rma types", "Activity");
            RMAType.fullList = GetRMATypes(Token);

            Logger.LogAction("Getting bank list", "Activity");
            Bank.fullList = GetBanks(FirmId.ToString(), Token).OrderBy(o => o.Name).ToList();
            Logger.LogAction("Getting fault list", "Activity");
            Fault.fullList = GetFaults(Token).ToList();
            Logger.LogAction("Getting return addresses", "Activity");

            Logger.LogAction("Getting part types", "Activity");
            PartType.fullList = GetPartTypes(Token).ToList();

            return true;


        }
    }


}