using DataLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DataLayer
{
    public class UrlRequests
    {

        // used to test end hardcoded responses from endpointes
        public static Tuple<HttpResponseMessage, string> UrlRequest_test(int type, string Action, string method = "POST", List<string> parameters = null, string content_type = "application/json", string data = null)
        {

            
            Tuple<HttpResponseMessage, string> returnValue = TestModules.TestAPI.TestPost(Action.Split('/')[0], type);
            HttpResponseMessage response_msg = returnValue.Item1; 
            string ResponseString = returnValue.Item2;

            response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json");
            return returnValue;
        }

        public static Tuple<HttpResponseMessage, string> UrlRequest(string userInfo, string Action, string method = "POST", List<string> parameters = null, string content_type = "application/json", string data = null, WebHeaderCollection WebHeaders = null, bool NoAuthorization = false)
        {
            // int type = 1;
            //return UrlRequest_test(type, Action);
            string baseURL = ConfigTools.GetConfigValue("API Base URL", "http://rma.paralan.us:1340/api/v1");

            string url = baseURL + "/" + Action + "/";
            try
            {
               
                if (parameters != null)
                    foreach (string parameter in parameters)
                    {
                        url += parameter + "/";
                    }

                //            HttpWebRequest webRequest = (ParamRequest==null) ? (HttpWebRequest)WebRequest.Create(url) : ParamRequest;

                Logger.LogAction("UrlRequest for: " + url + " method: " + method, "Activity");
                Logger.LogAction(url , "Endpoints");
                //if (data!=null)
                //    Logger.LogAction("data: " + data, "Activity");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = method;

                if (WebHeaders == null)
                    WebHeaders = new WebHeaderCollection();
                if (!WebHeaders.AllKeys.Contains("Platform"))
                    WebHeaders.Add("Platform", "Web - " + userInfo);
                webRequest.Headers = WebHeaders;
                //if (!NoAuthorization)
                //    AuthorizeRequest(url, ref webRequest);
                webRequest.ContentType = content_type;
                // Stream requestStream = webRequest.GetRequestStream();
                if (data != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(data);
                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                System.Diagnostics.Stopwatch timer = new Stopwatch();
                timer.Start();
                WebResponse response = webRequest.GetResponse();
                timer.Stop();
                //webRequest.CookieContainer = cookieContainer;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string ResponseString = reader.ReadToEnd();

                //HttpRequestMessage request = new HttpRequestMessage(
                //request.CreateResponse(

                HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);

                response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json"); //multipart/form-data
                //content = ResponseString;
                Logger.LogAction(ResponseString, "Activity");
                return Tuple.Create(response_msg, ResponseString);

            }
            catch (WebException ex)
            {
                // content = ex.Message;
                Logger.LogException(ex, AdditionalMsg: url + " " + method);

                if (ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    HttpResponseMessage response_msg = new HttpResponseMessage(resp.StatusCode);
                    using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        var ResponseString = sr.ReadToEnd();
                        Logger.LogAction(ResponseString, "Activity");
                        return Tuple.Create(response_msg, ResponseString);
                    }
                    
                }
                else
                {
                    HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
                    return Tuple.Create(response_msg, ex.Message);
                }

            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
                var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
                //content = ex.Message;
                return Tuple.Create(response_msg, ex.Message);
            }

        }
        
        


    }
}
