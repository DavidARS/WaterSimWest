using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WaterSimUI
{
    /// <summary>
    /// Summary description for WaterSimMetaDataMockService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WaterSimMetaDataMockService : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetFieldNamesAndValues(string KeyWord)
        {
            KeyWord = "{\"InfoRequest\":[\"all\"]}";
            if (WebConfigurationManager.AppSettings["UseMock"] == "true" ) 
            {
                string JsonFilePath = "";

                JsonFilePath = WebConfigurationManager.AppSettings["JsonStringFilePath3"];

                StreamReader sr = new StreamReader(Server.MapPath("//") + JsonFilePath); //Creating/Opening the json file in read mode

                string line;

                //Reading the JSON string from the InfoRequest.JSON file
                line = sr.ReadToEnd();

                sr.Close();

                return line;  //returning the json string
            }

            else
            {
                using (var client = new HttpClient())
                {
                    string result = "";
                    client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["URIWaterSim"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(new[] 
                    {
                        new KeyValuePair<string, string>("inputJsonArray", KeyWord)
                    });

                    String url = WebConfigurationManager.AppSettings["URLGetParameterInfo"];

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                            result = result.Replace("\r\n", "");
                            result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?><string xmlns=\"http://quayapps.com/\">", "");
                            result = result.Replace("</string>", "");
                            return result;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }
            }                  


 
        }

        [WebMethod]
        public string GetVersion()
        {
            if (WebConfigurationManager.AppSettings["UseMock"] == "true" ) 
            {

                return "Mock Version: WaterSim ";
            }

            else
            {
                using (var client = new HttpClient())
                {
                    string result = "";
                    client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["URIWaterSim"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
                    /*
                    var content = new FormUrlEncodedContent(new[] 
                    {
                       
                    });
                    */
                    String url = WebConfigurationManager.AppSettings["URLWaterSimVersion"];

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(url,null).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                            result = result.Replace("\r\n", "");
                            result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?><string xmlns=\"http://quayapps.com/\">", "");
                            result = result.Replace("</string>", "");
                            string jsonresult = "{ 'VERSION':'" + result + "'}";
                            return jsonresult;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }
            }                  


 
        }

        
    }

}
