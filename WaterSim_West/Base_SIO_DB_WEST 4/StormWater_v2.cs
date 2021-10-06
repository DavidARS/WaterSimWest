using System;
using System.Collections.Generic;
using WaterSimDCDC.Generic;
using System.Linq;
using System.Data;
using UniDB;
using System.Windows.Forms;
using System.Text;

namespace WaterSim_Base
{
    /// <summary>
    /// 
    /// </summary>
    public struct SWconstants
    {
        int Fyear;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public SWconstants(int year)
        {
            Fyear = year;
        }
        public const double Z = 254;
        public const double Perc_scale = 3;
        public const double Perc_store = 0.05;
        public const double Perc_slope = 0.1;
        //
        // Impervious as a function  of residential density
        // From ICLUS manual Graph C-3
        // Reference: 
        //Parameter Estimate, Approx Std Error, Approximate 95% Confidence Limits
        // A= 47.3018         2.1073            (43.1581 < a < 51.4455)
        // B= -875.3          87.4120           (-1047.1 < b < -703.4) 
        public const double A = 47.3018; //
        public const double B = -875.3; // 
                                        //
                                        //Industrial - 72 %, Commercial - 85 %, EigthAcre - 65 %, QuarterAcre - 38 %, ThirdAcre - 30 %, HalfAcre - 25 %, Acre - 20 %
        public const double Ind = 0.72;
        public const double Com = 0.85;
        public const double EigthAcre = 0.65;
        public const double QuarterAcre = 0.38;
        public const double ThirdAcre = 0.3;
        public const double HalfAcre = 0.25;
        public const double Acre = 0.20;


        //
    }
    /// <summary>
    /// Class to model stormwater runoff
    /// </summary>
    public class StormWater
    {
        WaterSimCRFModel CRF;
        UnitData FUnitData;
        public DataClassLcluArea RCNarea;
        public DataClassRCN RCN;
        DataClassRainFall RainFall;
        RainWaterHarvesting RW;
        //
        public bool isInstantiated = false;
        //

        //const double Z = 254;
        //
        /// <summary>
        /// The data file that holds the area for each LCLU class for each state and county
        /// </summary>
        //string LcluClassesDataFilename = "LCLUclassArea.csv";
        /// <summary>
        /// Runoff Curve numbers for each LCLU class
        /// </summary>
        //string LcluClassesRCNDataFilename = "LCLUrcn.csv";

