using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class State
    {
        [Display(Name = "StateId")]
        [Required]
        public Int32? StateId { get; set; }

        [Display(Name = "Name")]
        [Required, StringLength(50)]
        public String Name { get; set; }

        [Display(Name = "Abreviation")]
        [Required, StringLength(2)]
        public String Abreviation { get; set; }

        public static List<State> fullList { get; set;}
        public static List<State> GetStates(string Token = null, string userInfo = null, List<string> parameters = null)
        {
            try
            {
                
                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_States, "GET", parameters: parameters);

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    return new List<State>() { new State() { StateId = 0, Name = "Error getting state list" } };
                    throw new ApplicationException();
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
                        if ((State.fullList?.Count ?? 0) == 0)
                            State.fullList = states;

                        return states;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();
                        
                        return new List<State>() { new State() { StateId = 0, Name = message } };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex); return null;
            }
        }
    }
}
