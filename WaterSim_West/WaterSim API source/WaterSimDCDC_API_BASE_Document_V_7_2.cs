// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 7.2

//       A Class the adds Doxumentation support to the WaterSimDCDC.WaterSim Class

//       WaterSimDCDC_API_Documentation Version 7.2
//       Keeper Ray Quay  ray.quay@asu.edu
//       Copyright (C) 2011,2012 , The Arizona Board of Regents
//              on behalf of Arizona State University

//       All rights reserved.

//       Developed by the Decision Center for a Desert City
//       Lead Model Development - David A. Sampson <david.a.sampson@asu.edu>

//       This program is free software: you can redistribute it and/or modify
//       it under the terms of the GNU General Public License version 3 as published by
//       the Free Software Foundation.

//       This program is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU General Public License for more details.

//       You should have received a copy of the GNU General Public License
//       along with this program.  If not, please see <http://www.gnu.org/licenses/>.
//       2
//       Changes
//       12/3/14 ver 7.
//       Load External Documentation File Support Added
//       Reads an external file, with the name ExternalDocumentation.txt with information to change the extended documentation.  ALlows Customized extended documentation with out rerolling the code.
//       External File has lines as follows   
//       Each Line has a series of objects {NAME:VALUE}
//       All Values are text    
//       {FIELDNAME: "AA"} {DESCRIP:"AA"} {UNIT:"AA"} {LONGUNIT:"AA"} {WEBLABEL:"AA"} {WEBSCALE:["AA","AA",,,,"AA"]} {WEBSCALEVALUES:["AA","AA",,,,"AA"]}
//       First Item {FIELDNAME: "AA"} is mandatory, all other items are optional, FIELDNAME Identifies the parameter that will be changed.
//       Only changes the items that are included.  ie, of WEBLABEL is the only object, changes the WebLabel extended documenetation to this value and leaves all other 
//       extended documentation fields as they are.  NOTE a {NAME:""} will clear the extended documentation with a "" value. 
//
//====================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WaterSimDCDC.Documentation
{
    /// <summary>   Water Parameter description item. </summary>
    public class WaterSimDescripItem
    {
        /// <summary> The model parameter code. </summary>
        protected int FModelParam;
        /// <summary> The description. </summary>
        protected string FDescription;
        /// <summary> The units. </summary>
        protected string FUnits;
        /// <summary> The Long Form units. </summary>
        protected string FLongUnits;
        /// <summary>
        /// A Lable to use with web unterface inputs
        /// </summary>
        protected string FWebLabel;

        protected string[] FWebScale = new string[0];

        protected int[] FWebScaleValues = new int[0];

        /// <summary>  The Topic groups this parameter belongs to /// </summary>
        protected List<ModelParameterGroupClass> FTopicGroupList = new List<ModelParameterGroupClass>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aModelParam">  The model parameter. </param>
        /// <param name="aDescrip">     The descrip. </param>
        /// <param name="aUnit">        The unit. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimDescripItem(int aModelParam, string aDescrip, string aUnit)
        {
            FModelParam = aModelParam;
            FDescription = aDescrip;
            FUnits = aUnit;
            FLongUnits = aUnit;
            FWebLabel = "";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aModelParam">          The model parameter. </param>
        /// <param name="aDescrip">             The descrip. </param>
        /// <param name="aUnit">                The unit. </param>
        /// <param name="aLongUnit">            The long unit. </param>
        /// <param name="aWebLabel">            The web label. </param>
        /// <param name="theWebScale">          the web scale. </param>
        /// <param name="theWebScaleValues">    the web scale values. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimDescripItem(int aModelParam, string aDescrip, string aUnit, string aLongUnit, string aWebLabel, 
            string[]  theWebScale, int[] theWebScaleValues)
        {
            FModelParam = aModelParam;
            FDescription = aDescrip;
            FUnits = aUnit;
            FLongUnits = aLongUnit;
            FWebLabel = aWebLabel;
            FWebScale = theWebScale;
            FWebScaleValues = theWebScaleValues;
        }

        internal void AddIfNotFound(ModelParameterGroupClass MPG)
        {
            double SID = MPG.ID;
            ModelParameterGroupClass FoundMPG = FTopicGroupList.Find(delegate(ModelParameterGroupClass Item) { return (Item.ID == SID); });
            if (FoundMPG == null)
            {
                FTopicGroupList.Add(MPG);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aModelParam">          The model parameter. </param>
        /// <param name="aDescrip">             The descrip. </param>
        /// <param name="aUnit">                The unit. </param>
        /// <param name="aLongUnit">            The long unit. </param>
        /// <param name="aWebLabel">            The web label. </param>
        /// <param name="theWebScale">          the web scale. </param>
        /// <param name="theWebScaleValues">    the web scale values. </param>
        /// <param name="theTopicGroups">    Model Parameter TopicGroups the parameter belongs to. </param>
        ///-------------------------------------------------------------------------------------------------
        
        public WaterSimDescripItem(int aModelParam, string aDescrip, string aUnit, string aLongUnit, string aWebLabel,
            string[] theWebScale, int[] theWebScaleValues, ModelParameterGroupClass[] theTopicGroups)
        {
            FModelParam = aModelParam;
            FDescription = aDescrip;
            FUnits = aUnit;
            FLongUnits = aLongUnit;
            FWebLabel = aWebLabel;
            FWebScale = theWebScale;
            FWebScaleValues = theWebScaleValues;
            foreach (ModelParameterGroupClass MPG in theTopicGroups)
            {
                if (MPG != null)
                {
                    FTopicGroupList.Add(MPG);
                    MPG.Add(FModelParam);  // will add if not already in list;
                }
                else
                {
                    string test = aDescrip;
                   // what the
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the model parameter. </summary>
        ///
        /// <value> The model parameter. </value>
        ///-------------------------------------------------------------------------------------------------

        public int ModelParam
        {
            get
            {
                return FModelParam;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the description. </summary>
        /// <value> The description. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Description
        {
            get
            {
                if (FDescription == null)
                    return "";
                else
                    return FDescription;
            }
            set
            {
                if (value != "")
                {
                    FDescription = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the units </summary>
        /// <value> The unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Unit
        {
            get
            {
                if (FUnits == null)
                    return "";
                else
                    return FUnits;
            }
            set
            {
                if (value != "")
                {
                    FUnits = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the long unit. </summary>
        ///
        /// <value> The long unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public string LongUnit
        {
            get
            {
                if (FLongUnits == null)
                    return "";
                else
                    return FLongUnits;

            }
            set
            {
                if (value != "")
                {
                    FLongUnits = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the web scale. </summary>
        ///
        /// <value> The web scale. </value>
        ///-------------------------------------------------------------------------------------------------

        public string[] WebScale
        {
            get { return FWebScale; }
            set
            {
                if ((value != null) && (value.Length != 0))
                {
                    FWebScale = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the web scale value. </summary>
        ///
        /// <value> The web scale value. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] WebScaleValue
        {
            get { return FWebScaleValues; }
            set
            {
                if ((value != null) && (value.Length != 0))
                {
                    FWebScaleValues = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the web label. </summary>
        ///
        /// <value> The web label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string WebLabel
        {
            get { return FWebLabel; }
            set
            {
                if (value != "")
                {
                    FWebLabel = value;
                }
            }
        }
        /// <summary>
        /// The List of Topic Groups this belongs to
        /// </summary>
        public List<ModelParameterGroupClass> TopicGroups
        {
            get { return FTopicGroupList; }
        }

    }


    /// <summary>   WaterSim Parameter and Data Documentation Support Class </summary>
    /// <remarks> This class enhances the amount of information about a parameter that is available from the Parameter Manager
    ///           Includes an extended descriptiopn of the parameter and the units of the parameter</remarks>
    public class Extended_Parameter_Documentation
    {
        List<WaterSimDescripItem> MPDesc = new List<WaterSimDescripItem>();

        ParameterManagerClass FPM;

        string FLoadDocError = "";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="PM">   The Current ParameterManager </param>
        ///-------------------------------------------------------------------------------------------------

        public Extended_Parameter_Documentation(ParameterManagerClass PM)
        {
            // Assign the Parameter Manager
            FPM = PM;
            // Build the various Groups
            //BuildGroups();
            //// Build the extended Documentation Internal
            //BuildDescripList();

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds Item.. </summary>
        ///
        /// <param name="Item"> The WaterSimDescripItem to add. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Add(WaterSimDescripItem Item)
        {
            if (Item != null)
            {
                MPDesc.Add(Item);
                int MPCode = Item.ModelParam;
                ModelParameterBaseClass MP = FPM.Model_Parameter(MPCode);
                if (MP != null)
                {
#if ExtendedParameter
                    MP.Description = Item.Description;
                    MP.FLongDescrip = Item.Description;
                    MP.FlongUnits = Item.LongUnit;
                    MP.Funits = Item.Unit;
                    MP.FWebLabel = Item.WebLabel;
                    MP.FwebScale = Item.WebScale;
                    MP.FWebScaleValues = Item.WebScaleValue;
#endif
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the external documentation error. </summary>
        ///
        /// <value> The external documentation error. </value>
        ///-------------------------------------------------------------------------------------------------

        public string ExternalDocumentationError
        {
            get { return FLoadDocError; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Model Parameter Description </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///
        /// <returns> string </returns>
        ///-------------------------------------------------------------------------------------------------

        public string Description(int modelparam)
        {
            return FindDescription(modelparam);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Units of Model Parameter </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string Unit(int modelparam)
        {
            return FindUnit(modelparam);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Units of Model Parameter as Long Description. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string LongUnit(int modelparam)
        {
            return FindLongUnit(modelparam);
        }

        /// <summary>
        /// A Label to use with a Web Interface, shorter than regular label
        /// </summary>
        /// <remarks> If this is not available, returns an "" empty string</remarks>
        /// <param name="modelparam"></param>
        /// <returns></returns>
        public string WebLabel(int modelparam)
        {
            return FindWebLabel(modelparam);
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Topic groups. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<ModelParameterGroupClass> TopicGroups(int modelparam)
        {
            return FindGroup(modelparam);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Web scale. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///<remarks>returns null if modelparam not found</remarks>
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public string[] WebScale(int modelparam)
        {
            return FindWebScale(modelparam);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Web scale value. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///<remarks>returns null if modelparam not found</remarks>
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] WebScaleValue(int modelparam)
        {
            return FindWebScaleValue(modelparam);
        }


        internal WaterSimDescripItem FindDescripDoc(int modelparam)
        {
            return MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
        }

        // Fielname for external documentation
        const string ExternalFileName = "App_data\\DocumentationItems.txt";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Loads external documentation. </summary>
        ///
        /// <param name="DataPath"> Full pathname of the data file. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool LoadExternalDocumentation(string DataPath)
        {
            // set up for failure!
            bool result = false;
            // build path
            string ExternalFileNamePath = DataPath +  ExternalFileName;
            // OK, going out 
            try
            {
                // get all the lines in the file
                string[] AllLines = System.IO.File.ReadAllLines(ExternalFileNamePath);
                // Setup for success now
                result = true;
                bool isChanged = false;
                // cycle through and execute Documentation command
                foreach (string linestr in AllLines)
                {
                    // process command
                    if (ProcessExternalDocItem(linestr))
                    {
                        isChanged = true;
                    }
                }

                // all done processing
            }
            // opps some heavy error here!
            catch (Exception e)
            {
                FLoadDocError = e.Message;
                result = false;
            }
            // All done, head back
            return result;
        }

        //----------------------------------------------------------------
        internal string ParseFirstPair(string PairList, ref string NextPairs)
        {
            string APair = "";
            if (PairList != "")
            {
                int start = PairList.IndexOf("{");
                if (start > -1)
                {
                    int end = PairList.IndexOf("}", start + 1);
                    if (end>-1)
                    {
                        APair = PairList.Substring(start+1,(end-start)-1);
                        NextPairs = PairList.Substring(end + 1).Trim();
                    }
                }
            }
            return APair;
        }
        //---------------------------------------------------------
        internal string TrimQuotes(string source)
        {
            string temp = "";
            int q1 = source.IndexOf("\"");
            if (q1 > -1)
            {
                int q2 = source.IndexOf("\"", q1 + 1);
                if (q2 > q1)
                {
                    temp = source.Substring(q1 + 1, (q2 - q1) - 1);
                }
            }
            else
            {
                // no quotes
                temp = source;
            }
            return temp;
        }
        //---------------------------------------------------------
        internal bool ParsePair(string aPair, ref string Name, ref string Value)
        {
            bool result = false;
            // find the colon
            int colindex = aPair.IndexOf(":");
            if (colindex > 0)
            {
                // if there, everything before os the name (TRIMMED)
                Name = aPair.Substring(0, colindex).Trim();
                // find value markers
                int qindex = aPair.IndexOf("\"");
                int bindex = aPair.IndexOf("[");
                // ok, is this an array or a single value
                if (bindex > colindex)
                {
                    //if ((bindex > colindex) && (bindex < qindex))
                    //{
                        // assume this is an array values
                        int arraylen = aPair.Length - (bindex + 2);
                        Value = aPair.Substring(bindex+1, arraylen);

                    //}
                }
                else
                {   // suume this is a single value
                    if ((qindex > colindex) && (qindex < aPair.Length - 1))
                    {
                        int valuelen = aPair.Length - (qindex + 2);
                        Value = aPair.Substring(qindex + 1, valuelen);
                    }
                    else
                    {
                        Value = "";
                    }
                }
                if (Value.Length > 0)
                    result = true;
            }

            return result;
        }
        //----------------------------------------------------------------
        internal string[] ExtractArray(string aPairValue)
        {
            List<string> aElements = new List<string>();
            int start = 0;
            int comindex = aPairValue.IndexOf(",");
            while (comindex != -1)
            {
                string tempsub = aPairValue.Substring(start, comindex - start);
                string temp = TrimQuotes(tempsub);
                aElements.Add(temp);
                start = comindex + 1;
                comindex = aPairValue.IndexOf(",", start);
            }
            if (start < aPairValue.Length)
            {
                string temp = TrimQuotes(aPairValue.Substring(start));
                aElements.Add(temp);
            }
            string[] ScaleArray = new string[aElements.Count];
            for (int i = 0; i < aElements.Count; i++)
            {
                ScaleArray[i] = aElements[i];
            }
            return ScaleArray;
        }
        //----------------------------------------------------------------
        internal int[] ConvertToIntArray(string[] StringArray)
        {
            int[] values = new int[StringArray.Length];
            for(int i=0;i<StringArray.Length;i++)
            {
                int temp = 0;
                if (int.TryParse(StringArray[i], out temp))
                {
                    values[i] = temp;
                }
                else
                {
                    values[i] = int.MinValue;
                }
            }
            return values;
        }
        // 
        internal bool ProcessExternalDocItem(string itemText)
        {
            bool result = false;
            if ((itemText != "") && (itemText[0] != '/'))
            {
                List<string> Items = new List<string>();
                // get all the pairs
                string temp = itemText;
                try
                {
                    int wcnt = 0;
                    int maxItems = 15;
                    while (!string.IsNullOrEmpty(temp))
                    {
                        string item = ParseFirstPair(temp, ref temp);
                        if (!string.IsNullOrEmpty(item))
                        {
                            Items.Add(item);
                        }
                        wcnt++;
                        if (wcnt > maxItems)
                        {
                            throw new Exception(" Error (looping) Parsing " + itemText);
                        }

                    }
                    string aPairName = "";
                    string aPairValue = "";
                    // get first pair
                    if (ParsePair(Items[0], ref aPairName, ref aPairValue))
                    {
                        //if fieldname then proceed otherwise do nothing
                        if (aPairName.ToUpper() == "FIELDNAME")
                        {
                            // ok fetch the fieldname and get parameter
                            string fieldname = aPairValue;
                            // set up
                            ModelParameterClass MP = null;
                            // throws exception if bad so wrap in try
                            try
                            {
                                MP = FPM.Model_Parameter(fieldname);
                            }
                            catch
                            {
                                MP = null;
                            }

                            // ok check if found
                            if (MP != null)
                            {
                                // get the descriptitem
                                WaterSimDescripItem DI = FindDescripItem(MP.ModelParam);
                                // if found continue
                                if (DI != null)
                                {
                                    // loop threw all the rest
                                    for (int i = 1; i < Items.Count; i++)
                                    {
                                        // parse it
                                        bool test = ParsePair(Items[i], ref aPairName, ref aPairValue);
                                        // if parse ok , then take action on Name
                                        if (test)
                                        {
                                            switch (aPairName.ToUpper())
                                            {
                                                case "DESCRIP":
                                                    DI.Description = aPairValue;
                                                    result = true;
                                                    break;
                                                case "UNIT":
                                                    DI.Unit = aPairValue;
                                                    result = true;
                                                    break;
                                                case "LONGUNIT":
                                                    DI.LongUnit = aPairValue;
                                                    result = true;
                                                    break;
                                                case "WEBLABEL":
                                                    DI.WebLabel = aPairValue;
                                                    result = true;
                                                    break;
                                                case "WEBSCALE":

                                                    //  this comes as an array {"AAA","AAA",..."AAA"}
                                                    // ok now parse the array
                                                    DI.WebScale = ExtractArray(aPairValue);
                                                    result = true;
                                                    break;
                                                case "WEBSCALEVALUES":
                                                    // this comes as an array {"NN","NN",..."NN"}
                                                    DI.WebScaleValue = ConvertToIntArray(ExtractArray(aPairValue));
                                                    result = true;
                                                    break;

                                            } // switch aPairName
                                        } // if test
                                    } // for i items.count
                                    if (result)
                                    {
#if ExtendedParameter
                                        MP.setupExtended();
#endif
                                    }
                                } // if DI
                            } //if MP

                        } // if Fieldname
                    } // ParsePair
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Parsing External Documentation: " + ex.Message);
                }
            }    // check if null or '/'
            return result;
        }

        internal WaterSimDescripItem FindDescripItem(int modelparam)
        {
            return MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
        }

        /// <summary>
        /// Fetches the description from the documentation for this model param
        /// </summary>
        /// <param name="modelparam"></param>
        /// <returns></returns>
        internal string FindDescription(int modelparam)
        {
            string temp = "";

//            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.Description;

            return temp;
        }
        /// <summary>
        /// Fetches the short unit description from the documentstion
        /// </summary>
        /// <param name="modelparam"></param>
        /// <returns></returns>
        internal string FindUnit(int modelparam)
        {
            string temp = "";

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.Unit;
            return temp;
        }


        /// <summary>
        /// Fethces the long units description from the documentation
        /// </summary>
        /// <param name="modelparam"></param>
        /// <returns></returns>
        internal string FindLongUnit(int modelparam)
        {
            string temp = "";

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.LongUnit;
            return temp;
        }

        /// <summary>
        /// Fethces the Web label from documention for this modelparam
        /// </summary>
        /// <param name="modelparam"></param>
        /// <returns></returns>
        internal string FindWebLabel(int modelparam)
        {
            string temp = "";

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.WebLabel;
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first web scale. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///<remarks>Returns Null if ModelParam not found</remarks>
        /// <returns>   The found web scale. </returns>
        ///-------------------------------------------------------------------------------------------------

        internal string[] FindWebScale(int modelparam)
        {
            string[] temp;

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.WebScale;
            else
                temp = null;
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the first web scale value. </summary>
        ///
        /// <param name="modelparam">   The modelparam. </param>
        ///<remarks>Returns Null if ModelParam not found</remarks>
        /// <returns>   The found web scale value. </returns>
        ///-------------------------------------------------------------------------------------------------

        internal int[] FindWebScaleValue(int modelparam)
        {
            int[] temp;

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
                temp = DI.WebScaleValue;
            else
                temp = null;
            return temp;
        }

        internal List<ModelParameterGroupClass> FindGroup(int modelparam)
        {

            //            WaterSimDescripItem DI = MPDesc.Find(delegate(WaterSimDescripItem Item) { return (Item.ModelParam == modelparam); });
            WaterSimDescripItem DI = FindDescripItem(modelparam);
            if (DI != null)
            {
                return DI.TopicGroups;
            }
            else
            {
                return new List<ModelParameterGroupClass>();
            }
        }

    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Model documentation. </summary>
    ///<Remark>This is the class used to create documentation for Model,Parameters, and Processes</Remark>
    ///-------------------------------------------------------------------------------------------------

    public static class ModelDocumentation
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Model parameter spatial string. </summary>
        ///
        /// <param name="mpt">  The mpt. </param>
        ///
        /// <returns>   String f. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ModelParamSpatialString(modelParamtype mpt)
        {
            string value = "";
            switch (mpt)
            {
                case modelParamtype.mptInput2DGrid:
                case modelParamtype.mptInput3DGrid:
                case modelParamtype.mptInputBase:
                    value = "Region";
                    break;
                case modelParamtype.mptInputOther:
                    value = "Other";
                    break;
                case modelParamtype.mptInputProvider:
                    value = "Utility/City";
                    break;
                case modelParamtype.mptOutput2DGrid:
                case modelParamtype.mptOutput3DGrid:
                case modelParamtype.mptOutputBase:
                    value = "Region";
                    break;
                case modelParamtype.mptOutputOther:
                    value = "Other";
                    break;
                case modelParamtype.mptOutputProvider:
                    value = "Utility/City";
                    break;
                case modelParamtype.mptUnknown:
                    value = "unknown";
                    break;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Model parameter type string. </summary>
        ///
        /// <param name="mpt">  The mpt. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ModelParamTypeString(modelParamtype mpt)
        {
            string value = "";
            switch (mpt)
            {
                case modelParamtype.mptInput2DGrid:
                    value = "Input 2D Grid";
                    break;
                case modelParamtype.mptInput3DGrid:
                    value = "Input 3D Grid";
                    break;
                case modelParamtype.mptInputBase:
                    value = "Input Base";
                    break;
                case modelParamtype.mptInputOther:
                    value = "Input Other";
                    break;
                case modelParamtype.mptInputProvider:
                    value = "Input Provider";
                    break;
                case modelParamtype.mptOutput2DGrid:
                    value = "Output 2D Grid";
                    break;
                case modelParamtype.mptOutput3DGrid:
                    value = "Output 3D Grid";
                    break;
                case modelParamtype.mptOutputBase:
                    value = "Output Base";
                    break;
                case modelParamtype.mptOutputOther:
                    value = "Output Other";
                    break;
                case modelParamtype.mptOutputProvider:
                    value = "Output Provider";
                    break;
                case modelParamtype.mptUnknown:
                    value = "unknown";
                    break;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Model parameter input output string. </summary>
        ///
        /// <param name="mpt">  The mpt. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ModelParamInputOutputString(modelParamtype mpt)
        {
            string value = "";
            switch (mpt)
            {
                case modelParamtype.mptInput2DGrid:
                case modelParamtype.mptInput3DGrid:
                case modelParamtype.mptInputBase:
                case modelParamtype.mptInputOther:
                case modelParamtype.mptInputProvider:
                    value = "Input";
                    break;
                case modelParamtype.mptOutput2DGrid:
                case modelParamtype.mptOutput3DGrid:
                case modelParamtype.mptOutputBase:
                case modelParamtype.mptOutputOther:
                case modelParamtype.mptOutputProvider:
                    value = "Output";
                    break;
                case modelParamtype.mptUnknown:
                    value = "unknown";
                    break;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Create a Model Document Tree. </summary>
        /// <remarks> Creates a DocTreeNode Tree populated with Model information using WSim, tree includes parameters and processes</remarks>
        /// <param name="WSim"> The WaterSimManager node is based on </param>
        /// <returns> a DocTreeNode with model info </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DocTreeNode ModelTree(WaterSimManager WSim)
        {
            DocTreeNode ModelTree = new DocTreeNode("WATERSIM", "");
            DocTreeNode APIVersionNode = new DocTreeNode("APIVERSION", WSim.API_Version);// WSim.APiVersion);
            ModelTree.AddChild(APIVersionNode);
            DocTreeNode ModelVersionNode = new DocTreeNode("MODELVERSION",WSim.Model_Version);// WSim.ModelBuild);
            ModelTree.AddChild(ModelVersionNode);
 
            DocTreeNode temp = ParameterTree(WSim.ParamManager, "");
            ModelTree.AddChild(temp);
            temp = ProcessTree(WSim.ProcessManager);
            ModelTree.AddChild(temp);

            return ModelTree;
        }

        ///---------------------------------------------------------------*----------------------------------
        /// <summary>   Create a Parameter Document tree. </summary>
        /// <remarks> Creates a DocTreeNode Tree populated with just Parameter information using the ParmManager</remarks>
        /// <param name="ParmManager">  The ParameterManagerClass used to create Node. </param>
        /// <param name="Label">        The label for the node. </param>
        /// <returns>  a DocTreeNode with parameters . </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DocTreeNode ParameterTree(ParameterManagerClass ParmManager, string Label)
        {
            DocTreeNode ParmTree = new DocTreeNode("PARAMETERS", Label);
            foreach (ModelParameterClass MP in ParmManager.BaseInputs())
                ParmTree.AddChild(MP);
            foreach (ModelParameterClass MP in ParmManager.ProviderInputs())
                ParmTree.AddChild(MP);
            return ParmTree;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Create Process Document Tree. </summary>
        /// <remarks> Creates a DocTreeNode Tree populated with just Process information using the ProcManager</remarks>
        /// <param name="ProcManager">  ProcessManager used to create Tree. </param>
        /// <returns> DocTreeNode with Processes. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static DocTreeNode ProcessTree(ProcessManager ProcManager)
        {
            DocTreeNode ProcTree = new DocTreeNode("PROCESSES", "");
            foreach (AnnualFeedbackProcess AFP in ProcManager.AllProcesses())
                ProcTree.AddChild(AFP);
            return ProcTree;
        }
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Document tree node. </summary>
    ///<remarks>Tag must have a value and cannot be equal to ""</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class DocTreeNode
    {
        string FTag = "";
        string FCode = "";
        string FFieldname = "";
        string FValue = "";
        string FLabel = "";
        static int FCount = 0;
        List<DocTreeNode> FChildren = new List<DocTreeNode>();

        DocTreeNode FParent = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks>Tag is set to "TAGN" where N is an internal class counter</remarks>
        ///-------------------------------------------------------------------------------------------------

        public DocTreeNode()
        {
            Tag = "";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks>If aTag is null or "", the the tag is set to "TAGN" where N is an internal class counter</remarks>
        /// <param name="aTag">   The tag. </param>
        /// <param name="aValue"> The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public DocTreeNode(string aTag, string aValue)
        {
            Tag = aTag;

            FValue = aValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks>If aTag is null or "", the the tag is set to "TAGN" where N is an internal class counter</remarks>
        /// <param name="aTag">   The tag. </param>
        /// <param name="aValue"> The value. </param>
        /// <param name="aLabel"> The label. </param>
        ///-------------------------------------------------------------------------------------------------

        public DocTreeNode(string aTag, string aValue, string aLabel, string aCode, string aFieldname)
        {
            FTag = aTag;
            FValue = aValue;
            FLabel = aLabel;
            FCode = aCode;
            FFieldname = aFieldname;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the tag. </summary>
        /// <remarks>If value is null or "", the the tag is set to "TAGN" where N is an internal class counter</remarks>
        /// <value> The tag. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Tag
        { get {return FTag; }
            set
            {
                if (value == "")
                {
                    FTag = "Tag" + FCount.ToString();
                    FCount++;
                }
                else
                {
                    FTag = value;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the value. </summary>
        ///
        /// <value> The value. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Value 
        { get {return FValue; } set {FValue = value;} }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the label. </summary>
        ///
        /// <value> The label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Label
        { get { return FLabel; } set { FLabel = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the code. </summary>
        ///
        /// <value> The code. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Code
        { get { return FCode; } set { FCode = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the fieldname. </summary>
        ///
        /// <value> The fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Fieldname
        { get { return FFieldname; } set { FFieldname = value; } }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the parent. </summary>
        ///
        /// <value> The parent. </value>
        ///-------------------------------------------------------------------------------------------------

        public DocTreeNode Parent
        { get { return FParent; } }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Indexer to get items within this collection using array index syntax. </summary>
       ///
       /// <param name="i"> Zero-based index of the entry to access. </param>
       ///
       /// <returns> The indexed item. </returns>
       ///-------------------------------------------------------------------------------------------------

       public DocTreeNode this[int i]
       {
           get { return FChildren[i]; }
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets the List of Child Nodes. </summary>
       ///
       /// <value> The children. </value>
       ///-------------------------------------------------------------------------------------------------

       public List<DocTreeNode> Children
       { get { return FChildren; } }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Adds a child. </summary>
       /// <param name="aNode"> The node. </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChild(DocTreeNode aNode)
       {
           aNode.FParent = this;
           FChildren.Add(aNode);
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Adds a child. </summary>
       /// <param name="aTag">   The tag. </param>
       /// <param name="aValue"> The value. </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChild(string aTag, string aValue)
       {
           DocTreeNode temp = new DocTreeNode(aTag, aValue);
           AddChild(temp);
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Adds a child. </summary>
       /// <param name="aTag">       The tag. </param>
       /// <param name="aValue">     The value. </param>
       /// <param name="aLabel">     The label. </param>
       /// <param name="aCode">      The code. </param>
       /// <param name="aFieldname"> The fieldname. </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChild(string aTag, string aValue, string aLabel, string aCode, string aFieldname)
       {
           DocTreeNode temp = new DocTreeNode(aTag, aValue, aLabel, aCode, aFieldname);
           AddChild(temp);
       }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets deocumentation in an XML format in a string. </summary>
        /// <param name="Level"> The level of indentention to start with. </param>
        ///
        /// <returns> The XML. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string GetXML(int Level)
       {
           string temp = "<" + FTag;
           temp.PadLeft(Level * 3, ' ');
            if (FLabel !="")
                temp += " label='"+FLabel+"' ";
            if (FCode!="")
                temp += "code='"+FCode+"' ";
            if (FFieldname!="")
                temp += "fieldname='"+FFieldname+"' ";

            temp += ">";
            temp += ' '+ FValue.PadLeft(3*Level,' ');
            
           if (FChildren.Count>0)
           {
               Level++;
               foreach(DocTreeNode Node in FChildren)
               {
                   temp += Node.GetXML(Level);
               }
           }
           string close = "</" + FTag + ">\r\n";
           temp += close.PadLeft(3 * Level, ' ');
           return temp;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets documentation as A TreeNode  object for a TreeView control. </summary>
       ///
       /// <returns> The tree node. </returns>
       ///-------------------------------------------------------------------------------------------------

       public TreeNode GetTreeNode()
       {
           TreeNode Temp = new TreeNode();
           Temp.Name = FTag;
           if (FLabel != "")
           {
               Temp.Text = FTag + ": " + FLabel;
           }
           else
           {
               Temp.Text = FTag;
           }
           if (FValue != "")
           {
               TreeNode valuenode = new TreeNode("Value = " + FValue);
               Temp.Nodes.Add(valuenode);
           }
           if (FCode!="")
               Temp.Text += "Code: "+FCode;
           if (FFieldname!="")
               Temp.Text += "Fld: "+FFieldname;
           if (FChildren.Count > 0)
           {
               foreach (DocTreeNode node in FChildren)
               {
                   Temp.Nodes.Add(node.GetTreeNode());
               }
           }
           return Temp;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Gets Documentation as a text string. </summary>
       /// <param name="Level"> The level of indentention to start with. </param>
       ///
       /// <returns> The string. </returns>
       ///-------------------------------------------------------------------------------------------------

       public string GetString(int Level)
       {
           string temp = " <" + FTag + ">  "+FLabel ;
           temp = temp.PadLeft(Level * 3, ' ');
           if (FValue!="")
               temp = temp + " Value: [" + FValue.PadLeft(20)+"]  ";
           if (FCode!="")
               temp += "  Code: "+FCode.PadLeft(10);
           if (FFieldname!="")
               temp += "  Fld: "+FFieldname;
           
           temp += "\r\n";

           if (FChildren.Count > 0)
           {
               Level++;
               foreach (DocTreeNode Node in FChildren)
               {
                   temp += Node.GetString(Level);
               }
           }
           return temp;
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Adds a child using ModelParameterClass object. </summary>
       ///
       /// <param name="MP"> The ModelParmeter. </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChild(ModelParameterClass MP)
       {
           string aValue = "";
           if (MP.isProviderParam)
           {
               ProviderIntArray PIA = MP.ProviderProperty.getvalues();
               aValue = PIA[0].ToString();
               for (int i = 1; i < PIA.Length; i++)
                   aValue += " , " + PIA[i].ToString();
           }
           else
           {
               aValue = MP.Value.ToString();
           }
           DocTreeNode Temp = new DocTreeNode("MODELPARAM", aValue, MP.Label, MP.ModelParam.ToString(), MP.Fieldname);
           AddChild(Temp);
       }
//       public DocTreeNode(string aTag, string aValue, string aLabel, string aCode, string aFieldname)

       ///-------------------------------------------------------------------------------------------------
       /// <summary> Adds a child using an AnnualFeedbackProcess object. </summary>
       ///
       /// <remarks> Ray, 1/27/2013. </remarks>
       ///
       /// <param name="AFP"> The AnnualFeedbackProcess object </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChild(AnnualFeedbackProcess AFP)
       {
           DocTreeNode Temp = AFP.Documentation();
           AddChild(Temp);
       }

       ///-------------------------------------------------------------------------------------------------
       /// <summary>    Adds a child field to The Documentation. </summary>
       /// <param name="fieldLabel">    The field label. </param>
       /// <param name="fieldvalue">    The fieldvalue. </param>
       ///-------------------------------------------------------------------------------------------------

       public void AddChildField(string fieldLabel, string fieldvalue)
       {
           DocTreeNode Temp = new DocTreeNode("FIELD", fieldvalue, fieldLabel, "", "");
           AddChild(Temp);
       }
    }
}
