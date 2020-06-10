//      WaterSimDCDC Regional Water Demand and Supply Model Version 5.0

//       This provides canned string to support WaterSimDCDC user interface,
//       Mostly for errors.

//       Version 4.0
//       Keeper Ray Quay ray.quay@asu.edu
//       
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
//       
//      
//
//====================================================================================using System;
using System.Collections.Generic;
using System.Text;


namespace WaterSimDCDC
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> WaterSimDCDC strings  </summary>
    ///
    /// <remarks> Standardized list of string values, mostly error messages </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class WS_Strings
    {

        ///<summary> String = "Invalid Data Directory Path" </summary> 
        public const int wsBadDataDirectory = 0;
        ///<summary> String = "Only one object of WaterSim can be instatiated at time! Dispose() must be called before a WaterSim object loses scope." </summary> 
        public const int wsOnlyOneObject = 1;
        ///<summary> String = "Simulation can not be locked until it has been initialized!" </summary> 
        public const int wsLockAfterIntialization = 2;
        ///<summary> String = "Invalid Provider." </summary> 
        public const int wsInvalidProvider = 3;
        ///<summary> String = "Intern range check error on unknown or special parameter." </summary> 
        public const int wspublicRangeCheck = 4;
        ///<summary> String = "Invalid eModelParam." </summary> 
        public const int wsInvalid_EModelPAram = 5;
        ///<summary> String = "Model Parameter must be a modelParamType.mptInputBase." </summary> 
        public const int wsModelParamMustBeInputBase = 6;
        ///<summary> String = "Model Parameter must be a modelParamType.mptInputProvider." </summary> 
        public const int wsModelParamMustBeInputProvider = 7;
        ///<summary> String = "Unable to open oleDbConnection." </summary> 
        public const int wsUableOpenDbConnection = 8;
        ///<summary> String = "DbConnection value can not be null." </summary> 
        public const int wsdbConNull = 9;
        ///<summary> String = "Method parameters can not be null." </summary>  
        public const int wsdbNullParameter = 10;
        ///<summary> String = "Internal Error - A Model Parameter is missing!" </summary> 
        public const int wsModelParameterMissing = 11;
        ///<summary> String = "GPCD can only be set if Demand Option is set to 4 (User Deand) first" </summary> 
        public const int wsSetGPCDError = 12;
        ///<summary> String = "Must be a base parameter to access this type of property." </summary> 
        public const int wsMustBeBaseParameter = 13;
        ///<summary> String = "Must be a provider parameter to access this type of property." </summary> 
        public const int wsMustBeProviderParameter = 14;
        ///<summary> String = "Process already in the list for processing " </summary> 
        public const int wsProcessInList = 15;
        ///<summary> String = "Not a valid WaterSim_DB DataTable" </summary> 
        public const int wsInvalidDataTable = 16;
        ///<summary> String = "Parameter is Read Only!" </summary> 
        public const int wsReadOnly = 17;
        ///<summary> String = "Parameter must be Base." </summary> 
        public const int wsMustBeBase = 18;
        ///<summary> String = "eModelParm does not match the eProviderBuildType"." </summary>      
        public const int wsInvalidBuildType = 19;

        /// <summary> The AnnualFeedbackProcess object is a null value </summary>
        public const int wsWSMangerNullError = 20;
        

        /// <summary> String = "Error loading population data file"</summary>
        public const int wsPopDataLoadError = 21;
        
        /// <summary> String = "Invalid index to population data"</summary>
        public const int wsInvalidPopIndex = 22;
        /// <summary> The ws database was not open. </summary>
        public const int wsDbConNotOpen = 23;

        /// <summary> String = "Duplicate eModelParam: a ModelParameter with this modelparam value is already in the Parameter List."</summary>
        public const int wsDuplicateModelParam = 24;

        ///<summary> String = "Model Parameter must be a modelParamType.mptInputProvider or modelParamType.mptOutputProvider." </summary>
        public const int wsModelParamMustBeProvider = 25;

        ///<summary> String = "Model Parameter must be a modelParamType.mptInputBase or modelParamType.mptOutputBase." </summary>
        public const int wsModelParamMustBeBase = 26;

        ///<summary> String = "Model Parameter must be a modelParamType.mptInputGrid2D or modelParamType.mptOutputGrid2d." </summary>
        public const int wsModelParamMustBeGrid = 27;

        ///  "Invalid Grid Index"
        public const int wsInvalidGridIndex = 28;
        ///  "Get Method not defined or Null",
        public const int wsGetIsNull = 29;
        ///  "Set Method not defined or Null"
        public const int wsSetIsNull = 30;
        ///<summary> String = "Must be a provider parameter to access this type of property." </summary> 
        public const int wsMustBeGridParameter = 31;

        public const int wsBadServer = 32;

        
        public const int wseProviderOutOfRange = 33;
        /// <summary> Number of strings available</summary>
        public const int NumberOfStrings = 34;

        /// <summary> Error Strings.  </summary>
        internal static string[] values = new string[NumberOfStrings] {
            "Invalid Data Directory Path",
            "Only one object of WaterSim can be instatiated at time! Dispose() must be called before a WaterSim object loses scope.",
            "Simulation can not be locked until it has been initialized!",
            "Invalid Provider.",
            "Internal range check error on unknown or special parameter.",
            "Invalid eModelParam.",
            "Model Parameter must be a modelParamType.mptInputBase.",
            "Model Parameter must be a modelParamType.mptInputProvider.",
            "Unable to open oleDbConnection.",
            "DbConnection value can not be null.",
            "Method parameters can not be null.",
            "Internal Error - A Model Parameter is missing!",
            "GPCD can only be set if Demand Option is set to 4 (User Deand) first",
            "Must be a base parameter to access this type of property.",
            "Must be a provider parameter to access this type of property.",
            "Process already in the list for processing ",
            "Not a valid WaterSim_DB DataTable",
            "Parameter is Read Only!",
            "Parameter must be Base.",
            "eModelParm does not match the eProviderBuildType",
            "The WaterSimManger object is a null value",
            "Error loading population data file",
            "Invalid index to population data",
            "The OleDbConnection must be open",
            "Duplicate eModelParam: a ModelParameter with this modelparam value is already in the Parameter List.",
            "Model Parameter must be a modelParamType.mptInputProvider or modelParamType.mptOutputProvider." ,
            "Model Parameter must be a modelParamType.mptInputBase or modelParamType.mptOutputBase." ,
            "Model Parameter must be a modelParamType.mptInputGrid2D or modelParamType.mptOutputGrid2d.",
            "Invalid Grid Index",
            "Get Method not defined or Null",
            "Set Method not defined or Null",
            "Must be a Grid parameter to access this type of property." ,
            "Unable to Open Server",
            "eProvider index out of range"

    };

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Test of the index is a Valid Error Stings Index. </summary>
        /// <param name="index"> Zero-based index of the array of strings. </param>
        ///
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool Valid(int index)
        {
            return ((index>0)&(index<NumberOfStrings));
        }
        //-----------------------------------------------------------------

        /// <summary>   Get a WaterSimDCDC string. </summary>
        ///
        /// <remarks>Returns a WaterSimDCDC for the index value   </remarks>
        ///
        /// <param name="index">    Zero-based index of the array of strings. </param>
        ///
        /// <returns>   . </returns>

        public static string Get(int index)
        {
            if (Valid(index)) return values[index];
            else return "";
        }
    }
}
