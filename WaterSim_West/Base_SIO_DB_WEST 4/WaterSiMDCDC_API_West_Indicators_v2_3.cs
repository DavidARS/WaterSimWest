using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UniDB;
using ConsumerResourceModelFramework;
using WaterSimDCDC.Documentation;
using WaterSimDCDC.Generic;

//=======================================================================================
// LEAPING INDICATORS!
// version 2.0
//  2/16/18
// Status
//    2/16/18  QUAY Converted the old America indicators file to support the west multiple regions structure
//                  Extensive Changes!
//    version 2.1 3/11/18  QUAY Added insdutry efficiecny indicator
//  NOTES
//   

//  NASTY
//    2/16/18  This is using a temporary west indicator file that just uses the old America 11 states file by
//             replicating the state indicator values for each region in the state
//             

namespace WaterSimDCDC
{
    //=============================================================================================
    //
    //  Indicator Data Class
    // 
    //  Retreieves Data for use by all Indicators
    // 
    //=============================================================================================

    #region Indicator Data

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Indicator data class. </summary>
    ///
    /// <remarks>   Retreives data for use by all indicators. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class IndicatorDataClass
    {
        /// <summary>   Information describing the. </summary>
        DataTable TheData = null;
        /// <summary>   Pathname of the data directory. </summary>
        string FDataDirectory = "";
        /// <summary>   Filename of the file. </summary>
        string FFilename = "";
        /// <summary>   The scode field string. </summary>
        const string FScodeFieldStr = "SCODE";

        const string FRegionCodeFieldStr = "RC";
        const string FRegionNameFieldStr = "RN";

        /// <summary>   The safe yield field string. </summary>
        string FSafeYieldFieldStr = "SFYG";
        /// <summary>   The eco ratio field string. </summary>
        string FEcoRatioFieldStr = "ECOGPCD";
        /// <summary>   The eco value field string. </summary>
        string FEcoValueFieldStr = "ECOVALUE";
        /// <summary>   The sur ratio field string. </summary>
        string FSurRatioFieldStr = "SURRATIO";
        /// <summary>   The sur flow field string. </summary>
        string FSurFlowFieldStr = "STRMFLOW";
        ///// <summary>   The fag rate field string. </summary>
        //string FAGRateFieldStr = "AGRATE";
        ///// <summary>   The fagnet field string. </summary>
        //string FAGNETFieldStr = "AGNET";
        ///// <summary>   The fww surf field string. </summary>
        string FWWSurfFieldStr = "WWSURF";
        /// <summary>   The fww adjust field string. </summary>
        string FWWAdjustFieldStr = "WWADJ";
        /// <summary>   The fww weight field string. </summary>
        string FWWWeightFieldStr = "WWWGHT";
        /// <summary>   The base environment indicator field string. </summary>
        string FBaseEnvIndicatorFieldStr = "ENVIND";
        
        /// <summary>   The base environment surface goal field string. </summary>
        string FBaseEnvSurfaceGoalFieldStr = "SURGOAL";

        /// <summary> The Gallins ber $ Farm Income (AGNET) Goal.</summary>
        string FAGNETGOALFieldStr = "AGGOAL";
        //
        string FRecValueFieldStr = "RECVALUE";
        /// <summary>   The sur ratio field string. </summary>

        string FUnitCodeFieldStr = FRegionCodeFieldStr;
        string FUnitNameFieldStr = FRegionNameFieldStr;

        /// <summary>   Number of. </summary>
        int FCount = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="DataDirectory">    Pathname of the data directory. </param>
        /// <param name="Filename">         Filename of the file. </param>
        ///-------------------------------------------------------------------------------------------------

        public IndicatorDataClass(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            FCount = TheData.Rows.Count;
            if (isErr)
            {
                throw new Exception("Error loading Indicator Data. " + errMessage);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="DataDirectory">    Pathname of the data directory. </param>
        /// <param name="Filename">         Filename of the file. </param>
        /// <param name="RegionFieldName">  Name of the region field. </param>
        ///-------------------------------------------------------------------------------------------------

        public IndicatorDataClass(string DataDirectory, string Filename, string RegionFieldName)
        {
            FUnitCodeFieldStr = RegionFieldName;
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            FCount = TheData.Rows.Count;
            if (isErr)
            {
                throw new Exception("Error loading Indicator Data. " + errMessage);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Count The number of Data Units in the Indicator Array. </summary>
        ///
        /// <value> The count. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Count
        {
            get { return FCount; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// OK most of these BaseINdicator routines do teh same thing, so this is a genric routine that
        /// fetches all base indicator arrays.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="DataColumnName">   Name of the data column. </param>
        ///
        /// <returns>   A double[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseIndicator(string DataColumnName)
        {
            // make sure the column name is valid, else throw an exception
            if (!TheData.Columns.Contains(DataColumnName))
            {
                throw new Exception("Column name does not exists in " + TheData.TableName);
            }
            // create the return array
            double[] Temp = new double[FCount];
            // set all vaues to error
            for (int i = 0; i < FCount; i++)
            {
                Temp[i] = -1;
            }
            // loop through all recards and grab vakue
            bool iserr = true;
            string errMessage = "";
            int cnt = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // get value from record
                string valstr = DR[DataColumnName].ToString();
                // convert to dpuble
                double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                // if no err then save it
                if (!iserr)
                {
                    Temp[cnt] = tempDbl;
                }
                cnt++;
            }
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// OK most of these BaseINdicator routines do teh same thing, so this is a genric routine that
        /// fetches all base indicator arrays  As Int Values.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="DataColumnName">   Name of the data column. </param>
        ///
        /// <returns>   An int[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] BaseIndicatorAsInt(string DataColumnName)
        {
            // make sure the column name is valid, else throw an exception
            if (!TheData.Columns.Contains(DataColumnName))
            {
                throw new Exception("Column name does not exists in " + TheData.TableName);
            }
            // create the return array
            int[] Temp = new int[FCount];
            // set all vaues to error
            for (int i = 0; i < FCount; i++)
            {
                Temp[i] = -1;
            }
            // loop through all recards and grab vakue
            bool iserr = true;
            string errMessage = "";
            int cnt = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // get value from record
                string valstr = DR[DataColumnName].ToString();
                // convert to dpuble
                int tempint = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                // if no err then save it
                if (!iserr)
                {
                    Temp[cnt] = tempint;
                }
                cnt++;
            }
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Agriculture AGNET goal.</summary>
        ///
        /// <remarks> Quay, 3/19/2018.</remarks>
        ///
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] AgricultureAGNETgoal()
        {
            return BaseIndicator(FAGNETGOALFieldStr);
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The Base Values for the Envirionment Indicatoer. </summary>
        /// <remarks>   2/16/2018. </remarks>
        /// <returns>   a double array of indicator values. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseEnvirionmentIndicator()
        {
            return BaseIndicator(FBaseEnvIndicatorFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Base environment surface goal. </summary>
        ///
        /// <remarks>   4/3/2018. </remarks>
        ///
        /// <returns>   A double[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseEnvSurfaceGoal()
        {
            return BaseIndicator(FBaseEnvSurfaceGoalFieldStr);
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> The Base Values for the Envirionment Indicatoer.</summary>
        /// <remarks> Quay, 2/18/2018.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> a double array of indicator values.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double BaseEnvirionmentIndicator(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string theUnitcode = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(theUnitcode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FBaseEnvIndicatorFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastwwater surface flow. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   A double[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] WastewaterSurfaceFlow()
        {
            return BaseIndicator(FWWSurfFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Wastwwater surface flow.</summary>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterSurfaceFlow(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string UnitCodeStr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(UnitCodeStr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FWWSurfFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastewater weight. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] WastewaterWeight()
        {
            return BaseIndicator(FWWWeightFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Wastewater weight.</summary>
        ///
        /// <remarks> Quay, 2/19/2018.</remarks>
        ///
        /// <param name="UnitCode"> The unit code.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterWeight(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FWWWeightFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastewater adjustment. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   A double array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] WastewaterAdjustment()
        {
            return BaseIndicator(FWWAdjustFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Wastewater adjustment.</summary>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterAdjustment(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FWWAdjustFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Surface ratio. </summary>
        /// <remarks>
        /// This is the Base Ratio of USGS Urban Surface Water WithDrawl to Total Stream Flow as a
        /// Percent.
        /// </remarks>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <returns>   A double array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] SurfaceRatio()
        {
            return BaseIndicator(FSurRatioFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface ratio.</summary>
        /// <remarks> This is the Base Ratio of USGS Urban Surface Water WithDrawl to Total Stream Flow as
        ///     a Percent.</remarks>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double SurfaceRatio(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FSurRatioFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Surface flows. </summary>
        /// <remarks>
        /// Total of mean Flows of surface waters at intake points for ubran water treatment plants, MGD.
        /// </remarks>
        /// <returns>   A double array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] SurfaceFlows()
        {
            return BaseIndicator(FSurFlowFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface flows.</summary>
        /// <remarks> Total of mean Flows of surface waters at intake points for ubran water treatment
        ///     plants, MGD.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double SurfaceFlows(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FSurFlowFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Safe yield goal. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] SafeYieldGoal()
        {
            return BaseIndicatorAsInt(FSafeYieldFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Safe yield goal.</summary>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> An int array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int SafeYieldGoal(int UnitCode)
        {
            int result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FSafeYieldFieldStr].ToString();
                        temp = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = temp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Economic ratio. </summary>
        ///
        /// <remarks>
        /// No Units Ratio of Local water used for Economic Production by Total NonSaline Water Used.
        /// </remarks>
        ///
        /// <returns>   A double array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] EconomicGPCD()
        {
            return BaseIndicator(FEcoRatioFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Economic ratio.</summary>
        /// <remarks> No Units Ratio of Local water used for Economic Production by Total NonSaline Water
        ///     Used.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double EconomicGPCD(int UnitCode)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FEcoRatioFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Eco value. </summary>
        ///
        /// <remarks>   Units is $ dollars per gallon of water used for local production. </remarks>
        ///
        /// <returns>   An int array. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] EcoValue()
        {
            return BaseIndicatorAsInt(FEcoValueFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Eco value.</summary>
        /// <remarks> Units is $ dollars per gallon of water used for local production.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> An int array.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int EcoValue(int UnitCode)
        {
            int result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FEcoValueFieldStr].ToString();
                        temp = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = temp;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        // =================================================================================================
        // Edit Sampson 08.07.2018

        public int[] RecycleValue()
        {
            return BaseIndicatorAsInt(FEcoValueFieldStr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Recycle value.</summary>
        /// <remarks> </remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> An int array.</returns>
        ///-------------------------------------------------------------------------------------------------
        public int RecycleValue(int UnitCode)
        {
            int result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string unitcodestr = DR[FUnitCodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == UnitCode)
                    {
                        string valstr = DR[FEcoValueFieldStr].ToString();
                        temp = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = temp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        // =================================================================================================

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ag rate. </summary>
        ///
        /// <remarks>   Annual Growth Rate of Agricuture production as percent. </remarks>
        ///
        /// <returns>   A double array. </returns>
        ///-------------------------------------------------------------------------------------------------

        //public double[] AGRate()
        //{
        //    return BaseIndicator(FAGRateFieldStr);
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Ag rate.</summary>
        /// <remarks> Annual Growth Rate of Agricuture production as percent.</remarks>
        /// <param name="UnitCode"> The unit code.</param>
        /// <returns> A double array.</returns>
        ///-------------------------------------------------------------------------------------------------

        //public double AGRate(int UnitCode)
        //{
        //    double result = -1;
        //    bool iserr = true;
        //    string errMessage = "";
        //    foreach (DataRow DR in TheData.Rows)
        //    {
        //        string unitcodestr = DR[FUnitCodeFieldStr].ToString();
        //        int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
        //        if (!iserr)
        //        {
        //            if (temp == UnitCode)
        //            {
        //                string valstr = DR[FAGRateFieldStr].ToString();
        //                double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
        //                if (!iserr)
        //                {
        //                    result = tempDbl;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ag net. </summary>
        ///
        /// <remarks>   Net Annual Total Farm Income in Millions of Dollars. </remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        //    public double[] AGNet()
        //    {
        //        return BaseIndicator(FAGNETFieldStr);
        //    }

        //    ///-------------------------------------------------------------------------------------------------
        //    /// <summary> Ag net.</summary>
        //    /// <remarks> Net Annual Total Farm Income in Millions of Dollars.</remarks>
        //    /// <param name="UnitCode"> The unit code.</param>
        //    /// <returns> A double.</returns>
        //    ///-------------------------------------------------------------------------------------------------

        //    public double AGNet(int UnitCode)
        //    {
        //        double result = -1;
        //        bool iserr = true;
        //        string errMessage = "";
        //        foreach (DataRow DR in TheData.Rows)
        //        {
        //            string unitcodestr = DR[FUnitCodeFieldStr].ToString();
        //            int temp = Tools.ConvertToInt32(unitcodestr, ref iserr, ref errMessage);
        //            if (!iserr)
        //            {
        //                if (temp == UnitCode)
        //                {
        //                    string valstr = DR[FAGNETFieldStr].ToString();
        //                    double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
        //                    if (!iserr)
        //                    {
        //                        result = tempDbl;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return result;
        //    }
    }

    #endregion

    public static class IndicatorTools
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sweet spot indicator conversion.</summary>
        ///
        /// <remarks> Quay, 4/4/2018.</remarks>
        ///
        /// <param name="Target">            Target value to convert to indicatorfor the.</param>
        /// <param name="SpotValueLow">      The low value for the sweet spot.</param>
        /// <param name="SpotValueHigh">     The high value for the sweet spot.</param>
        /// <param name="IndicatorSpotLow">  The indicator corrsponding to the sweet spot low.</param>
        /// <param name="IndicatorSpotHigh"> The indicator corresponding to the sweet spot high.</param>
        /// <param name="MinIndicator">      The minimum indicator.</param>
        /// <param name="MAxIndicator">      The max indicator.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        static public double SweetSpotIndicator(double Target, double SpotValueLow, double SpotValueHigh, double IndicatorSpotLow, double IndicatorSpotHigh, double MinIndicator, double MAxIndicator)
        {
            double result = 0;
            if (Target < SpotValueLow)
            {
                double LOGdif = (Math.Log10((SpotValueLow * 1.1)-Target));
                double LOGSpot = Math.Log10(SpotValueLow * 1.1);
                double P4 = LOGSpot - LOGdif;
                result = IndicatorSpotLow * P4;
                //// reduce the Indicator Low value by the LOg10 of the differecnce betwene Target and Spotvalue (adding 10 for effect) minus 1 to make a percent.


                //result = IndicatorSpotLow - (IndicatorSpotLow * ((Math.Log10((SpotValueLow - Target) + 10)) - 1));
                //// Do not let it go lower than min indicator
                //if (result < MinIndicator) result = MinIndicator;
            }
            else
            if (Target > SpotValueHigh)
            {
                // OK, this basically adjust the 
                double MidSpotValue = (SpotValueLow + SpotValueHigh) / 2;
                double BaseIndicator = IndicatorSpotHigh / Math.Log10(SpotValueHigh - MidSpotValue);
                result = Math.Log10(Target - MidSpotValue) * BaseIndicator;
                if (result > MAxIndicator) result = MAxIndicator;
            }
            else
            {
                // Basically calculate slope and conversion for high and low indicator to high and low value
                result = ((Target - SpotValueLow) / ((SpotValueHigh-SpotValueLow)/(IndicatorSpotHigh-IndicatorSpotLow) )) + IndicatorSpotLow;

            }
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Convert int 32 and distribute.</summary>
        /// <param name="Values">       The values.</param>
        /// <param name="low">          The lowest double value before 0.</param>
        /// <param name="high">         The highest double value.</param>
        /// <param name="ConvertValue"> The conversion value - NewValues = Convert.Int32(OldValues *
        ///                             ConversionValue)</param>
        /// <param name="ErrValue">     The error value if convert throws exception.</param>
        ///
        /// <returns> The int converted 32 and distribute.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int[] ConvertInt32AndDistribute(double[] Values, double low, double high, double ConvertValue, int ErrValue)
        {
            int[] NewValues = new int[Values.Length];
            double NewRange = high - low;
            double DataRange = (Values.Max()*ConvertValue) - (Values.Min()*ConvertValue);
            double Conversion = NewRange / DataRange; 

            for (int i = 0; i < Values.Length; i++)
            {
                double NewValue = (Values[i]*ConvertValue) * Conversion;
                try
                    {
                        NewValues[i] = Convert.ToInt32(NewValue);
                    }
                catch
                    {
                        // Ouch, not sure how this can happen
                        NewValues[i] = ErrValue;
                    }
                
            }
            return NewValues;

        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Convert int 32 and bracket.</summary>
        /// <remarks> Quay, 2/18/2018.</remarks>
        /// <param name="Values">       The values.</param>
        /// <param name="low">          The lowest double value before 0.</param>
        /// <param name="high">         The highest double value.</param>
        /// <param name="ConvertValue"> The conversion value - NewValues = Convert.Int32(OldValues * ConversionValue) </param>
        /// <param name="ErrValue">     The error value if convert throws exception.</param>
        /// <returns> The int[] converted to int32 and bracketed.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int[] ConvertInt32AndBracket(double[] Values, double low, double high, int ConvertValue, int ErrValue)
        {
            int[] NewValues = new int[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] > high)
                {
                    NewValues[i] = ConvertValue;
                }
                else
                // check if will be less than 1
                if (Values[i] < low)
                {
                    NewValues[i] = 0;
                }
                else
                // OK calcualte value between 100 and 1
                {
                    try
                    {
                        NewValues[i] = Convert.ToInt32(Values[i] * (double)ConvertValue);
                    }
                    catch
                    {
                        // Ouch, not sure how this can happen
                        NewValues[i] = ErrValue;
                    }
                }
            }
            return NewValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Convert int 32 to double.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="IntArray"> Array of ints.</param>
        /// <param name="ErrValue"> The error value if convert throws exception.</param>
        ///
        /// <returns> The int[] converted 32 to double[]</returns>
        ///-------------------------------------------------------------------------------------------------

        public static double[] ConvertInt32ToDouble(int[] IntArray, double ErrValue)
        {
            double[] NewValues = new double[IntArray.Length];
            for (int i=0;i<IntArray.Length;i++)
            {
                double Temp = ErrValue;
                try
                {
                    Temp = Convert.ToDouble(IntArray[i]);
                }
                finally
                {
                    NewValues[i] = Temp;
                }
            }
            return NewValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Convert double to int 32.</summary>
        ///
        /// <remarks> Quay, 4/2/2018.</remarks>
        ///
        /// <param name="DoubleArray"> Array of doubles.</param>
        /// <param name="scale">       The error value if convert throws exception.</param>
        ///
        /// <returns> The double converted to int 32.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static int[] ConvertDoubleToInt32(double[] DoubleArray, double scale)
        {
            int[] NewValues = new int[DoubleArray.Length];
            for (int i = 0; i < DoubleArray.Length; i++)
            {
                   NewValues[i] = (int)(DoubleArray[i]*scale);
            }
            return NewValues;
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary> Zero out.</summary>
        /// <remarks> Quay, 2/18/2018.</remarks>
        /// <param name="AnArray"> [in,out] Array of int.</param>
        ///-------------------------------------------------------------------------------------------------

        public static void ZeroOut(ref int[] AnArray)
        {
            for (int i = 0; i < AnArray.Length; i++) AnArray[i] = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Zero out.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="AnArray"> [in,out] Array of double</param>
        ///-------------------------------------------------------------------------------------------------

        public static void ZeroOut(ref double[] AnArray)
        {
            for (int i = 0; i < AnArray.Length; i++) AnArray[i] = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Fill array.</summary>
        /// <remarks> Quay, 2/18/2018.</remarks>
        /// <param name="AnArray"> [in,out] Array of int.</param>
        /// <param name="aValue">  The value int.</param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillArray(ref int[] AnArray, int aValue)
        {
            for (int i = 0; i < AnArray.Length; i++) AnArray[i] = aValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Fill array.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="AnArray"> [in,out] Array of double.</param>
        /// <param name="aValue">  The value double</param>
        ///-------------------------------------------------------------------------------------------------

        public static void FillArray(ref double[] AnArray, int aValue)
        {
            for (int i = 0; i < AnArray.Length; i++) AnArray[i] = aValue;
        }



    }
    //##########################################################################
    //  
    //  INDICATORS
    //  
    //   
    //##########################################################################

    #region Indicators

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A web indicator. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public abstract class WebIndicator
    {
        /// <summary>   The wsim. </summary>
        protected WaterSimManager FWSim = null;
        /// <summary>   Information describing the fi. </summary>
        protected IndicatorDataClass FIData = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti indicator.</summary>
        ///
        /// <remarks> Quay, 3/28/2018.
        ///           This is the primary method to retrieve a WebIndicators value</remarks>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract int[] Geti_Indicator();

    }
    // ========================================================================================
    // 
    //   Groundwater Indicator Classes
    //   Modified 2/18/18 Quay
    // 
    //  ======================================================================================= 
    #region Groundwater Indicator

    public class GWIndicatorUnitsProcess : AnnualFeedbackProcess
    {
        WaterSimModel MyWaterSimModel = null;
        /// <summary>   The state code. </summary>
        int FStateCode = 0;
        /// <summary>   The total sy deficit. </summary>
        int[] FTotalSYDeficit = null;
        /// <summary>   The goal. </summary>
        int[] FGoal = null;
        /// <summary>   The maximum withdraw. </summary>
        int[] FMaxWithdraw = null;
        /// <summary>   The maximum withdraw as int. </summary>
        int[] FMaxWithdrawAsInt = null;
        /// <summary>   The base withdrawal. </summary>
        int[] FBaseWithdrawal = null;
        /// <summary>   The annual ratio. </summary>
        int[] FAnnualRatio = null;
        /// <summary>   The cumulative ratio. </summary>
        int[] FCumulativeRatio = null;

        int FModelCount = 0; 
        
        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public GWIndicatorUnitsProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            MyWaterSimModel = WSim.WaterSimWestModel;
            FIData = iData;
            FModelCount = MyWaterSimModel.ModelCount;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Self Yield Goal Tracker Process: This process keeps track of the cumaltive Safe Yield Deficit based on the state's Safe Yield goal";
            FProcessCode = "CSY";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// method that is called right before the first year of a simulation is called.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {

           
            // zero out cumulatives
            FTotalSYDeficit = new int[FModelCount];
            // get the state code for this run
            //int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            //FStateCode = statecode;
            // get the gola for this state
            FGoal = FIData.SafeYieldGoal();
            // get the initial withdrawal
            FBaseWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epP_Groundwater).ProviderProperty.getvalues().Values;
            // calulate SafeYield Withdrawal
            for (int i = 0; i < FGoal.Length; i++)
            {
                FMaxWithdraw[i] = FBaseWithdrawal[i] - FGoal[i];
                FMaxWithdrawAsInt[i] = FMaxWithdraw[i];
            }

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <remarks>   2/16/2018. Substatially Modified Quay 2/18/18
        ///             Converted single Model Structure to Multi Model Structure</remarks>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // get the current total GW withdrawal
            int[] GWWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epP_Groundwater).ProviderProperty.getvalues().Values;
            // Calculate cumulative net SY defecit

            //int AnnualNet = GWWithdrawal - FMaxWithdrawAsInt;
            int[] AnnualNet = new int[FModelCount];
            int cnt = 0;
            foreach (WaterSimCRFModel WSCM in MyWaterSimModel.WaterSimCRFModels)
            {
                AnnualNet[cnt] = GWWithdrawal[cnt] - FMaxWithdrawAsInt[cnt];
                FTotalSYDeficit[cnt] += AnnualNet[cnt];
                cnt++;
            }

            //// setup for rations
            //double[] GW = GWWithdrawal;
            //double Goal = FGoal;
            //double CumulativeAnnual = FTotalSYDeficit;
            // calcualte ratios
            double[] SYAnnualRatio = new double[FModelCount];
            double[] SYCumulativeRatio = new double[FModelCount];
            cnt = 0;
            foreach (WaterSimCRFModel WSCM in MyWaterSimModel.WaterSimCRFModels)
            {
                if(GWWithdrawal[cnt] != 0)
                {
                    SYAnnualRatio[cnt] = (double)FMaxWithdraw[cnt] / (double)GWWithdrawal[cnt];//Goal / GW;
                    SYCumulativeRatio[cnt] = (double)FTotalSYDeficit[cnt] / (double)GWWithdrawal[cnt];
                }
                cnt++;
            }


            // Convert SYAnnualRatio double values to Bracket int Array 0 to 100
            FAnnualRatio = IndicatorTools.ConvertInt32AndBracket(SYAnnualRatio, .001, 1, 100, 0);
            // Convert SYAnnualRatio double values to Bracket int Array 0 to 100
            FCumulativeRatio = IndicatorTools.ConvertInt32AndBracket(SYCumulativeRatio, .001, 1, 100, 0);


            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the goal. </summary>
        ///
        /// <value> The goal. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] Goal
        {
            get { return FGoal; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the annual ratio. </summary>
        ///
        /// <value> The annual ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] AnnualRatio
        {
            get { return FAnnualRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cummulative ratio. </summary>
        ///
        /// <value> The cummulative ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] CummulativeRatio
        {
            get {
                int[] Temp = new int[FModelCount];
                int cnt = 0;
                foreach (int CR in FCumulativeRatio)
                {
                    Temp[cnt] = 100 - FCumulativeRatio[cnt];
                }
                return Temp;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cumulative safe yield net. </summary>
        ///
        /// <value> The cumulative safe yield net. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] CumulativeSafeYieldNet
        {
            get { return FTotalSYDeficit; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the safe yield. </summary>
        ///
        /// <value> The safe yield. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] SafeYield
        {
            get { return FMaxWithdrawAsInt; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state code. </summary>
        ///
        /// <value> The state code. </value>
        ///-------------------------------------------------------------------------------------------------

        //public int StateCode
        //{
        //    get { return FStateCode; }
        //}


    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A gw indicator process. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------


    public class GWIndicatorProcess : AnnualFeedbackProcess
    {
        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        /// <summary>   The state code. </summary>
        //int FStateCode = 0;
        /// <summary>   The total sy deficit. </summary>
        int[] FTotalSYDeficit = null;
        /// <summary>   The goal. </summary>
        int[] FGoal = null;
        /// <summary>   The maximum withdraw. </summary>
        double[] FMaxWithdraw = null;
        /// <summary>   The maximum withdraw as int. </summary>
        int[] FMaxWithdrawAsInt = null;
        /// <summary>   The base withdrawal. </summary>
        int[] FBaseWithdrawal = null;
        /// <summary>   The annual ratio. </summary>
        int[] FAnnualRatio = null;
        /// <summary>   The cumulative ratio. </summary>
        int[] FCumulativeRatio = null;

        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public GWIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            // build the description
            BuildDescStrings();
            // Set properties
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            MyWaterSimModel = FWsim.WaterSimWestModel;
            FIData = iData;
            FModelCount = MyWaterSimModel.ModelCount;
            // create the variable arrays
            FTotalSYDeficit = new int[FModelCount];
            FGoal = new int[FModelCount]; ;
            FMaxWithdraw = new double[FModelCount];
            FMaxWithdrawAsInt = new int[FModelCount]; ;
            FBaseWithdrawal = new int[FModelCount]; ;
            FAnnualRatio = new int[FModelCount]; ;
            FCumulativeRatio = new int[FModelCount]; ;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Self Yield Goal Tracker Process: This process keeps track of the cumaltive Safe Yield Deficit based on the state's Safe Yield goal";
            FProcessCode = "CSY";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// method that is called right before the first year of a simulation is called.
        /// </summary>
        /// <remarks>   2/16/2018. </remarks>
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            IndicatorTools.ZeroOut(ref FTotalSYDeficit);
            // get the state code for this run
            //int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            //FStateCode = statecode;
            // get the goal for this state
            FGoal = FIData.SafeYieldGoal();
            // get the initial withdrawal
            FBaseWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epP_Groundwater).ProviderProperty.getvalues().Values;
            // calulate SafeYield Withdrawal
            // 
            for (int i = 0; i < FModelCount; i++)
            {
                FMaxWithdraw[i] = FBaseWithdrawal[i] - FGoal[i];
                try
                {
                    FMaxWithdrawAsInt[i] = Convert.ToInt32(FMaxWithdraw[i]);
                }
                catch (Exception ex)
                {
                    // OUCH!!
                }
            }
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        /// <remarks>   2/16/2018. </remarks>
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // get the current total GW withdrawal
            int[] GWWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epP_Groundwater).ProviderProperty.getvalues().Values;
            // Setup Ratios
            double[] SYAnnualRatio = new double[FModelCount];
            double[] SYCumulativeRatio = new Double[FModelCount];

            // Calculate cumulative net SY defecit and Ratios
            int[] AnnualNet = new int[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                AnnualNet[i] = GWWithdrawal[i] - FGoal[i];
                FTotalSYDeficit[i] += AnnualNet[i];
                // check for zero values
                if (GWWithdrawal[i] != 0)
                {
                    double TempRatio = (double)FGoal[i] / (double)GWWithdrawal[i];
                    if (TempRatio > 1) TempRatio = 1;
                    SYAnnualRatio[i] = TempRatio;
                    SYCumulativeRatio[i] = (double)FTotalSYDeficit[i] / (double)GWWithdrawal[i];

                }
                else
                {
                    SYAnnualRatio[i] = 1;
                }
            }
            //// get the current total GW withdrawal
            //int[] GWWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epP_Groundwater).ProviderProperty.getvalues().Values;
            //// Setup Ratios
            //double[] SYAnnualRatio = new double[FModelCount];
            //double[] SYCumulativeRatio = new Double[FModelCount];

            //// Calculate cumulative net SY defecit and Ratios
            //int[] AnnualNet = new int[FModelCount];
            //for (int i = 0; i < FModelCount; i++)
            //{
            //    AnnualNet[i] = GWWithdrawal[i] - FMaxWithdrawAsInt[i];
            //    FTotalSYDeficit[i] += AnnualNet[i];
            //    // check for zero values
            //    if (GWWithdrawal[i] != 0)
            //    {
            //        SYAnnualRatio[i] = (double)FMaxWithdraw[i] / (double)GWWithdrawal[i];
            //        SYCumulativeRatio[i] = (double)FTotalSYDeficit[i] / (double)GWWithdrawal[i];

            //    }
            //}
            // setup for rations
            //double[] GW = GWWithdrawal;
            //double Goal = FGoal;
            //double CumulativeAnnual = FTotalSYDeficit;
            //// calcualte ratios
            //double[] SYAnnualRatio = new double[FModelCount];
            //double[] SYCumulativeRatio = new Double[FModelCount];

            //if (GW != 0)
            //{
            //    SYAnnualRatio = FMaxWithdraw / GW;//Goal / GW;
            //    SYCumulativeRatio = CumulativeAnnual / GW;
            //}
            // 

            // Convert doubel Ratio Arrays to int32 arrays baracketed 0 to 100
            //FAnnualRatio = IndicatorTools.ConvertInt32AndBracket(SYAnnualRatio, 0.001, 1.0, 100, 0);
            FAnnualRatio = IndicatorTools.ConvertDoubleToInt32(SYAnnualRatio, 100); 
            FCumulativeRatio = IndicatorTools.ConvertInt32AndBracket(SYCumulativeRatio, 0.001, 1.0, 100, 0);


            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the goal. </summary>
        ///
        /// <value> The goal. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] Goal
        {
            get { return FGoal; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the annual ratio. </summary>
        ///
        /// <value> The annual ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] AnnualRatio
        {
            get { return FAnnualRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cummulative ratio. </summary>
        ///
        /// <value> The cummulative ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] CummulativeRatio
        {
            get
            {
                int[] Values = new int[FModelCount];
                for (int i = 0; i < FCumulativeRatio.Length; i++)
                {
                    Values[i] = 100 - FCumulativeRatio[i];
                }
                return Values;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cumulative safe yield net. </summary>
        ///
        /// <value> The cumulative safe yield net. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] CumulativeSafeYieldNet
        {
            get { return FTotalSYDeficit; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the safe yield. </summary>
        ///
        /// <value> The safe yield. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] SafeYield
        {
            get { return FMaxWithdrawAsInt; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state code. </summary>
        ///
        /// <value> The state code. </value>
        ///-------------------------------------------------------------------------------------------------

        //public int StateCode
        //{
        //    get { return FStateCode; }
        //}
 
    }
    //=================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A web indicator ground water. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class WebIndicator_GroundWater : WebIndicator
    {
        ///// <summary>   The gw annual indicator. </summary>
        //const int eGWAnnual_Indicator = 1;
        ///// <summary>   The gw cumulative indicator. </summary>
        //const int eGWCumulative_Indicator = 2;
        ///// <summary>   The gw cumulative deficit. </summary>
        //const int eGWCumulative_Deficit = 3;
        ///// <summary>   The gw annual safe yield deficit. </summary>
        //const int eGWAnnual_SafeYield_Deficit = 4;
        ///// <summary>   The gw safe yield reduction. </summary>
        //const int eGWSafeYield_Reduction = 5;
        ///// <summary>   The gw safe yield. </summary>
        //const int eGWSafeYield = 6;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        //      int FBaseParam = 0;
        /// <summary>   The fgw process. </summary>
        GWIndicatorProcess FGWProcess = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///
        /// ### <param name="BaseEModelParam">  The base model parameter. </param>
        ///-------------------------------------------------------------------------------------------------

        public WebIndicator_GroundWater(WaterSimManager WSim, IndicatorDataClass iData)
        {

            FWSim = WSim;
            MyWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;

            FGWProcess = new GWIndicatorProcess("Safe Yield Indicator",FWSim, iData );
            FWSim.ProcessManager.AddProcess(FGWProcess);
            CreateModelParameters();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti annual indicator. </summary>
        /// <remarks>   2/16/2018. 
        ///             Modified Quay 2/18/18  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] value = null;
            if (FGWProcess != null)
            {
                value = FGWProcess.AnnualRatio;
            }
            return value;
        }


        /// <summary> The annual indicator.</summary>
        public providerArrayProperty AnnualIndicator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti cumulative indicator. </summary>
        /// <remarks>   2/16/2018. 
        ///             Modified Quay 2/18/18  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_CumulativeIndicator()
        {
            int[] value = null;
            if (FGWProcess != null)
            {
                value = FGWProcess.CummulativeRatio;
            }
            return value;
        }


        /// <summary> The cumulative indicator.</summary>
        public providerArrayProperty CumulativeIndicator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti cumulative safe yield net. </summary>
        /// <remarks>   2/16/2018. 
        ///             Modified Quay 2/18/18  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_CumulativeSafeYieldNet()
        {
            int[] value = null;
            if (FGWProcess != null)
            {
                value = FGWProcess.CumulativeSafeYieldNet;
            }
            return value;
        }


        /// <summary> The cumulative safe yield net.</summary>
        public providerArrayProperty CumulativeSafeYieldNet;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti safe yield reduction goal. </summary>
        /// <remarks>   2/16/2018. 
        ///             Modified Quay 2/18/18  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_SafeYield_Reduction_Goal()
        {
            int[] value = null;
            if (FGWProcess != null)
            {
                value = FGWProcess.Goal;
            }
            return value;
        }


        /// <summary> The safe yield reduction goal.</summary>
        providerArrayProperty SafeYield_Reduction_Goal;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti safe yield. </summary>
        /// <remarks>   2/16/2018. 
        ///             Modified Quay 2/18/18  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_SafeYield()
        {
            int[] value = null;
            if (FGWProcess != null)
            {
                value = FGWProcess.SafeYield;
            }
            return value;
        }


        /// <summary> The safe yield.</summary>
        providerArrayProperty SafeYield;

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the state code. </summary>
        /////
        ///// <remarks>   2/16/2018. </remarks>
        /////
        ///// <returns>   An int. </returns>
        /////-------------------------------------------------------------------------------------------------

        //public int StateCode()
        //{
        //    int value = 0;
        //    if (FGWProcess != null)
        //    {
        //        value = FGWProcess.CumulativeSafeYieldNet;
        //    }
        //    return value;
            
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates the model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            AnnualIndicator = new providerArrayProperty(FPM, eModelParam.epP_GW_Annual_Indicator, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_Annual_Indicator, "Annual Safe Yield Indicatior", "GWSYA", AnnualIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_Annual_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the annual ground water withdrawal, 0 means safe yield achived, 100 is max value", "%", "Percent", "Groundwater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            CumulativeIndicator = new providerArrayProperty(FPM, eModelParam.epP_GW_Cumulative_Indicator, Geti_CumulativeIndicator, eProviderAggregateMode.agAverage);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_GW_Cumulative_Indicator, "Scenario Safe Yield Indicatior", eModelFields.epP_GW_Cumulative_Indicator, CumulativeIndicator));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_Cumulative_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the cumaulative difference between Safe Yield Withdrawal and annual ground water withdrawal, 0 means safe yield balance is achieved, 100 is max value", "%", "Percent", "Groundwater Cumulative Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            CumulativeSafeYieldNet = new providerArrayProperty(FPM, eModelParam.epP_GW_Cumulative_SafeYieldNet, Geti_CumulativeSafeYieldNet, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_GW_Cumulative_SafeYieldNet, "Safe Yield Cumulative Net", eModelFields.epP_GW_Cumulative_SafeYieldNet, CumulativeSafeYieldNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_Cumulative_SafeYieldNet, "The total cumulative overdraft of groundwater based on the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield and annual ground water withdrawal, 0 means safe yield balance is achieved, 100 is max value", "mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            SafeYield_Reduction_Goal = new providerArrayProperty(FPM, eModelParam.epP_GW_SafeYield_Reduction_Goal, Geti_SafeYield_Reduction_Goal, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_GW_SafeYield_Reduction_Goal, "Safe Yield Reduction Goal", eModelFields.epP_GW_SafeYield_Reduction_Goal, SafeYield_Reduction_Goal));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_SafeYield_Reduction_Goal, "The amount of reduction in annual groundwater based on the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield","mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            SafeYield = new providerArrayProperty(FPM, eModelParam.epP_GW_SafeYield, Geti_SafeYield, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_GW_SafeYield, "Groundwater Safe Yield", eModelFields.epP_GW_SafeYield, SafeYield));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_SafeYield, "The amount of annual groundwater withdrawal that represents safe yield or a balance with the amount of annual hgroundwater recharge", "mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    
    #endregion
    //=========================================================================================
    //
    //  Economic Indicator Classes  
    //  Modified 2/18/18 Quay
    // 
    // ========================================================================================  
    #region Economic Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Economic indicator process. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class EconomicIndicatorProcess : AnnualFeedbackProcess
    {
        // <summary>   The state code. </summary>
        //int FStateCode = 0;
        /// <summary>   The wsim. </summary>
        WaterSimManager FWsim = null;
        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;
        /// <summary>   The eco gpcD. </summary>
        double[] FEcoGPCD= null;
        /// <summary>   The eco value. </summary>
        int[] FEcoValue = null;
        /// <summary>   The base total demand. </summary>
        int[] FBaseTotalDemand = null;
        /// <summary>   The base pop. </summary>
        int[] FBasePop = null;
        /// <summary>   The base eco demand. </summary>
        int[] FBaseEcoDemand = null;
        /// <summary>   The base ratio. </summary>
        double[] FBaseRatio = null;
        /// <summary>   The base gpcy. </summary>
        double[] FBaseGPCY = null;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public EconomicIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            // Build the description strings as required
            BuildDescStrings();
            // Set the names
            Fname = aName;
            this.Name = this.GetType().Name;
            // Set WaterSim Manage3r
            FWsim = WSim;
            // Get The West Model
            MyWaterSimModel = WSim.WaterSimWestModel;
            // The Model Count
            FModelCount = MyWaterSimModel.ModelCount;
            // Get the Indicator Database
            FIData = iData;

            // create variable Arrays
            FEcoGPCD = new double[FModelCount];
            FEcoValue = new int[FModelCount];
            FBaseTotalDemand = new int[FModelCount];
            FBasePop = new int[FModelCount];
            FBaseEcoDemand = new int[FModelCount];
            FBaseRatio = new double[FModelCount];
            FBaseGPCY = new double[FModelCount];



        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Economic Ratio and Value Tracker Process: This process keeps track economic impact of reduce water use";
            FProcessCode = "ERT";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// method that is called right before the first year of a simulation is called.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
           
            // get the data
            FEcoGPCD = FIData.EconomicGPCD();
            FEcoValue = FIData.EcoValue();
            FBaseTotalDemand = (WSim as WaterSimManager).WaterSimWestModel.Geti_TotalDemand();
//            FBasePop = WSim.ParamManager.Model_Parameter(eModelParam.epMagicPop).Value;
            FBasePop = WSim.ParamManager.Model_Parameter(eModelParam.epP_Population).ProviderProperty.getvalues().Values;
            // Calculate The Base Economic Demand
            for (int i = 0; i < FModelCount; i++)
            {
                int TempValue = 0;
                try
                {
                    TempValue = Convert.ToInt32(FEcoGPCD[i] * (double)FBasePop[i]);
                }
                finally
                {
                    FBaseEcoDemand[i] = TempValue;
                }
            }
            // Convert Int Values =to Double Array
            FBaseGPCY = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_TotalGPCY).ProviderProperty.getvalues().Values,0);
            // Calculate Base Ratios
            for (int i = 0;i < FModelCount; i++)
            {
                // Get ration between Indicator data FEcoGPCD and The Base GPCY.  Note GPCD needs copnverted to year.  This is the base ration to comapre to
                if (FBaseGPCY[i] !=0) { FBaseRatio[i] = (FEcoGPCD[i]*365) / FBaseGPCY[i]; }
            }
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the economic gpcd. </summary>
        ///
        /// <value> The economic gpcd. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] EconomicGPCD
        {
            get { return FEcoGPCD; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base ratio. </summary>
        ///
        /// <value> The base ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseRatio
        {
            get { return FBaseRatio; }
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A web indicator economic.</summary>
    /// <remarks>   2/16/2018. Modified  Quay, 2/19/2018.</remarks>
    /// <seealso cref="T:WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class WebIndicator_Economic : WebIndicator
    {
        /// <summary>   The feip. </summary>
        EconomicIndicatorProcess FEIP = null;

        WaterSimModel MYWaterSimModel = null;
        int FModelCount = 0;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///
        /// ### <param name="BaseEModelParam">  The base model parameter. </param>
        ///-------------------------------------------------------------------------------------------------

        public WebIndicator_Economic( WaterSimManager WSim, IndicatorDataClass iData)
        {

            FWSim = WSim;
            MYWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MYWaterSimModel.ModelCount;
//            FBaseParam = BaseEModelParam;
            FIData = iData;
            FEIP = new EconomicIndicatorProcess("Economic Indicator Tracking", WSim, FIData);
            FWSim.ProcessManager.AddProcess(FEIP);
            CreateModelParameters();
        }


        /// <summary> The eco demand.</summary>
        providerArrayProperty EcoDemand;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti eco demand per day. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_EcoDemand()
        {
            // get the population
            int[] Pop = FWSim.ParamManager.Model_Parameter(eModelParam.epP_Population).ProviderProperty.getvalues().Values;
            // initialize EcoDemand
            int[] EcoDemand = new int[FModelCount];
            // Loop through can calcualte EcoDemand
            for(int i=0;i<FModelCount;i++)
            {
                int Temp = 0;
                try
                {
                    Temp = Convert.ToInt32((FEIP.EconomicGPCD[i] * (double)Pop[i]) / 1000000);
                }
                finally
                {
                    EcoDemand[i] = Temp;
                }
            }
            return EcoDemand;
            
        }


        /// <summary> The eco ratio.</summary>
        public providerArrayProperty EcoRatio;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti eco ratio. </summary>
        /// <remarks>   2/16/2018. Modified 2/18/18 Quay</remarks>
        /// <returns>   An int[] </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            // get the Annual GPCY from the model
            double[] AnnualGPCY = IndicatorTools.ConvertInt32ToDouble((FWSim as WaterSimManager).TotalGPCY.getvalues().Values,0);
            // get the Economic GPCY for all models
            double[] EcoGPCD = FEIP.EconomicGPCD;
            // Calculate Ratios
            double[] CurrentRatio = new double[FModelCount];
            double[] StressRatio = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                if (0 < AnnualGPCY[i]) CurrentRatio[i] = (EcoGPCD[i]*365) / AnnualGPCY[i];
                if (0 < CurrentRatio[i])
                {
                    StressRatio[i] = FEIP.BaseRatio[i] / CurrentRatio[i];
                    // OK, here is a web adjustement
                    //StressRatio[i] = StressRatio[i] / 1.3;
                    StressRatio[i] = StressRatio[i] / 1.2;
                }
            }
            int[] TempInt = IndicatorTools.ConvertDoubleToInt32(StressRatio, 100);
            //int[] TempInt = IndicatorTools.ConvertInt32AndBracket(StressRatio, 0.001, 1, 100, 0);
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates the model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            EcoDemand = new providerArrayProperty(FPM, eModelParam.epP_ECODemand, Geti_EcoDemand, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_ECODemand, "Demand for Economic Production", "ECOD",EcoDemand));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ECODemand, "The volume of water needed to meet local water needs for economic production to support the population", "mgd", "Million Gallans Per Day", "Economic Demand", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            EcoRatio = new providerArrayProperty(FPM, eModelParam.epP_ECORatio, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epP_ECORatio, "Econmic Water Stress", "ECOR", EcoRatio));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ECODemand, "The ratio of Needed Gallons Per Capita Per Day to Annual Gallons Per Capita Per Day", "%", "Percent", "Economic Water Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
        }
    }
    #endregion
    //=========================================================================================
    //  
    //   Surface Water Indicator
    //   Modified 2/18/18 Quay
    //    
    //=========================================================================================
    #region Surface Water

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Software indicator process. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class SWIndicatorProcess : AnnualFeedbackProcess
    {
        ///// <summary>   The state code. </summary>
        //int FStateCode = 0;
        /// <summary>   The base ratio. </summary>
        double[] FBaseRatio = null;
        /// <summary>   The stream flow. </summary>
        double[] FStreamFlow = null;

        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;

        int FModelCount = 0;
        WaterSimModel MYWaterSimModel = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public SWIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            MYWaterSimModel = WSim.WaterSimWestModel;
            FIData = iData;
            // create variablwe arrays
            FModelCount = WSim.WaterSimWestModel.ModelCount;
            FBaseRatio = new double[FModelCount];
            FStreamFlow = new double[FModelCount];
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>
        /// Override this method to build description strings for new classes and objects.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Surface Water Tracker Process: Keeps track of initial base surface water indicator parameters";
            FProcessCode = "SWP";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base ratio. </summary>
        ///
        /// <value> The base ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseRatio
        {
            get { return FBaseRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the stream flow. </summary>
        ///
        /// <value> The stream flow. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] StreamFlow
        {
            get { return FStreamFlow; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the started. </summary>
        ///
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            // get the state code for this run
            //int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            //FStateCode = statecode;

            FBaseRatio = FIData.SurfaceRatio();
            FStreamFlow = FIData.SurfaceFlows();
            
            return true;
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Surface water indicator. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterIndicator: WebIndicator
    {
        /// <summary>   The swip. </summary>
        SWIndicatorProcess SWIP = null;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            FWSim = WSim;
            MyWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;
            FIData = iData;
            SWIP = new SWIndicatorProcess("Surface Water Indicator Tracking", WSim, FIData);
            FWSim.ProcessManager.AddProcess(SWIP);
            CreateModelParameters();
        }

        /// <summary>   The maximum range. </summary>
        int FMaxRange = 6;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti surface change indicator. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];
            // get current surfacewater withdrawal for urban
            double[] UrbanSW = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_UrbanSurfacewater).ProviderProperty.getvalues().Values,0);
            // get what we are using for stream flow
            double[] BaseFlow = SWIP.StreamFlow;

            // Calculate the CHange Indicator
            double[] ChangeIndicator = new double[FModelCount];
            for(int i=0;i<FModelCount;i++)
            {
                ChangeIndicator[i] = (1 - (Math.Log10((UrbanSW[i] / BaseFlow[i]) * 1000000) / (double)FMaxRange)) * 100;
            }
            // Convert and Bracket this indicator
            result = IndicatorTools.ConvertInt32AndBracket(ChangeIndicator, 0.001, 1, 100, 0);
            //
            return result;
        }

        public providerArrayProperty SurfaceChangeIndicator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            SurfaceChangeIndicator = new providerArrayProperty(FPM, eModelParam.epP_SWIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epSWIndicator, "Surfacewater Indicator", "SWI", SurfaceChangeIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSWIndicator, "Change in impact on Surface Water", "?", "?", "Surfacewater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    #endregion
    //=========================================================================================
    //  
    //   Agriculture Indicator
    //   Modified 2/18/18 Quay
    //   
    //=========================================================================================

    #region Agriculture Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ag indicator process. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class AGIndicatorProcess : AnnualFeedbackProcess
    {
        // <summary>   The state code. </summary>
        //int FStateCode = 0;
        // <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;

        /// <summary>   The base ag demand. </summary>
        double[] FAgDemand = null;
        // QUAY EDIT 3/18/18 
        // This is not needed now
        ///// <summary>   The fag rate. </summary>
        //double[] FAGRate = null;
        // End Edit 3/18/18
         
        /// <summary>   The annula Farm Net Income. </summary>
        int[] FAGNet = null;
        /// <summary>   The base AGNEt gallon per dollar. </summary>
        public double[] FAGGallonPerDollar = null;

        /// <summary> The base AGNET gallon per dollar.</summary>
        double[] FBaseAGGallonPerDollar = null;


        // EDIT QUAY 3/18/18
        // Not Needed
        ///// <summary>   The base percent. </summary>
        //double[] FBasePercent = null;
        ///// <summary>   The ag national maximum gpdd. </summary>
        //public const double AG_National_MAX_GPDD = 20.0;
        ///// <summary>   The ag national shift. </summary>
        //public const double AG_National_Shift = 1.2;
        // END EDIT 3/18/18

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public AGIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            // Build the Description Strings as required
            BuildDescStrings();
            // Set The name
            Fname = aName;
            this.Name = this.GetType().Name;  // System Name is class type
            // Set WatersimManager
            FWsim = WSim;
            // Get the West Model
            MyWaterSimModel = WSim.WaterSimWestModel;
            // Get the number of models for arrays
            FModelCount = MyWaterSimModel.ModelCount;
            // Set the Indicator Database link
            FIData = iData;
            // create variabl;e arrays
            // Store the annusl Ag Demand
            FAgDemand = new double[FModelCount];

            // QUAY EDIT 3/18/18 
            // This is not needed now
            //FAGRate = new double[FModelCount];
            // END EDIT 3/18/18
            // 
            // // this is the Farm Income.  (which changes each year)
            FAGNet = new int[FModelCount];
            // This is the primary value each year for indicator calculation
            FAGGallonPerDollar = new double[FModelCount];
            // Nopt Sure what this is used for, possible delete
            FBaseAGGallonPerDollar = new double[FModelCount];

            // QUAY EDIT 3/18/18 
            // This is not needed now
            //FBasePercent = new double[FModelCount];
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "Gallons per Agriculture $ Tracker";
            FProcessLongDescription = "Gallons Per  Agriculture DollarTracker Process: This process keeps track of the annual changes in the annual Gallons per Agricultual Dollar of production";
            FProcessCode = "AGPD";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> method that is called right before the first year of a simulation is called.</summary>
        ///
        /// <remarks> Year is the first year fo the simulation.  Input parameters available at this point
        ///     have been set for the Simulation, however ther is output data since the model has not yet
        ///     been run (ie Output values are not meaningful)</remarks>
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        /// <remarks>   Modified quasy 3/3/18 to get base data from GrowthRate table instead of indicator table
        ///             also stopped setting AgricultureNet with thjis routine
        ///             Modified Quay 3/18/18 Removed everything except initial GPDD</remarks>
        /// <param name="year"> The year about to be run.</param>
        /// <param name="WSim"> The WaterSimManager that is making call.</param>
        ///
        /// <returns> true if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // QUAY EDIT 3/18/18 
            // This is not needed now
            //// Get the indicator data
            //// Growth Rate is found in the base growth rate data, access it fom the WestModel
            //FAGRate = (WSim as WaterSimManager).WaterSimWestModel.ModelRateData.AGRate();
            // END EDIT 3/18/18

            // Net Farm Income
            ModelParameterClass MP = (WSim as WaterSimManager).ParamManager.Model_Parameter(eModelParam.epP_AgricultureProduction);
            FAGNet = MP.ProviderProperty.getvalues().Values;
            

            // Get initial AG Water Demand
            FAgDemand = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_Agriculture).ProviderProperty.getvalues().Values,0);
            
            //// Calcualte Base $ per Gallon
            //for (int i = 0; i < FModelCount; i++)
            //{
            //    FAGGallonPerDollar[i] = (double)FAgDemand[i] / (double)FAGNet[i];
            // QUAY EDIT 3/18/18 
            // This is not needed now
            //FBasePercent[i] = (FBaseAGGallonPerDollar[i] / AG_National_MAX_GPDD) * AG_National_Shift;
            // END EDIT 3/18/18
            //}

            //// Setup each modesl Intial Ag Parameters
            int cnt = 0;
            foreach (WaterSimCRFModel WCRF in MyWaterSimModel.WaterSimCRFModels)
            {
                // QUAY EDIT 3/18/18 
                // This is not needed now
                //WCRF.AgricultureGrowthRate = FAGRate[cnt];
                //WCRF.AgricultureInitialGPDD = FBaseAGGallonPerDollar[cnt];
                // WCRF.AgricultureNet = FBaseAGNet[cnt];
                // END EDIT 3/18/18
                if (FAGNet[cnt] > 0)
                {
                    FBaseAGGallonPerDollar[cnt] = (FAgDemand[cnt] * WaterSimCRFModel.convertDemand) /( (double)FAGNet[cnt] * WaterSimCRFModel.convertAgProduction);
                }
                    cnt++;

            }
            // END EDIT 3/18/18

            // Exit as you should
            return base.PostProcess(year, WSim);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Method that is called before after each annual run.</summary>
        ///
        /// <remarks> Override this method to do some activity after each annual run To test if this is the
        ///     first year use (year == Wsim.Simulation_Start_Year)
        ///     To test if this is the last year use  (year == Wsim.Simulation_End_Year)</remarks>
        ///
        /// <param name="year"> The year just run.</param>
        /// <param name="WSim"> The WaterSimManager that is making call.</param>
        ///
        /// <returns> true if it succeeds, false if it fails. Error should be placed in FErrorMessage.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // Net Farm Income
            FAGNet = (WSim as WaterSimManager).ParamManager.Model_Parameter(eModelParam.epP_AgricultureProduction).ProviderProperty.getvalues().Values;
            //// Get initial AG Water Demand
            FAgDemand = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_Agriculture).ProviderProperty.getvalues().Values, 0);
            //// Calcualte Base $ per Gallon
            for (int i = 0; i < FModelCount; i++)
            {
                if (FAGNet[i] > 0)
                {
                    FAGGallonPerDollar[i] = ((double)FAgDemand[i] * WaterSimCRFModel.convertDemand) / ((double)FAGNet[i] * WaterSimCRFModel.convertAgProduction);
                }
            }
            // OK exit as should
            return base.PostProcess(year, WSim);
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before each annual run. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PreProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PreProcess(int year, WaterSimManagerClass WSim)
        {
            return true;
        }

        // QUAY EDIT 3/18/18
        // THIS IS NOT NEEDED
        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the rate of growth. </summary>
        /////
        ///// <value> The rate of growth. </value>
        /////-------------------------------------------------------------------------------------------------
        //public double[] RateOfGrowth
        //{
        //    get { return FAGRate; }
        //}

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the base net. </summary>
        /////
        ///// <value> The base net. </value>
        /////-------------------------------------------------------------------------------------------------

        //public double[] BaseNet
        //{
        //    get { return FBaseAGNet; }
        //}
        // END EDIT 3/18/18

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base gallon per dollar. </summary>
        ///
        /// <value> The base gallon per dollar. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] GallonPerDollar
        {
            get { return FAGGallonPerDollar; }
        }

        /// <summary>
        /// Base Gallons Per AgNet - NOT SURE WHY WE ARE KEEPING THIS
        /// </summary>
        
        public double[] BaseGallonPerDollar
        {
            get { return FBaseAGGallonPerDollar; }
        }
        //
        // edits Sampson 08.10.18
        public int[] FarmIncome
        {
            get { return FAGNet; }
        }

        // end sampson edits
        // QUAY EDIT 3/18/18
        // THIS IS NOT NEEDED
        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the base percent. </summary>
        /////
        ///// <value> The base percent. </value>
        /////-------------------------------------------------------------------------------------------------

        //public double[] BasePercent
        //{
        //    get { return FBasePercent; }
        //}

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the national maximum gpdd. </summary>
        /////
        ///// <value> The national maximum gpdd. </value>
        /////-------------------------------------------------------------------------------------------------

        //public double National_Max_GPDD
        //{
        //    get { return AG_National_MAX_GPDD; }
        //}
        // END EDIT 3/18/18


    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Agriculture indicator. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class AgricultureIndicator : WebIndicator
    {
        /// <summary>   The agip. </summary>
        AGIndicatorProcess AGIP = null;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        int[] FAgTargetEfficiency = null;
        double[] FAGNET_GPDD_Goal = null;
        double[] FGoal_Percent = null;
        int[] FIndicator = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------
        public AgricultureIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            // Set WaterSimManager
            FWSim = WSim;
            // Set THe West Mode
            MyWaterSimModel = WSim.WaterSimWestModel;
            // Get the Model Count
            FModelCount = MyWaterSimModel.ModelCount;
            // GET THE Indicator Data reference
            FIData = iData;
            // ??
            FAgTargetEfficiency = new int[FModelCount];
            // Create AGNET GOal ARray
            FAGNET_GPDD_Goal = new double[FModelCount];
            FGoal_Percent = new double[FModelCount];
            FIndicator = new int[FModelCount];

            // Fetch the Goals from the Indicator Coef Database
            FAGNET_GPDD_Goal = FIData.AgricultureAGNETgoal();
            // Create the Ag Indicator Porcess
            AGIP = new AGIndicatorProcess("Agriculture Water Indicator Tracking", WSim, FIData);
            // Launch it
            FWSim.ProcessManager.AddProcess(AGIP);
            // Setup The Model Parameters
            CreateModelParameters();
        }

        //double[] AgScale = new double[10] { 0.02, 0.04, 0.1, 0.5, 1, 5, 10, 20, 30, 45 };
        //double[] InitialAgIndicatorScale = new double[10] { 0.04, 0.10, 0.25, 0.50, 0.75, 1.00, 2.00, 5.00, 10.00, 30.00 };

        /// <summary>   this is a 80% change with a 25% change. </summary>
        const double FAdjustFactor = 1.2;

        double AgSweetSpotLowFactor = 0.6;
        double AgSweetSpotHighFactor = 1.15;
        double AgIndSpotLow = 33;
        double AgIndSpotHigh = 66;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag indicator. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];

            double[] GallonsPerAGNET = AGIP.GallonPerDollar;
            //double[] CurrentDemand = new double[FModelCount];
            // get current demand and production from model
            double [] CurrentDemand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Agriculture).ProviderProperty.getvalues().Values,0); 
            double[] CurremtProduction = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_AgricultureProduction).ProviderProperty.getvalues().Values,0);

            // Calculate Adjusted percent
            //double[] CurrentAGPD = new double[FModelCount];
            //double[] RealPercent = new double[FModelCount];
            //double[] AdjustedPercent = new double[FModelCount];
            for (int i=0;i<FModelCount;i++)
            {
                // OK Expand this so we can see what is happening
                double GalPerAgNet = GallonsPerAGNET[i]*100;
                double GPDD_Goal = FAGNET_GPDD_Goal[i]*100;

                // OK here we go
                double AgSweetSpotLow = GPDD_Goal * AgSweetSpotLowFactor;
                double AgSweetSpotHigh = GPDD_Goal * AgSweetSpotHighFactor;
                //double Goal_PCT =  GPDD_Goal / GalPerAgNet;
                //FGoal_Percent[i] = GallonsPerAGNET[i] / FAGNET_GPDD_Goal[i];
                //FIndicator[i] = (int)(FGoal_Percent[i] * 100);

                double Alternative = IndicatorTools.SweetSpotIndicator(GalPerAgNet, AgSweetSpotLow, AgSweetSpotHigh, AgIndSpotLow, AgIndSpotHigh, 0, 100);
                FIndicator[i] = (int)Alternative;
                //CurrentAGPD[i] = CurrentDemand[i] / CurremtProduction[i];
                //RealPercent[i] = CurrentAGPD[i] / AGIP.National_Max_GPDD;
                //AdjustedPercent[i] = AGIP.BasePercent[i] + ((RealPercent[i] - AGIP.BasePercent[i]) * FAdjustFactor);
            }
            // Convert to int indicator
            result = FIndicator;
            //result = IndicatorTools.ConvertInt32AndBracket(AdjustedPercent, 0.001, 1, 100, 0);

            return result;
        }


        /// <summary>   The ag indicator. </summary>
        public providerArrayProperty AgIndicator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture target efficiency. </summary>
        ///
        /// <value> The agriculture target efficiency. </value>
        ///-------------------------------------------------------------------------------------------------

        public int[] AgricultureTargetEfficiency
        {
            get { return FAgTargetEfficiency; }
            set { FAgTargetEfficiency = value;  }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti base a gpd 100. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_BaseAGPD100()
        {
            int[] result = new int[FModelCount];

            // Calculate Base AGP as 100 base percent
            // and convert to int idnicator
            result = IndicatorTools.ConvertInt32AndBracket(AGIP.BaseGallonPerDollar, 0.001, 1, 100, 0);

            return result;
        }


        /// <summary> The base a gpd 100.</summary>
        providerArrayProperty BaseAGPD100;

        public int[] Geti_AgRate()
        {
            int[] result = new int[FModelCount];

            // Calculate Base AGP as 100 base percent
            // and convert to int idnicator
            result = IndicatorTools.ConvertInt32AndBracket(AGIP.BaseGallonPerDollar, 0.001, 1, 100, 0);

            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates the model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            BaseAGPD100 = new providerArrayProperty(FPM, eModelParam.epP_AgricultureBaseGPD100, Geti_BaseAGPD100, eProviderAggregateMode.agAverage);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_AgricultureBaseGPD100, "Ag Base GPD", eModelFields.epP_AgricultureBaseGPD100, BaseAGPD100));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureBaseGPD100, "Initial 100 Gallons per Dollar", "GP$", "100 Gallons Per Dollar", "Initial GPD", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            AgIndicator = new providerArrayProperty(FPM, eModelParam.epP_AgricultureIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epAgricultureIndicator, "Ag Indicator", eModelFields.epAgricultureIndicator, AgIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Efficiency of Ag Water Use", "?", "?", "Ag Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    #endregion
    //
    // ==================================================================================================================================================================================
    // ==================================================================================================================================================================================
    //
   

    //
    //==========================================================================================
    //  
    //  Urban Efficienct Indicator
    //  Modified 2/18/18 Quay
    //   
    //==========================================================================================

    #region Urban Efficiency Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Urban efficiency indicator. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class UrbanEfficiencyIndicator : WebIndicator
    {
        /// <summary>   The data. </summary>
        IndicatorDataClass FData = null;
        /// <summary>   The firmware simulation. </summary>
        WaterSimManager FWSim = null;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="TheWSManager"> Manager for the ws. </param>
        /// <param name="TheData">      Access to the Idicator Data. </param>
        ///-------------------------------------------------------------------------------------------------

        public UrbanEfficiencyIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
        {
            FData = TheData;
            FWSim = TheWSManager;
            MyWaterSimModel = FWSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;
            CreateModelParameters();
        }

        // QUAY EDIT 4/5/18
        // MAJOR EDIT HERE
        // Created a sweet spot tool which is then used here
        // uses high and low end of sweet spot along with indictor values for this sweetspot, values outside the sweet spot are 
         
        ///// <summary>   The under 100 factor. </summary>
        //double Under100Factor = 3.3;
        ///// <summary>   The over 100 divisor. </summary>
        //double Over100Divisor = 41;
        ///// <summary>   The over 100 beta. </summary>
        //double Over100Beta = 99;
        /// <summary>   The sweet spot low. </summary>
        double SweetSpotLow = 60;
        /// <summary>   The sweet spot high. </summary>
        double SweetSpotHigh = 110;
        ///// <summary>   The sweet spot indicator. </summary>
        //double SweetSpotIndicator = 38;
        ///// <summary>   The sweet spot adjust. </summary>
        //double SweetSpotAdjust = 65;

        double SweetSpotIndicatorLow = 33;
        double SweetSpotIndicatorHigh = 66;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti urban efficiency indicator. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];
            // get model parameters
            double[] UrbanDemand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Urban).ProviderProperty.getvalues().Values,0);
            double[] POP = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Population).ProviderProperty.getvalues().Values, 0);
            //double[] CurrentGPCD = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_GPCD_urban).ProviderProperty.getvalues().Values,0);
            // cacluclate GPCD
            double[] CALCGPCD = new double[FModelCount];
            // now convert to 1 to 100 sacle
            double[] GPCDIndicator = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                // be sure to convert to gallons ie * million
                CALCGPCD[i] = (UrbanDemand[i] * 1000000) / POP[i];

                // Green spot of inmdicator is centerd on a value of 38 with 0 the short red end and 100 the long red end
                // the sweet spot (center of green) is intended to be 95 to 105;
                //double IndicatorGPCD = SweetSpotIndicator;
                // OK lets expose this so easier to see numbers
                double TestGPCD = CALCGPCD[i];
                //if (TestGPCD < SweetSpotLow)
                //{
                //    // if under SweetSpotLow the indicator declines slower approaching 0 as GPCD gets smaller
                //    IndicatorGPCD = 100 / ((100 - TestGPCD) / Under100Factor);
                //}
                //else
                //{
                //    if (TestGPCD > SweetSpotHigh)
                //    {
                //        // if over SweetSpotHigh then increases slower as GPCD gets bigger
                //        IndicatorGPCD = Math.Log10(TestGPCD - Over100Beta) * Over100Divisor;
                //    }
                //    else
                //    {
                //        IndicatorGPCD = TestGPCD - SweetSpotAdjust;
                //    }
                //}
                double Alternative = IndicatorTools.SweetSpotIndicator(TestGPCD, SweetSpotLow, SweetSpotHigh, SweetSpotIndicatorLow , SweetSpotIndicatorHigh, 0, 100);
                GPCDIndicator[i] = Alternative;

                //                GPCDIndicator[i] = IndicatorGPCD;
            }

            // convert to Int Indicator
            result = IndicatorTools.ConvertDoubleToInt32(GPCDIndicator, 1);
                //            result = IndicatorTools.ConvertInt32AndBracket(GPCDIndicator, 1, 100, 1, 0);
            return result;
        }
        // EDN EDIT 4/5/18  MAJOR EDIT

        /// <summary> The urban efficiency indicator.</summary>
        public providerArrayProperty UrbanEfficiency;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            UrbanEfficiency = new providerArrayProperty(FPM, eModelParam.epP_UrbanEfficiencyIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epUrbanEfficiencyIndicator, "Urban Water Efficiency Stress", "UEF", UrbanEfficiency));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Urban Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Urban Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }


    #endregion Urban Efficiency

    //==========================================================================================
    //  
    //  Environment Indicator
    //  Modified 2/18/18 Quay
    //   
    //==========================================================================================

    #region Environment Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Environment process. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class EnvironmentProcess : AnnualFeedbackProcess
    {
        // EDIT QUAY 3/30/18
        // Added a base SUrfaceWater withdrawl field
        double[] FBaseSurface = null;
        // End EDIT
         
        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;
        /// <summary>   The fww flow. </summary>
        double[] FWWFlow = null;
        /// <summary>   The fww adjust. </summary>
        double[] FWWAdjust = null;
        /// <summary>   The firmware weight. </summary>
        double[] FWWeight = null;
        /// <summary>   The reset factor. </summary>
        double[] FResetFactor = null;
        /// <summary>   The initial dilution f dactor. </summary>
        double[] FInitialDilutionFDactor = null;
        /// <summary>   The initial indicator. </summary>
        double[] FInitialIndicator = null;

        /// <summary>   The base evironment surface goal. </summary>
        double[] FBaseEvnSurfaceGoal = null;
        /// <summary>   The current indicator score. </summary>
        double[] FCurrentIndicatorScore = null;

        /// <summary>   True to factors ready. </summary>
        bool FFactorsReady = false;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="WSim">     The WaterSimManager that is making call. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public EnvironmentProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            MyWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;
            FIData = iData;
            // Create variable Arrays
            FWWFlow = new double[FModelCount];
            FWWAdjust = new double[FModelCount];
            FWWeight = new double[FModelCount];
            FResetFactor = new double[FModelCount];
            FInitialDilutionFDactor = new double[FModelCount];
            FInitialIndicator = new double[FModelCount];
            FCurrentIndicatorScore = new double[FModelCount];
            // QUAY EDIT 3/30/18
            // ADDING THE BASE SURFACE
            FBaseSurface = new double[FModelCount];
            FBaseEvnSurfaceGoal =iData.BaseEnvSurfaceGoal();
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "Effluent Tracker";
            FProcessLongDescription = "Tracks gallons of effluent discharged";
            FProcessCode = "EFD";
        }

        /// <summary>   The maximum log value. </summary>
        double fMaxLogValue = 4;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the surface flow to surface withdraw ratio indicator. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="SurfaceFlow">          The surface flow. </param>
        /// <param name="WWDischarge">          The ww discharge. </param>
        /// <param name="UrbanSurfWithdrawal">  The urban surf withdrawal. </param>
        ///
        /// <returns>   The calculated surface flow to surface withdraw ratio indicator. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(double[] SurfaceFlow, double[] WWDischarge, double[] UrbanSurfWithdrawal)
        {
            // Ratio = Totalsurface flow divide by surface Water withdrawn
            // TotalSurface flow is (UrbanRiverFlow + UrbanWastewaterDischarge)
            // UrbanWastewaterDischarged is weight based on INdicator data from 1 to 100;  
            // Indicator is Log10 of Ration Divide by MaxLogValue from % states
            // EDIT QUAY 2/17/18
            // OK, these log functions are deadly slow while debugging and very slow running under release
            // For the purposes of this indictaor we just need an integer estmate of LOG10, not a precise double value.
            // A special function was created to create an intger log10 that is 25 times faster under debug and 5 times faster under release
            // SpecialValues.log10(double)  
            // It produces and accurate integer estimate of LOG10 with limitations, see definition or method
            // 
            double[] result = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                int BaseValue = SpecialValues.FastLog10((int)((SurfaceFlow[i] + (WWDischarge[i] * FWWeight[i])) / UrbanSurfWithdrawal[i]));
                double Temp1 = (((double)BaseValue / fMaxLogValue) * 100) - FResetFactor[i];
                //double Temp2 = ((Math.Log10((SurfaceFlow[i] + (WWDischarge[i] * FWWeight[i])) / UrbanSurfWithdrawal[i]) / fMaxLogValue) * 100) - FResetFactor[i];
                //result[i] = Temp2;
                result[i] = Temp1;
            }
            return result;
        }

        public double[] CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(double[] WWDischarge, double[] UrbanSurfWithdrawal, int[] TotalWastewater)
        {
            // 
            double[] result = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                double Ratio = 1;
                double RatioWasteToSurface = 1;
                double RatioDischargeToWaste = 1;
                if (UrbanSurfWithdrawal[i] > 0)
                {
                    RatioWasteToSurface = (double) TotalWastewater[i] / UrbanSurfWithdrawal[i];
                    if (TotalWastewater[i] > 0)
                        RatioDischargeToWaste = WWDischarge[i] / TotalWastewater[i];
                    Ratio = RatioDischargeToWaste * RatioWasteToSurface; 
                }
                result[i] = Ratio;
                //int BaseValue =  SpecialValues.FastLog10((int)((SurfaceFlow[i] + (WWDischarge[i] * FWWeight[i])) / UrbanSurfWithdrawal[i]));
                //double Temp1 = (((double)BaseValue / fMaxLogValue) * 100) - FResetFactor[i];
                ////double Temp2 = ((Math.Log10((SurfaceFlow[i] + (WWDischarge[i] * FWWeight[i])) / UrbanSurfWithdrawal[i]) / fMaxLogValue) * 100) - FResetFactor[i];
                ////result[i] = Temp2;
                //result[i] = Temp1;
                
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the dilution factor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WWFlow">       The ww flow. </param>
        /// <param name="Reclaimed">    The reclaimed. </param>
        ///
        /// <returns>   The calculated dilution factor doule[] </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] CalcDilutionFactor(double[] WWFlow, double[] Reclaimed)
        {
            double[] result = new double[FModelCount];
            if (FFactorsReady)
            {
                // to estimate how much of this is discharged 
                // adjust this for ocean lake outfalls and amount being reclaimed
                for (int i = 0; i < FModelCount; i++)
                {

                    double OceanLake = FWWAdjust[i];
                    double Adjustment = 0;

                    if (Reclaimed[i] > OceanLake)
                    {
                        Adjustment = Reclaimed[i];
                    }
                    else
                    {
                        Adjustment = OceanLake - Reclaimed[i];
                    }
                    // Estimate Discharged
                    double Discharged = WWFlow[i] - Adjustment;

                    // calculate the Dilution Factor
                    double DilutionFactor = (Discharged + FWWFlow[i]) / Discharged;

                    result[i] = DilutionFactor;
                }
            }
            return result;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Base surface water.</summary>
        /// <remarks> Quay, 4/2/2018.</remarks>
        /// <returns> A double[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseSurfaceWater()
        {
            return FBaseSurface;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Base surface goal. </summary>
        /// <remarks>   4/3/2018. </remarks>
        /// <returns>   A double[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseSurfaceGoal()
        {
            return FBaseEvnSurfaceGoal;
        }
        /// <summary>   The dilution range. </summary>
        const double FDilutionRange = 4000;

        /// <summary> The percent surface goal.</summary>
        const double FPercentSurfaceGoal = 0.4;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process for started simulation. </summary>
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // EDIT QUAY 3/30/18
            // Added this BaseSurface 
            // Get the current total surface water and use as a base
            int[] InitialSurface = (WSim as WaterSimManager).WaterSimWestModel.SurfaceFresh.getvalues().Values;
            int ModelCount = (WSim as WaterSimManager).WaterSimWestModel.ModelCount;
            
            // Create base 20% less The Surface Goal
            for (int i = 0; i < ModelCount; i++)
            {
                FBaseSurface[i] = (double)InitialSurface[i] * FBaseEvnSurfaceGoal[i];
            }
            // END EDIT


            // zero out cumulatives
            //// get the state code for this run
            //int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            // Get the indicator data

            FWWFlow = FIData.WastewaterSurfaceFlow();
            FWWAdjust = FIData.WastewaterAdjustment();
            FInitialIndicator = FIData.BaseEnvirionmentIndicator();
            FWWeight = FIData.WastewaterWeight();
            // initialize the reset factor 
            IndicatorTools.ZeroOut(ref FResetFactor);
            FFactorsReady = true;

            // Get Model Data
            double[] TotWWFlow = (WSim as WaterSimManager).Wastewater();
            double[] Reclaimed = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent).ProviderProperty.getvalues().Values,0);
            double[] USurf = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_UrbanSurfacewater).ProviderProperty.getvalues().Values, 0);
            // to estimate how much of this is discharged 
            // adjust this for ocean lake outfalls and amount being reclaimed
            double[] OceanLake = FWWAdjust;
            double Adjustment = 0.0;
            double[] Discharged = new double[FModelCount];

            for (int i = 0; i < FModelCount; i++)
            {
                if (Reclaimed[i] > OceanLake[i])
                {
                    Adjustment = Reclaimed[i];
                }
                else
                {
                    Adjustment = OceanLake[i] - Reclaimed[i];
                }
                Discharged[i] = TotWWFlow[i] - Adjustment;
            }
            // calculate the Dilution Factor

            double[] newIndicator = CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(FWWFlow, Discharged, USurf);
            // Calculate Reset Factor
            for (int i = 0; i < FModelCount; i++)
            {
                FResetFactor[i] = newIndicator[i] - FInitialIndicator[i];
            }
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <remarks>
        /// Override this method to do some activity after each annual run To test if this is the first
        /// year use (year == Wsim.Simulation_Start_Year)
        /// To test if this is the last year use  (year == Wsim.Simulation_End_Year)
        /// </remarks>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // get estimate of wastewater flow
            double[] TotWWFlow = (WSim as WaterSimManager).Wastewater();
            double[] Reclaimed = IndicatorTools.ConvertInt32ToDouble(WSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent).ProviderProperty.getvalues().Values,0);
            //// calculate the Dilution Factor
            double[] CurrentDilutionFactor = CalcDilutionFactor(TotWWFlow, Reclaimed);
            if (year == WSim.Simulation_Start_Year)
            {
                FInitialDilutionFDactor = CurrentDilutionFactor;
            }
            double[] Change = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
               Change[i] = CurrentDilutionFactor[i] / BaseDilutionFactor[i];
              FCurrentIndicatorScore[i] = BaseIndicator[i] / Change[i];
            }

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the current indicator score. </summary>
        ///
        /// <value> The current indicator score. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] CurrentIndicatorScore
        {
            get { return FCurrentIndicatorScore; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base dilution factor. </summary>
        ///
        /// <value> The base dilution factor. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseDilutionFactor
        {
            get { return FInitialDilutionFDactor; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base indicator. </summary>
        ///
        /// <value> The base indicator. </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] BaseIndicator
        {
            get { return FInitialIndicator; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the wastewater surface flow. </summary>
        ///
        /// <remarks>   Total mean flow of Rivers that have wastewater effluent discharge. </remarks>
        ///
        /// <value> The wastewater surface flow. (mgd) </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] WastewaterSurfaceFlow
        {
            get { return FWWFlow; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the wastewater discharged to lakes and oceansadjust. </summary>
        ///
        /// <remarks>
        /// Some wastewater effluent is discharged to Lakes and Ocean, this is the adjustment for that.
        /// </remarks>
        ///
        /// <value> The wastewater adjust. (mgd) </value>
        ///-------------------------------------------------------------------------------------------------

        public double[] WastewaterOceanLakeDischarge
        {
            get { return FWWAdjust; }
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An environment indicator. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class EnvironmentIndicator : WebIndicator
    {
        /// <summary>   The data. </summary>
        IndicatorDataClass FData = null;
        /// <summary>   The firmware simulation. </summary>
        WaterSimManager FWSim = null;
        /// <summary>   The fep. </summary>
        EnvironmentProcess FEP = null;
        /// <summary>   The base dilution. </summary>
        double FBaseDilution = 0.0;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="TheWSManager"> Manager for the ws. </param>
        /// <param name="TheData">      Information describing the. </param>
        ///-------------------------------------------------------------------------------------------------

        public EnvironmentIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
        {
            FWSim = TheWSManager;
            MyWaterSimModel = FWSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;
            FData = TheData;
            FEP = new EnvironmentProcess("Wastewater Tracker", FWSim, FData);
            FWSim.ProcessManager.AddProcess(FEP);
            CreateModelParameters();
        }

        /// <summary>   Values should range from 0 to 1900. </summary>
        const double DilutionRange = 1900;


        // The orignal way to do this was based on Thuys data, but we do not have that and what was then down below makes little sense
        //
        // Let's do somthing along these lines
        // Two things can be occuring, 
        //   1) the amount of surface water being taken from river means less water for riparian environment
        //   2) And some may be returned
        //        Ag may return some, and if ag uses mostly ground water, this could be more
        //        Cities and towns is only effluent discharge, but if mostly groundwater is the source, this could greatly offset surface removal or even add
        //        Power none
        //        Industry some, but polluted so lets just say none
        //  
        //  So we need two things, 
        //      we need a base to measure future surface withdrawls from
        //      and we need a way to measure net surface withdrawal, surface withdrawl - discharged 
        //  The first we will create in a post process.  Let's assume at the start everyone should return 50%  of the surface water they take.
        //  
        //  We can create a base surfacewater withdrawal of 50% of the usrfacewater being used at start of scenario
        //  in the process started.  This is set by constant FPercentSurfaceGoal in the envirionment process.
        //  
        //  Each year we will take surface water withdrawl that year and subtract from that discharge and 5% of agriculture
        //  use.  USGS (https://water.usgs.gov/edu/wuir.html  indicates 50% ag water is evaporated, 50 goes back to environment.  
        //  We are focused on water that returns to a river for riparian use, which seems like it would be low
        //  so this is a rough guess how much ends up back in river.  We will ignore quality at this
        //  point.  This is set by AgGiveBackPercent constant in this class.
        //  
        //  We will then divide by our base number.  As surface water use increases and or reclaimed use increases, this
        //  ratio will go down.  As surface ater use decreases and reclaimed water does not increase, this ratio will go up. 
        //  Communities with high groundwater and low surface water may have higher ratios with less reclaimed use.
        //  
        //  OK , lets' do it.
        //  
        const double AgGiveBackPercent = 0.15;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti environment. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];
            // get estimate of wastewater flow

            double[] TotWWFlow = FWSim.Wastewater();
            // adjust this

            // get UrbanSurfacewater
            double[] USurf = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_UrbanSurfacewater).ProviderProperty.getvalues().Values, 0);
            // get TotalSurfaceWater
            double[] Surf = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_SurfaceFresh).ProviderProperty.getvalues().Values, 0);

            // OK Adjust the, if Reclaimed is less than ocaen discharge, adjust difference, if Reclaimed is larger use difference
            double[] Reclaimed = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent).ProviderProperty.getvalues().Values, 0);
            double[] OceanLakeDischarge = FEP.WastewaterOceanLakeDischarge;
            double Adjustment = 0.0;
            double[] Discharged = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {

                if (Reclaimed[i] > OceanLakeDischarge[i])
                {
                    Adjustment = Reclaimed[i] - OceanLakeDischarge[i];
                }
                else
                {
                    Adjustment = OceanLakeDischarge[i] - Reclaimed[i];
                }
                // Estimate Discharged
                Discharged[i] = TotWWFlow[i] - Adjustment;

                if (Discharged[i] < 0)
                {
                    Discharged[i] = 0;
                }
                ////// calculate the Dilution Factor
                //double CurrentDilutionFactor = FEP.CalcDilutionFactor(TotWWFlow, Reclaimed);

                //double Change = FEP.BaseDilutionFactor / CurrentDilutionFactor;
                //            double NewIndicatorScore = FEP.BaseIndicator / Change;

            }

            // Create a new indicator array
            double[] NewIndicatorScore = new double[FModelCount];
            // get the base data
            double[] BaseSurface = FEP.BaseSurfaceWater();
            //// get the ag surface water use, this is an obscure parameter reference, and is slow, but should work
            //double[] AgSurface = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_SUR_AD).ProviderProperty.getvalues().Values, 0);
            double[] AgWater = IndicatorTools.ConvertInt32ToDouble(FWSim.WaterSimWestModel.Geti_Agriculture(),0);
            
            // Create a adjusted net surface use
            
            // QUAY EDIT 4/4/18
            // Changed ag from just surfece to all ag water, all ag water is used for irrigation not just surface just as all urban water drives effluent, not just surface
            //double[] AdjustedSurface = new double[FModelCount];
            // calculate the new ration
            for (int i = 0; i < FModelCount; i++)
            {
                
                // Take surfacewater subtract discharged and a percent of ag use
                // Spell this out so we can see these values
                double InitialSur = Surf[i];
                double TheDischarge = Discharged[i];
                //double AgPart = AgSurface[i] * AgGiveBackPercent;
                double AgPart = AgWater[i] * AgGiveBackPercent;
                // END EDIT 4/4/18
                double BasePart = BaseSurface[i];
                double AdjustedSurface = InitialSur - (TheDischarge + AgPart);
                // Calculate Ratio
                NewIndicatorScore[i] = 1;
                if (AdjustedSurface > 0)
                {
                    NewIndicatorScore[i] = BasePart / AdjustedSurface;

                    // OK it is possible this is bigger than 1 lets tone it down
                    if (NewIndicatorScore[i] > 1) NewIndicatorScore[i] = 1.0;
                }

                result[i] = (int)(NewIndicatorScore[i] * 100);
            }
            // OLD AMERICA CODE
            // NewIndicatorScore
            //double[] NewIndicatorScore = FEP.CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(FEP.WastewaterSurfaceFlow, Discharged, USurf);

            //// OK EXPLAIN THIS
            //// Seytup for new indicator
            //double[] NewIndicatorScore = new double[FModelCount];
            //// loop through the models
            //for (int i = 0; i < FModelCount; i++)
            //{
            //    // Set these to 1 (100) if numertaor is zero and coan not calc ratio
            //    double Ratio = 1;
            //    double RatioWasteToSurface = 1;
            //    double RatioDischargeToWaste = 1;
            //    // RatioWasteWaterToSurface is the ratio of TotalWastewater flow to total surface water withdrawn
            //    // For cities with High Groundwater and low surface this could be high
            //    // For cities with High surface water and low groundwater this will be lower
            //    // A higher ration, particulalry above 1 means wastewater could be added to env
            //    if (USurf[i] > 0)
            //    {
            //        RatioWasteToSurface = TotWWFlow[i] / Surf[i];
            //    }
            //    // RatioDischarge to Wastewater
            //    // The more relcaimed the less discharge, ration goes down, the less reclaimed the more dishcarge this goes up
            //    if (TotWWFlow[i] > 0)
            //    {
            //        RatioDischargeToWaste = 1 - (Discharged[i] / TotWWFlow[i]);
            //    }
            //    // Multiply these
            //    Ratio = RatioDischargeToWaste * RatioWasteToSurface;

            //    NewIndicatorScore[i] = Ratio;
            //}


//            //            double[] NewIndicatorScore = FEP.CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(Discharged, USurf, Wastewater);

//            double high = NewIndicatorScore.Max();
//            double low = NewIndicatorScore.Min();
//            result = IndicatorTools.ConvertInt32AndDistribute(NewIndicatorScore, 0, 100, 1, 0);
////            result = IndicatorTools.ConvertInt32AndBracket(NewIndicatorScore, 20, 80, 1, 0);

            return result;
        }


        /// <summary> The environment.</summary>
        public providerArrayProperty Environment;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            Environment = new providerArrayProperty(FPM, eModelParam.epP_ENVIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);
            //FPM.AddParameter(new ModelParameterClass(eModelParam.epENVIndicator, "Environment Indicators", "ENVIND", Geti_Environment));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epENVIndicator, "Impact of Influent Reductions on Changes in Surface Water Flows", "0-100", "", "Environment Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


        }
    }


    #endregion Environment Indicator

    //==========================================================================================
    /// Industry efficiency indicator. </summary>
    ///
    /// 2/16/2018
    //==========================================================================================

    #region industry Efficiency Indicator

    public class industryEfficiencyIndicator : WebIndicator
    {
        
        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="TheWSManager"> Manager for the ws. </param>
        /// <param name="TheData">      Access to the Idicator Data. </param>
        ///-------------------------------------------------------------------------------------------------

        public industryEfficiencyIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
       {
            FIData = TheData;
            FWSim = TheWSManager;
            MyWaterSimModel = FWSim.WaterSimWestModel;
            FModelCount = MyWaterSimModel.ModelCount;
            CreateModelParameters();
        }

        /// <summary>   The under 100 factor. </summary>
        double Under100Factor = 3.3;
        /// <summary>   The over 100 divisor. </summary>
        double Over100Divisor = 41;
        /// <summary>   The over 100 beta. </summary>
        double Over100Beta = 99;
        /// <summary>   The sweet spot low. </summary>
        double SweetSpotLow = 15;
        /// <summary>   The sweet spot high. </summary>
        double SweetSpotHigh = 45;

        /// <summary>   The maximum gpmw. </summary>
        double MaxGPMW = 250000;
        /// <summary>   The sweet spot indicator. </summary>
        double SweetSpotIndicator = 25;
        /// <summary>   The sweet spot adjust. </summary>
        double SweetSpotAdjust = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti industry efficiency indicator. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];
            // get model values
            double[] industryerDemand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Industrial).ProviderProperty.getvalues().Values,0);
            double[] industryEmployees = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_IndustryEmployees).ProviderProperty.getvalues().Values, 0);
            double[] CALC_GPED = new double[FModelCount];
            
            for (int i = 0; i < FModelCount; i++)
            {
                if (industryEmployees[i]>0)
                {
                    // Calculate the Gallons Per Employee per day
                    CALC_GPED[i] = (industryerDemand[i] * Generic.WaterSimCRFModel.convertDemand) / (industryEmployees[i] * Generic.WaterSimCRFModel.convertEmployee);

                }
            }
            // OK here is the fudge factor, lets assume we are a little inefficient, and our sweet spot is 70 gallons per employee
            // so we will fudge this to create a scor with 70 as 50   A range of  0 80 should fit this

            result = IndicatorTools.ConvertInt32AndDistribute(CALC_GPED, 0, 80, 1, 0);
//            result = IndicatorTools.ConvertInt32AndBracket(CALC_GPED, 0.001, 1, 100, 0);
            return result;
        }


        /// <summary> The power efficiency.</summary>
        public providerArrayProperty industryEfficiency;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            industryEfficiency = new providerArrayProperty(FPM, eModelParam.epP_IndustryEfficiency, Geti_Indicator, eProviderAggregateMode.agAverage);
           
        }
    }
  
    #endregion industry Efficiency Indicator

    //==========================================================================================
    ///-----------------------------------------------------------------------------------------
    /// A power efficiency indicator.
    ///-----------------------------------------------------------------------------------------

    #region Power Efficiency

    public class PowerEfficiencyIndicator :WebIndicator
            {
                /// <summary>   The data. </summary>
                IndicatorDataClass FData = null;
                /// <summary>   The firmware simulation. </summary>
        
                int FModelCount = 0;
                WaterSimModel MyWaterSimModel = null;

                ///-------------------------------------------------------------------------------------------------
                /// <summary>   Constructor. </summary>
                ///
                /// <remarks>   2/16/2018. </remarks>
                ///
                /// <param name="TheWSManager"> Manager for the ws. </param>
                /// <param name="TheData">      Access to the Idicator Data. </param>
                ///-------------------------------------------------------------------------------------------------

                public PowerEfficiencyIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
                {
                    FData = TheData;
                    FWSim = TheWSManager;
                    MyWaterSimModel = FWSim.WaterSimWestModel;
                    FModelCount = MyWaterSimModel.ModelCount;
                    CreateModelParameters();
                }

                /// <summary>   The under 100 factor. </summary>
                double Under100Factor = 3.3;
                /// <summary>   The over 100 divisor. </summary>
                double Over100Divisor = 41;
                /// <summary>   The over 100 beta. </summary>
                double Over100Beta = 99;
                /// <summary>   The sweet spot low. </summary>
                double SweetSpotLow = 15;
                /// <summary>   The sweet spot high. </summary>
                double SweetSpotHigh = 45;

                /// <summary>   The maximum gpmw. </summary>
                double MaxGPMW = 250000;
                /// <summary>   The sweet spot indicator. </summary>
                double SweetSpotIndicator = 25;
                /// <summary>   The sweet spot adjust. </summary>
                double SweetSpotAdjust = 0;

                ///-------------------------------------------------------------------------------------------------
                /// <summary>   Geti power efficiency indicator. </summary>
                ///
                /// <remarks>   2/16/2018. </remarks>
                ///
                /// <returns>   An int. </returns>
                ///-------------------------------------------------------------------------------------------------

                public override int[] Geti_Indicator() // PowerEfficiencyIndicator()
                {
                    int[] result = new int[FModelCount];
                    // get model values
                    double[] PowerDemand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Power).ProviderProperty.getvalues().Values, 0);
                    double[] PowerGen = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_PowerEnergy).ProviderProperty.getvalues().Values, 0);
                    //double PowerSaline = FWSim.ParamManager.Model_Parameter(eModelParam.epPowerSaline).Value;
                    // OK estimate how much of PowerGen is Saline
                    //double PowerGenSaline = PowerGen * (PowerSaline / PowerDemand);
                    // OK just want to use NON Saline water for GPMW
                    //double PowerGenNonSaline = PowerGen - PowerGenSaline;
                    //double CALCGPMW = 0;
                    //if (PowerGenNonSaline > 0)
                    //{
                    //    CALCGPMW = (PowerDemand - PowerSaline) / PowerGenNonSaline;
                    //}
                    double[] CALCGPMW = new double[FModelCount];
                    int[] IndInd = new int[FModelCount];

                    for (int i = 0; i < FModelCount; i++)
                    {
                        if (PowerGen[i] > 0)
                        {
                            CALCGPMW[i] = (((PowerDemand[i] * WaterSimCRFModel.convertDemand) / PowerGen[i]));// / MaxGPMW);
                        }
              
                    }
                    // Green spot of inmdicator is centerd on a average values of region which is 1800 with 0 the short red end and 100 the long red end
                    // so we can disburse from 0 to 100
                    IndInd = IndicatorTools.ConvertInt32AndDistribute(CALCGPMW, 0, 100, 1, 0);
                    // However, some regions indstry is 0 (or close to it), and that looks bad,
                    // So lets set those at the average 50 becuase there is nothing they can do
                    for (int i = 0; i < FModelCount; i++)
                    {
                        if (IndInd[i] <=1)
                        {
                            IndInd[i] = 50;
                        }
                    }
                    result = IndInd;
                    return result;
                   // result = IndicatorTools.ConvertInt32AndBracket(CALCGPMW, 0.001, 1, 100, 0);
                    //double IndicatorGPCD = CALCGPMW;
                    //if (CALCGPMW < SweetSpotLow)
                    //{
                    //    // if under 95 then declines faster as number gets smaller
                    //    IndicatorGPCD = 100 / ((100 - CALCGPMW) / Under100Factor);
                    //}
                    //else
                    //{
                    //    if (CALCGPMW > SweetSpotHigh)
                    //    {
                    //        // if over 105 then increases slower as number gets bigger
                    //        IndicatorGPCD = Math.Log10(CALCGPMW - Over100Beta) * Over100Divisor;

                    //    }
                    //    else
                    //    {
                    //        IndicatorGPCD = CALCGPMW - SweetSpotAdjust;
                    //    }
                    //}
                    //int tempint = 0;
                    //try
                    //{
                    //    tempint = Convert.ToInt32(IndicatorGPCD);
                    //    result = tempint;
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                   // return result;
                }


                /// <summary> The power efficiency.</summary>
                public providerArrayProperty PowerEfficiency;

                ///-------------------------------------------------------------------------------------------------
                /// <summary>   Creates model parameters. </summary>
                ///
                /// <remarks>   2/16/2018. </remarks>
                ///-------------------------------------------------------------------------------------------------

                public void CreateModelParameters()
                {
                    ParameterManagerClass FPM = FWSim.ParamManager;
                    Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

                    PowerEfficiency = new providerArrayProperty(FPM, eModelParam.epP_PowerEfficiency, Geti_Indicator, eProviderAggregateMode.agAverage);
                    //FPM.AddParameter(new ModelParameterClass(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "PEF", Geti_PowerEfficiencyIndicator));
                    //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Power Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                }
            }


    #endregion Power Efficiency
    // -----------------------------------------------------------------------------------------
    //      Assessment Indicator
    // -----------------------------------------------------------------------------------------
    #region Assessment
    ///--------------------------------------------------------------------
    /// <summary> The General Overall Assessment Indictor</summary>
    /// <remarks> Quay, 3/28/2018.</remarks>
    /// <remarks> This class creates and overall indicator based on the amount of deficit and the values for the
    ///           indicators included in the TheIndicators list</remarks>
    /// <seealso cref="T:WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class WebAssessmentIndicator : WebIndicator
    {
        List<WebIndicator> FTheIndicators = null;
        int FModelCount = 0;
        int FIndicatorCount = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WebAssessmentIndicator class.</summary>
        ///
        /// <remarks> Quay, 3/28/2018.</remarks>
        ///
        /// <param name="WSim">          The simulation.</param>
        /// <param name="TheIndicators"> the indicators.</param>
        /// <seealso cref="WebIndicator"/>
        ///-------------------------------------------------------------------------------------------------

        public WebAssessmentIndicator(WaterSimManager WSim, List<WebIndicator> TheIndicators)
        {
            // Set the indictor list
            FTheIndicators = TheIndicators;
            // Set WaterSimManger
            FWSim = WSim;

            // Set the model count
            FModelCount = FWSim.WaterSimWestModel.ModelCount;
            // Set the Indicator count
            FIndicatorCount = TheIndicators.Count;
            // Setup The Parmateres
            CreateModelParameters();
        }



        /// <summary> The General assessment.</summary>
        public providerArrayProperty Assessment;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti General indicator.</summary>
        /// <remarks> Quay, 3/28/2018. This is the primary method to retrieve a WebIndicators value.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public override int[] Geti_Indicator()
        {
            int[] result = new int[FModelCount];

            //// Setup the AllIndicators array and get all the values
            //int[][] AllIndicatorsByModel = new int[FTheIndicators.Count][];
            //for(int i=0;i<FIndicatorCount;i++)
            //{
            //    AllIndicatorsByModel[i] = FTheIndicators[i].Geti_Indicator();
            //}
            //// Now calculate the average of these
            //int[] AvgIndicator = new int[FModelCount];
            //// Loop through each of the models and calcualte total and count for all indicators and average for each model
            //for (int modeli = 0; modeli < FModelCount; modeli++)
            //{
            //    double Total = 0;
            //    double Cnt = 0;
            //    // loop through indicators
            //    for (int indi = 0; indi < FIndicatorCount; indi++)
            //    {
            //        // Do not count indicators that are zero, that is an error state
            //        if (AllIndicatorsByModel[indi][modeli]>0)
            //        {
            //            Total += AllIndicatorsByModel[indi][modeli];
            //            Cnt++;
            //        }
            //    }
            //    // ok have total and count, calculate average
            //    double Average = 0.0;
            //    if (Cnt > 0)
            //    {
            //        Average = Total / Cnt;
            //    }
            //    // Set the average
            //    AvgIndicator[modeli] = (int)Average;
            //}


            //// OK now get the Total Demand and the Net (deficit), 
            //// this should be the inverse of net to demand ratio, ie low net should be close to 100 
            //int[] TheReverseRatio = new int[FModelCount];
            //for (int modeli = 0; modeli < FModelCount; modeli++)
            //{
            //    // the reverse value, if demand =0 then this should be 1 (100)
            //    double ReverseValue = 1;
            //    // get net
            //    double TheNet = FWSim.WaterSimWestModel.WaterSimCRFModels[modeli].TotalNet;
            //    // get demand
            //    double TheDemand = FWSim.WaterSimWestModel.WaterSimCRFModels[modeli].TotalDemand;
            //    // only of demand >0
            //    if (TheDemand>0)
            //    {
            //        // calculate ratio
            //        double Temp = TheNet / TheDemand;
            //        // cap this at 1
            //        if (Temp > 1) Temp = 1;
            //        // figure out reverse
            //        ReverseValue = 1 - Temp;
            //    }
            //    // set values as int with 100 = 1;
            //    TheReverseRatio[modeli] = (int)(ReverseValue * 100);
            //}

            // Get the Average Sustainability
            int[] AvgIndicator = Geti_AverageSustainability();

            // Get Net DEmand ratio indicator
            int[] TheReverseRatio = Geti_NetDemandRatioIndicator();

            // OK, go ahead figure out the assessment value as average of net ratio and sustainability
            for (int modeli = 0; modeli < FModelCount; modeli++)
            {
                result[modeli] = (TheReverseRatio[modeli] + (AvgIndicator[modeli]*2)) / 3;
            }
            // return this
            return result;
        }

        // Sustainability Indicator
        public providerArrayProperty AverageSustainabilityIndicator;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti average sustainability.</summary>
        ///
        /// <remarks> Quay, 4/1/2018.</remarks>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_AverageSustainability()
        {

            // Setup the AllIndicators array and get all the values
            int[][] AllIndicatorsByModel = new int[FTheIndicators.Count][];
            for (int i = 0; i < FIndicatorCount; i++)
            {
                AllIndicatorsByModel[i] = FTheIndicators[i].Geti_Indicator();
            }
            // Now calculate the average of these
            int[] AvgIndicator = new int[FModelCount];
            // Loop through each of the models and calcualte total and count for all indicators and average for each model
            for (int modeli = 0; modeli < FModelCount; modeli++)
            {
                double Total = 0;
                double Cnt = 0;
                // loop through indicators
                for (int indi = 0; indi < FIndicatorCount; indi++)
                {
                    // Do not count indicators that are zero, that is an error state
                    if (AllIndicatorsByModel[indi][modeli] > 0)
                    {
                        Total += AllIndicatorsByModel[indi][modeli];
                        Cnt++;
                    }
                }
                // ok have total and count, calculate average
                double Average = 0.0;
                if (Cnt > 0)
                {
                    Average = Total / Cnt;
                }
                // Set the average
                AvgIndicator[modeli] = (int)Average;
            }
            return AvgIndicator;
        }


        /// <summary> The reverse net demand ratio.</summary>
        public providerArrayProperty ReverseNetDemandRatio;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti net/ demand ratio indicator.</summary>
        /// <remarks> Quay, 4/1/2018.</remarks>
        /// <remarks> This is the reverse of the ration of net demand to total demand, as net approaches zero
        ///           the indicator should approach 100</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_NetDemandRatioIndicator()
        {
            // OK now get the Total Demand and the Net (deficit), 
            // this should be the inverse of net to demand ratio, ie low net should be close to 100 
            int[] TheReverseRatio = new int[FModelCount];
            for (int modeli = 0; modeli < FModelCount; modeli++)
            {
                // the reverse value, if demand =0 then this should be 1 (100)
                double ReverseValue = 1;
                // get net
                double TheNet = FWSim.WaterSimWestModel.WaterSimCRFModels[modeli].TotalNet;
                // get demand
                double TheDemand = FWSim.WaterSimWestModel.WaterSimCRFModels[modeli].TotalDemand;
                // only of demand >0
                if (TheDemand > 0)
                {
                    // calculate ratio
                    double Temp = TheNet / TheDemand;
                    // cap this at 1
                    if (Temp > 1) Temp = 1;
                    // figure out reverse
                    ReverseValue = 1 - Temp;
                }
                // set values as int with 100 = 1;
                TheReverseRatio[modeli] = (int)(ReverseValue * 100);
            }
            // OK return this
            return TheReverseRatio;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Creates model parameters.</summary>
        ///
        /// <remarks> Quay, 4/1/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            Assessment = new providerArrayProperty(eModelParam.epP_Assessment, Geti_Indicator);
            AverageSustainabilityIndicator = new providerArrayProperty(eModelParam.epP_AverageSustainability, Geti_AverageSustainability);
            ReverseNetDemandRatio = new providerArrayProperty(eModelParam.epP_ReverseNetDemandRatio, Geti_NetDemandRatioIndicator);
                
        }


    }
    #endregion Assessment
    //
    // -----------------------------------------------------------------------------------------
    // Reclaimed Water Recycled Indicator
    // 08.08.18 sampson
    //
    // =========================================================================================

    #region Recycling Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>  Recycling indicator process. </summary>
    ///
    /// <remarks>   08.07.18 Sampson Edits </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------
    public class RecyclingIndicatorProcess : AnnualFeedbackProcess
    {
        // <summary>   The state code. </summary>
        //int FStateCode = 0;
        /// <summary>   The wsim. </summary>
        WaterSimManager FWsim = null;
        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;

        /// <summary>
        /// The Percent recycle value
        /// </summary>
        //int[] FRecycleValue = null;
        /// <summary>
        /// Total Demand Met using recycled water
        /// </summary>
        int[] FBaseTotalDemand = null;
        /// <summary>
           /// <summary>
        ///        /// The total sum of the fluxes for recycled water
        /// 08.07.18 das
        /// </summary>
        int[] FBaseRecycledDemand = null;
        //
        /// <summary>
        /// 
        /// </summary>
        int[] FBaseMaxRecycle = null;
        //
        int[] FBaseRECtoUrban = null;
        //
        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public RecyclingIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            // Build the description strings as required
            BuildDescStrings();
            // Set the names
            Fname = aName;
            this.Name = this.GetType().Name;
            // Set WaterSim Manage3r
            FWsim = WSim;
            // Get The West Model
            MyWaterSimModel = WSim.WaterSimWestModel;
            // The Model Count
            FModelCount = MyWaterSimModel.ModelCount;
            // Get the Indicator Database
            FIData = iData;

            // create variable Arrays
            //FRecycleValue = new int[FModelCount];
            FBaseTotalDemand = new int[FModelCount];
            FBaseRecycledDemand = new int[FModelCount];
            FBaseMaxRecycle = new int[FModelCount];
            FBaseRECtoUrban = new int[FModelCount];
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Recycling Ratio and Value Tracker Process: This process keeps track of recycling impact of reduce water use";
            FProcessCode = "RRT";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// method that is called right before the first year of a simulation is called.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {

            // get the data - These are initial data: one rune

            FBaseTotalDemand = (WSim as WaterSimManager).WaterSimWestModel.Geti_TotalDemand();
            //          
            FBaseRecycledDemand = (WSim as WaterSimManager).WaterSimWestModel.Geti_Effluent();
            // this needs confirmation
            FBaseMaxRecycle = (WSim as WaterSimManager).WaterSimWestModel.Geti_AvailableReclaimed();
            // 08.18.18
            FBaseRECtoUrban = (WSim as WaterSimManager).WaterSimWestModel.Geti_REC_UD();
            //
            return true;
        }
        //
      
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
             return true;
        }

        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Maximum available recycled water
        /// </summary>
        public int [] RecycleMax
        {
            get { return FBaseMaxRecycle; }
        }
        /// <summary>
        /// recycled water used
        /// </summary>
        public int[] Recycled
        {
            get { return FBaseRecycledDemand; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int[]ReclaimedBase
        {
            get { return FBaseRECtoUrban; }
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A web indicator economic.</summary>
    /// <remarks>   2/16/2018. Modified  Quay, 2/19/2018.</remarks>
    /// <seealso cref="T:WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class RecyclingIndicator : WebIndicator
    {
        /// <summary>   The feip. </summary>
        RecyclingIndicatorProcess FRIP = null;

        WaterSimModel MYWaterSimModel = null;
        int FModelCount = 0;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///
        /// ### <param name="BaseEModelParam">  The base model parameter. </param>
        ///-------------------------------------------------------------------------------------------------
        public RecyclingIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            FWSim = WSim;
            MYWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MYWaterSimModel.ModelCount;
            //            FBaseParam = BaseEModelParam;
            FIData = iData;
            FRIP = new RecyclingIndicatorProcess("Recycling Indicator Tracking", WSim, FIData);
            FWSim.ProcessManager.AddProcess(FRIP);
            CreateModelParameters();
        }
        // 08.18.18
        /// <summary>
        ///  Grab the base value of the flux of reclaimed to Urban data value
        /// </summary>
        /// <returns></returns>
        public int[] Geti_RECtoUrban()
        {
            int[] RECtoUrban = new int[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                RECtoUrban[i] = FRIP.ReclaimedBase[i];
            }

                return RECtoUrban;
        }

            /// <summary> The percent of available effluent that is used (recycled).</summary>
            providerArrayProperty PercentRecycled;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti recycled water percent of available effluent</summary>
        ///
        /// <remarks>   08.07.18  </remarks>
        ///
        /// <returns>   An int []. </returns>
        ///-------------------------------------------------------------------------------------------------
        public int[] Geti_PCTRecycled()
        {
            // initialize 
            int[] RecToUrban = new int[FModelCount];
            int[] multiply = new int[FModelCount];
            int[] PercentRecycled = new int[FModelCount];
            int[] recycle = new int[FModelCount];
            double[] d_recycle = new double[FModelCount];
            int[] maxRecycle = new int[FModelCount];
            double[] d_maxRecycle = new double[FModelCount];

            recycle = FWSim.WaterSimWestModel.Geti_Effluent();
            maxRecycle = FWSim.WaterSimWestModel.Geti_AvailableReclaimed();
            //
            // Loop through can calcualte PctRecycled
            for (int i = 0; i < FModelCount; i++)
            {
                double Temp = 0;
                d_recycle[i] = recycle[i];
                d_maxRecycle[i] = maxRecycle[i];
                //          
                try
                {
                    if (maxRecycle[i] > 0)
                    Temp = (d_recycle[i] / d_maxRecycle[i]);
                }
                finally
                {
                    PercentRecycled[i] = Convert.ToInt32(Temp*100);
                }
            }
            //
           // RecToUrban = this.MYWaterSimModel.Geti_GrayWaterFlow();
           // multiply = this.MYWaterSimModel.Geti_GrayWaterManagement();
         
            //grayWaterPotential(RecToUrban, multiply);
            return PercentRecycled;
        }
        // -------------------------------------------------------------------------------------------------
        providerArrayProperty PercentRecycledOfDemand;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] Geti_PCTRecycledDemand()
        {
            // initialize 
            int[] PercentRecycledDemand = new int[FModelCount];
            int[] recycle = new int[FModelCount];
            double[] d_recycle = new double[FModelCount];


            double[] d_demand = new double[FModelCount];
            int[] demand = new int[FModelCount];
            recycle = FWSim.WaterSimWestModel.Geti_Effluent();
            demand = FWSim.WaterSimWestModel.Geti_TotalDemand();
            // Loop through can calcualte PctRecycled
            for (int i = 0; i < FModelCount; i++)
            {
                double Temp = 0;
                d_recycle[i] = recycle[i];
                d_demand[i] = demand[i];

                try
                {
                    if(demand[i] > 0)
                    Temp = (d_recycle[i] / d_demand[i]);
                }
                finally
                {
                    PercentRecycledDemand[i] = Convert.ToInt32(Temp * 100);
                }
            }
            return PercentRecycledDemand;
        }
        //
        ProviderIntArray In = new ProviderIntArray(0);

        internal int[] MyValue = new int[ProviderClass.NumberOfProviders];


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="baseRecUrban"></param>
        ///// <param name="management"></param>
        //public void grayWaterPotential(int[] baseRecUrban, int[] management)
        //{
        //    int[] gray = new int[FModelCount];
        //    gray = this.MYWaterSimModel.Geti_GrayWaterManagement();
        //    //double[] Gbase = new double[FModelCount];
        //    int[] iGray = new int[FModelCount];
        //    double G = 0;
        //    for (int i = 0; i < FModelCount; i++)
        //    {
        //        int result = 0;
        //        double Temp = 0;
        //        G = (double)gray[i];
        //        if (baseRecUrban[i] == 0)
        //            Temp = management[i] * G;
        //        else
        //            Temp = baseRecUrban[i];
        //        result = (int)Temp;
        //        iGray[i] = result;
        //    }
        //    In.Values = iGray;
        //    ModelParameterClass MP = FWSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent);
        //    // MP.ProviderProperty.setvalues(In);
        //    this.MYWaterSimModel.Seti_Effluent(iGray);
        //   //  this.MYWaterSimModel.Effluent.setvalues(In);

        //}
        //
        // FINISH THIS CODE ..!!!!!!! 08.28.18 das Need to set a new Effluent Limit.
        //
        //
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty RecycleIndicator;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti recycling indicators. </summary>
        /// <remarks>   08.07.2018 Sampson
        ///             have a choice of indicators to use. Efficiency as a % of reclaimed water used
        ///             in relation to that produced, or as a % of reclaimed water used in relation
        ///             to the total water demand. I suspect that we will want to use as a % of
        ///             total demand. BECAUSE: the effficiency control sets the efficiency %. i.e.,
        ///             
        ///             PercentRecycledOfDemand versus PercentRecycled
        /// </remarks>
        /// <returns>   An int[] </returns>
        ///-------------------------------------------------------------------------------------------------
        public override int[] Geti_Indicator()
        {
             int[] TempInt = Geti_PCTRecycledDemand();
            //int[] TempInt = Geti_PCTRecycled();
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates the model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            PercentRecycled = new providerArrayProperty(FPM, eModelParam.epP_PCTRecycled, Geti_PCTRecycled, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_PCTRecycled, "Percent of available reclaimed water used", "PCTR", PercentRecycled));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PCTRecycled, "The ratio of reclaimed water used to total available effluent", "%", "Percent", "Percent Recycled", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            PercentRecycledOfDemand = new providerArrayProperty(FPM, eModelParam.epP_PCTRecycledOfTotDem, Geti_PCTRecycledDemand, eProviderAggregateMode.agSum);
            FPM.AddParameter(new ModelParameterClass(eModelParam.epP_PCTRecycledOfTotDem, "The Percent of reclaimed water recycled in relation to total demand", "PCTRD", PercentRecycledOfDemand));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PCTRecycledOfTotDem, "The ratio of reclaimed water used to total demand", "%", "Percent", "Percent Recycled of Demand", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            RecycleIndicator = new providerArrayProperty(FPM, eModelParam.epP_RecyclingIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);

        }
    }
    #endregion Recycling Intidator 
    // =========================================================================================
    // -----------------------------------------------------------------------------------------
    // Reducing Water Use Indicator
    // 08.09.18 sampson
    //
    // =========================================================================================

    #region Reducing Water use Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>  Recycling indicator process. </summary>
    ///
    /// <remarks>   08.07.18 Sampson Edits </remarks>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------
    public class ReduceUseindicatorProcess : AnnualFeedbackProcess
    {
        // <summary>   The state code. </summary>
        //int FStateCode = 0;
        /// <summary>   The wsim. </summary>
        WaterSimManager FWsim = null;
        /// <summary>   Information describing the fi. </summary>
        IndicatorDataClass FIData = null;
        //
        AGIndicatorProcess AIP;
        //  /// <summary>
        /// 
        /// </summary>
        int[] FBaseMax = null;
        //
        int[] FBaseMinAg = null;

        int FModelCount = 0;
        WaterSimModel MyWaterSimModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>
        /// It should not be assumed that the WaterSimManager value being passed is the WaterSimManager
        /// that will make' pre and post process calls.
        /// </remarks>
        ///
        /// <param name="aName">    The name of the process. </param>
        /// <param name="WSim">     The WaterSimManager Using the Process. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public ReduceUseindicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            // Build the description strings as required
            BuildDescStrings();
            // Set the names
            Fname = aName;
            this.Name = this.GetType().Name;
            // Set WaterSim Manage3r
            FWsim = WSim;
            // Get The West Model
            MyWaterSimModel = WSim.WaterSimWestModel;
            // The Model Count
            FModelCount = MyWaterSimModel.ModelCount;
            // Get the Indicator Database
            FIData = iData;
            //
            // create variable Arrays
            FBaseMax = new int[FModelCount];
        }
 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Reduce Use Process: This process keeps track of reducing water use ";
            FProcessCode = "RUT";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// method that is called right before the first year of a simulation is called.
        /// </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {

            // get the data - These are initial data: one run

            FBaseMax = (WSim as WaterSimManager).WaterSimWestModel.Geti_TotalDemand();

            //
            return true;
        }
        //

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails. Error should be placed in FErrorMessage.
        /// </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            return true;
        }

        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Maximum available recycled water
        /// </summary>
        public int[] DemandMax
        {
            get { return FBaseMax; }
        }
     }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A web indicator economic.</summary>
    /// <remarks>   2/16/2018. Modified  Quay, 2/19/2018.</remarks>
    /// <seealso cref="T:WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class ReduceUseIndicator : WebIndicator
    {
        /// <summary>   The feip. </summary>
        ReduceUseindicatorProcess FRUIP = null;

        WaterSimModel MYWaterSimModel = null;
        //
        AGIndicatorProcess AIP;
        //
        int FModelCount = 0;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///
        /// ### <param name="BaseEModelParam">  The base model parameter. </param>
        ///-------------------------------------------------------------------------------------------------
        public ReduceUseIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            FWSim = WSim;
            MYWaterSimModel = WSim.WaterSimWestModel;
            FModelCount = MYWaterSimModel.ModelCount;
            //            FBaseParam = BaseEModelParam;
            FIData = iData;
            FRUIP = new ReduceUseindicatorProcess("Reduce Use Indicator Tracking", WSim, FIData);
            FWSim.ProcessManager.AddProcess(FRUIP);

            //AGIndicatorProcess AIP;
            ProcessManager PM = WSim.ProcessManager;
            List<string> MyProcessNames = PM.ActiveProcesses;

            foreach (AnnualFeedbackProcess AFP in PM.AllProcesses())
            {
                string AFPName = AFP.Name;
                if (AFPName == "AGIndicatorProcess")
                {
                    if (AFP is AGIndicatorProcess)
                        AIP = (AFP as AGIndicatorProcess);
                    break;
                }
            }

            CreateModelParameters();
        }
        // -------------------------------------------------------------------------------------------------
        // 
        #region generic code
        // -------------------------------------------------------------------------------------------------
        private double d_min = 0;
        private double d_max = 0;
        internal double Min
        {
            set { d_min = value; }
            get { return d_min; }
        }
        internal double Max
        {
            set { d_max = value; }
            get { return d_max; }
        }
        private double d_difference;
        internal double DifferenceMinMax
        {
            set { d_difference = value; }
            get { return d_difference; }
        }
        internal double[] GrabMinandMax(double[] input, double[] demand, double[] totDemand)
        {
            double[] result = new double[FModelCount];
            double[] temp = new double[FModelCount];
            double[] normal = new double[FModelCount];
            double[] weight = new double[FModelCount];
            double[] Temp = new double[FModelCount];
            //
            for (int i = 0; i < FModelCount; i++)
            {
                temp[i] = input[i];
            }
            //
            Max = temp.Max();
            Min = temp.Min();
            //
            if (0 < Max)
            {
                DifferenceMinMax = Max - Min;
                normal = Normal(input);
            }
            weight = weighting(demand, totDemand);
            for (int i = 0; i < FModelCount; i++)
            {
                Temp[i] = normal[i] * weight[i];
            }
            result = Temp;
            return result;
        }
        //
        internal double[] Normalized;
        internal double[] Normalize
        {
            set { Normalized = value; }
            get { return Normalized; }

        }
        double[] Normal(double[] input)
        {
            double[] x = new double[FModelCount];
            double[] result = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                x[i] = input[i];
                double scaleMax = 100;
                result[i] = ((scaleMax / DifferenceMinMax) * (x[i] - Min));
            }
            return result;
        }
        double[] weighting(double[] demand, double[] totalDemand)
        {
            double[] result = new double[FModelCount];
            for (int i = 0; i < FModelCount; i++)
            {
                result[i] = 1;
                if(0 < totalDemand[i]) result[i] = demand[i] / totalDemand[i];
            }
                return result;
        }
        #endregion generic code

        // -------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Returns gallons from Ag
        /// </summary>
        internal double[] Farm(double[] TD)
        {
            double[] result = new double[FModelCount];          
            double[] GPD = new double[FModelCount];
            GPD = AIP.FAGGallonPerDollar;
            //int[] AD = FWSim.WaterSimWestModel.Geti_Agriculture();
            double[] Demand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Agriculture).ProviderProperty.getvalues().Values, 0);
            result = GrabMinandMax(GPD,Demand, TD);
            return result;
        }
        /// <summary>
        /// returns gallons from the Cities and towns efficiency 
        /// </summary>
        internal double[] CitiesAndTowns(double[] TD)
        {
            double[] result = new double[FModelCount];
            double[] gpcd = new double[FModelCount];
            //
            gpcd = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_GPCD_urban).ProviderProperty.getvalues().Values, 0);
            int[] GPCD = FWSim.WaterSimWestModel.Geti_gpcd();
            //people = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Population).ProviderProperty.getvalues().Values, 0);
            //
            double[] Demand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Urban).ProviderProperty.getvalues().Values, 0);
            result = GrabMinandMax(gpcd, Demand, TD);
            return result;
        }
        //
        internal double[] Industry(double[] TD)
        {
            double[] result = new double[FModelCount];
            double[] gallonsPerEmployee = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_IndustrialGPED).ProviderProperty.getvalues().Values, 0);
            //
            double[] Demand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Industrial).ProviderProperty.getvalues().Values, 0);
            result = GrabMinandMax(gallonsPerEmployee, Demand, TD);
            return result;
        }
        //
        internal double[] Power(double[] TD)
        {
            double[] result = new double[FModelCount];
            double[] temp = new double[FModelCount];
            int[] PW = FWSim.WaterSimWestModel.Geti_PowerGPMWD();
            temp = IndicatorTools.ConvertInt32ToDouble(PW, 0); 
            double[] Demand = IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_Power).ProviderProperty.getvalues().Values, 0);
            result = GrabMinandMax(temp, Demand, TD);
            return result;
        }

        //
        // For some reason this is called twice in 2015 - first call has no data
        //
        bool _start = false;
        bool Start
        {
            set { _start = value; }
            get { return _start; }

        }
        //
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty reduceUseIndicator;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] Geti_ReduceUseResult()
        {
            int[] result = new int[FModelCount];
            double[] farm = new double[FModelCount];
            double[] CandT = new double[FModelCount];
            double[] Ind = new double[FModelCount];
            double[] Pow = new double[FModelCount];
            //
            double[] temp = new double[FModelCount];
            int[] i_temp = new int[FModelCount];

            // initialize 
            //double[] TD = new double[FModelCount];
            double[] TD= IndicatorTools.ConvertInt32ToDouble(FWSim.ParamManager.Model_Parameter(eModelParam.epP_TotalDemand).ProviderProperty.getvalues().Values, 0);
            farm = Farm(TD);
            CandT = CitiesAndTowns(TD);
            Ind = Industry(TD);
            Pow = Power(TD);
               // Loop through can calcualte 
            for (int i = 0; i < FModelCount; i++)
            {
                temp[i] = 0;
                try
                {
                     temp[i] = farm[i] + CandT[i] + Ind[i] + Pow[i];
                     i_temp[i] = Convert.ToInt32(temp[i]);
                }
               finally
                {
                    result[i] = i_temp[i];
                }
            }
            return result;
        }
        //
         ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the indicators. </summary>
        /// <remarks>   08.09.2018 Sampson
        ///          
        ///             
        ///          
        /// </remarks>
        /// <returns>   An int[] </returns>
        ///-------------------------------------------------------------------------------------------------
        public override int[] Geti_Indicator()
        {
            int[] TempInt = Geti_ReduceUseResult();
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates the model parameters. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            reduceUseIndicator = new providerArrayProperty(FPM, eModelParam.epP_ReduceUseIndicator, Geti_Indicator, eProviderAggregateMode.agAverage);
        }
    }
    #endregion Reducing Water Use Indicator
    // =========================================================================================



    #endregion Indicators


    //##########################################################################
    //  
    //   WaterSimManager  Partial Classes
    //    
    //##########################################################################
    #region WaterSimManager Classes

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Model parameter. </summary>
    ///
    /// <remarks>   2/16/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static partial class eModelParam
    {
        // =======================================================
        // Single Model Parameters
        // ======================================================
        /// <summary>   The ep gw indicator base. </summary>
        public const int epGWIndicatorBase = 200;
        /// <summary>   The ep gw annual indicator. </summary>
        public const int epGW_Annual_Indicator = 201;
        /// <summary>   The ep gw cumulative indicator. </summary>
        public const int epGW_Cumulative_Indicator = 202;
        /// <summary>   The ep gw cumulative safe yield net. </summary>
        public const int epGW_Cumulative_SafeYieldNet = 203;
        /// <summary>   The ep gw safe yield reduction goal. </summary>
        public const int epGW_SafeYield_Reduction_Goal = 204;
        /// <summary>   The ep gw safe yield. </summary>
        public const int epGW_SafeYield = 205;

        /// <summary>   The ep eco indicator base. </summary>
        public const int epECOIndicatorBase = 210;
        /// <summary>   The ep eco demand. </summary>
        public const int epECODemand = 211;
        /// <summary>   The ep eco ratio. </summary>
        public const int epECORatio = 212;

        /// <summary>   The ep software indicator base. </summary>
        public const int epSWIndicatorBase = 220;
        /// <summary>   The ep software indicator. </summary>
        public const int epSWIndicator = 221;

        /// <summary>   The ep environment indicator base. </summary>
        public const int epENVIndicatorBase = 230;
        /// <summary>   The ep environment indicator. </summary>
        public const int epENVIndicator = 231;

        /// <summary>   The ep uef indicator base. </summary>
        public const int epUEFIndicatorBase = 240;
        /// <summary>   The ep urban efficiency indicator. </summary>
        public const int epUrbanEfficiencyIndicator = 241;

        /// <summary>   The ep an ef indicator base. </summary>
        public const int epAEFIndicatorBase = 250;
        /// <summary>   The ep agricuture gallons per dollar. </summary>
        public const int epAgricutureGallonsPerDollar = 251;

        /// <summary>   The ep agriculture base gpd 100. </summary>
        public const int epAgricultureBaseGPD100 = 254;
        /// <summary>   The ep agricuture rate adjust. </summary>
        public const int epAgricutureRateAdjust = 255;
        /// <summary>   The ep agriculture indicator. </summary>
        public const int epAgricultureIndicator = 257;

        /// <summary>   The ep pef indicator base. </summary>
        public const int epPEFIndicatorBase = 260;
        /// <summary>   The ep power efficiency. </summary>
        public const int epPowerEfficiency = 261;


        /// <summary>   The ep total demand. </summary>
        public const int epTotalDemand = 291;
        /// <summary>   The ep total demand net. </summary>
        public const int epTotalDemandNet = 292;
        /// <summary>   The ep total gpcy. </summary>
        public const int epTotalGPCY = 293;


        // =======================================================
        // Multi Model Parameters
        // ======================================================
        /// <summary>   The ep gw indicator base. </summary>
        public const int epP_GWIndicatorBase = 1200;
        /// <summary>   The ep gw annual indicator. </summary>
        public const int epP_GW_Annual_Indicator = 1201;
        /// <summary>   The ep gw cumulative indicator. </summary>
        public const int epP_GW_Cumulative_Indicator = 1202;
        /// <summary>   The ep gw cumulative safe yield net. </summary>
        public const int epP_GW_Cumulative_SafeYieldNet = 1203;
        /// <summary>   The ep gw safe yield reduction goal. </summary>
        public const int epP_GW_SafeYield_Reduction_Goal = 1204;
        /// <summary>   The ep gw safe yield. </summary>
        public const int epP_GW_SafeYield = 1205;

        /// <summary>   The ep eco indicator base. </summary>
        public const int epP_ECOIndicatorBase = 1210;
        /// <summary>   The ep eco demand. </summary>
        public const int epP_ECODemand = 1211;
        /// <summary>   The ep eco ratio. </summary>
        public const int epP_ECORatio = 1212;

        /// <summary>   The ep software indicator base. </summary>
        public const int epP_SWIndicatorBase = 1220;
        /// <summary>   The ep software indicator. </summary>
        public const int epP_SWIndicator = 1221;

        /// <summary>   The ep environment indicator base. </summary>
        public const int epP_ENVIndicatorBase = 1230;
        /// <summary>   The ep environment indicator. </summary>
        public const int epP_ENVIndicator = 1231;

        /// <summary>   The ep uef indicator base. </summary>
        public const int epP_UEFIndicatorBase = 1240;
        /// <summary>   The ep urban efficiency indicator. </summary>
        public const int epP_UrbanEfficiencyIndicator = 1241;

        /// <summary>   The ep an ef indicator base. </summary>
        public const int epP_AEFIndicatorBase = 1250;
        /// <summary>   The ep agricuture gallons per dollar. </summary>
        //public const int epP_AgricutureGallonsPerDollar = 1251;

        public const int epP_AgricultureTargetEfficiency = 1252;

        /// <summary>   The ep agriculture base gpd 100. </summary>
        public const int epP_AgricultureBaseGPD100 = 1254;
        /// <summary>   The ep agricuture rate adjust. </summary>
        public const int epP_AgricutureRateAdjust = 1255;
        /// <summary>   The ep agriculture indicator. </summary>
        public const int epP_AgricultureIndicator = 1257;

        /// <summary>   The ep Power Efficicieny indicator base. </summary>
        public const int epP_PEFIndicatorBase = 1260;
        /// <summary>   The ep power efficiency. </summary>
        public const int epP_PowerEfficiency = 1261;
        // EDIT QUAY 3/11/18
        /// <summary> The ep industry efficiency.</summary>
        public const int epP_IndustryEfficiency = 1262;
        // EDN EDIT 3/11/18     
        // EDIT QUAY 3/28/18
        /// <summary> The Assessment Indicator.</summary>
        public const int epP_Assessment = 1263;

        /// <summary> The ep average sustainability.</summary>
        public const int epP_AverageSustainability = 1264;

        /// <summary> The ep reverse net demand ratio.</summary>
        public const int epP_ReverseNetDemandRatio = 1265;
        //
        // Sampson edits 08.07.18
        public const int epP_PCTRecycled = 1270;
        /// <summary>   The ep eco ratio. </summary>
        public const int epP_PCTRecycledOfTotDem = 1271;
        public const int epP_RecyclingIndicator = 1272;
        // end Sampson edits 08.07.18
        //
        // Sampson edits 08.09.18
        public const int epP_ReduceUseIndicator = 1275;
        // end Sampson edits 08.09.18

        // END EDIT
        // <summary>   The ep total demand. </summary>
        //public const int epP_TotalDemand = 1291;
        // <summary>   The ep total demand net. </summary>
        //public const int epP_TotalDemandNet = 1292;
        // <summary>   The ep total gpcy. </summary>
        public const int epP_TotalGPCY = 1293;
        //public const int epMagicPop = 294;
        //public const int epUrbanSurfacewater = 295;
        //public const int epSurfaceLake = 296;
        //public const int epPowerSurfacewater = 297;
        //public const int epPowerSaline = 298;
        //public const int epPowerGW = 299;


    }
    /// <summary>
    /// 
    /// </summary>
    public partial class eModelFields
    {
        /// <summary>   The ep gw indicator base. </summary>
        public const string epP_GWIndicatorBase = "GWI_P";
        /// <summary>   The ep gw annual indicator. </summary>
        public const string epP_GW_Annual_Indicator = "GWSYA_P";
        /// <summary>   The ep gw cumulative indicator. </summary>
        public const string epP_GW_Cumulative_Indicator = "GWSYC_P";
        /// <summary>   The ep gw cumulative safe yield net. </summary>
        public const string epP_GW_Cumulative_SafeYieldNet = "GWSYN_P";
        /// <summary>   The ep gw safe yield reduction goal. </summary>
        public const string epP_GW_SafeYield_Reduction_Goal = "GWSYG_P";
        /// <summary>   The ep gw safe yield. </summary>
        public const string epP_GW_SafeYield = "GWSY_P";

        /// <summary>   The ep eco indicator base. </summary>
        public const string epP_ECOIndicatorBase = "ECO_P";
        /// <summary>   The ep eco demand. </summary>
        public const string epP_ECODemand = "ECOD_P";
        /// <summary>   The ep eco ratio. </summary>
        public const string epP_ECORatio = "ECOR_P";

        /// <summary>   The ep software indicator base. </summary>
        public const string epP_SWIndicatorBase = "SWIB_P";
        /// <summary>   The ep software indicator. </summary>
        public const string epP_SWIndicator = "SWI_P";

        /// <summary>   The ep environment indicator base. </summary>
        public const string epP_ENVIndicatorBase = "EVINDB_P";
        /// <summary>   The ep environment indicator. </summary>
        public const string epP_ENVIndicator = "EVIND_P";

        /// <summary>   The ep uef indicator base. </summary>
        public const string epP_UEFIndicatorBase = "UEFB_P";
        /// <summary>   The ep urban efficiency indicator. </summary>
        public const string epP_UrbanEfficiencyIndicator = "UEF_P";

        /// <summary>   The agriculture efficiency indicator base. </summary>
        public const string epP_AEFIndicatorBase = "AGINDB_P";
        /// <summary>   The ep agricuture gallons per dollar. </summary>
        public const string epP_AgricutureGallonsPerDollar = "AGGPD_P";

        /// <summary>   The ep agriculture base gpd 100. </summary>
        public const string epP_AgricultureBaseGPD100 = "ABGPD100_P";
        /// <summary>   The ep agricuture rate adjust. </summary>
        public const string epP_AgricutureRateAdjust = "AGCON_P";
        /// <summary>   The ep agriculture indicator. </summary>
        public const string epP_AgricultureIndicator = "AGIND_P";
        public const string epP_AgricuktureTargetEfficiency = "AGTEFF_P";

        /// <summary>   The ep Power Efficiency indicator base. </summary>
        public const string epP_PEFIndicatorBase = "PEFB_P";
        /// <summary>   The ep power efficiency. </summary>
        public const string epP_PowerEfficiency = "PEF_P";

        /// <summary> The ep industrial efficiency.</summary>
        public const string epP_IndustrialEfficiency = "IEF_P";

        /// <summary> The ep assessment.</summary>
        public const string epP_Assessment = "SAI_P";

        /// <summary> The ep average sustainability.</summary>
        public const string epP_AverageSustainability = "ASI_P";

        /// <summary> The ep reverse net demand ratio.</summary>
        public const string epP_ReverseNetDemandRatio = "RNDR_P";

        /// <summary>   The ep total demand. </summary>
        public const string epP_TotalDemand = "TD_P";
        /// <summary>   The ep total demand net. </summary>
        public const string epP_TotalDemandNet = "TDN_P";
        /// <summary>   The ep total gpcy. </summary>
        public const string epP_TotalGPCY = "TGPCY_P";
        //
        // Sampson edits 08.08.18
        /// <summary>
        /// The parameter used as the indicator for % reclaimed water use
        /// </summary>
        public const string epP_RECYCIndicator = "RECIND_P";
        /// <summary>
        /// 
        /// </summary>
        public const string epP_ReduceUseIndicator = "RUIND_P";
        // end edits Sampson 08.08.18
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for water simulations. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public partial class WaterSimManager : WaterSimManagerClass
    {
        /// <summary>   Information describing the indicator. </summary>
        protected IndicatorDataClass IndicatorData;
        /// <summary>   The gw indicator. </summary>
        protected WebIndicator_GroundWater GWIndicator;
        /// <summary>   The ecoi ndicator. </summary>
        protected WebIndicator_Economic ECOINdicator;
        /// <summary>   The sur indicator. </summary>
        protected SurfaceWaterIndicator SURIndicator;
        /// <summary>   The ag indicator. </summary>
        protected AgricultureIndicator AGIndicator;
        /// <summary>   The uef indicator. </summary>
        protected UrbanEfficiencyIndicator UEFIndicator;
        /// <summary>   The environment indicator. </summary>
        protected EnvironmentIndicator ENVIndicator;
        /// <summary>   The pow indicator. </summary>
        protected PowerEfficiencyIndicator POWIndicator;
        // EDIT QUAY 3/11/18
        /// <summary> The ind indicator.</summary>
        protected industryEfficiencyIndicator INDIndicator;
        // Sampson edits 08.08.18
        /// <summary>
        /// 
        /// </summary>
        protected RecyclingIndicator RECYCIndicator;
        // ENd Sampson edits 08.08.18
        //Sampson edits 08.09.18
        /// <summary>
        /// 
        /// </summary>
        protected ReduceUseIndicator REDUSEIndicator;
        // End Sampson edits 08.09.18

        /// <summary> The assess indicator.</summary>
        protected WebAssessmentIndicator AssessIndicator;
        // EDN EDIT 3/11/18
       

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the wastewater. </summary>
        ///
        /// <remarks>   2/16/2018. modified Quay 2/18/18</remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double Wastewater(string UnitName)
        {
            WaterSimCRFModel WSmith = WestModel.GetUnitModel(UnitName);
            double TempValue = 0;
            if (WSmith != null)
            {
                TempValue = WSmith.MaxReclaimed();
            }
            return TempValue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the wastewater.</summary>
        ///
        /// <remarks> Quay 2/18/18.</remarks>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double[] Wastewater()
        {
            // Get Size of Provider Array
            int ModelNumber = WestModel.ModelCount;
            // create array
            double[] Values = new double[ModelNumber];
            int cnt = 0;
            // loop through models to set values
            foreach (WaterSimCRFModel CRFM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = (int)CRFM.WasteWaterFlow();
                // increment
                cnt++;
            }
            return Values;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total demand. </summary>
        /// <remarks> Modified Quay 2/18/18</remarks>
        /// <value> The total number of demand. </value>
        ///-------------------------------------------------------------------------------------------------

        //public int TotalModelDemand(string UnitName)
        //{
        //    return Geti_TotalDemand(UnitName); 
        //}

        //===================================================================================================
        // Total GAllons per Capita per year
        //===================================================================================================

        #region TotalGAllonsperCapitaperyear


        /// <summary> The total gpcy.</summary>
        // Why is this called GPCY?, I have no idea
        // 
        public providerArrayProperty TotalGPCY;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti total gpcy for all models.</summary>
        /// <remarks> Quay, 2/18/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        protected int[] Geti_TotalGPCY()
        {
            int[] TheTotaldemand =  WestModel.Geti_TotalDemand();//  1000000.0;
            int ModelCnt = WestModel.ModelCount;
            int[] POP = ParamManager.Model_Parameter(eModelParam.epP_Population).ProviderProperty.getvalues().Values; //.epMagicPop).Value;
            int[] Values = new int[ModelCnt];
            for (int i = 0; i < ModelCnt; i++)
            {
                double TempGPCY = -1;
                if (0 < POP[i]) TempGPCY = ((double)TheTotaldemand[i]* 1000000) / (double)POP[i];
                int TempInt = 0;
                try
                {
                    TempInt = Convert.ToInt32(TempGPCY);
                }
                finally
                {
                    Values[i] = TempInt;
                }
            }
            return Values;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total gpcd. </summary>
        /// <remarks> modified Quay 2/18/18</remarks>
        /// <value> The total number of gpcd. </value>
        ///-------------------------------------------------------------------------------------------------

        //public int[] TotalGPCY
        //{
        //    get { return Geti_TotalGPCY(); }
        //}

#endregion
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated. </summary>
        ///
        /// <remarks>   Mcquay, 3/15/2016. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="Res">  The resource. </param>
        /// <param name="Cons"> The cons. </param>
        ///
        /// <returns>   The flux allocated. </returns>
        ///-------------------------------------------------------------------------------------------------

        int getFluxAllocated(ConsumerResourceModelFramework.CRF_Resource Res, ConsumerResourceModelFramework.CRF_Consumer Cons)
        {
            int result = 0;
            ConsumerResourceModelFramework.CRF_Flux TheFlux = Res.ToFluxs.FindTarget(Cons.Name);
            if (TheFlux != null)
            {
                double value = TheFlux.Allocated();
                try
                {
                    int valueInt = Convert.ToInt32(value);
                    result = valueInt;
                }
                catch (Exception ex)
                {
                    // ouch
                    throw new Exception("Error Getting Flux " + Res.Name + " to " + Cons.Name + " with value " + value.ToString());
                }
            }
            return result; 
        }

        //int getFluxAllocated(string ResourceStr, string ConsStr)
        //{
        //    int result = 0;
        //    ConsumerResourceModelFramework.CRF_Flux TheFlux = Res.ToFluxs.FindTarget(Cons.Name);
        //    if (TheFlux != null)
        //    {
        //        double value = TheFlux.Allocated();
        //        try
        //        {
        //            int valueInt = Convert.ToInt32(value);
        //            result = valueInt;
        //        }
        //        catch (Exception ex)
        //        {
        //            // ouch
        //            throw new Exception("Error Getting Flux " + Res.Name + " to " + Cons.Name + " with value " + value.ToString());
        //        }
        //    }
        //    return result;
        //}

        #region UrbanSurfaceWater


        /// <summary> The urban surface water.</summary>
        providerArrayProperty UrbanSurfaceWater;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti urban surface water.</summary>
        /// <remarks>   2/16/2018. modified quay 2/18/18 Changed StateData to UnitData add reference to WaterSimCRF Model </remarks>
        /// <param name="WSmith"> The WaterSim Model</param>
        /// <returns> An int. -1 if conversion error</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_UrbanSurfaceWater(WaterSimCRFModel WSmith)
        {
            int result = -1;
            double UrbanSW = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Resource Res in CRFNetwork.Resources)
            {
                if (Res.Name == UnitData.SurfaceWaterFieldname)
                {
                    ConsumerResourceModelFramework.CRF_Flux UrbanConFlux = Res.ToFluxs.FindTarget(UnitData.UrbanWaterFieldname);
                    if (UrbanConFlux != null)
                    {
                        UrbanSW = UrbanConFlux.Allocated();
                        break;
                    }
                }
            }
            try
            {
                result = Convert.ToInt32(UrbanSW);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets urban surface water for all Models.</summary>
        /// <remarks> Quay, 2/18/2018. </remarks>
        /// <returns> An array of int. -1 value means conversion error</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_UrbanSurfaceWater()
        {
            int NumberOfModels = WestModel.ModelCount;
            int[] Values = new int[NumberOfModels];
            int cnt = 0;
            foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = Geti_UrbanSurfaceWater(WSM);
                cnt++;
            }
            return Values;
        }

        #endregion UrbanSurfaceWater

        //==============================================================
        // Power Saline
        // =============================================================
        #region PowerSaline


        /// <summary> The power saline.</summary>
        providerArrayProperty PowerSaline;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti power saline. </summary>
        /// <remarks>   2/16/2018. modified quay 2/18/18 Quay changed statedata to unitdata and add WaerSImCRFModel parameter</remarks>
        /// <param name="WSmith"> The WaterSim Model</param>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_PowerSaline(WaterSimCRFModel WSmith)
        {
            int result = -1;
            double PowerSal = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Resource Res in CRFNetwork.Resources)
            {
                if (Res.Name == UnitData.SalineWaterFieldname)
                {
                    ConsumerResourceModelFramework.CRF_Flux PowerConFlux = Res.ToFluxs.FindTarget(UnitData.PowerWaterFieldname);
                    if (PowerConFlux != null)
                    {
                        PowerSal = PowerConFlux.Allocated();
                    }
                }
            }
            try
            {
                result = Convert.ToInt32(PowerSal);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the power saline for all models.</summary>
        /// <remarks> Quay 2/18/18 </remarks>
        /// <returns> An int array, -1 means a conversion error</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_PowerSaline()
        {
            int NumberOfModels = WestModel.ModelCount;
            int[] Values = new int[NumberOfModels];
            int cnt = 0;
            foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = Geti_PowerSaline(WSM);
            }
            return Values;
        }

        #endregion PowerSaline

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti surface lake From CRF_Network. </summary>
        /// <remarks>   2/16/2018. modified quay 2/18/18 Quay change added WaerSImCRFModel parameter and error -1</remarks>
        /// <param name="WSmith"> The WaterSim Model</param>
        /// <returns>   An int. -1 if conversion error</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_SurfaceLake(WaterSimCRFModel WSmith)
        {
            int result = -1;
            double temp = WSmith.TheCRFNetwork.SurfaceLake.Limit;
            try
            {
                int tempint = Convert.ToInt32(Math.Round(temp));
                result = tempint;
            }
            catch (Exception ex)
            {
                // ouch
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the geti surface lake From CRF_Network.</summary>
        /// <remarks>2/18/18 Quay </remarks>
        /// <returns> An int. -1 if conversion error.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_SurfaceLake()
        {
            int NumberOfModels = WestModel.ModelCount;
            int[] Values = new int[NumberOfModels];
            int cnt = 0;
            foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = Geti_SurfaceLake(WSM);
            }
            return Values;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti agriculture demand. </summary>
        /// <remarks>   2/16/2018. modified 2/18/18 Quay 
        ///             changed statedata to unitdata and 
        ///             added WaterSImCRFModel parameter
        ///             made -1 value on conversion error</remarks>
        /// <param name="WSmith"> The WaterSim Model</param>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_NetWorkAgricultureDemand(WaterSimCRFModel WSmith)
        {
            int result = -1;
            double AgDemand = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Consumer Con in CRFNetwork.Consumers)
            {
                if (Con.Name == UnitData.AgricultureFieldname)
                {
                            AgDemand = Con.Demand;
                            break;
                }
            }
            try
            {
                result = Convert.ToInt32(AgDemand);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the geti agriculture demand.</summary>
        ///
        /// <remarks> 2/16/2018 Quay</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] Geti_NetWorkAgricultureDemand()
        {
            int NumberOfModels = WestModel.ModelCount;
            int[] Values = new int[NumberOfModels];
            int cnt = 0;
            foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = Geti_NetWorkAgricultureDemand(WSM);
            }
            return Values;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti population. </summary>
        /// <remarks>   2/16/2018. modified 2/18/18 Quay 
        ///             added WaterSImCRFModel parameter
        ///             made -1 value on conversion error</remarks>
        /// <param name="WSmith"> The WaterSim Model</param>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        // ?? NEEDS WORK  CRFNetwork not used!!  Population set to 0  Wha is this
        public int Geti_Population(WaterSimCRFModel WSmith)
        {
            int result = -1;
            double Pop2010 = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            try
            {
                result = Convert.ToInt32(Pop2010);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dampen rate. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <param name="rate">     The rate. </param>
        /// <param name="damper">   Gets the geti ag production. </param>
        /// <param name="period">   The period. </param>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        double DampenRate(double rate, double damper, double period )
        {
            double NewRate = 0;
            NewRate = rate * Math.Pow(damper, -1 * period);
            return NewRate;
        }


        const double Damper = 1.01;

        //======================================================================
        // Agriculture Target Efficiency
        // ====================================================================
        #region AgTargetEfficiency


        //int FAgTargetEfficieny = 100;



        /// <summary> The ag target efficiency.</summary>
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Agricuture Target Efficiency.</summary>
        /// <remarks> Quay, 2/18/2018  Revised Sunstantially 3/14/18.</remarks>
        /// <remarks>   2/16/2018. Completely rewritten to be mukti model 
        ///             3/14/18  Completely rewritten concept</remarks>
        /// <param name="WSmith"> The WaterSim Model.</param>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------
        providerArrayProperty AgTargetEfficiency;

        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Agricuture Target Efficiency.</summary>
        ///// <remarks> Quay, 2/18/2018  Revised Sunstantially 3/14/18.</remarks>
        ///// <remarks>   2/16/2018. Completely rewritten to be mukti model 
        /////             3/14/18  Completely rewritten concept</remarks>
        ///// <param name="WSmith"> The WaterSim Model.</param>
        ///// <returns> An int.</returns>
        /////-------------------------------------------------------------------------------------------------

        //private int Geti_agTargetEfficiency(WaterSimCRFModel WSmith)
        //{
        //    double Temp = WSmith.AgricultureInitialGPDD;
        //    int result = -1;
        //    try
        //    {
        //        result = Convert.ToInt32(Temp);
        //    }
        //    catch (Exception ex)
        //    {
        //        // OUCH!
        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti ag target efficiency.</summary>
        /// <remarks> 2/18/18 Quay.</remarks>
        /// <returns> An int[]</returns>
        ///-------------------------------------------------------------------------------------------------

        private int[] Geti_agTargetEfficiency()
        {
            return AGIndicator.AgricultureTargetEfficiency;
                //int NumberOfModels = WestModel.ModelCount;
                //int[] Values = new int[NumberOfModels];
                //int cnt = 0;
                //foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
                //{
                //    Values[cnt] = Geti_agTargetEfficiency(WSM);
                //}
                //return Values;

        }
        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Seti ag target efficiency. </summary>
        /////
        ///// <remarks>   2/16/2018. rewritten 2/18/18 Quay</remarks>
        /////
        ///// <param name="value">    The value. </param>
        /////-------------------------------------------------------------------------------------------------

        //void Seti_agTargetEfficiency(int value, WaterSimCRFModel WSmith)
        //{
        //    //FAgTargetEfficieny = value;
        //    //WSmith.AgricultureTargetGPDD = value;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag target efficiency for all models.</summary>
        ///
        /// <remarks> Quay, 2/18/2018. </remarks>
        ///
        /// <param name="values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        void Seti_agTargetEfficiency(int[] values)
        {
            AGIndicator.AgricultureTargetEfficiency = values;
            //int cnt = 0;
            //foreach (WaterSimCRFModel WSCRF in WestModel.WaterSimCRFModels)
            //{
            //    //WSCRF.AgricultureTargetGPDD = values[cnt];
            //    cnt++;
            //}
        }

        #endregion AgTargetEfficiency

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the initial power generated. </summary>
        ///
        /// <remarks>   2/16/2018. modofied 2/18/18 Quay</remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_InitialPowerGenerated(WaterSimCRFModel WSmith)
        {
            int result = 0;
            //int StateCode = ParamManager.Model_Parameter(eModelParam.epState).Value;
            //string Statename = FStateNames[StateCode];
            result = WSmith.TheCRFNetwork.InitialPowerGenerated; // (Statename);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initial power generated.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] InitialPowerGenerated()
        {
            int NumberOfModels = WestModel.ModelCount;
            int[] Values = new int[NumberOfModels];
            int cnt = 0;
            foreach (WaterSimCRFModel WSM in WestModel.WaterSimCRFModels)
            {
                Values[cnt] = Geti_InitialPowerGenerated(WSM);
            }
            return Values;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <returns>   The flux allocated, 0 if not found. </returns>
        ///-------------------------------------------------------------------------------------------------

        //public int GetFluxAllocated(string ResField, string ConsField)
        //{
        //    int result = 0;
        //    CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField,ConsField);
        //    if (theFlux != null)
        //    {
        //        double value = theFlux.Allocated();
        //        try
        //        {
        //            int tempint = Convert.ToInt32(value);
        //            result = tempint;
        //        }
        //        catch (Exception ex)
        //        {
        //            // ouch
        //        }
        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <param name="aValue">       The value. </param>
        ///-------------------------------------------------------------------------------------------------

        //public void SetFluxAllocated(string ResField, string ConsField, double aValue)
        //{
        //    CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField, ConsField);
        //    if (theFlux != null)
        //    {
        //        theFlux.SetAllocation(aValue);
        //    }
        //}

        //public int Geti_SUR_PD()
        //{
        //    int result = GetFluxAllocated("SUR", "PTOT");
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the indicators. </summary>
        ///
        /// <remarks>   2/16/2018. Extensively Modified 2/18/18 Quay</remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool initializeIndicators()
        {
            bool result = true;
            try
            {
                Extended_Parameter_Documentation ExtendDoc = ParamManager.Extended;

                // EDIT QUAY
                // ALL THESE PRAMETERS GOT MOVED TO WATERSIMMANAGER
                // New Parameters
                //WestModel.Population = new providerArrayProperty(_pm, eModelParam.epP_Population, WestModel.Geti_Pop, eProviderAggregateMode.agSum);
                //_pm.AddParameter(new ModelParameterClass(eModelParam.epP_Population, "Population Served", "POP_P", WestModel.Population));
                //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Population, "State Population People in any given year- we use an estimate of slope to project out to 2065", "ppl", "State Population (ppl)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                //TotalDemand = new providerArrayProperty(ParamManager, eModelParam.epP_TotalDemand, Geti_TotalDemand, eProviderAggregateMode.agSum);
                //ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_TotalDemand, "Total Demand", eModelFields.epP_TotalDemand, TotalDemand));
                //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_TotalDemand, "Total Water Demand for all consumers", "MG", "Total Water DEmand (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                //TotalDemandNet = new providerArrayProperty(ParamManager, eModelParam.epP_TotalDemandNet, Geti_TotalDemand, eProviderAggregateMode.agSum);
                //ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_TotalDemandNet, "Total Demand (Net)", eModelFields.epP_TotalDemandNet, TotalDemandNet));
                //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_TotalDemandNet, "Total NetWater Demand for all consumers, essentially Water Sources - Demand", "MGD", "Total Net Water DEmand (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
                // END EDIT

                TotalGPCY = new providerArrayProperty(ParamManager, eModelParam.epP_TotalGPCY, Geti_TotalGPCY, eProviderAggregateMode.agSum);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_TotalGPCY, "Total GPCY", eModelFields.epP_TotalGPCY, TotalGPCY));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_TotalGPCY, "Total Gallons Per Capita for all consumers per year, Essentailly Total Demand divided by Population", "GPCY", "Total Gallons Per Capita Per Year (G)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                UrbanSurfaceWater = new providerArrayProperty(ParamManager, eModelParam.epP_UrbanSurfacewater, Geti_UrbanSurfaceWater, eProviderAggregateMode.agSum);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_UrbanSurfacewater, "Urban Surfacewater", "USUR_P", UrbanSurfaceWater));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_UrbanSurfacewater, "Urban Surfacewater: Amount of Surface water used to meet Urban Demand", "MGD", "Million Gallons Per Day", "Urban Surfacewater", new string[] { "No Change", "Moderate", "Extreme" }, new int[] { 100, 70, 30 }, new ModelParameterGroupClass[] { }));

                PowerSaline = new providerArrayProperty(ParamManager, eModelParam.epP_PowerSaline, Geti_PowerSaline, eProviderAggregateMode.agSum);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_PowerSaline, "Power Saline","PSAL_P", PowerSaline));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerSaline, "Power Saline: Amount of Saline water used to meet Power Demand", "MGD", "Million Gallons Per Day", "Urban Surfacewater", new string[] { "No Change", "Moderate", "Extreme" }, new int[] { 100, 70, 30 }, new ModelParameterGroupClass[] { }));







                //               ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricultureDemand, "Agriculture Demand", "ADP", Geti_AgDemand));
                //                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricultureProduction, "Agriculture Net $", "ANP", Geti_AgProduction));

                AgTargetEfficiency = new providerArrayProperty(ParamManager, eModelParam.epP_AgricultureTargetEfficiency, Geti_agTargetEfficiency, Seti_agTargetEfficiency, eProviderAggregateMode.agAverage);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_AgricultureTargetEfficiency, "Ag Target Efficiency", eModelFields.epP_AgricuktureTargetEfficiency, rangeChecktype.rctCheckRange, 0, 10000, null, AgTargetEfficiency ));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureTargetEfficiency, "Target Ag Efficiency", "GPDD", "Gallons Per Dollar FArm Income per Day", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                //ParamManager.AddParameter(new ModelParameterClass(eModelParam.epSurfaceLake, "Surface Lake Water", "SURL", Geti_SurfaceLake));
                //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceLake, "Surface Lake Water", "mgd", "Million Gallons Per Day", "Lake Water", new string[] {}, new int[] { }, new ModelParameterGroupClass[] { }));
                
                // Load the data
                try
                {
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "IndicatorData.csv");
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "ElevenStateIndicatorData.csv");
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "WestRegionsIndicatorData_v2.csv");
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "WestRegionsIndicatorData_v3.csv");
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "WestRegionsIndicatorData_v4.csv");
                    //  IndicatorData = new IndicatorDataClass(DataDirectory, "ArizonaWebAppRegionsIndicatorData_v5.csv");
                    IndicatorData = new IndicatorDataClass(DataDirectory, "ArizonaWebAppRegionsIndicatorData_v6.csv");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // I changed the use of this control switch to allow the model to run two years - 2015 and 2016 - before a policy in implemented
                // so that the default values from just11StatesNoPower show up in the outputs (I want to see the effect take place)
                // 12.19.16
                //WSmith.invokePolicies = true;

                // create the base indicator classes and create the Indicator parameters 

                // GroundWater Indicator
                GWIndicator = new WebIndicator_GroundWater(this, IndicatorData);
                
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_GW_Annual_Indicator, "Annual Safe Yield Indicatior", eModelFields.epP_GW_Annual_Indicator, GWIndicator.AnnualIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_Annual_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the annual ground water withdrawal, 0 means safe yield achived, 100 is max value", "%", "Percent", "Groundwater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Economic Indicator
                ECOINdicator = new WebIndicator_Economic(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_ECORatio, "Economic Water Stress", eModelFields.epP_ECORatio, ECOINdicator.EcoRatio));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ECODemand, "The ratio of Needed Gallons Per Capita Per Day to Annual Gallons Per Capita Per Day", "%", "Percent", "Economic Water Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Urban Surface Water Indicator
                SURIndicator = new SurfaceWaterIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_SWIndicator, "Surfacewater Indicator", eModelFields.epP_SWIndicator, SURIndicator.SurfaceChangeIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SWIndicator, "Change in impact on Surface Water", "?", "?", "Surfacewater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Agricutural Indicator
                AGIndicator = new AgricultureIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_AgricultureIndicator, "Ag Indicator", eModelFields.epP_AgricultureIndicator, AGIndicator.AgIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureBaseGPD100, "Efficiency of Ag Water Use", "?", "?", "Ag Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Urban Efficiency Indicator
                UEFIndicator = new UrbanEfficiencyIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_UrbanEfficiencyIndicator, "Urban Water Efficiency Stress", eModelFields.epP_UrbanEfficiencyIndicator, UEFIndicator.UrbanEfficiency));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureBaseGPD100, "Urban Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Urban Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Environment Indicator
                ENVIndicator = new EnvironmentIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_ENVIndicator, "Environment Indicators", eModelFields.epP_ENVIndicator, ENVIndicator.Environment));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ENVIndicator, "Impact of Influent and Withdrawls on Changes in Total Surface Water Flows", "0-100", "", "Environment Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Power Efficiency Indicator
                POWIndicator = new PowerEfficiencyIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_PowerEfficiency, "Power Water Efficiency Stress", eModelFields.epP_PowerEfficiency, POWIndicator.PowerEfficiency));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerEfficiency, "Power Water Efficiency Stress ", "0-100", "Indicator Stress 0 - 100", "Power Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // industry Efficiency Indicator

                INDIndicator = new industryEfficiencyIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_IndustryEfficiency, "Industry Efficiency Stress", eModelFields.epP_IndustrialEfficiency, INDIndicator.industryEfficiency));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_IndustryEfficiency, "Industry Water Efficiency", "0-100", "Indicator Stress 0 - 100", "industry Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
                //
                // Reclaimed Water Recycled for use Indicator
                //   Sampson edits 08.08.18
                RECYCIndicator = new RecyclingIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_RecyclingIndicator, "Reclaimed Water Indicator", eModelFields.epP_RECYCIndicator, RECYCIndicator.RecycleIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_RecyclingIndicator, "The percent of reclaimed water recycled in relation to total demand", "%", "Percent", "Reclaimed Water Recycled", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
                //    End Sampson edits 08.08.18

                // Reduce Use Indicator
                //   Sampson edits 08.09.18
                REDUSEIndicator = new ReduceUseIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_ReduceUseIndicator, "Reduce Water Use Indicator", eModelFields.epP_ReduceUseIndicator, REDUSEIndicator.reduceUseIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ReduceUseIndicator, "A Weighted Indicator of Conservation", "Unitless", "Unitless", "Reduced Use", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
                //    End Sampson edits 08.09.18






                // The Assessment indicators
                // Only including Grounwater, Environment, and Economic for now becuase the other indicators have sweet spots iwe could be tohigh or tolow
                List<WebIndicator> TheIndicatorList = new List<WebIndicator>();
                TheIndicatorList.Add(GWIndicator);
                TheIndicatorList.Add(ECOINdicator);
                TheIndicatorList.Add(ENVIndicator);

                AssessIndicator = new WebAssessmentIndicator(this, TheIndicatorList);

                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_Assessment, "Success Indicator", eModelFields.epP_Assessment, AssessIndicator.Assessment));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Assessment, "General Scenario Assessment", "0-100", "Success", "Scenario Assessment", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_AverageSustainability, "Average Sustainability Indicator", eModelFields.epP_AverageSustainability, AssessIndicator.AverageSustainabilityIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AverageSustainability, "Average of the Sustainability Indicator Values", "0-100", "Avg Sustainability", "Avg Sustainability", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_ReverseNetDemandRatio, "Net Demand Ratio Indicator", eModelFields.epP_ReverseNetDemandRatio, AssessIndicator.ReverseNetDemandRatio));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ReverseNetDemandRatio, "The reverse of the Net Demand to Total Demand Ratio", "0-100", "Net Demand Ratio Indicator", "Net Demand Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            }
            catch (Exception ex)
            {
                string Mesg = ex.Message;
                // ouch need error trapping
            }

           
            return result;
        }
    }
#endregion

}
