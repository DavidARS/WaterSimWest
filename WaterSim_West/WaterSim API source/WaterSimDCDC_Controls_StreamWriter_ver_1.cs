// ===========================================================
//     WaterSimDCDC Regional Water Demand and Supply Model Version 7.0

//       A Class the adds output support classes for WaterSimDCDC Controls

//       WaterSimDCDC.Controls_Graphic_Utils
//       Version 1.0
//       Keeper David A Sampson
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
//====================================================================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using WaterSimDCDC;


namespace WaterSimDCDC.Controls
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Fieldinfo. </summary>
    /// <remarks> A structure that holds information about a tabke column/field</remarks>
    ///-------------------------------------------------------------------------------------------------

    public struct Fieldinfo
    {

        string FFieldname;
        string FLabel;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="fieldname">  The fieldname. </param>
        /// <param name="fieldlabel"> The fieldlabel. </param>
        ///-------------------------------------------------------------------------------------------------

        public Fieldinfo(string fieldname, string fieldlabel)
        {
            FFieldname = fieldname;
            FLabel = fieldlabel;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the fieldname. </summary>
        /// <value> The fieldname. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Fieldname
        { get { return FFieldname; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the label. </summary>
        /// <value> The label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Label
        { get { return FLabel; } }

    }
    //------------------------------------

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Providerinfo. </summary>
    ///
    /// <remarks> a structure that hold information about a provider </remarks>
    ///-------------------------------------------------------------------------------------------------

    public struct providerInfo
        {
           string FPcode;
           string FLabel;

           ///-------------------------------------------------------------------------------------------------
           /// <summary> Constructor. </summary>
           /// <param name="pcode"> The pcode. </param>
           /// <param name="label"> The label. </param>
           ///-------------------------------------------------------------------------------------------------

           public providerInfo ( string pcode, string label)
           {
               FPcode = pcode;
               FLabel = label;
           }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the providercode. </summary>
        /// <value> The providercode. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Providercode
        { get {return FPcode;} }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the label. </summary>
        ///
        /// <value> The label. </value>
         ///-------------------------------------------------------------------------------------------------

        public string Label 
        { get {return FLabel;}}

        }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Chart points by year. </summary>
    ///
    /// <remarks> A structire that holds arrays of in data for a chart of data by year </remarks>
    ///-------------------------------------------------------------------------------------------------

    public struct chartPointsByYear
    {
        int[] FDataValues;
        int[] FYearValues;
        int[] FRawDataValues;
        string FLabel;
        
        //---------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <exception cref="Exception"> Thrown when the length of data and years arrays are not the same. </exception>
        /// <param name="years">  The years. </param>
        /// <param name="data">   The data. </param>
        /// <param name="aLabel"> The label for the data series. </param>
        ///-------------------------------------------------------------------------------------------------

        public chartPointsByYear(int[] years, int[] data, string aLabel)
        {
            FRawDataValues = data;
            FYearValues = years;
            FLabel = aLabel;
            if (data.Length != years.Length)
            { throw new Exception("Number of years does not equal number of data items"); }
            FDataValues = new int[data.Length];
            for (int i=0;i<data.Length;i++)  FDataValues[i] = FRawDataValues[i]; 
        }
        ////---------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the series label. </summary>
        /// <value> The series label. </value>
        ///-------------------------------------------------------------------------------------------------

        public string SeriesLabel
        { get { return FLabel;}}

        //---------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the years data array </summary>
        /// <value> The years. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] Years
        {
            get { return FYearValues; }
           // set { FYearValues = value; }
        }
        //---------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the data values array. </summary>
        /// <remarks>This data may have been altered to map multiple scales</remarks>
        /// <value> The values. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] Values
        {
            get { return FDataValues; }
          //  set { FDataValues = value; }
        }
        //---------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the data value array in its original form. </summary>
        ///
        /// <value> the data values. </value>
        ///-------------------------------------------------------------------------------------------------



    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A Class that moves data from a data table into a Chart object . </summary>
    ///-------------------------------------------------------------------------------------------------

    public class StreamManager
    {
        DateTime now = DateTime.Now;
        internal StreamWriter sw;
        internal StreamWriter sw1;
        internal StreamWriter sw2;
        internal StreamWriter sw3;

        string STitle;
        public StreamManager(string StreamTitle)
        {           
            STitle = StreamTitle;
        }

        //------------------------------------------------------
        public void BuildAnnualParameterStream(WaterSimManagerClass WSIM, SimulationResults SimRes, string fldname, string fieldlabel, bool start)
        {
            if (start) { StreamW(@"Outputs\\"); StreamW1(@"Outputs\\"); StreamW2(@"Outputs\\"); StreamW3(@"Outputs\\"); }
            //if (fldname == "POP_P") { StreamW(@"Outputs\\"); StreamW1(@"Outputs\\"); StreamW2(@"Outputs\\"); StreamW3(@"Outputs\\"); }
            BuildAnnualParameterStream(WSIM, SimRes, fldname, fieldlabel, null);
         
        }
        public void BuildAnnualParameterStream(WaterSimManagerClass WSIM, SimulationResults SimRes, string fldname, string fieldlabel, string[] Regions)
        {
            List<string> debug = new List<string>();
            int year = 0;
           
            int yearCount = SimRes.Length;     

            // run through once
            if (fldname == "AD_P")
            {
                int ProviderCount = SimRes[0].Outputs.ProviderOutput[0].Values.Length;
              
             
                for (int i = 0; i < 89; i++)
                {
                    // cities and towns demand
                    if (i == 18)
                    {
                        for (int j = 0; j < 36; j++)
                        {
                            year = j + 2015;
                            for (int k = 0; k < 24; k++)
                            {
                                string ProviderName = ProviderClass.ProviderNameList[k];
                                int[] R = SimRes[j].Outputs.ProviderOutput[i].Values;
                                sw.WriteLine(ProviderName + "," +  i + "," + j + "," + k + "," + year + "," + R[k]);

                            }
                        }
                    }
                 }
                sw.Flush();
                sw.Close();
                for (int i = 0; i < 89; i++)
                {
                    // industry
                    if (i == 22)
                    {
                        for (int j = 0; j < 36; j++)
                        {
                            year = j + 2015;
                              for (int k = 0; k < 24; k++)
                            {
                                string ProviderName = ProviderClass.ProviderNameList[k];
                                int[] R = SimRes[j].Outputs.ProviderOutput[i].Values;
                                sw1.WriteLine(ProviderName + "," + i + "," + j + "," + k + "," + year + "," + R[k]);

                            }
                        }
                    }
                }
                sw1.Flush();
                sw1.Close();
                for (int i = 0; i < 89; i++)
                {
                    // power
                    if (i == 26)
                    {
                        for (int j = 0; j < 36; j++)
                        {
                            year = j + 2015;
                            for (int k = 0; k < 24; k++)
                            {
                                string ProviderName = ProviderClass.ProviderNameList[k];
                                int[] R = SimRes[j].Outputs.ProviderOutput[i].Values;
                                sw2.WriteLine(ProviderName + "," + i + "," + j + "," + k + "," + year + "," + R[k]);

                            }
                        }
                    }
                }
                sw2.Flush();
                sw2.Close();
                for (int i = 0; i < 89; i++)
                {
                    // Agriculture
                    if (i == 20)
                    {
                        for (int j = 0; j < 36; j++)
                        {
                            year = j + 2015;
                            for (int k = 0; k < 24; k++)
                            {
                                string ProviderName = ProviderClass.ProviderNameList[k];
                                int[] R = SimRes[j].Outputs.ProviderOutput[i].Values;
                                sw3.WriteLine(ProviderName + "," + i + "," + j + "," + k + "," + year + "," + R[k]);

                            }
                        }
                    }
                }
                sw3.Flush();
                sw3.Close();


            }

        }

        public void StreamW(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Urban" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw = File.AppendText(filename);
        }

        public void StreamW1(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Industry" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw1 = File.AppendText(filename);
        }
        public void StreamW2(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Power" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw2 = File.AppendText(filename);
        }
        public void StreamW3(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Agriculture" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw3 = File.AppendText(filename);
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="AChart">     The chart. </param>
        /// <param name="ChartTitle"> The chart title. </param>
        ///-------------------------------------------------------------------------------------------------



        ///-------------------------------------------------------------------------------------------------
        /// <summary> Grab parameter provider data.</summary>
        ///
        /// <remarks> Quay, 3/1/2018.</remarks>
        /// 
        /// <param name="SimResults"> The simulation results.</param>
        /// <param name="ParmType">   Type of the parameter.</param>
        /// <param name="index">      Zero-based index of the.</param>
        ///
        /// <returns> An int[] if parmtype an index are correct, else returns a null</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] grabParmProviderData(AnnualSimulationResults SimResults, modelParamtype ParmType, int index)
        {
            // check if index is 0 or greater
            if (index >= 0)
            {
                // check if this is a provider parameter
                if ((ParmType == modelParamtype.mptInputProvider) || (ParmType == modelParamtype.mptOutputProvider))
                {
                    switch (ParmType)
                    {
                        case modelParamtype.mptInputProvider:
                                if (index < SimResults.Inputs.ProviderInput.Length)
                                {
                                    {
                                        return SimResults.Inputs.ProviderInput.Values[index].Values;
                                    }
                                }
                            break;
                        case modelParamtype.mptOutputProvider:
                            if (index < SimResults.Outputs.ProviderOutput.Length)
                            {
                                {
                                    return SimResults.Outputs.ProviderOutput.Values[index].Values;
                                }
                            }
                            break;
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Grab parameter base data.</summary>
        ///
        /// <remarks> Quay, 3/1/2018.</remarks>
        ///
        /// <param name="SimResults"> The simulation results.</param>
        /// <param name="ParmType">   Type of the parameter.</param>
        /// <param name="index">      Zero-based index of the.</param>
        ///
        /// <returns> An int if parmtype and index are correct, else returns int.minvalue .</returns>
        ///-------------------------------------------------------------------------------------------------

        public int grabParmBaseData(AnnualSimulationResults SimResults, modelParamtype ParmType, int index)
        {
            // check if index is 0 or greater
            if (index >= 0)
            {
                // check if this is a provider parameter
                if ((ParmType == modelParamtype.mptInputBase) || (ParmType == modelParamtype.mptOutputBase))
                {
                    switch (ParmType)
                    {
                        case modelParamtype.mptInputBase:
                            if (index < SimResults.Inputs.BaseInput.Values.Length)
                               return SimResults.Inputs.BaseInput.Values[index];
                            break;
                        case modelParamtype.mptOutputBase:
                            if (index < SimResults.Outputs.BaseOutput.Values.Length)
                                return SimResults.Outputs.BaseOutput.Values[index];
                            break;
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            return int.MinValue;
        }

        int[] grabBaseDataByYear(SimulationResults SR, ModelParameterClass MP)
        {
            int ArrayLength = SR.Length;

            int[] TheData = new int[ArrayLength];
            int DataIndex = SR[0].GetParmIndex(MP);

            for (int i=0;i<ArrayLength;i++)
            {
                TheData[i] = grabParmBaseData(SR[i], MP.ParamType, DataIndex);
            }
            return TheData;
        }

        int[][] grabProviderDataByYear(SimulationResults SR, ModelParameterClass MP)
        {
            int years = SR.Length;
            int ProviderCount = SR[0].Outputs.ProviderOutput[0].Values.Length;
            int[][] TheData = new int[ProviderCount][];
            for (int i = 0; i < ProviderCount; i++) TheData[i] = new int[years];
            int DataIndex = SR[0].GetParmIndex(MP);

            for (int i = 0; i < years; i++)
            {
                int [] ProviderData = grabParmProviderData(SR[i], MP.ParamType, DataIndex);
                for (int Pindex = 0; Pindex < ProviderCount; Pindex++)
                {
                    TheData[Pindex][i] = ProviderData[Pindex]; 
                }
            }
            return TheData;


        }

  
       

    }

 }



