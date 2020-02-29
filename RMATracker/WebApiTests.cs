using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DataLayerTests
{

    public class WebApiTests
    {
        private TestAPI _testing;


        public void Initialize()
        {
            var endPoint = "https://swapi.co/api/people/1/"; // "http://samplerest:1337/api/part";
            _testing = new TestAPI(endPoint);

        }


        public void GetPartType()
        {
            _testing.MakeRequest();
        }


        public void Send()
        {
            var partType = new PartType
            {
                PartNo = "300-100-1",
                PartDescription = "Reader - MMR 300-100-1"
            };

            _testing.Send(partType);
        }
        public void Get()
        {
            
            _testing.Get();
        }
    }

    public class PartType
    {
        public string Id { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
    }

    public enum httpVerbs
    {
        GET,
        POST,
        PULL,
        DELETE
    }

    public class TestAPI
    {
        public string EndPoint { get; set; }
        public httpVerbs httpMethod { get; set; }

        public TestAPI(string endPoint)
        {
            EndPoint = endPoint;
            httpMethod = httpVerbs.GET;
        }

        public string MakeRequest()
        {
            var responseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = httpMethod.ToString();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK) throw new ApplicationException();

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) throw new ApplicationException();

                    using (var reader = new StreamReader(responseStream))
                    {
                        responseValue = reader.ReadToEnd();
                        var result = JsonConvert.DeserializeObject<List<PartType>>(responseValue);
                    }
                }
            }

            return responseValue;
        }
        public string Get()
        {
            var responseValue = string.Empty;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = httpVerbs.GET.ToString();
            request.ContentType = "application/json";

           
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK) throw new ApplicationException();

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null) throw new ApplicationException();

                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                            var result = JsonConvert.DeserializeObject<List<PartType>>(responseValue);
                        }
                    }
                }
            }

            catch (Exception ex)
            {

            }
            return responseValue;

        }
        public string Send(PartType partType)
        {
            var responseValue = string.Empty;
            var json = JsonConvert.SerializeObject(partType);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = httpVerbs.GET.ToString();
            request.ContentType = "application/json";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                json = JsonConvert.SerializeObject(partType);
                sw.Write(json);
                sw.Flush();
                sw.Close();
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK) throw new ApplicationException();

                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream == null) throw new ApplicationException();

                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                                var result = JsonConvert.DeserializeObject<List<PartType>>(responseValue);
                            }
                        }
                    }
                }

                catch (Exception ex)
                {

                }
                return responseValue;

            }
        }
    }
}
