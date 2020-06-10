using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;


namespace WaterSimUI
{
    /// <summary>
    /// Summary description for WaterSimMockService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WaterSimMockService : System.Web.Services.WebService
    {
        public class Data
        {
            public string input;
            public string output;
        }

        [WebMethod]
        public string GetData(string inputJsonArray, string outputJsonArray)
        {
            dynamic jsonData = JsonConvert.DeserializeObject(inputJsonArray);

            string JsonFilePath;

            if (WebConfigurationManager.AppSettings["UseMock"] == "true")
            {
                if (jsonData.Inputs[0].VAL == jsonData.Inputs[1].VAL)
                    JsonFilePath = WebConfigurationManager.AppSettings["JsonStringFilePath1"];

                else
                    JsonFilePath = WebConfigurationManager.AppSettings["JsonStringFilePath2"];
                
                //Creating/Opening the json file in read mode
                StreamReader sr = new StreamReader(Server.MapPath("//") + JsonFilePath); 
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

                    Data data = new Data();
                    data.input = inputJsonArray;
                    data.output = outputJsonArray;

                    var content = new FormUrlEncodedContent(new[] 
                    {
                        new KeyValuePair<string, string>("inputJsonArray", inputJsonArray),
                        new KeyValuePair<string, string>("outputJsonArray", outputJsonArray)
                    });

                    String url = WebConfigurationManager.AppSettings["URLRunWaterSim"];

                    try
                    {
                        HttpResponseMessage response = client.PostAsync(url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                            result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<string xmlns=\"http://quayapps.com/\">", "");
                            result = result.Replace("</string>", "");
                            return result;
                        }
                        else
                        {
                            return data.ToString();
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
            if (WebConfigurationManager.AppSettings["UseMock"] == "true")
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
                        HttpResponseMessage response = client.PostAsync(url, null).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                            result = result.Replace("\r\n", "");
                            result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?><string xmlns=\"http://quayapps.com/\">", "");
                            result = result.Replace("</string>", "");
                            string jsonresult = "{ \"VERSION\":\"" + result + "\"}";
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