        ///
        public StormWater(DataClassLcluArea DC, DataClassRCN rcn)
        {
            RCNarea = DC;

            RCN = rcn;
            isInstantiated = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitData"></param>
        /// <param name="Rain"></param>
        /// <param name="DC"></param>
        /// <param name="rcn"></param>
        public StormWater(UnitData UnitData, DataClassRainFall Rain, DataClassLcluArea DC, DataClassRCN rcn)
        {
            RCNarea = DC;
            RCN = rcn;
            FUnitData = UnitData;
            RainFall = Rain;
            Initialize_Variables();
            isInstantiated = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitData"></param>
        /// <param name="Rain"></param>
        /// <param name="RWH"></param>
        /// <param name="DC"></param>
        /// <param name="rcn"></param>
        public StormWater(UnitData UnitData, DataClassRainFall Rain, RainWaterHarvesting RWH, DataClassLcluArea DC, DataClassRCN rcn)
        {
            RCNarea = DC;
            RCN = rcn;
            FUnitData = UnitData;
            RainFall = Rain;
            RW = RWH;
            Initialize_Variables();
            isInstantiated = true;
        }


        internal void Initialize_Variables()
        {
            //throw new NotImplementedException();
        }
        enum LcluClasses
        {
            Turf,
            Woods,
            OpenWater,
            Pasture,
            RowCrops,
            Fallow,
            Industrial,
            Commercial,
            Paved,
            EigthAcre,
            QuarterAcre,
            ThirdAcre,
            HalfAcre,
            Acre
        }
        //



        // Industrial-72%, Commercial-85%, EigthAcre-65%, QuarterAcre-38%, ThirdAcre-30%, HalfAcre-25%, Acre-20%

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double AreaByRCN(string Name, int year)
        {
            string soil = "A";
            double total = 0;

            //foreach (string Name in FUnitData.UnitNames)
            //{
                foreach (LcluClasses e in Enum.GetValues(typeof(LcluClasses)))
                {
                    double temp = 0;

                    double T = RCNvalueByClass(Name, e, soil);
                    double t = RCNareaByClassRN(Name, e, year);
                    double u = screenClasses(e,t);
                    double a = RCNarea.FastTotalArea_UN(Name, year);
                    try
                    {
                        if (t < a)
                        {
                            if (0 < a) temp = Math.Max(0, T * (u / a));
                        }
                        else
                        {
                            MessageBox.Show("error on acerage data for LCLU RCN classe- line 121 in StormWater.cs");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("division by zero for total area_ runoff curve classses area estimates", ex);
                    }
                    //
 
                    //
                    total += temp;
                }
                //Storage(FUnitData, total);
            //}

            return total;
        }
        // ---------------------------------------------------------------------------------------------------------------
        //
        // % impervious area for an individual  residential density class
        internal double TypeIIexponential(double DUA)
        {
            double temp = 0;
            temp = SWconstants.A * Math.Exp(SWconstants.B / DUA);
            return temp;
        }
        double screenClasses(LcluClasses e, double acres)
        {
            double temp = 0;
            string loop = e.ToString();
            if (loop == "Ind") { temp = acres * SWconstants.Ind; }
            if(loop == "Com") { temp = acres * SWconstants.Com; }
            if(loop == "EigthAcre")  {
                //double Imp = TypeIIexponential(DUA) / 100.0;
                temp = acres * SWconstants.EigthAcre;  }
            if(loop == "QuarterdAcre") { temp = acres * SWconstants.QuarterAcre; }
            if (loop == "ThirdAcre") { temp = acres * SWconstants.ThirdAcre; }
            if (loop == "HalfAcre") { temp = acres * SWconstants.HalfAcre; }
            if (loop == "Acre") { temp = acres * SWconstants.Acre; }
            return temp;
         }

        //
        /// <summary>
        ///  Main method to calculate the rainfall butgets- call from WaterSim_model
        /// </summary>
        /// <param name="FUnitData"></param>
        /// 
        /// <returns></returns>
        public double waterBudgetByClass()
        {

            double total = 0;
            int i = 0;
            int j = 0;
            for (int year = RainFall.FirstYear; year < RainFall.LastYear; year++)
            //while (int year <= RainFall.DataTableRows)
            {
                foreach (string Name in FUnitData.UnitNames)
                {
                    double rcn = AreaByRCN(Name, year);
                    double rainFall = getRainFall(Name, year);
                    double remove = rainFall * RW.RWcaptureYear_ratio[i,j];
                    StreamThroughPut(rainFall, rcn, remove);
                    i++;
                }
                j++;
                i = 0;
            }
            return total;
        }
        // =====================================================================
        public double waterBudgetByClassYearly(string name, int year)
        {

            double total = 0;
            int i = 0;
            int j = 0;
            j = year;
            int cYear = year + 2020;
                    double rcn = AreaByRCN(name, cYear);
                    double rainFall = getRainFall(name, cYear);
                    i = Region(name);
                    double remove = rainFall * RW.RWcaptureYear_ratio[i, j];
                    StreamThroughPut(rainFall, rcn, remove);
           
            return total;
        }
        //
        int Region(string name)
        {
            int iout= 0;
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
    // ======================================================================
    // Neeed to add a data file to extract rainfall for this method
    internal double getRainFall(string Name, int year)
        {
            double temp = 0;
            double T = RainFall.FastRainFall(Name, year);
            return temp=T;
        }


        //
        /// <summary>
        /// document=runoff-curve-numbers1.pdf from Engineering Hydrology Training Series Module 104, Runoff curve number computations.
        ///  national employee development staff, soil conservation service, USDA September 1989
        /// and https://www.wsdot.wa.gov/publications/fulltext/Hydraulics/HRM/App4B_2014.pdf
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="e"></param>
        /// <param name="soil"></param>
        /// <returns></returns>
        double RCNvalueByClass(string unit, LcluClasses e, string soil)
        {
            double temp = 0;
            string loop = e.ToString();
            switch (soil)
            {
                case "A":
                    temp = RCN.Fast_A_RCN(loop);
                    break;
                case "B":
                    temp = RCN.Fast_B_RCN(loop);
                    break;
                case "C":
                    temp = RCN.Fast_C_RCN(loop);
                    break;
                case "D":
                    temp = RCN.Fast_D_RCN(loop);
                    break;
            }
            return temp;
        }
        double RCNareaByClassRN(string unitName, LcluClasses e, int year)
        {
            double temp = 0;
            string loop = e.ToString();
            temp = RCNarea.FastArea_UN(unitName, loop, year);
            return temp;
        }
        double RCNareaByClassRC(int code, LcluClasses e, int year)
        {
            double temp = 0;
            string loop = e.ToString();
            temp = RCNarea.FastArea_UC(code, loop, year);
            return temp;

        }
        //
        /// <summary>
        ///  rainfall uints = mm
        ///  remove units = MGD
        /// </summary>
        /// <param name="rainFall"></param>
        /// <param name="rcn"></param>
        /// <param name="remove"></param>
        public void StreamThroughPut(double rainFall, double rcn, double remove)
        {
            ThroughPut(Math.Max(0,rainFall-remove), rcn);
        }


        /// <summary>
        /// Put all the separate functions together in a stream to calculate RCN, runoff, and percolation and evaporation
        /// This is calculated for each region
        /// </summary>
        /// <param name="rainFall"></param>
        /// <param name="rcn"></param>
        public void ThroughPut(double rainFall, double rcn)
        {
            SoilEvaporation = Evaporation(rainFall);
            double rainPost = rainFall * (1-SoilEvaporation);
            SoilRunoff = Runoff(rainPost,rcn);
            SoilPercolation = Percolation(rainPost);
        }
        //========================================================
        //internal double MGDtomm3()
        //{
        //    // rainfall in mm yr-1
        //    // output is MGD year-1
        //    double temp = 0;
        //    double t1 = 365* (1 / RWconstants.gallons_to_MG * 1 / RWconstants.cubicmeters_to_gallons * 1 / RWconstants.cubicmm_to_cubicmeters);// mm3
        //    temp = t1;
        //    return temp;
        //}
        //internal double AcresTomm2(double area)
        //{
        //    // rainfall in mm yr-1
        //    // output is MGD year-1
        //    // area in acres yr-1
        //    double temp = 1;
        //    double t2 = area * RWconstants.acres_to_ft2 * RWconstants.ft2_to_inches2 * RWconstants.inches2_to_mm2;
        //    if(0 < t2)temp = t2;
        //    return temp;
        //}

        // ==============================================================
        //
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FUnitData"></param>
        /// <param name="rcn"></param>
        //public void Storage(UnitData FUnitData, double rcn)
        //{
        //    double temp = 0;
        //    foreach (string Name in FUnitData.UnitNames)
        //    {

        //        if (0 < rcn) { temp = SWconstants.Z * (100 * (1 / rcn) - 1); }
        //        else { temp = SWconstants.Z; }
        //    }

        //    SoilWaterStorage = temp;

        //}
        ///
        public void Storage(double rainFall, double rcn)
        {
            double temp = 0;
            if (0 < rcn) { temp = SWconstants.Z * (100 * (1 / rcn) - 1); }
            else { temp = SWconstants.Z; }
            SoilWaterStorage = temp;
        }
        double Runoff(double rainFall, double rcn)
        {
            double temp = 0;
            if(0 < rcn)
            {
                Storage(rainFall, rcn);
            }
            else
            {
                SoilWaterStorage = SWconstants.Z;
            }
            if (0 < rainFall)
            {
                temp = Math.Min(rainFall, Math.Max(0, Math.Pow((rainFall - 0.2 * SoilWaterStorage), 2) * (1 / (rainFall + 0.8 * SoilWaterStorage))));
              
            }         
            return temp;
        }
        internal double Evaporation(double precipitation)
        {
            // Hyperbola estimate of evaporation based on rainfall - From the FORTRAN code function "fHyperbolaRunoff(lv_annualRain(i,1))"
            double temp = 1;
            double a = 25;
            double b = 1;
            if (0 < precipitation)
            {
                temp = 1 - (precipitation / (a + b * precipitation));
            }

            return temp;
        }
        double Percolation(double rainfall)
        {
            // 
            double temp = 0;
            if (0 < rainfall)
            {
                temp = SWconstants.Perc_scale * (rainfall / (SWconstants.Perc_store * SoilWaterStorage + SWconstants.Perc_slope * rainfall));
            }

            return temp;
        }
        //
        /// <summary>
        /// Storage (Zi) for the 14 LCLU classes
        /// </summary>
        public double SoilWaterStorage
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public double SoilEvaporation
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public double SoilPercolation
        {
            get;
            set;
        }
        public double SoilRunoff
        {
            get;
            set;
        }
    }


    #region strut(s)
    /// <summary>
    /// Struct to read in the runoff curve numbers for the LCLU classes
    /// </summary>
    public struct DataStructRCN
    {
        string FLCLU;
        int FOrder;
        double FSand;
        double FLoam;
        double FSCL;
        double FClay;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="A">Sand</param>
        /// <param name="B">Loam</param>
        /// <param name="C">Sandy Clay Loam</param>
        /// <param name="D">Clay</param>
        /// <param name="LCLU">LCLU class - i.e., turf, fallow, crops, paved, etc.</param>
        public DataStructRCN(int order, double A, double B, double C, double D, string LCLU)
        {
            bool isErr = false;
            string errMsg = "";
            FOrder = order;
            FSand = A;
            FLoam = B;
            FSCL = C;
            FClay = D;
            FLCLU = LCLU;
        }
        public int Order
        {
            get { return FOrder; }
        }
        public double Sand
        {
            get { return FSand; }
        }
        public double Loam
        {
            get { return FLoam; }
        }
        public double SCL
        {
            get { return FSCL; }
        }
        public double Clay
        {
            get { return FClay; }
        }
        public string LCLU
        {
            get { return FLCLU; }
        }


    }

    /// 
    /// </summary>
    public struct DataStructRCNArea
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;

        int FYear;
        double FAreaTurf;
        double FAreaWoods;
        double FAreaOpenWater;
        double FAreaPasture;
        double FAreaRowCrops;
        double FAreaFallow;
        double FAreaIndustry;
        double FAreaCommercial;
        double FAreaPaved;
        double FAreaEigthAcre;
        double FAreaQuarterAcre;
        double FAreaThirdAcre;
        double FAreaHalfAcre;
        double FAreaAcre;
        double FAreaTotal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitCode"></param>
        /// <param name="aYear"></param>
        /// <param name="turf"></param>
        /// <param name="woods"></param>
        /// <param name="openWater"></param>
        /// <param name="pasture"></param>
        /// <param name="rowCrops"></param>
        /// <param name="fallow"></param>
        /// <param name="industrial"></param>
        /// <param name="commercial"></param>
        /// <param name="paved"></param>
        /// <param name="eigthAcre"></param>
        /// <param name="quarterAcre"></param>
        /// <param name="thirdAcre"></param>
        /// <param name="halfAcre"></param>
        /// <param name="acre"></param>
        /// <param name="total"></param>
        /// <param name="aUnitName"></param>
        public DataStructRCNArea(string aUnitCode, int aYear, double turf, double woods, double openWater,
                            double pasture, double rowCrops, double fallow, double industrial, double commercial,
                            double paved, double eigthAcre, double quarterAcre, double thirdAcre, double halfAcre, double acre, double total, string aUnitName)
        {
            bool isErr = false;
            string errMsg = "";

            FUnitName = aUnitName;
            FUnitCodeStr = aUnitCode;

            int temp = Tools.ConvertToInt32(FUnitCodeStr, ref isErr, ref errMsg);
            if (!isErr)
            {
                FUnitCode = temp;
            }
            else
            {
                FUnitCode = UDI.BadIntValue;
            }
            FYear = aYear;
            FAreaTurf = turf;
            FAreaWoods = woods;
            FAreaOpenWater = openWater;
            FAreaPasture = pasture;
            FAreaRowCrops = rowCrops;
            FAreaFallow = fallow;
            FAreaIndustry = industrial;
            FAreaCommercial = commercial;
            FAreaPaved = paved;
            FAreaEigthAcre = eigthAcre;
            FAreaQuarterAcre = quarterAcre;
            FAreaThirdAcre = thirdAcre;
            FAreaHalfAcre = halfAcre;
            FAreaAcre = acre;
            FAreaTotal = total;


        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the unit. </summary>
        ///
        /// <value> The name of the unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public string UnitName
        {
            get { return FUnitName; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the unit code string. </summary>
        ///
        /// <value> The f unit code string. </value>
        ///-------------------------------------------------------------------------------------------------

        public string UnitCodeStr
        {
            get { return FUnitCodeStr; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the code. </summary>
        ///
        /// <value> The code. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Code
        {
            get { return FUnitCode; }
        }

        public double Turf
        {
            get { return FAreaTurf; }
        }
        public double Woods
        {
            get { return FAreaWoods; }
        }
        public double Water
        {
            get { return FAreaOpenWater; }
        }
        public double Pasture
        {
            get { return FAreaPasture; }
        }
        public double RowCrops
        {
            get { return FAreaRowCrops; }
        }
        public double Fallow
        {
            get { return FAreaFallow; }
        }
        public double Industry
        {
            get { return FAreaIndustry; }
        }
        public double Commercial
        {
            get { return FAreaCommercial; }
        }
        public double Paved
        {
            get { return FAreaPaved; }
        }
        public double EigthAcre
        {
            get { return FAreaEigthAcre; }
        }
        public double QuarterAcre
        {
            get { return FAreaQuarterAcre; }
        }
        public double ThirdAcre
        {
            get { return FAreaThirdAcre; }
        }
        public double HalfAcre
        {
            get { return FAreaHalfAcre; }
        }
        public double Acre
        {
            get { return FAreaAcre; }
        }
        public int TheYear
        {
            get { return FYear; }
        }
        public double TotalArea
        {
            get { return FAreaTotal; }
        }
        // ==================================



        // ===================================
    }
    #endregion struct
    #region DataClasses
    // ================================
    public class DataClassRCN
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        //
        string FRCNOrderFieldStr = "ORDER";
        string FRCNSandFieldStr = "SAND";
        string FRCNLOAMFieldStr = "LOAM";
        string FRCNSCLFieldStr = "SCL";
        string FRCNClayFieldStr = "CLAY";
        string FRCNlcluFieldStr = "LCLU";
        //
        // Data Array Parameters

        const double InvalidRCN = -1;//double.NaN;
        //
        string[] FOrderArray = null;
        string[] FSandArray = null;
        string[] FLoamArray = null;
        string[] FSCLArray = null;
        string[] FClayArray = null;
        string[] FLCLUArray = null;
        //
        List<DataStructRCN> FRCNDataList = new List<DataStructRCN>();
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassRCN(string DataDirectory, string Filename)
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
                throw new Exception("Error loading Runoff Curve Number Data- file RCNbyLCLU" + errMessage);
            }
            // build data arrays
            int arraysize = TheData.Rows.Count;
            //
            FOrderArray = new string[arraysize];
            FSandArray = new string[arraysize];
            FLoamArray = new string[arraysize];
            FSCLArray = new string[arraysize];
            FClayArray = new string[arraysize];
            FLCLUArray = new string[arraysize];
            //
            foreach (DataRow DR in TheData.Rows)
            {
                string codestr = DR[FRCNlcluFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                //int codeint = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                bool pass = true;
                if (pass)
                {
                    string orderStr = DR[FRCNOrderFieldStr].ToString();
                    string sandStr = DR[FRCNSandFieldStr].ToString();
                    string loamStr = DR[FRCNLOAMFieldStr].ToString();
                    string sclStr = DR[FRCNSCLFieldStr].ToString();
                    string clayStr = DR[FRCNClayFieldStr].ToString();
                    string lcluStr = DR[FRCNlcluFieldStr].ToString();
                    //
                    int tempOrder = Tools.ConvertToInt32(orderStr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double tempSand = Tools.ConvertToDouble(sandStr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double tempLoam = Tools.ConvertToDouble(loamStr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double tempSCL = Tools.ConvertToDouble(sclStr, ref isErr, ref errMessage);
                                if (!isErr)
                                {

                                    double tempClay = Tools.ConvertToDouble(clayStr, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {
                                        string tempLCLU = lcluStr;//, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            DataStructRCN AD = new DataStructRCN(tempOrder, tempSand, tempLoam, tempSCL, tempClay, tempLCLU);
                                            FRCNDataList.Add(AD);
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
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

        // ======================================================================
        //   
        public double Fast_A_RCN(string lclu)
        {
            double temp = InvalidRCN;
            DataStructRCN TheData = FRCNDataList.Find(delegate (DataStructRCN AD)
            {
                return (AD.LCLU == lclu);
            });
            temp = TheData.Sand;
            return temp;
        }
        //=========================
        //   
        public double Fast_B_RCN(string lclu)
        {
            double temp = InvalidRCN;
            DataStructRCN TheData = FRCNDataList.Find(delegate (DataStructRCN AD)
            {
                return (AD.LCLU == lclu);
            });
            temp = TheData.Loam;
            return temp;
        }
        //=========================
        //   
        public double Fast_C_RCN(string lclu)
        {
            double temp = InvalidRCN;
            DataStructRCN TheData = FRCNDataList.Find(delegate (DataStructRCN AD)
            {
                return (AD.LCLU == lclu);
            });
            temp = TheData.SCL;
            return temp;
        }
        //=========================
        //   
        public double Fast_D_RCN(string lclu)
        {
            double temp = InvalidRCN;
            DataStructRCN TheData = FRCNDataList.Find(delegate (DataStructRCN AD)
            {
                return (AD.LCLU == lclu);
            });
            temp = TheData.Clay;
            return temp;
        }




    }

    /// <summary>
    /// 
    /// </summary>
    public class DataClassLcluArea
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
        //
        string FAreaTurfFieldStr = "Turf";
        string FAreaWoodsFieldStr = "Woods";
        string FAreaWaterFieldStr = "OpenWater";
        string FAreaPastureFieldStr = "Pasture";
        string FAreaRowCropsFieldStr = "RowCrops";
        string FAreaFallowFieldStr = "Fallow";
        string FAreaIndFieldStr = "Ind";
        string FAreaCommFieldStr = "Comm";
        string FAreaPavedFieldStr = "Paved";
        string FAreaEigthAcreFieldStr = "EigthAcre";
        string FAreaQuarterAcreFieldStr = "QuarterAcre";
        string FAreaThirdAcreFieldStr = "ThirdAcre";
        string FAreaHalfAcreFieldStr = "HalfAcre";
        string FAreaAcreFieldStr = "Acre";
        string FAreaTotalFieldStr = "Total";
        //

        string FcurrentYearFieldStr = "YEAR";

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;
        //
        double[] FTurfArray = null;
        double[] FWoodsArray = null;
        double[] FWaterArray = null;
        double[] FPastureArray = null;
        double[] FCropsArray = null;
        double[] FFallowarray = null;
        double[] FIndArray = null;
        double[] FCommArray = null;
        double[] FPaved = null;
        double[] FEightArray = null;
        double[] FQuarterArray = null;
        double[] FThirdArray = null;
        double[] FHalfArray = null;
        double[] FAcreArray = null;
        double[] FTotalArray = null;
        //
        double[] FYearArray = null;

        List<DataStructRCNArea> FLcluClassesDataList = new List<DataStructRCNArea>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassLcluArea(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory + "\\Inputs\\";
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
            //
            FTurfArray = new double[arraysize];
            FWoodsArray = new double[arraysize];
            FWaterArray = new double[arraysize];
            FPastureArray = new double[arraysize];
            FCropsArray = new double[arraysize];
            FFallowarray = new double[arraysize];
            FIndArray = new double[arraysize];
            FCommArray = new double[arraysize];
            FPaved = new double[arraysize];
            FEightArray = new double[arraysize];
            FQuarterArray = new double[arraysize];
            FThirdArray = new double[arraysize];
            FHalfArray = new double[arraysize];
            FAcreArray = new double[arraysize];
            FTotalArray = new double[arraysize];
            //
            //FAcresIndArray = new double[arraysize];


            //
            FYearArray = new double[arraysize];
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                int codeint = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                if (!isErr)
                {

                    string turffStr = DR[FAreaTurfFieldStr].ToString();
                    string woodsStr = DR[FAreaWoodsFieldStr].ToString();
                    string waterStr = DR[FAreaWaterFieldStr].ToString();
                    string pastureStr = DR[FAreaPastureFieldStr].ToString();
                    string cropsStr = DR[FAreaRowCropsFieldStr].ToString();
                    string fallowStr = DR[FAreaFallowFieldStr].ToString();
                    string indStr = DR[FAreaIndFieldStr].ToString();
                    string commStr = DR[FAreaCommFieldStr].ToString();
                    string pavedStr = DR[FAreaPavedFieldStr].ToString();
                    string eigthStr = DR[FAreaEigthAcreFieldStr].ToString();
                    string quarterStr = DR[FAreaQuarterAcreFieldStr].ToString();
                    string thirdStr = DR[FAreaThirdAcreFieldStr].ToString();
                    string halfStr = DR[FAreaHalfAcreFieldStr].ToString();
                    string acreStr = DR[FAreaAcreFieldStr].ToString();
                    string totalStr = DR[FAreaTotalFieldStr].ToString();
                    //
                    //string rindacresstr = DR[FAcerageIndFieldStr].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double tempTurf = Tools.ConvertToDouble(turffStr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double tempWoods = Tools.ConvertToDouble(woodsStr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double tempWater = Tools.ConvertToDouble(waterStr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double tempPasture = Tools.ConvertToDouble(pastureStr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    double tempCrops = Tools.ConvertToDouble(cropsStr, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {
                                        double tempFallow = Tools.ConvertToDouble(fallowStr, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            double tempInd = Tools.ConvertToDouble(indStr, ref isErr, ref errMessage);
                                            if (!isErr)
                                            {
                                                double tempComm = Tools.ConvertToDouble(commStr, ref isErr, ref errMessage);
                                                if (!isErr)
                                                {
                                                    double tempPaved = Tools.ConvertToDouble(pavedStr, ref isErr, ref errMessage);
                                                    if (!isErr)
                                                    {
                                                        double tempEigth = Tools.ConvertToDouble(eigthStr, ref isErr, ref errMessage);
                                                        if (!isErr)
                                                        {
                                                            double tempQuarter = Tools.ConvertToDouble(quarterStr, ref isErr, ref errMessage);
                                                            if (!isErr)
                                                            {
                                                                double tempThird = Tools.ConvertToDouble(thirdStr, ref isErr, ref errMessage);
                                                                if (!isErr)
                                                                {
                                                                    double tempHalf = Tools.ConvertToDouble(halfStr, ref isErr, ref errMessage);
                                                                    if (!isErr)
                                                                    {
                                                                        double tempAcre = Tools.ConvertToDouble(acreStr, ref isErr, ref errMessage);
                                                                        if (!isErr)
                                                                        {
                                                                            int tempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                                                                            if (!isErr)
                                                                            {
                                                                                double tempTot = Tools.ConvertToInt32(totalStr, ref isErr, ref errMessage);
                                                                                if (!isErr)
                                                                                {
                                                                                    // OK 
                                                                                    DataStructRCNArea AD = new DataStructRCNArea(codestr, tempYear, tempTurf, tempWoods, tempWater,
                                                                                    tempPasture, tempCrops, tempFallow, tempInd, tempComm, tempPaved, tempEigth, tempQuarter,
                                                                                    tempThird, tempHalf, tempAcre, tempTot, namestr);
                                                                                    FLcluClassesDataList.Add(AD);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
        //public double FastTurfArea(string UnitName, int year)
        //{
        //    double temp = InvalidRate;
        //    DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
        //    {
        //        return ((AD.TheYear == year) && (AD.UnitName == UnitName));
        //    });

        //    if (TheData.UnitName == UnitName)
        //    {
        //        temp = TheData.Turf;
        //    }
        //    return temp;
        //}
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName">The region names</param>
        /// <param name="lcluClass"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastArea_UN(string UnitName, string lcluClass, int year)
        {
            double temp = InvalidRate;
            DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                switch (lcluClass)
                {
                    case "Turf":
                        temp = TheData.Turf;
                        break;
                    case "Woods":
                        temp = TheData.Woods;
                        break;
                    case "OpenWater":
                        temp = TheData.Water;
                        break;
                    case "Pasture":
                        temp = TheData.Pasture;
                        break;
                    case "RowCrops":
                        temp = TheData.RowCrops;
                        break;
                    case "Fallow":
                        temp = TheData.Fallow;
                        break;
                    case "Ind":
                        temp = TheData.Industry;
                        break;
                    case "Comm":
                        temp = TheData.Commercial;
                        break;
                    case "Paved":
                        temp = TheData.Paved;
                        break;
                    case "EigthAcre":
                        temp = TheData.EigthAcre;
                        break;
                    case "QuarterAcre":
                        temp = TheData.QuarterAcre;
                        break;
                    case "ThirdAcre":
                        temp = TheData.ThirdAcre;
                        break;
                    case "HalfAcre":
                        temp = TheData.HalfAcre;
                        break;
                    case "Acre":
                        temp = TheData.Acre;
                        break;
                }
            }
            return temp;
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitCode">The region codes</param>
        /// <param name="lcluClass"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastArea_UC(int UnitCode, string lcluClass, int year)
        {
            double temp = InvalidRate;
            DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
            {
                return ((AD.TheYear == year) && (AD.Code == UnitCode));
            });

            if (TheData.Code == UnitCode)
            {
                switch (lcluClass)
                {
                    case "Turf":
                        temp = TheData.Turf;
                        break;
                    case "Woods":
                        temp = TheData.Woods;
                        break;
                    case "OpenWater":
                        temp = TheData.Water;
                        break;
                    case "Pasture":
                        temp = TheData.Pasture;
                        break;
                    case "RowCrops":
                        temp = TheData.RowCrops;
                        break;
                    case "Fallow":
                        temp = TheData.Fallow;
                        break;
                    case "Ind":
                        temp = TheData.Industry;
                        break;
                    case "Comm":
                        temp = TheData.Commercial;
                        break;
                    case "Paved":
                        temp = TheData.Paved;
                        break;
                    case "EigthAcre":
                        temp = TheData.EigthAcre;
                        break;
                    case "QuarterAcre":
                        temp = TheData.QuarterAcre;
                        break;
                    case "ThirdAcre":
                        temp = TheData.ThirdAcre;
                        break;
                    case "HalfAcre":
                        temp = TheData.HalfAcre;
                        break;
                    case "Acre":
                        temp = TheData.Acre;
                        break;
                }
            }
            return temp;
        }

        // ============================================================================================
        // total area of all RCN land cover land use classes
        // -----------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName">Region Name</param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastTotalUrbanArea_UN(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.EigthAcre;
                temp += TheData.QuarterAcre;
                temp += TheData.ThirdAcre;
                temp += TheData.HalfAcre;
                temp += TheData.Acre;
                //temp = TheData.TotalArea;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastTotalArea_UN(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
           
               temp = TheData.TotalArea;
            }
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitCode">Region code</param>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastTotalArea_UC(int UnitCode, int year)
        {
            double temp = InvalidRate;
            DataStructRCNArea TheData = FLcluClassesDataList.Find(delegate (DataStructRCNArea AD)
            {
                return ((AD.TheYear == year) && (AD.Code == UnitCode));
            });

            if (TheData.Code == UnitCode)
            {
                temp = TheData.TotalArea;
            }
            return temp;
        }
        //Urban high intensity: 10 DUA < UH 
        //Urban low intensity: 1.6 < UL< 10 DUA
        //Suburban: 0.4 < S< 1.6 DUA
        //Exurban high intensity: 0.1 < EH< 0.4 DUA
        //Exurban low intensity: 0.02 < EL< 0.1 DUA
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lcluClass"></param>
        /// <returns></returns>
        public double FastDUA_(string lcluClass)
        {
            double temp = 0;
            double DUA = 0;
            switch (lcluClass)
            {
                case "EigthAcre":
                    temp = 10;
                    break;
                case "QuarterAcre":
                    temp = 5.8;
                    break;
                case "ThirdAcre":
                    temp = 1;
                    break;
                case "HalfAcre":
                    temp = 0.25;
                    break;
                case "Acre":
                    temp = 0.06;
                    break;
            }
            DUA = temp;
            return DUA;
        }
        public double FastPPH_(string lcluClass)
        {
            double temp = 0;
            double PPH = 0;
            switch (lcluClass)
            {
                case "EigthAcre":
                    temp = 2.4;
                    break;
                case "QuarterAcre":
                    temp = 2.45;
                    break;
                case "ThirdAcre":
                    temp = 2.5;
                    break;
                case "HalfAcre":
                    temp = 2.5;
                    break;
                case "Acre":
                    temp = 2.5;
                    break;
            }
            PPH = temp;
            return PPH;
        }
        //Urban high intensity: 10 DUA < UH 
        //Urban low intensity: 1.6 < UL< 10 DUA
        //Suburban: 0.4 < S< 1.6 DUA
        //Exurban high intensity: 0.1 < EH< 0.4 DUA
        //Exurban low intensity: 0.02 < EL< 0.1 DUA

        // ==========================================================================================
    }

    #endregion DataClass
}
