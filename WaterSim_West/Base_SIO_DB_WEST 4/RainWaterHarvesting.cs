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
    //https://www.ncei.noaa.gov/news/noaa-offers-climate-data-counties
    // This might be a good place to obtain data for the rainwater



    //
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
        int Tyear = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassRainFall(string DataDirectory, string Filename)
        {


            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory ;
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
                                if (i == 1) FirstYear = TempYear;
                                Tyear = TempYear;
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
            LastYear = Tyear;
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
        public int LastYear
        { get; set; }
        public int FirstYear
        { get; set; }
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
        //
     




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
        // THese will repressnt the proportional area of impervious area total for each class
        // that is usuable roof area for rainwater/ storm water capture. Data come from
        // US EPA LCLU ICLUS ssp2 and ssp5 scenarios. I am simply estimating the propotional 
        // area that could be usuable roof area
        // 08.25.21 das
        internal const double RoofAreaLoss = 0.95;
        internal const double RoofAreaEfficiency = 0.9; // roof evaporation/ other losses so 90% efficient

        // 75% max in this document https://www.pdskc.org/portals/pdskc/documents/zoning_pdf/crestview_hills/chip.pdf
        internal const double IND_RoofAreaProp = 0.63; // 63% of land occupied From file:///C:/Users/dasamps1/Downloads/sustainability-12-10611-v2.pdf
        internal const double IND_Impervious = 0.72;
        //
        internal const double COM_RoofAreaProp = 0.33; // 33% of land occupied (2.5:1 to 3.5:1 lot to building size ratio) From https://www.thebalancesmb.com/how-to-calculate-the-land-to-building-ratio-2866427
        internal const double COM_Impervious = 0.85;
        //
        internal const double EA_RoofAreaProp = 0.224; // 22% of land occupied https://www.storagecafe.com/blog/lot-size-home-size-in-top-20-biggest-us-cities/
        internal const double EA_Impervious = 0.65;
        // 1.0541
        internal const double Mod_roof = 0.5; // adjust 1/2 acre and acre lots (reduce roof area)
        internal const double compliance = 0.3; // proportion of population that installs rainwater systems
        //
        public const double mm_to_meters = 0.001;
        public const double acres_to_ft2 = 43560;
        public const double acres_to_m2 = 4046.8564224;
        public const double Liters_to_Gallons=0.26417205236;
        public const double mm_to_inches= 0.03937007874;
        public const double ft2_to_inches2 = 144;
        public const double inches2_to_mm2 = 645.16;
        public const double cubicmm_to_cubicmeters = 0.0000000001;
        public const double cubicmeters_to_gallons = 264.17205236;
        public const double gallons_to_MG =  0.000001;
        //
        public const double MultiFamily_households = 100;
        public const double households = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    public class RainWaterHarvesting
    {
        DataClassRainFall RainFall;
        UnitData FUnitData;
        DataClassLcluArea LCLUclasses;
        public int FUnitCount = 0;
        public int FYearCount = 0;
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="Rain"></param>
        public RainWaterHarvesting(DataClassRainFall Rain)
        {
            RainFall = Rain;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rain"></param>
        /// <param name="LCLUclass"></param>
        public RainWaterHarvesting(DataClassRainFall Rain, DataClassLcluArea LCLUclass)
        {
            RainFall = Rain;
            LCLUclasses = LCLUclass;
        }
        //
        public RainWaterHarvesting(DataClassRainFall Rain, DataClassLcluArea LCLUclass, UnitData UnitData)
        {
            RainFall = Rain;
            LCLUclasses = LCLUclass;
            FUnitData = UnitData;
            FUnitCount = FUnitData.UnitCount;
            FYearCount = RainFall.LastYear - RainFall.FirstYear;
            Initialize();

        }
        internal void Initialize()
        {
            RWcaptureYear_MGD = new double[FUnitCount, FYearCount];
            RWYear_MGD = new double[FUnitCount, FYearCount];
            RWcaptureYear_ratio = new double[FUnitCount, FYearCount];
            RWcapture = new double[FUnitCount];

        }

        //
        public double [] IND_harvesting
        {
            get; set;
        }
        public double [] COMM_harvesting
        {
            get; set;
        }
        public double [] EA_harvesting
        {
            get; set;
        }
        public double [] QA_harvesting
        {
            get; set;
        }
        public double [] TA_harvesting
        {
            get; set;
        }
        public double [] HA_harvesting
        {
            get; set;
        }
        public double [] A_harvesting
        {
            get; set;
        }

        public double [] RWcapture
        {
            get; set;
        }
        public double[,] RWcaptureYear_MGD
        {
            get; set;
        }
        public double[,] RWcaptureYear_ratio
        {
            get; set;
        }
        public double[,] RWYear_MGD
        {
            get; set;
        }
        public double[] RWacres
        {
            get; set;
        }
        internal int I
        {
            get; set;
        }
        // ====================================================================================================
        // Percent impervious by class
        // ICLUS 4th national climate assessment US EPA
        // Industrial-72%, Commercial-85%, EigthAcre-65%, QuarterAcre-38%, ThirdAcre-30%, HalfAcre-25%, Acre-20%
        //
        // =============================================================
        // Industrial Buildings - roof area, rainfall runoff, capture
        internal double INDroof(int year)
        {
            double temp = 0;
            int i = 0;
            int j = 0;
             IND_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {           
                double ind = LCLUclasses.FastArea_UN(name, "Ind", year);
                RWacres[i] = ind;
                // acres
                temp = ind * RWconstants.IND_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency 
                    * RWconstants.IND_Impervious ;
                IND_harvesting[i] = temp;
                i++;
            }
            return temp;
        }
        // =================================================
        internal double INDroofYearly(int year, string name)
        {
            double temp = 0;
            int i = I;
            IND_harvesting = new double[FUnitCount];
            double ind = LCLUclasses.FastArea_UN(name, "Ind", year);
            RWacres[i] = ind;
            // acres
            temp = ind * RWconstants.IND_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
               // * RWconstants.IND_Impervious;
            IND_harvesting[i] = temp;

            return temp;
        }



        // ===============================================================
        internal double COMroof(int year)
        {
            double temp = 0;
            int i = 0;
            COMM_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double comm = LCLUclasses.FastArea_UN(name, "Comm", year);
                RWacres[i] += comm;
                // acres
                temp = comm * RWconstants.COM_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency
                    * RWconstants.COM_Impervious;
                COMM_harvesting[i] = temp;
                i++;
            }
            return temp;
        }
        internal double COMroofYearly(int year, string name)
        {
            double temp = 0;
            int i = I;
            COMM_harvesting = new double[FUnitCount];
            double comm = LCLUclasses.FastArea_UN(name, "Comm", year);
            RWacres[i] += comm;
            // acres
            temp = comm * RWconstants.COM_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                //* RWconstants.COM_Impervious;
            COMM_harvesting[i] = temp;

            //}
            return temp;
        }
        // edits 09.13.21 das 
        // Why do the urban methods have no constants for impervious? Have I simply NOT put them in yet?
        // end edits

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        internal double EigthAcreroof(int year)
        {
            double temp = 0;
            int i = 0;
            EA_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double EA = LCLUclasses.FastArea_UN(name, "EigthAcre", year);
                RWacres[i] += EA;
                // acres
                temp = EA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                EA_harvesting[i] = temp;
                i++;
            }
            return temp;
        }
        internal double EigthAcreroofYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            int i = 0;
            i = I;
            EA_harvesting = new double[FUnitCount];
            double EA = LCLUclasses.FastArea_UN(name, "EigthAcre", year) * CRF.UrbanHighDensityChange;
            RWacres[i] += EA;
            // acres
            temp = EA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency 
                * RWconstants.compliance;
            EA_harvesting[i] = temp;

            return temp;
        }
        internal double QuarterAcreroof(int year)
        {
            double temp = 0;
            int i = 0;
            QA_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double QA = LCLUclasses.FastArea_UN(name, "QuarterAcre", year);
                RWacres[i] += QA;
                // acres
                temp = QA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                QA_harvesting[i] = temp;
                i++;
            }
            return temp;
        }
        internal double QuarterAcreroofYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            int i = I;
            QA_harvesting = new double[FUnitCount];
            double QA = LCLUclasses.FastArea_UN(name, "QuarterAcre", year) * CRF.UrbanLowDensityChange;
            RWacres[i] += QA;
            // acres
            temp = QA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency * RWconstants.compliance;
            QA_harvesting[i] = temp;

            return temp;
        }
        internal double ThirdAcreroof(int year)
        {
            double temp = 0;
            int i = 0;
            TA_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double TA = LCLUclasses.FastArea_UN(name, "ThirdAcre", year);
                RWacres[i] += TA;
                // acres
                temp = TA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                TA_harvesting[i] = temp;
                i++;
            }
            return temp;
        }
        internal double ThirdAcreroofYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            int i = I;
            TA_harvesting = new double[FUnitCount];
            double TA = LCLUclasses.FastArea_UN(name, "ThirdAcre", year) * CRF.SuburbanDensityChange;
            RWacres[i] += TA;
            // acres
            temp = TA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency * RWconstants.compliance;
            TA_harvesting[i] = temp;

            return temp;
        }
        internal double HalfAcreroof(int year)
        {
            double temp = 0;
            int i = 0;
            HA_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double HA = LCLUclasses.FastArea_UN(name, "HalfAcre", year);
                RWacres[i] += HA;
                // acres
                temp = HA * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                HA_harvesting [i] = temp;
                i++;
            }
            return temp;
        }
        internal double HalfAcreroofYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            int i = I;
            HA_harvesting = new double[FUnitCount];

            double HA = LCLUclasses.FastArea_UN(name, "HalfAcre", year) * CRF.ExurbanHighDensityChange;
            RWacres[i] += HA;
            // acres
            temp = HA * (RWconstants.EA_RoofAreaProp * RWconstants.Mod_roof) * RWconstants.RoofAreaLoss *
                RWconstants.RoofAreaEfficiency * RWconstants.compliance ;
            HA_harvesting[i] = temp;
            return temp;
        }
        // ===================================================================
        internal double Acreroof(int year)
        {
            double temp = 0;
            int i = 0;
            A_harvesting = new double[FUnitCount];
            //RWacres = new double[FUnitCount];
            foreach (string name in FUnitData.UnitNames)
            {
                double A = LCLUclasses.FastArea_UN(name, "Acre", year);
                RWacres[i] += A;
                // acres
                temp = A * RWconstants.EA_RoofAreaProp * RWconstants.RoofAreaLoss * RWconstants.RoofAreaEfficiency;
                A_harvesting[i] = temp;
                i++;
            }
          
            return temp;
        }
        // ================================
        internal double AcreroofYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            int i = I;
            A_harvesting = new double[FUnitCount];
            double A = LCLUclasses.FastArea_UN(name, "Acre", year) * CRF.ExurbanLowDensityChange;
            RWacres[i] += A;
            // acres
            temp = A * (RWconstants.EA_RoofAreaProp * RWconstants.Mod_roof ) * RWconstants.RoofAreaLoss * 
                RWconstants.RoofAreaEfficiency * RWconstants.compliance;
            A_harvesting[i] = temp;
            return temp;
        }
        //======================================================================
        internal void roofCapture(int year)
        {
            double temp = 0;
            RWacres = new double[FUnitCount];
            temp = INDroof(year) + COMroof(year)+ EigthAcreroof(year) + QuarterAcreroof(year)+ThirdAcreroof(year) + 
                HalfAcreroof(year) + Acreroof(year);
        }
        internal void roofCaptureYearly(int year, string name, WaterSimCRFModel CRF)
        {
            double temp = 0;
            RWacres = new double[FUnitCount];
            RWacres[I] = 0;
            temp = INDroofYearly(year, name) + COMroofYearly(year,name) + EigthAcreroofYearly(year,name,CRF) + 
                QuarterAcreroofYearly(year,name,CRF) + ThirdAcreroofYearly(year,name,CRF) +
                HalfAcreroofYearly(year,name,CRF) + AcreroofYearly(year,name,CRF);
            I += 1;
            if (23 < I) I = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void  rwHarvesting()
        {
            // rainfall in mm yr-1
            // output is MGD year-1
            double temp = 0;
            int i = 0;
            int j = 0;
            //RWcaptureYear_MGD = new double[FUnitCount, FYearCount];
            //RWYear_MGD = new double[FUnitCount, FYearCount];
            //RWcaptureYear_ratio = new double[FUnitCount, FYearCount]; 
            RWcapture = new double[FUnitCount];

            for (int year = RainFall.FirstYear; year < RainFall.LastYear; year++)
            //while (int year <= RainFall.DataTableRows)
            {
                roofCapture(year);
                foreach (string code in FUnitData.UnitNames)
                {             
                    double T = RainFall.FastRainFall(code, year);
                    RWYear_MGD[i,j] = RWacres[i] * UnitConvertLiters(T) / Utilities.daysInAYear(year);
                    temp = (IND_harvesting[i] + COMM_harvesting[i] + EA_harvesting[i] + QA_harvesting[i] +
                       TA_harvesting[i] + HA_harvesting[i] + A_harvesting[i]) * UnitConvertLiters(T); // MG;
                    RWcaptureYear_MGD[i,j] = temp/ Utilities.daysInAYear(year);
                    if(0 < RWYear_MGD[i, j]) RWcaptureYear_ratio[i, j] =
                        RWcaptureYear_MGD[i, j] / RWYear_MGD[i, j]; // always < 1
                    i++;
                }
                j++;
                i = 0;
            }
           // return temp;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The region name</param>
        /// <param name="year"></param>
        /// <param name="CRF">the CRF object</param>
        /// <returns></returns>
        public double rwHarvestingYearly(string name, int year, WaterSimCRFModel CRF)
        {
            // rainfall in mm yr-1
            // output is supposed to be MGD year-1
            double temp = 0;
           // double check = 0;
            int i = 0;
            i = I;
            int j = 0;
        
     
            //RWcaptureYear_MGD = new double[FUnitCount, FYearCount];
            //RWYear_MGD = new double[FUnitCount, FYearCount];
            //RWcaptureYear_ratio = new double[FUnitCount, FYearCount];

            j = year;
            int cYear = year + 2020;

           // RWcapture = new double[FUnitCount];
            roofCaptureYearly(cYear, name,CRF);
            double T = RainFall.FastRainFall(name, cYear);
            RWYear_MGD[i, j] = RWacres[i] * UnitConvertLiters(T) / Utilities.daysInAYear(cYear);
            // area in acres * rainfall conversion
            //temp = (IND_harvesting[i] + COMM_harvesting[i] + EA_harvesting[i] + QA_harvesting[i] +
            // TA_harvesting[i] + HA_harvesting[i] + A_harvesting[i]) * UnitConvert(T); // MGD;
            temp = (IND_harvesting[i] + COMM_harvesting[i] + EA_harvesting[i] + QA_harvesting[i] +
             TA_harvesting[i] + HA_harvesting[i] + A_harvesting[i]) * UnitConvertLiters(T); // MGD;
            RWcaptureYear_MGD[i, j] = temp / Utilities.daysInAYear(cYear);
            if (0 < RWYear_MGD[i, j]) RWcaptureYear_ratio[i, j] =
                 RWcaptureYear_MGD[i, j] / RWYear_MGD[i, j]; // always < 1
                                                             //
             //return RWacres[i];// RWcaptureYear_ratio[i, j];
            return RWcaptureYear_MGD[i, j];
            //return RWcaptureYear_ratio[i, j];
        }

        //========================================================
        //internal double UnitConvert(double rainFall)
        //{
        //    // rainfall in mm yr-1 (which is also L m-2)
        //    // output is MGD year-1
        //    double temp = 0;
        //    temp = (RWconstants.acres_to_ft2 * RWconstants.ft2_to_inches2 * RWconstants.inches2_to_mm2 * rainFall
        //           * RWconstants.cubicmm_to_cubicmeters * RWconstants.cubicmeters_to_gallons * RWconstants.gallons_to_MG); // MGD         
        //    return temp;
        //}
        //
        internal double UnitConvertLiters(double rainFall)
        {
            // rainfall in mm yr-1 (which is also L m-2)
            // output is MGD year-1
            double temp = 0;
            temp = (RWconstants.Liters_to_Gallons * RWconstants.gallons_to_MG * rainFall * RWconstants.acres_to_m2 ); // MG       
            return temp;
        }
        //
        internal double UnitConvert_()
        {
            // rainfall in mm yr-1
            // output is MGD year-1
            double temp = 0;
            temp = 1/RWconstants.cubicmm_to_cubicmeters * 1/RWconstants.cubicmeters_to_gallons * 1/RWconstants.gallons_to_MG; // MGD         
            return temp;
        }

        // ==============================================================
        //
        //
        //
        int Region(string name)
        {
            int iout = 0;
            switch (name)
            {
                case "Arizona Central South":
                    iout = 0;
                    break;
                case "Arizona West":
                    iout = 1;
                    break;
                case "Arizona North":
                    iout = 2;
                    break;
                case "Arizona Southeast":
                    iout = 3;
                    break;
                case "Arizona Central North":
                    iout = 4;
                    break;
                case "California Southwest":
                    iout = 5;
                    break;
                case "California Southeast":
                    iout = 6;
                    break;
                case "California North":
                    iout = 7;
                    break;
                case "Colorado Front Range":
                    iout = 8;
                    break;
                case "Colorado In Basin":
                    iout = 9;
                    break;
                case "Colorado Not In Basin":
                    iout = 10;
                    break;
                case "Nevada South":
                    iout = 11;
                    break;
                case "Nevada In Basin":
                    iout = 12;
                    break;
                case "Nevada Not In Basin":
                    iout = 13;
                    break;
                case "New Mexico Central":
                    iout = 14;
                    break;
                case "New Mexico In Basin":
                    iout = 15;
                    break;
                case "New Mexico Not In Basin":
                    iout = 16;
                    break;
                case "New Mexico Gila":
                    iout = 17;
                    break;
                case "Utah Salt Lake":
                    iout = 18;
                    break;
                case "Utah In Basin":
                    iout = 19;
                    break;
                case "Utah Not In Basin":
                    iout = 20;
                    break;
                case "Wyoming Southwest":
                    iout = 21;
                    break;
                case "Wyoming In Basin":
                    iout = 22;
                    break;
                case "Wyoming Not In Basin":
                    iout = 23;
                    break;
                    //case "Colorado River Basin":
                    //    iout = 24;
                    //    break;

            }
            return iout;
        }
        enum classs
        {
            Ind=0,
            Com=1,
            EigthAcre=2,
            QuarterAcre=3,
            ThirdAcre=4,
            HalfAcre=5,
            Acre=6
        }
    }
}
