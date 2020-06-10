using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WaterSimDCDC;
using WaterSimDCDC.Documentation;
using System.IO;
using JSTOOLS;
using WaterSimDCDC.Processes;
using System.Web.Script.Services;


namespace WaterSimWebServiceVersion2
{
    
    [WebService(Namespace = "http://quayapps.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WaterSimDCDCModel : System.Web.Services.WebService
    {
        string WebServiceVersion = "1.7 6.1.14";
        // Strings for new Application Fields
        string WaterSimFlag = "WaterSimFlag";
        string WaterSimStatus = "WaterSimStatus";
        string WaterSimObject = "WaterSimObject";

        // FLags for creating WSIM object only once
        const string WSNOTCREATED = "WSNOTCREATED";
        const string WSCREATED = "WSCREATED";

        // flags to test status of model
        const string WSIDLE = "WSIDLE";
        const string WSBUSY = "WSBUSY";

        // This is where the watersim data is located
        string TheDirectory = "~\\";  //"G:\\Data\\WaterSimWeb\\";

        //----------------------------------------------------------------------------------
        // For Thread Safe Locking
        static readonly object GetWSimThreadLock = new object();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a water simulation manager. </summary>
        ///
        /// <remarks>   Ray Quay, 1/8/2014. </remarks>
        /// <remarks>   Creates a new WaterSim object if does not exist, retrieves the object from application
        ///             memory if it does exist.</remarks>
        /// <exception cref="WaterSimWebServiceException">  Thrown when watersimwebservice. </exception>
        ///
        /// <param name="StatusMessage">    [in,out] Message describing the status. </param>
        ///
        /// <returns>   The water simulation manager. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected WaterSimManager_SIO GetWaterSimManager(ref string StatusMessage)
        {
            // Make Thread Safe
            lock (GetWSimThreadLock)
            {
                // gmap the data directory to the server host space
                string WSDirectory = Server.MapPath(TheDirectory); 
                // create WSim field
                WaterSimManager_SIO WSim = null;
                // Check if WaterSim Object Exists flag has been created  in application memory, if not create it
                if (Application[WaterSimFlag] == null)
                {
                    Application[WaterSimFlag] = WSNOTCREATED;
                }
                // OK, test WaterSim Created flag if not created, the instatntiate and object
                if (Application[WaterSimFlag].ToString() != WSCREATED)
                {
                    try
                    {
                        // double make sure no writes occur in server space
                        WaterSimManager._FortranOutput = false;
                        WaterSimManager.CreateModelOutputFiles = false;
                        // create the object
                        WSim = new WaterSimManager_SIO(WSDirectory, WSDirectory);
                        WSim.IncludeAggregates = true;
                        AnnualFeedbackProcess TrackGW = new TrackAvailableGroundwater("Track Groundwter", WSim);
                        WSim.ProcessManager.AddProcess(TrackGW);
                        AnnualFeedbackProcess PersonalUse = new Personal_GPCD_WebFeedbackProcess("Track Personal", WSim);
                        WSim.ProcessManager.AddProcess(PersonalUse);

                        // set create flag to created
                        Application[WaterSimFlag] = WSCREATED;
                        // store the watersim object in application memory
                        Application[WaterSimObject] = WSim;
                        // save status
                        StatusMessage = "Created";
                    }
                    catch (Exception ex)
                    {
                        // Ouch! and exception has occurred, not good
                        // if object was created, still should not use, null it out and dispose
                        if (WSim != null)
                        {
                            WSim.Dispose();
                            WSim = null;
                        }
                        // rethrow this excepotion, with attached message
                        throw new WaterSimWebServiceException("Error Creating : " + ex.Message);
                    }
                }
                else
                    // OK, one was created and exists in application memory
                    if (Application[WaterSimFlag].ToString() == WSCREATED)
                    {
                        try
                        { 
                            // Retrieve it
                            WSim = (WaterSimManager_SIO)Application[WaterSimObject];
                            // not sure if we have to do this, ie, test to make sure the object is valid, for now lets do simple test
                            string temp = "API:" + WSim.APiVersion + "/Build:" + WSim.ModelBuild;
                            // save status
                            StatusMessage = "Retreived";
                        }
                        catch (Exception ex)
                        {
                            // Ouch again!! this means that the simple integrety test failed
                            // you know what to do
                            if (WSim != null)
                            {
                                WSim.Dispose();
                                WSim = null;
                            }
                            throw new WaterSimWebServiceException("Error Connecting  : " + ex.Message);
                        }
                    }
                    else
                    {
                        // OK, this is really the most serious of conditions, it means the flag has been corrupted,
                        // an thus every thing else is likely corrupted
                        // Get rid of the flag, get rid of the object if it exists, clear object in app memory
                        if (WSim != null)
                        {
                            WSim.Dispose();
                            WSim = null;
                        }
                        string oldcorruptflag = Application[WaterSimFlag].ToString();
                        Application[WaterSimFlag] = null;
                        Application[WaterSimObject] = null;
                        throw new WaterSimWebServiceException("Corrupt State for Application[" + WaterSimFlag + "] = " + oldcorruptflag+".  System reset, try web service again.");
                    }
                return WSim;
            }
        }
        //============================================================================
        
        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the current directory contents for the App_Data Directory, . </summary>
        /////
        ///// <remarks>   Ray Quay, 1/8/2014. </remarks>
        /////
        ///// <returns>   The current directory. </returns>
        /////-------------------------------------------------------------------------------------------------

        //[WebMethod]
        //public string GetCurrentDirectory()
        //{
        //    string temp = "";
           
        //    //string dirPath = WaterSimManagerInfo.GetCurrentDirectory;
        //    string dirPath = Server.MapPath(@"~/App_Data"); 
        //    List<string> dirs = new List<string>(Directory.EnumerateDirectories(dirPath));
        //    foreach (string str in dirs)
        //    {
        //        temp += str+" , " + Environment.NewLine;
        //    }
        //    return temp;
        //}
        ////============================================================================
        // 

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the WaterSim Model and API version. </summary>
        ///
        /// <remarks>   Ray Quay, 1/8/2014. </remarks>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        [WebMethod]
        public string WaterSimVersion()
        {
            string response = "";
            string ErrMessage = "";
            try
            {
                WaterSimManager MyWSim = GetWaterSimManager(ref ErrMessage);
                if (MyWSim != null)
                {
                    response = "Version - WebService: "+WebServiceVersion+ "  API:" + MyWSim.APiVersion + "  Model:" + MyWSim.ModelBuild;
                }
                else
                {
                    response = ErrMessage;
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        public class VersionJson
        {
            public string Version;
            public VersionJson()
            {
                Version = "Not Defined";
            }
            public VersionJson(string value)
            {
                Version = value;
            }
        }

        [WebMethod]
       [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public VersionJson WaterSimVersion_Json()
        {
            string response = "";
            string ErrMessage = "";
            try
            {
                WaterSimManager MyWSim = GetWaterSimManager(ref ErrMessage);
                if (MyWSim != null)
                {
                    response = "Version - API:" + MyWSim.APiVersion + " Build:" + MyWSim.ModelBuild + " WebServ: " + WebServiceVersion;
                }
                else
                {
                    response = ErrMessage;
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            VersionJson ResultJson = new VersionJson(response);
            return ResultJson;
        }

          
        ////----------------------------------------------------------------------------------------


        internal JSON_Object BuildStatusJSON_Object(bool iserror, string errMessage, int startyr, int endyear, string ScnName)
        {
            JSON_Object JO = new JSON_Object();
            string errStr = "ERROR";
            if (!iserror)
            {
                errStr = "SUCCESS";
            }
            JSON_NameValuePair JSVP = new JSON_NameValuePair("STATUS", errStr);
            JO.Add(JSVP);
            JSVP = new JSON_NameValuePair("MESSAGE", errMessage);
            JO.Add(JSVP);
            JSVP = new JSON_NameValuePair("STARTYEAR", startyr.ToString());
            JO.Add(JSVP);
            JSVP = new JSON_NameValuePair("ENDYEAR", endyear.ToString());
            JO.Add(JSVP);
            JSVP = new JSON_NameValuePair("SCNNAME", ScnName);
            JO.Add(JSVP);
            return JO;
        }

        //---------------------------------------
        string AddError(string Source, string NewStr)
        {
            string temp = Source;
            if (Source != "")
                temp += " ; ";
            temp += NewStr;
            return temp;
        }
        //---------------------------------------

        string Internal_ErrMessage = "";
        int Internal_Errors = 0;
        void SetError(string Message)
        {
            Internal_Errors++;
            Internal_ErrMessage = AddError(Internal_ErrMessage, Message);
        }
        //---------------------------------------
        bool isInternalError()
        {
            return (Internal_Errors > 0);
        }
        //---------------------------------------
        void ResetInternalError()
        {
            Internal_ErrMessage = "";
            Internal_Errors = 0;
        }
        //---------------------------------------
        public bool SetModelParameters(JSON_Object InputFields, WaterSimManager_SIO WSim, ref string ErrMessage)
        {
            // keep track of erros
            bool isError = true;
            ResetInternalError();
            // CHeck for at least one value pair 
            if (InputFields.Values.Count <=0) { SetError("No Name Value Pair in Object");}
            else
            {
                // check if Inputs defined
                if (InputFields.Values[0].Name.ToUpper() != "INPUTS") { SetError("INPUTS not defined"); }
                else
                {
                    // check if value is an array
                    if (!InputFields.Values[0].Value.isJSON_Array) { SetError("Value is not an array"); }
                    else
                    {
                        // get input field array
                        JSON_Array InputArray = (InputFields.Values[0].Value as JSON_Array);
                        int Arrayindex = 0;
                        foreach (JSON_Value JSV in InputArray.Values)
                        {
                            // check is array value is object, else break
                            if (!JSV.isJSON_Object) { SetError("Value"+Arrayindex.ToString()+" in Array is not a JSON Object"); }
                            else
                            {
                                if ((JSV as JSON_Object).Values.Count < 2) { SetError("Not enough Name Value Pairs in Object #"+Arrayindex.ToString()); }
                                else
                                {
                                    string WSFldNameStr = "";
                                    // get 1 and 2 value pairs
                                    JSON_NameValuePair FLD_VP = (JSV as JSON_Object).Find("FLD");
                                    JSON_NameValuePair VAL_VP = (JSV as JSON_Object).Find("VAL");
                                    JSON_NameValuePair PVC_VP = (JSV as JSON_Object).Find("PVC");
                                    // test if FLD value
                                    if ((FLD_VP == null) || (VAL_VP == null))
                                    {
                                        if (FLD_VP == null) SetError("Object #" + Arrayindex.ToString() + " FLD: not defined");
                                        if (VAL_VP == null) SetError("Object #" + Arrayindex.ToString() + " VAL: not defined");
                                    }
                                    else
                                    {
                                        if (!FLD_VP.Value.isJSON_String) { SetError("Object #" + Arrayindex.ToString() + "value is not a string"); }
                                        else
                                        {
                                            WSFldNameStr = (FLD_VP.Value as JSON_String).Value;
                                            try
                                            {
                                                ModelParameterClass MP = WSim.ParamManager.Model_Parameter(WSFldNameStr);
                                                if (MP == null)
                                                {
                                                    isError = true;
                                                    SetError("Invalid Fieldname : " + WSFldNameStr);
                                                    break;
                                                }
                                                else
                                                {
                                                    switch (MP.ParamType)
                                                    {
                                                        case modelParamtype.mptInputProvider:
                                                            {
                                                                if (PVC_VP==null) { SetError(WSFldNameStr + " - PVC not defined"); }
                                                                else
                                                                {
                                                                    if (!PVC_VP.Value.isJSON_String) { SetError(WSFldNameStr + "PVC value not a string"); }
                                                                    else
                                                                    {
                                                                        int providerindex = -1;
                                                                        string pcode = (PVC_VP.Value as JSON_String).Value;
                                                                        try
                                                                        {
                                                                            providerindex = ProviderClass.index(pcode);
                                                                        }
                                                                        catch
                                                                        {
                                                                            providerindex = -1;
                                                                        }
                                                                        if (providerindex < 0) 
                                                                          { SetError(WSFldNameStr + " - PVC value ["+pcode+"] is invalid"); }
                                                                        else
                                                                        {
                                                                            int MPValue = int.MinValue;
                                                                            if (VAL_VP.Value.isJSON_Number)
                                                                            {

                                                                                if (!(VAL_VP.Value as JSON_Number).asInt(ref MPValue)) { SetError(WSFldNameStr + "VAL: value invalid");}
                                                                            }
                                                                            else
                                                                            if (VAL_VP.Value.isJSON_String)
                                                                            {
                                                                                try
                                                                                {
                                                                                    int temp = Convert.ToInt32((VAL_VP.Value as JSON_String).Value);
                                                                                    MPValue = temp;
                                                                                }
                                                                                catch (Exception ConvertEx)
                                                                                {
                                                                                    SetError(WSFldNameStr + "[" + (VAL_VP.Value as JSON_String).Value + "] invalid " +ConvertEx.Message);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                SetError(WSFldNameStr + " - Value Malformed or invalid");
                                                                            }
                                                                            // ok if value found set it
                                                                            if (MPValue > int.MinValue)
                                                                            {
                                                                                ProviderIntArray PData = MP.ProviderProperty.getvalues();
                                                                                PData[providerindex] = MPValue;
                                                                                try
                                                                                {
                                                                                    MP.ProviderProperty.setvalues(PData);
                                                                                }
                                                                                catch (Exception ValueEx)     
                                                                                {
                                                                                    SetError(WSFldNameStr+" : "+ValueEx.Message);
                                                                                }
                                                                            }
                                                                        } // else Provider index valid
                                                                    } // is string
                                                                 } // = PVC

                                                                break;
                                                            }// end of case mptInputProvider:
                                                        case modelParamtype.mptInputOther:
                                                            break;
                                                        case modelParamtype.mptInput2DGrid:
                                                            break;
                                                        case modelParamtype.mptInputBase:
                                                            {
                                                                // get value
                                                                int MPValue = 0;
                                                                if (VAL_VP.Value.isJSON_Number)
                                                                {
                                                                    if (!(VAL_VP.Value as JSON_Number).asInt(ref MPValue)) { SetError(WSFldNameStr+" VAL: value conversion error"); }
                                                                    MP.Value = MPValue;
                                                                }
                                                                else
                                                                if (VAL_VP.Value.isJSON_String)
                                                                {
                                                                    try
                                                                    {
                                                                        int temp = Convert.ToInt32((VAL_VP.Value as JSON_String).Value);
                                                                        MPValue = temp;
                                                                        MP.Value = MPValue;
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        SetError(WSFldNameStr + "[" + (VAL_VP.Value as JSON_String).Value + "] invalid " + ex.Message);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    SetError(WSFldNameStr+" - Value Malformed or invalid");
                                                                }
                                                             break;
                                                         } // Case base
                                                    } // switch
                                                } // mp null
                                            } // try
                                            catch (Exception BigEx)
                                            {
                                                SetError("Errort with : " + WSFldNameStr+" - "+BigEx.Message);
                                            }
                                        
                                        } //else FLD_VP value is string
                                    }  // else FLD_VP and VAL_VP not null
                                } // else enough value pairs
                            } // else is object
                        } // For each
                    } // else is JSON_ARRAY
                } //  Else name = "INPUTS"
            } // else values.count >0
            isError = isInternalError();
            if (isError)
            {
                ErrMessage = Internal_ErrMessage;
            }
            ResetInternalError();
            return isError;
        }
        //-----------------------------------------------------------
        string GetRangeCheckTypeString(rangeChecktype rcht)
        {
            string temp = "";
            switch (rcht)
            {
                case rangeChecktype.rctCheckPositive:
                    temp = "Positive";
                    break;
                case rangeChecktype.rctCheckRange:
                    temp = "Range";
                    break;
                case rangeChecktype.rctCheckRangeSpecial:
                    temp = "Special";
                    break;
                case rangeChecktype.rctNoRangeCheck:
                    temp = "None";
                    break;
                case rangeChecktype.rctUnknown:
                default:
                    temp = "Not defined";
                    break;
            }
            return temp;
        }

        
        //string GetRangeCheckString (
        //-----------------------------------------------------------------
        string GetParmTypeString(modelParamtype mpt)
        {
            string temp = "";
            
            switch(mpt)
            {
                case modelParamtype.mptInputBase:
                    temp = "IB";
                    break;
                case modelParamtype.mptInputOther:
                    temp = "IO";
                    break;
                case modelParamtype.mptInputProvider:
                    temp = "IP";
                    break;
                case modelParamtype.mptOutputBase:
                    temp = "OB";
                    break;
                case modelParamtype.mptOutputOther:
                    temp = "OO";
                    break;
                case modelParamtype.mptOutputProvider:
                    temp = "OP";
                    break;
                case modelParamtype.mptInput2DGrid:
                    temp = "OG";
                    break;
            }
            return temp;
        }
        //-------------------------------------------------------
        public bool GetModelResults(JSON_Object OutputFields, SimulationResults SimResults, ParameterManagerClass PManager, 
            ref JSON_Array ModelResults, ref string errMessage)
        {
            JSON_Array JSON_FieldsArray = new JSON_Array();
            // setup for return
            bool iserror = false;
            ResetInternalError();
            //JSON_Array JSON_Results = new JSON_Array();
            // Get List of Outputs and Cities
            JSON_NameValuePair VPsOut = OutputFields.Find("Outputs");
            JSON_NameValuePair PrvsOut = OutputFields.Find("Providers");
            // make sure they are there
            if ((VPsOut == null)||(PrvsOut==null)) {

                string temp = "Output Definition Error: ";
                if (VPsOut==null) temp+= "'Outputs' not defined";
                if (PrvsOut == null) temp += "'Providers' not defined";
                SetError(temp);
            }
            else
            {
                // Setup CHecking

                // create lists
                List<eProvider> ProviderList = new List<eProvider>();
                List<int> ParmCodeList = new List<int>();
                
                // OK get values, check if arrays
                JSON_Value JSVFld = VPsOut.Value;
                JSON_Value JSVPrv = PrvsOut.Value;

                // OK get providers
                if (JSVPrv.isJSON_Array)
                {
                    // cycle through and grab provider code
                    foreach (JSON_Value JSV in (JSVPrv as JSON_Array).Values)
                    {
                        // make sure a string
                        if (!JSV.isJSON_String) { SetError("Provider Code not a string"); }
                        else
                        {
                            // get code
                            string code = (JSV as JSON_String).Value;
                            if (code == "all")
                            {
                                foreach (eProvider ep in ProviderClass.providers())
                                {
                                    ProviderList.Add(ep);
                                }
                            }
                            else
                                // convert string code to eprovider, wacth for exception
                                try
                                {
                                    eProvider ep = ProviderClass.provider(code);
                                    ProviderList.Add(ep);
                                }
                                catch
                                {
                                    // opps not valid
                                    SetError(" Invalid Provider Code : " + code);
                                }

                        } // is string
                    } // foreach
                } // is array


                // ok get field list
                if (JSVFld.isJSON_Array)
                {
                    // Get out fld list
                    foreach (JSON_Value JSV in (JSVFld as JSON_Array).Values)
                    {
                        if (JSV.isJSON_String)
                        {
                            try
                            {
                                string fldstr = (JSV as JSON_String).Value;
                                if (fldstr == "all")
                                {
                                    foreach (ModelParameterClass MPi in PManager.BaseOutputs())
                                    {
                                        ParmCodeList.Add(MPi.ModelParam);
                                    }
                                    foreach (ModelParameterClass MPi in PManager.ProviderOutputs())
                                    {
                                        ParmCodeList.Add(MPi.ModelParam);
                                    }

                                }
                                try
                                {
                                    ModelParameterClass MP = PManager.Model_Parameter(fldstr);

                                    int eMP = MP.ModelParam;
                                    ParmCodeList.Add(eMP);
                                }
                                catch (Exception ex)
                                {
                                    SetError(ex.Message);
                                }
                            } //try
                            catch (Exception ex)
                            {
                                SetError(ex.Message);
                            }// catch
                        } // is string
                    } // for each JSV
                    // OK, lets see if we have a list
                    if (ParmCodeList.Count > 0)
                    {
                        foreach (int parmcode in ParmCodeList)
                        {
                            // create field object
                            int dyear = 0;
                            int dparmindex = 0;

                            try
                            {
                                // number of year
                                int years = SimResults.Length;
                                // ok create a JSON array for data
                                JSON_Array JSON_ParmData = new JSON_Array();
                                // get parameter  could be exception
                                ModelParameterClass MP = PManager.Model_Parameter(parmcode);
                                // get fieldname and add to object
                                string FldNameStr = MP.Fieldname;
                                // get param type and add to object
                                modelParamtype ept = MP.ParamType;
                                // ok build value will vary based on type
                                // Provider
                                if (MP.isProviderParam)
                                {
                                    // Provider Field
                                    if (ProviderList.Count > 0)
                                    {

                                        // OK build Provider JSON_Array
                                        // OK good to go  Go through all Provider codes
                                        foreach (eProvider pcode in ProviderList)
                                        {
                                            JSON_Object FldObject = new JSON_Object();
                                            FldObject.Add("FLD", FldNameStr);
                                            FldObject.Add("TYP", GetParmTypeString(ept));
                        
                                            // get provider code string
                                            string providerStr = ProviderClass.FieldName(pcode);
                                            // setup for the values
                                            int[] MValues = new int[years];
                                            // ok go through each year, grab the data for this (parmcode) parameter and provider (pcode)
                                            int year = 0;
                                            foreach (AnnualSimulationResults ASR in SimResults)
                                            {
                                                if (MP.isOutputParam)
                                                {
                                                    // OK this is output
                                                    int ModelParamIndex = ASR.Outputs.ProviderIndex(parmcode);
                                                    int ProviderIndex = ProviderClass.index(pcode, ASR.Outputs.AggregatesIncluded);
                                                    if ((ModelParamIndex >= 0) && (ProviderIndex >= 0))
                                                    {
                                                        MValues[year] = ASR.Outputs.ProviderOutput[ModelParamIndex].Values[ProviderIndex];
                                                    }
                                                } // output
                                                else
                                                {
                                                    // OK this is input
                                                    int ModelParamIndex = ASR.Inputs.ProviderIndex(parmcode);
                                                    int ProviderIndex = ProviderClass.index(pcode, ASR.Outputs.AggregatesIncluded);
                                                    if ((ModelParamIndex >= 0) && (ProviderIndex >= 0))
                                                    {
                                                        MValues[year] = ASR.Inputs.ProviderInput[ModelParamIndex].Values[ProviderIndex];
                                                    }                                                    // 
                                                } // input
                                                // increment to  next year/////////
                                                year++;
                                            } // for each ASR
                                            // OK should have all date for parameter and provier, build array and add object
                                            // build array
                                            JSON_Array JSON_DataByYear = new JSON_Array();
                                            foreach (int value in MValues)
                                            {
                                                JSON_Number JSN = new JSON_Number(value);
                                                JSON_DataByYear.Add(JSN);
                                            }
                                            // OK Add an object to the array
                                            JSON_Object JSON_Prov_For_Year = new JSON_Object();

                                            FldObject.Add("PVC", providerStr);
                                            FldObject.Add("VALS", JSON_DataByYear);
                                            //JSON_Prov_For_Year.Add("PVC", providerStr);
                                            //JSON_Prov_For_Year.Add("VALS", JSON_DataByYear);
                                            JSON_FieldsArray.Add(FldObject);
                                        } // For each pcode
                                    } // cnt>0
                                }// provider
                                else
                                {
                                    JSON_Object FldObject = new JSON_Object();
                                    FldObject.Add("FLD", FldNameStr);
                                    FldObject.Add("TYP", GetParmTypeString(ept));
               
                                    if (MP.isBaseParam)
                                    {
                                        // Base Field
                                        int[] MValues = new int[years];
                                        int year = 0;
                                        foreach (AnnualSimulationResults ASR in SimResults)
                                        {
                                            dyear = year;
                                            if (MP.isOutputParam)
                                            {
                                                // OK this is output
                                                int ModelParamIndex = ASR.Outputs.BaseIndex(parmcode);
                                                dparmindex = ModelParamIndex;
                                                if (ModelParamIndex >= 0)
                                                {
                                                    MValues[year] = ASR.Outputs.BaseOutput[ModelParamIndex];
                                                }
                                            } // output
                                            else
                                            {
                                                // OK this is input
                                                int ModelParamIndex = ASR.Inputs.BaseIndex(parmcode);
                                                if (ModelParamIndex >= 0)
                                                {
                                                    MValues[year] = ASR.Inputs.BaseInput[ModelParamIndex];
                                                }                                            // 
                                            } // input
                                            year++;
                                        }
                                        foreach (int value in MValues)
                                        {
                                            JSON_Number JSN = new JSON_Number(value);
                                            JSON_ParmData.Add(JSN);
                                        }

                                    }// if Base
                                    else
                                    {
                                        JSON_String JSS = new JSON_String("Unknown");
                                        JSON_ParmData.Add(JSS);
                                        SetError("Not sure what field type this is - " + MP.Fieldname);
                                    }
                                    // ok got data for this field, add object
                                    JSON_NameValuePair JSON_VAL = new JSON_NameValuePair("VALS", JSON_ParmData);
                                    FldObject.Add(JSON_VAL);
                                    JSON_FieldsArray.Add(FldObject);
                                }
                            } // try get MP
                            catch (Exception ex)
                            {
                                SetError("Serious! " + ex.Message);
                                // should not be possible!
                            } // get parm code

                        } // for each parmcode
                        ModelResults = JSON_FieldsArray;

                    } // parmcode list > 0
                    
                } // if Array
                else
                {
                    SetError("Array of Field Names not specified");
                }
            } // if list object not null
            iserror = isInternalError();
            errMessage += Internal_ErrMessage;
            return iserror;
        }

        public bool GetModelResults2(JSON_Object OutputFields, SimulationResults SimResults, ParameterManagerClass PManager,
       ref JSON_Array ModelResults, ref string errMessage)
        {
            JSON_Array JSON_FieldsArray = new JSON_Array();
            // setup for return
            bool iserror = false;
            ResetInternalError();
            //JSON_Array JSON_Results = new JSON_Array();
            // Get List of Outputs and Cities
            JSON_NameValuePair VPsOut = OutputFields.Find("Outputs");
            JSON_NameValuePair PrvsOut = OutputFields.Find("Providers");
            // make sure they are there
            if ((VPsOut == null) || (PrvsOut == null))
            {

                string temp = "Output Definition Error: ";
                if (VPsOut == null) temp += "Outputs not defined";
                if (PrvsOut == null) temp += "Providers not defined";
                SetError(temp);
            }
            else
            {
                // Setup CHecking

                // create lists
                List<eProvider> ProviderList = new List<eProvider>();
                List<int> ParmCodeList = new List<int>();

                // OK get values, check if arrays
                JSON_Value JSVFld = VPsOut.Value;
                JSON_Value JSVPrv = PrvsOut.Value;

                // OK get providers
                if (JSVPrv.isJSON_Array)
                {
                    // cycle through and grab provider code
                    foreach (JSON_Value JSV in (JSVPrv as JSON_Array).Values)
                    {
                        // make sure a string
                        if (!JSV.isJSON_String) { SetError("Provider Code not a string"); }
                        else
                        {
                            // get code
                            string code = (JSV as JSON_String).Value;
                            if (code == "all")
                            {
                                foreach (eProvider ep in ProviderClass.providers())
                                {
                                    ProviderList.Add(ep);
                                }
                            }
                            else
                                // convert string code to eprovider, wacth for exception
                                try
                                {
                                    eProvider ep = ProviderClass.provider(code);
                                    ProviderList.Add(ep);
                                }
                                catch
                                {
                                    // opps not valid
                                    SetError(" Invalid Provider Code : " + code);
                                }

                        } // is string
                    } // foreach
                } // is array


                // ok get field list
                if (JSVFld.isJSON_Array)
                {
                    // Get out fld list
                    foreach (JSON_Value JSV in (JSVFld as JSON_Array).Values)
                    {
                        if (JSV.isJSON_String)
                        {
                            string fldstr = (JSV as JSON_String).Value;
                            if (fldstr == "all")
                            {
                                try
                                {
                                    foreach (ModelParameterClass MPi in PManager.BaseOutputs())
                                    {
                                        ParmCodeList.Add(MPi.ModelParam);
                                    }
                                    foreach (ModelParameterClass MPi in PManager.ProviderOutputs())
                                    {
                                        ParmCodeList.Add(MPi.ModelParam);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SetError("Ouch!! Internal Model Parameter Error "+ex.Message);
                                }
                            } // if all
                            else
                            {
                                try
                                {
                                    ModelParameterClass MP = PManager.Model_Parameter(fldstr);

                                    int eMP = MP.ModelParam;
                                    ParmCodeList.Add(eMP);
                                }
                                catch (Exception ex)
                                {
                                    SetError("Invalid Field Request FLD:"+fldstr+" - "+ex.Message);
                                }
                            } // else all
                        } // is string
                    } // for each JSV
                    // OK, lets see if we have a list
                    if (ParmCodeList.Count > 0)
                    {
                        foreach (int parmcode in ParmCodeList)
                        {
                            // create field object
                            int dyear = 0;
                            int dparmindex = 0;

                            try
                            {
                                // number of year
                                int years = SimResults.Length;
                                // ok create a JSON array for data
                                JSON_Array JSON_ParmData = new JSON_Array();
                                // get parameter  could be exception
                                ModelParameterClass MP = PManager.Model_Parameter(parmcode);
                                // get fieldname and add to object
                                string FldNameStr = MP.Fieldname;
                                // get param type and add to object
                                modelParamtype ept = MP.ParamType;
                                // ok build value will vary based on type
                                // Provider
                                if (MP.isProviderParam)
                                {

                                    // Provider Field
                                    if (ProviderList.Count > 0)
                                    {
                                        JSON_Object FldObject = new JSON_Object();
                                        FldObject.Add("FLD", FldNameStr);
                                        FldObject.Add("TYP", GetParmTypeString(ept));
                                        JSON_Array PVCDataArray = new JSON_Array();
                                        // OK build Provider JSON_Array
                                        // OK good to go  Go through all Provider codes
                                        foreach (eProvider pcode in ProviderList)
                                        {
                                    
                                            // get provider code string
                                            string providerStr = ProviderClass.FieldName(pcode);
                                            // setup for the values
                                            int[] MValues = new int[years];
                                            // ok go through each year, grab the data for this (parmcode) parameter and provider (pcode)
                                            int year = 0;
                                            foreach (AnnualSimulationResults ASR in SimResults)
                                            {
                                                if (MP.isOutputParam)
                                                {
                                                    // OK this is output
                                                    int ModelParamIndex = ASR.Outputs.ProviderIndex(parmcode);
                                                    int ProviderIndex = ProviderClass.index(pcode, ASR.Outputs.AggregatesIncluded);
                                                    if ((ModelParamIndex >= 0) && (ProviderIndex >= 0))
                                                    {
                                                        MValues[year] = ASR.Outputs.ProviderOutput[ModelParamIndex].Values[ProviderIndex];
                                                    }
                                                } // output
                                                else
                                                {
                                                    // OK this is input
                                                    int ModelParamIndex = ASR.Inputs.ProviderIndex(parmcode);
                                                    int ProviderIndex = ProviderClass.index(pcode, ASR.Outputs.AggregatesIncluded);
                                                    if ((ModelParamIndex >= 0) && (ProviderIndex >= 0))
                                                    {
                                                        // check to make sure there are values here for this index, not sure why this bug is happening
                                                        // but for parameters with agNone then there are not
                                                        if (ASR.Inputs.ProviderInput[ModelParamIndex].Values.Length > ProviderIndex)
                                                        {
                                                            MValues[year] = ASR.Inputs.ProviderInput[ModelParamIndex].Values[ProviderIndex];
                                                        }
                                                        else
                                                            // OK, this is a bad value sao provide missing value value
                                                        {
                                                            MValues[year] = int.MinValue;
                                                        }
                                                    }                                                // 
                                                } // input
                                                // increment to  next year/////////
                                                year++;
                                            } // for each ASR
                                            // OK should have all date for parameter and provier, build array and add object
                                            // build array
                                            JSON_Array JSON_DataByYear = new JSON_Array();
                                            foreach (int value in MValues)
                                            {
                                                JSON_Number JSN = new JSON_Number(value);
                                                JSON_DataByYear.Add(JSN);
                                            }
                                            // OK Add an object to the array
                                            JSON_Object JSON_Prov_For_Year = new JSON_Object();
                                            JSON_Object AProviderObject = new JSON_Object();
                                            AProviderObject.Add("PVC", providerStr);
                                            AProviderObject.Add("VALS", JSON_DataByYear);

                                            PVCDataArray.Add(AProviderObject);
                                            //FldObject.Add("PVC", providerStr);
                                            //FldObject.Add("VALS", JSON_DataByYear);

                                            //JSON_Prov_For_Year.Add("PVC", providerStr);
                                            //JSON_Prov_For_Year.Add("VALS", JSON_DataByYear);
                                        } // For each pcode
                                        JSON_NameValuePair PVCNVP = new JSON_NameValuePair("VALS", PVCDataArray);
                                        FldObject.Add(PVCNVP);
                                        JSON_FieldsArray.Add(FldObject);
                                    } // cnt>0
                                }// provider
                                else
                                {
                                    JSON_Object FldObject = new JSON_Object();
                                    FldObject.Add("FLD", FldNameStr);
                                    FldObject.Add("TYP", GetParmTypeString(ept));

                                    if (MP.isBaseParam)
                                    {
                                        // Base Field
                                        int[] MValues = new int[years];
                                        int year = 0;
                                        foreach (AnnualSimulationResults ASR in SimResults)
                                        {
                                            dyear = year;
                                            if (MP.isOutputParam)
                                            {
                                                // OK this is output
                                                int ModelParamIndex = ASR.Outputs.BaseIndex(parmcode);
                                                dparmindex = ModelParamIndex;
                                                if (ModelParamIndex >= 0)
                                                {
                                                    MValues[year] = ASR.Outputs.BaseOutput[ModelParamIndex];
                                                }
                                            } // output
                                            else
                                            {
                                                // OK this is input
                                                int ModelParamIndex = ASR.Inputs.BaseIndex(parmcode);
                                                if (ModelParamIndex >= 0)
                                                {
                                                    MValues[year] = ASR.Inputs.BaseInput[ModelParamIndex];
                                                }                                            // 
                                            } // input
                                            year++;
                                        }
                                        foreach (int value in MValues)
                                        {
                                            JSON_Number JSN = new JSON_Number(value);
                                            JSON_ParmData.Add(JSN);
                                        }

                                    }// if Base
                                    else
                                    {
                                        JSON_String JSS = new JSON_String("Unknown");
                                        JSON_ParmData.Add(JSS);
                                        SetError("Not sure what field type this is - " + MP.Fieldname);
                                    }
                                    // ok got data for this field, add object
                                    JSON_NameValuePair JSON_VAL = new JSON_NameValuePair("VALS", JSON_ParmData);
                                    FldObject.Add(JSON_VAL);
                                    JSON_FieldsArray.Add(FldObject);
                                }
                            } // try get MP
                            catch (Exception ex)
                            {
                                SetError("Serious! " + ex.Message);
                                // should not be possible!
                            } // get parm code

                        } // for each parmcode
                        ModelResults = JSON_FieldsArray;

                    } // parmcode list > 0

                } // if Array
                else
                {
                    SetError("Array of Field Names not specified");
                }
            } // if list object not null
            iserror = isInternalError();
            if (iserror)
            {
                errMessage = Internal_ErrMessage;
            }
            return iserror;
        }


        /******************************************
        *  Get WaterSim Parameter Info Web Service
        *  Ver 1
        *  ThreadSafe
        *  GetParameterInfo(string inputJsonArray)
        *  inputJsonArray example "{"InfoRequest":["STOPYR","CORFLOW"]}"
        *  return string {"FieldInfo":[{"FLD":"STOPYR","LAB":"Stop Year","LNG":"The last year of the model simulation",
         *                "UNITS":"Year","MIN":2000,"MAX":2086,"TYP":"BI","DEP":[]},{}...{}],"Providers":[{"NAME":"Phoenix","CODE":"ph"},{},{}], "Error":"None"}"
        * ****************************************/
        #region

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a parameter information. </summary>
        /// <param name="inputJsonArray">   Array of input objects in json format. </param>
        /// <returns>   The parameter information. </returns>
        ///-------------------------------------------------------------------------------------------------

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetParameterInfo(string inputJsonArray)
        {   
            string ServiceResult = "";
            JSON_Object ResultJson = new JSON_Object();
            bool iserror = true;
            string errMessage = "";
            // Get the WaterSimObject
            WaterSimManager WSim = GetWaterSimManager(ref errMessage);
            if (WSim != null)
            {
                Extended_Parameter_Documentation EPD = new Extended_Parameter_Documentation(WSim.ParamManager);
                // no need for locking, this is thread safe
                // get the list of fields
                JSON_Object FieldResquest = new JSON_Object(inputJsonArray);
                // test to make sure that there are values
                errMessage = "No Fields Specified";
                if  (FieldResquest.Values.Count > 0)
                { 
                    // find the inforequest object
                    JSON_NameValuePair JSNP = FieldResquest.Find("InfoRequest");
                    // if found continue
                    if (JSNP!=null)
                    {
                        // make sure the value of object is an array
                        if (JSNP.Value.isJSON_Array)
                        {
                            
                            if ((JSNP.Value as JSON_Array).Length > 0)
                            {
                                // ok creat the fields list
                                List<string> FldsToFetch = new List<string>();
                                // create the return array
                                JSON_Array FieldsJsonArray = new JSON_Array();
                                // fetch the first value and check if "all"
                                JSON_Value FirstJson = (JSNP.Value as JSON_Array).Values[0];
                                // needs to be a string
                                if (FirstJson.isJSON_String)
                                {
                                    // check if all
                                    if ((FirstJson as JSON_String).Value.ToUpper() == "ALL")
                                    {
                                        // create the list for all parameters
                                        foreach (int emp in WSim.ParamManager.eModelParameters())
                                        {
                                            try
                                            {
                                                FldsToFetch.Add(WSim.ParamManager.Model_Parameter(emp).Fieldname);
                                            }
                                            catch (Exception ex)
                                            {
                                                string err = ex.Message;
                                            }
                                        }
                                    }
                                    else
                                    // ok cycle through teh array and grab fieldname
                                    {
                                        foreach (JSON_String JS in (JSNP.Value as JSON_Array).Values)
                                        {
                                            FldsToFetch.Add(JS.Value);
                                        }
                                    }
                                    iserror = false;

                                    // of for each fieldname, get the info and create an info object
                                    foreach (string Fldstr in FldsToFetch)
                                    {
                                        // get the parameter
                                        try
                                        {
                                            ModelParameterClass MP = WSim.ParamManager.Model_Parameter(Fldstr);

                                            // if valid get info
                                            if (MP != null)
                                            {
                                                // create JSON object for field ino
                                                JSON_Object FldInfo = new JSON_Object();
                                                // see if webLabel is available, otherwise use regular label
                                                string tempLab = MP.WebLabel;
                                                if (tempLab == "")
                                                {
                                                    tempLab = MP.Label;
                                                }

                                                // assign filedinfo values
                                                FldInfo.Add(new JSON_NameValuePair("FLD", MP.Fieldname));
                                                FldInfo.Add(new JSON_NameValuePair("LAB", tempLab));
                                                FldInfo.Add(new JSON_NameValuePair("MIN", MP.LowRange.ToString()));
                                                FldInfo.Add(new JSON_NameValuePair("MAX", MP.HighRange.ToString()));
                                                // create a string for parmtype
                                                string TypeStr = GetParmTypeString(MP.ParamType);
                                                FldInfo.Add(new JSON_NameValuePair("TYP", TypeStr));
                                                // get range check type
                                                rangeChecktype RCHtype = MP.RangeCheckType;
                                                string RCstr = GetRangeCheckTypeString(RCHtype);
                                                FldInfo.Add(new JSON_NameValuePair("RCT", RCstr));
                                                // if special
                                                if (RCHtype == rangeChecktype.rctCheckRangeSpecial)
                                                {
                                                    string GroupInfo = "INFO";
                                                    string InfoStr = "";
                                                    if ((MP.ParamType == modelParamtype.mptInputProvider)&&(MP.SpecialProviderCheck != null))
                                                    {
                                                        if (MP.SpecialProviderCheck(int.MinValue, 0, ref GroupInfo, MP))
                                                        {
                                                            InfoStr = GroupInfo;
                                                        }
                                                    }
                                                    else
                                                        if ((MP.ParamType == modelParamtype.mptInputBase)&&(MP.SpecialBaseCheck!=null))
                                                        {
                                                            if (MP.SpecialBaseCheck(int.MinValue, ref GroupInfo, MP))
                                                            {
                                                                InfoStr = GroupInfo;
                                                            }
                                                        }
                                                    FldInfo.Add(new JSON_NameValuePair("GRP", GroupInfo));
                                                }
                                                // get extended info from documentation
                                                FldInfo.Add(new JSON_NameValuePair("LNG", EPD.Description(MP.ModelParam)));
                                                FldInfo.Add(new JSON_NameValuePair("UNT", EPD.Unit(MP.ModelParam)));
                                                FldInfo.Add(new JSON_NameValuePair("UNTL", EPD.LongUnit(MP.ModelParam)));

                                                // Get dependencies
                                                JSON_Array DependLst = new JSON_Array();
                                                if ((MP.DerivedFrom!=null)&&(MP.DerivedFrom.Count>0))
                                                {
                                                    foreach (int value in MP.DerivedFrom.ModelParameters())
                                                    {
                                                        try
                                                        {
                                                            ModelParameterClass Dmp = WSim.ParamManager.Model_Parameter(value);
                                                            DependLst.Add(new JSON_String(Dmp.Fieldname));
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                                FldInfo.Add(new JSON_NameValuePair("DEP", DependLst));
                                                FieldsJsonArray.Add(FldInfo);
                                            } // MP null
                                        }
                                        catch (Exception ex)
                                        {
                                            iserror = true;
                                            if (errMessage != "")
                                                errMessage += " ; ";
                                            errMessage += ex.Message;
                                        }
                                    } // foreach FldsToFetch
                                    ResultJson.Add("FieldInfo", FieldsJsonArray);
                                    // now providers
                                    JSON_Array ProviderJsonArray = new JSON_Array();
                                    foreach (eProvider ep in ProviderClass.providersAll())
                                    {
                                        string thecode = ProviderClass.FieldName(ep);
                                        string thelabel = ProviderClass.Label(ep);
                                        JSON_NameValuePair JSVPCode = new JSON_NameValuePair("FLD", new JSON_String(thecode));
                                        JSON_NameValuePair JSVPlable = new JSON_NameValuePair("LAB", new JSON_String(thelabel));
                                        JSON_Object ProvObject = new JSON_Object();
                                        ProvObject.Add(JSVPCode);
                                        ProvObject.Add(JSVPlable);
                                        ProviderJsonArray.Add(ProvObject);
                                    }
                                    ResultJson.Add("ProviderInfo", ProviderJsonArray);
                                } // length >0
                            }// is array
                        }
                    }
                }
            
            }
            if (iserror)
            {
                ResultJson.Add("Error", errMessage);
            }
            else
            {
                ResultJson.Add("Error", "NONE");
            }
            ServiceResult = ResultJson.Serialize(true);
            return ServiceResult;
        }
        #endregion
        /******************************************
         *  Run WaterSim Web Service
         *  Ver 2
         *  ThreadSafe
         *  RunWaterSim(string inputJsonArray, string outputJsonArray)
         *  inputJsonArray Example "{"Inputs":[{"FLD":"STOPYR","VAL":2065},{"FLD":"COCLMADJ,"VAL":80},{"FLD":"PCGRTON","VAL":10,"PVC":"ph"}] }"
         *  outputJsonArray Example "{"Outputs":["COCLMADJ","CORFLOW","SVRFLOW","MGWPUMP","POPUSED","TOTDEM"],"Providers":["ph","sc"]}"
         *  return string
         * ****************************************/
        #region
        //---------------------------------------------------------------------
        // For Thread Safe Locking
        static readonly object RunModelThreadLock = new object();
       
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes a WaterSim Simulation</summary> 
        ///             <remarks>using the Inputs passed, returning a list based on the requested output list  simulation operation. </remarks>
        ///
        /// <remarks>   Ray Quay, 1/8/2014. </remarks>
        ///
        /// <param name="inputJsonArray">   Array of input objects in json format. </param>
        /// <param name="outputJsonArray">  Array of requested outputs in jsons format </param>
        ///
        /// <returns> A list of output objects in json format. </returns>
        ///-------------------------------------------------------------------------------------------------
        [WebMethod]
        public string RunWaterSim(string inputJsonArray, string outputJsonArray)
        {
            //Setup Status fields
            int StartYear = 0;
            int EndYear = 0;
            string ScenarioName = "";
            bool isSetError = false;
            bool isGetError = false;
            bool isFetchError = false;
            bool isHardError = false;
            bool isError = true;
            string FetchErrMessage = "";
            string SetErrMessage = "No Error";
            string GetErrMessage = "No Error";
            string HardErrMessage = "No Error";
            //string ErrMessage = "No Error";
            // Setup Return Values
            JSON_Array JSON_Results_Array = new JSON_Array();
            
            // OK, need to lock this down to make thread safe
            lock (RunModelThreadLock)
                {
                // get WSIM object
                WaterSimManager_SIO MyWSim = GetWaterSimManager(ref FetchErrMessage);
                if (MyWSim != null)
                {
                    // put JSON input data in JSONObjects
                    JSON_Object InputFields = new JSON_Object(inputJsonArray);
                    JSON_Object OutputFields = new JSON_Object(outputJsonArray);

                    // checkif Status field exists if not create 
                    if (Application[WaterSimStatus] == null)
                        Application[WaterSimStatus] = WSIDLE;

                    // check if Busy  should insert some code to wait until not busy
                    if (Application[WaterSimStatus].ToString() != WSBUSY)

                        try
                        {   // we are busy
                            Application[WaterSimStatus] = WSBUSY;
                            // Initialation Simulation
                            MyWSim.Simulation_Initialize();
                            List<string> ProcessList = new List<string>();
                            ProcessList = MyWSim.ProcessManager.ActiveProcesses;
                            // Set the Parameters based on recieved data
                            isSetError = SetModelParameters(InputFields, MyWSim, ref SetErrMessage);
                            // Run all years, using defaults for values not set
                            MyWSim.Simulation_AllYears();

                            // Stop
                            MyWSim.Simulation_Stop();
                            // Gather up Output
                            StartYear = MyWSim.ParamManager.Model_Parameter(eModelParam.epSimulation_Start_Year).Value;
                            EndYear = MyWSim.ParamManager.Model_Parameter(eModelParam.epSimulation_End_Year).Value;
                            // get the results
                            SimulationResults SR = MyWSim.SimulationRunResults;
                            // OK we are now Idle
                            Application[WaterSimStatus] = WSIDLE;
                            // OK get the results

                            isGetError = GetModelResults2(OutputFields, SR, MyWSim.ParamManager, ref JSON_Results_Array, ref GetErrMessage);
                        } // try

                        // Yikes this is bad!  the API or the model threw and exception
                        catch (Exception ex)
                        {
                            isHardError = true;
                            HardErrMessage += ex.Message;
                            Application[WaterSimStatus] = WSIDLE;
                        }
                } // Wsim Null
                else
                {
                    isFetchError = true;
                }
 
            }  // ok unlock this now
            isError = (isFetchError || isSetError || isGetError || isHardError);
            string StatusStr = "";
            if (isError)
            {
                StatusStr = "  FETCH:(" + FetchErrMessage + ")   FIELD SET:(" + SetErrMessage + ")   FIELD GET:(" + GetErrMessage + ")   MODEL:(" + HardErrMessage+")";
            }
            else
            {
                StatusStr = "Model "+FetchErrMessage;
            }
            JSON_Object JO_Status = BuildStatusJSON_Object(isError, StatusStr, StartYear, EndYear, ScenarioName);
            JSON_Object ModelRunResults = new JSON_Object();
            ModelRunResults.Add(new JSON_NameValuePair("MODELSTATUS",JO_Status));
            ModelRunResults.Add(new JSON_NameValuePair("RESULTS",JSON_Results_Array));
            return ModelRunResults.Serialize();
            
        } // Run Model
        #endregion
        
    // WEbservice class
  /// /******************************************
///         *  Run WaterSim Web Service
///         *  Ver 2
///         *  ThreadSafe
///         *  RunWaterSim(string inputJsonArray, string outputJsonArray)
///         *  inputJsonArray Example "{"Inputs":[{"FLD":"STOPYR","VAL":2065},{"FLD":"COCLMADJ,"VAL":80},{"FLD":"PCGRTON","VAL":10,"PVC":"ph"}] }"
///         *  outputJsonArray Example "{"Outputs":["COCLMADJ","CORFLOW","SVRFLOW","MGWPUMP","POPUSED","TOTDEM"],"Providers":["ph","sc"]}"
///         *  return JSON ModelResults
///         * ****************************************/
        #region
///        
   public class ModelResults {
      string ModelStatus;
      //string Results;

      public ModelResults()
      {
      }

      public ModelResults(string theStatus, string theResults)
      {
          ModelStatus = theStatus;
          //Results = theResults;
      }

  }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ModelResults RunWaterSim_Json(string inputJsonArray, string outputJsonArray)
        {
            ModelResults MR = new ModelResults("Status Test", "Results Test");


            //            ModelResults MR = new ModelResults(JO_StatusStr,JSON_Results_ArrayStr);
            return MR;

//            //Setup Status fields
//            int StartYear = 0;
//            int EndYear = 0;
//            string ScenarioName = "";
//            bool isError = false;
//            string ErrMessage = "";
//            string ResultMessage = "";
//            // Setup Return Values
//            JSON_Array JSON_Results_Array = new JSON_Array();
            
//            // OK, need to lock this down to make thread safe
//            lock (RunModelThreadLock)
//                {
//                    // get WSIM object
//                WaterSimManager_SIO WSim = GetWaterSimManager(ref ErrMessage);
//                if (WSim != null)
//                {
//                    // put JSON input data in JSONObjects
//                    JSON_Object InputFields = new JSON_Object(inputJsonArray);
//                    JSON_Object OutputFields = new JSON_Object(outputJsonArray);
                   
//                    // checkif Status field exists if not create 
//                    if (Application[WaterSimStatus] == null)
//                        Application[WaterSimStatus] = WSIDLE;

//                    // check if Busy  should insert some code to wait until not busy
//                    if (Application[WaterSimStatus].ToString() != WSBUSY)

//                        try
//                        {   // we are busy
//                            Application[WaterSimStatus] = WSBUSY;
//                            // get the WaterSimMnager object
                            
//                            WaterSimManager_SIO MyWSim = GetWaterSimManager(ref ResultMessage);
//                            // Initialation Simulation
//                            MyWSim.Simulation_Initialize();
//                            // Set the Parameters based on recieved data
//                            isError = SetModelParameters(InputFields, MyWSim, ref ErrMessage);
//                            // Run all years, using defaults for values not set
//                            MyWSim.Simulation_AllYears();
                            
//                            // Stop
//                            MyWSim.Simulation_Stop();
//                            // Gather up Output
//                            StartYear = MyWSim.ParamManager.Model_Parameter(eModelParam.epSimulation_Start_Year).Value;
//                            EndYear = MyWSim.ParamManager.Model_Parameter(eModelParam.epSimulation_End_Year).Value;
//                            // get the results
//                            SimulationResults SR = MyWSim.SimulationRunResults;
//                            // OK we are now Idle
//                            Application[WaterSimStatus] = WSIDLE;
//                            // OK get the results
                            
//                            isError = GetModelResults(OutputFields, SR, MyWSim.ParamManager, ref JSON_Results_Array, ref ErrMessage);
                            
//                        } // try


//                        catch (Exception ex)
//                        {
//                            isError = true;
//                            ErrMessage += ex.Message;
//                            Application[WaterSimStatus] = WSIDLE;
//                        }
//                } // Wsim Null
//            }  // ok unlock this now
//            string StatusStr = ResultMessage;
//            if (isError) StatusStr += " - " + ErrMessage;
//            JSON_Object JO_Status = BuildStatusJSON_Object(isError, StatusStr, StartYear, EndYear, ScenarioName);
//            string JO_StatusStr = '"' + JO_Status.Serialize()+'"';
//            string JSON_Results_ArrayStr = '"'+JSON_Results_Array.Serialize()+'"';
//           // ModelResults MR = new ModelResults("Status Test","Results Test");


////            ModelResults MR = new ModelResults(JO_StatusStr,JSON_Results_ArrayStr);
//        //    return MR;
//            //JSON_Object ModelRunResults = new JSON_Object();


//            //ModelRunResults.Add(new JSON_NameValuePair("MODELSTATUS",JO_Status));
//            //ModelRunResults.Add(new JSON_NameValuePair("RESULTS",JSON_Results_Array));
//            //return ModelRunResults.Serialize();
            
        } // Run Model
        #endregion
        
    }// WEbservice class
    //=========================================================================
    public class WaterSimWebServiceException : Exception
    {
        public WaterSimWebServiceException(string Message)
            : base(Message)
        {
        }
    }

}