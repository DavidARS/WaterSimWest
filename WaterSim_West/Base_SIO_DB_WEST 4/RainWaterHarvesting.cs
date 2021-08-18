using System;
using System.Collections.Generic;
using WaterSimDCDC.Generic;
using System.Linq;
using System.Text;
using DCDC_Utilities;
using UniDB;
using System.Data;

namespace WaterSim_Base
{
    // =======================================================================
    /// <summary>
    /// Daa Struct to handle the regional rainfall estimates
    /// </summary>
    public struct DataStructRainWater
    {
        string FRegion;
        int FStateCode;
        int FRegionCode;
        double FRain;
        int FYear;
       
        public DataStructRainWater(int stateC, int regionC, string region, int  year, double Rainfall)
        {
            bool isErr = false;
            string errMsg = "";
            FStateCode = stateC;
            FRegionCode = regionC;
            FYear = year;
            FRegion = region;
            FRain = Rainfall;
        }
        public int StateCode
        {
            get { return FStateCode; }
        }
        public int RegionCode
        {
            get { return FRegionCode; }
        }

        public double Rainfall
        {
            get { return FRain; }
        }
        public string Region
        {
            get { return FRegion; }
        }
        public int Year
        {
            get{return FYear; }
                    
        }

    }
    // =====================================================================
    /// <summary>
    /// The class used in input the rainfall data for the regions
    /// </summary>
    public class DataClassRainFall
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        const string FScodeFieldStr = "SC";
        const string FSnameFieldStr = "SN";
        // EDIT QUAY 2/19/18
        // Added Region Filed strings and setup to use State or Region
        const string FRnameFieldStr = "RN";
        const string FRcodeFieldStr = "RC";
        

        string FNameFieldStr = FRnameFieldStr;
        string FCodeFieldStr = FRcodeFieldStr;

        string FRainFallFieldStr = "rainfall";
        string FcurrentYearFieldStr = "year";

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FRainFallArray = null;
        double[] FYearArray = null;

        List<DataStructRainWater> FRainFallDataList = new List<DataStructRainWater>();
        //
        /// <summary>
        /// 
        /// </summary>
        public int DataTableRows
        {
        get;
        set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassRainFall(string DataDirectory, string Filename)
        {


            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "");
            DbCon.UseFieldHeaders = true;
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            if (isErr)
            {
                throw new Exception("Error loading Rate Data. " + errMessage);
            }
            // build data arrays
            int arraysize = TheData.Rows.Count;
            FRainFallArray = new double[arraysize];           
            FYearArray = new double[arraysize];
            //

            int i = 0;
            //
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                i += 1;
              
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FScodeFieldStr].ToString();
                string Rcodestr = DR[FRcodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                if (!isErr)
                {
                    string rrainfallstr = DR[FRainFallFieldStr].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    int StateC = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        int RegionC = Tools.ConvertToInt32(Rcodestr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double TempRain = Tools.ConvertToInt32(rrainfallstr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    // OK 
                                    DataStructRainWater RW = new DataStructRainWater(StateC, RegionC, namestr, TempYear, TempRain);
                                    FRainFallDataList.Add(RW);
                                    
                                }
                            }
                        }
                    }
                }
                DataTableRows = i;
            }

        }
        // ==============================================================
        private double[] GetDataArray(string FieldStr)
        {
            double[] result = new double[TheData.Rows.Count];
            int cnt = 0;
            double tempDbl = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                string valstr = DR[FieldStr].ToString();
                if (double.TryParse(valstr, out tempDbl)) result[cnt] = tempDbl;
                else result[cnt] = double.NaN;
                cnt++;
            }
            return result;
        }

        // ==============================================================
        //   
        //public double[] AGRate()
        //{
        //    return GetDataArray(FAGRateFieldStr);
        //}

        /// <summary>
        /// Get the rainfall data for each region
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastRainFall(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataStructRainWater TheData = FRainFallDataList.Find(delegate (DataStructRainWater RW)
            {
                return ((RW.Year == year) && (RW.Region == UnitName));
            });

            if (TheData.Region == UnitName)
            {
                temp = TheData.Rainfall;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastRainFall(int UnitCode, int year)
        {
            double temp = InvalidRate;
            DataStructRainWater TheData = FRainFallDataList.Find(delegate (DataStructRainWater RW)
            {
                return ((RW.Year == year) && (RW.RegionCode == UnitCode));
            });

            if (TheData.RegionCode == UnitCode)
            {
                temp = TheData.Rainfall;
            }
            return temp;
        }

    }
    // ======================================================================




    /// <summary>
    /// Constants used for rainWater Harvesting
    /// </summary>
    public struct RWconstants
    {
        int Fyear;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public RWconstants(int year)
        {
            Fyear = year;
        }
        /// <summary>
        /// 
        /// </summary>
        public const double runoffCoef = 0.9;
        /// <summary>
        /// The size of the individual storage tanks
        /// </summary>
        public const double Storage_capacity = 1000;
        public const double Roof_Area_LSF = 464;
        public const double Roof_Area_TSF = 232;
        public const double Roof_Area_SSF = 200;
        public const double Roof_Area_SMF = 464;
        public const double Roof_Area_WMF = 464;
        //
        public const double mm_to_meters = 0.001;
        public const double cubicmeters_to_gallons = 264.17205236;
        public const double gallons_to_MGD =  0.000001;
        //
        public const double MultiFamily_households = 100;
        public const double households = 0;
    }
    public class RainWaterHarvesting
    {
        DataClassRainFall RainFall;
        UnitData FUnitData;
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="Rain"></param>
        public RainWaterHarvesting(DataClassRainFall Rain)
        {
            RainFall = Rain;
        }

        public double harvest(UnitData FUnitData, int year)
        {
            double temp = 0;

            foreach (string Name in FUnitData.UnitNames)
            {
                int i = 0;
                try
                {
                    while (year <= RainFall.DataTableRows)
                    {

                        double T = RainFall.FastRainFall(Name, year);
                    }



                }
                catch { }
            }
            return temp;
        }


    }
}
