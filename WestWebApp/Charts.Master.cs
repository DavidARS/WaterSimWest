using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Collections;

namespace WaterSimUI
{
    public partial class ChartsMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string jsonString = GetControlsData(this.Page.Master.Controls);
            //jsonString = InitializeControls(jsonString);
            //hvJSONData.Value = jsonString;
            //CallLoadUserControls(jsonString);
        }

        /*
         * To read the initial values from Meta data service and initialize all the input and output controls 
         * 
         */

        //to construct the json data of input control keywords        
        public class inputData
        {
            public List<string> inputs = new List<string>();
        }
        
        inputData input = new inputData();
        
        public string GetControlsData(ControlCollection controls)
        {
            
            //iterating through all controls
            foreach (Control cntrl in controls)
            {
                

                //if the control is user control
                if (cntrl is UserControl)
                {
                   
                    //if it is an Input Control
                    if (cntrl.GetType().ToString().Contains("inputusercontrol"))
                    {                        
                        input.inputs.Add(((UserControls.InputUserControl)cntrl).FieldKeyWord);
                    }
                }

                //if control is having controls ...
                if (cntrl.Controls.Count > 0)
                    GetControlsData(cntrl.Controls);
            }

            return JsonConvert.SerializeObject(input);
        }

        //Getting data from web service
        public string InitializeControls(string jsonString)
        {
            MetaDataService.WaterSimMetaDataMockService metaData = new MetaDataService.WaterSimMetaDataMockService();
            return metaData.GetFieldNamesAndValues(jsonString);
        }

        //calling the function for loading user controls
        public void CallLoadUserControls(string jsonString)
        {
            dynamic jsonData = JsonConvert.DeserializeObject(jsonString);

            foreach (dynamic fldInfo in jsonData.FieldInfo)
            {
                if (fldInfo.TYP == "IP" || fldInfo.TYP == "IB")

                    if (fldInfo.DEP != null)
                    {
                        foreach (dynamic subctrl in fldInfo.DEP)
                        {
                            LoadUserControl(subctrl.ToString(), jsonData.ToString());
                        }
                    }
            }
        }

        //loading user controls
        public void LoadUserControl(string keyWord, string jsonString)
        {
            //Loading the user control as slider input control
            UserControls.InputUserControl userControl = (UserControls.InputUserControl)LoadControl("~/UserControls/InputUserControl.ascx");

            //getting the data of the control from meta data service using the key word parameter            
            dynamic jsonData = JsonConvert.DeserializeObject(jsonString);

            foreach (dynamic fldInfo in jsonData.FieldInfo)
            {
                if (fldInfo.FLD.ToString() == keyWord)
                {
                    userControl.FieldKeyWord = keyWord;                    
                }
            }
            userControl.ID = keyWord;

            //panelDependents.Controls.AddAt(0, userControl);

        }
    }
}