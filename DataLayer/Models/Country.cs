using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Country
    {
        [Display(Name = "CountryId")]
        [Required]
        public Int32? CountryId { get; set; }

        [Display(Name = "Name")]
        [Required, StringLength(50)]
        public String Name { get; set; }

        public static List<Country> fullList { get; set; }
        public List<State> stateList { get; set; }
        public static List<Country> GetCountries(string Token = null, string userInfo = null)
        {
            try
            {

                Tuple<HttpResponseMessage, string> response = UrlRequests.UrlRequest(userInfo, Constants.EP_Countries, "GET");

                HttpResponseMessage objResponseMsg = response.Item1;
                string strResponseMsg = response.Item2;

                if (objResponseMsg.StatusCode != HttpStatusCode.OK)
                {
                    return new List<Country>() { new Country() { CountryId = 0, Name = "Error getting Country list" } };
                    throw new ApplicationException();
                }
                else
                {
                    var status = (JsonMaker.GetJsonExtract(strResponseMsg, "$.status") ?? "").ToString();
                    if (status.ToString() == "1")
                    {
                        JArray data = (JArray)JsonMaker.GetJsonExtract(strResponseMsg, "$.data");
                        List<Country> countries = new List<Country>();
                        foreach (JToken item in data)
                        {
                            countries.Add(item.ToObject<Country>());
                        }
                        if ((Country.fullList?.Count ?? 0) == 0)
                            Country.fullList = countries;

                        return countries;
                    }
                    else
                    {
                        var message = (JsonMaker.GetJsonExtract(strResponseMsg, "$.message") ?? "").ToString();

                        return new List<Country>() { new Country() { CountryId = 0, Name = message } };
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

