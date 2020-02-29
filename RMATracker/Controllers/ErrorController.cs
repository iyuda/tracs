using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
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
    public class ErrorController : Controller
    {

        [HttpPost]
        public EmptyResult JSErrorHandler()
        {
            StringBuilder errorMsg = new StringBuilder();
            if (!String.IsNullOrEmpty(Request["msg"]))
            {
                errorMsg.Append("Message: ");
                errorMsg.Append(Request["msg"]);
                errorMsg.Append(Environment.NewLine);
            }
            if (!String.IsNullOrEmpty(Request["url"]))
            {
                errorMsg.Append("URL: ");
                errorMsg.Append(Request["url"]);
                errorMsg.Append(Environment.NewLine);
            }
            if (!String.IsNullOrEmpty(Request["line"]))
            {
                errorMsg.Append("Line: ");
                errorMsg.Append(Request["line"]);
                errorMsg.Append(Environment.NewLine);
            }
            Logger.LogActivity(errorMsg.ToString(), "JS Errors");
            return null;
            
        }
    }
}