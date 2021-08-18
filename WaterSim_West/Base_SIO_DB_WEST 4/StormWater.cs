using System;
using System.Collections.Generic;
using WaterSimDCDC.Generic;
using System.Linq;
using System.Data;
using UniDB;
using System.Text;

namespace WaterSim_Base
{
    /// <summary>
    /// Class to model stormwater runoff
    /// </summary>
    public class StormWater
    {
        WaterSimCRFModel CRF;
        UnitData FUnitData;
        public DataClassLcluArea RCNarea;
        public DataClassLcluRCN RCN;
        //
        public bool isInstantiated = false;
        /// <summary>
        /// The data file that holds the area for each LCLU class for each state and county
        /// </summary>
        //string LcluClassesDataFilename = "LCLUclassArea.csv";
        /// <summary>
        /// Runoff Curve numbers for each LCLU class
        /// </summary>
        //string LcluClassesRCNDataFilename = "LCLUrcn.csv";

        ///
        public StormWater(DataClassLcluArea DC, DataClassLcluRCN rcn)
        {
            RCNarea = DC;
            RCN = rcn;
            isInstantiated = true;
            //string soil = "B";
            //double test = rcn.FastTurfRCN(soil);
            //string unit = "California Southwest";
            //double Test = RCNarea.FastTurfArea(unit,2020);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnitData"></param>
        /// <param name="DC"></param>
        /// <param name="rcn"></param>
        public StormWater(UnitData UnitData, DataClassLcluArea DC, DataClassLcluRCN rcn)
        {
            RCNarea = DC;
            RCN = rcn;
            FUnitData = UnitData;
            isInstantiated = true;
            //string soil = "B";
            //double test = rcn.FastTurfRCN(soil);
            //string unit = "California Southwest";
            //double Test = RCNarea.FastTurfArea(unit, 2020);
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

        //
        public void AreaByRCN(string region, int year)
        {
            string soil = "A";
            double T;
            double total = T = 0;
            
            foreach (string Name in FUnitData.UnitNames)
            {
                foreach (LcluClasses e in Enum.GetValues(typeof(LcluClasses)))
                {
                    T = RCNvalueByClass(region, e, year, soil);
                }
                total += T;
            }
        }
        double RCNvalueByClass(string unit, LcluClasses e, int year, string soil)
        {
            double temp = 0;
            double turf = RCNarea.FastTurfArea(unit, year);
            double total = RCNarea.FastTotalArea(unit, year);
            double rcn = RCN.FastTurfRCN(soil);
            temp = (turf/total) * rcn;
            return temp;
        }


    }





    #region strut(s)
    public struct DataStructRCN
    {
        string FLCLU;
        int FOrder;
        double FSand;
        double FLoam;
        double FSCL;
        double FClay;

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


    public struct DataStructLcluRCN
    {
        //string FUnitName;
        //string FUnitCodeStr;
        //int FUnitCode;

        string FSoil;
        double FRCNTurf;
        double FRCNWoods;
        double FRCNOpenWater;
        double FRCNPasture;
        double FRCNRowCrops;
        double FRCNFallow;
        double FRCNIndustry;
        double FRCNCommercial;
        double FRCNPaved;
        double FRCNEigthAcre;
        double FRCNQuarterAcre;
        double FRCNThirdAcre;
        double FRCNHalfAcre;
        double FRCNAcre;
        public DataStructLcluRCN(string aSoil, double turf, double woods, double openWater,
                     double pasture, double rowCrops, double fallow, double industrial, double commercial,
                     double paved, double eigthAcre, double quarterAcre, double thirdAcre, double halfAcre, double acre)
        {
            bool isErr = false;
            string errMsg = "";

            //FUnitName = aUnitName;
            //FUnitCodeStr = aUnitCode;

            //int temp = Tools.ConvertToInt32(FUnitCodeStr, ref isErr, ref errMsg);
            //if (!isErr)
            //{
              //  FUnitCode = temp;
            //}
            //else
            //{
             //   FUnitCode = UDI.BadIntValue;
            //}

            FSoil = aSoil;
            FRCNTurf = turf;
            FRCNWoods = woods;
            FRCNOpenWater = openWater;
            FRCNPasture = pasture;
            FRCNRowCrops = rowCrops;
            FRCNFallow = fallow;
            FRCNIndustry = industrial;
            FRCNCommercial = commercial;
            FRCNPaved = paved;
            FRCNEigthAcre = eigthAcre;
            FRCNQuarterAcre = quarterAcre;
            FRCNThirdAcre = thirdAcre;
            FRCNHalfAcre = halfAcre;
            FRCNAcre = acre;

        }
        //public string UnitName
        //{
        //    get { return FUnitName; }
        //}

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the unit code string. </summary>
        /////
        ///// <value> The f unit code string. </value>
        /////-------------------------------------------------------------------------------------------------

        //public string UnitCodeStr
        //{
        //    get { return FUnitCodeStr; }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the code. </summary>
        ///
        /// <value> The code. </value>
        ///-------------------------------------------------------------------------------------------------

        //public int Code
        //{
        //    get { return FUnitCode; }
        //}

        public double Turf
        {
            get { return FRCNTurf; }
        }
        public double Woods
        {
            get { return FRCNWoods; }
        }
        public double Water
        {
            get { return FRCNOpenWater; }
        }
        public double Pasture
        {
            get { return FRCNPasture; }
        }
        public double RowCrops
        {
            get { return FRCNRowCrops; }
        }
        public double Fallow
        {
            get { return FRCNFallow; }
        }
        public double Industry
        {
            get { return FRCNIndustry; }
        }
        public double Commercial
        {
            get { return FRCNCommercial; }
        }
        public double Paved
        {
            get { return FRCNPaved; }
        }
        public double EigthAcre
        {
            get { return FRCNEigthAcre; }
        }
        public double QuarterAcre
        {
            get { return FRCNQuarterAcre; }
        }
        public double ThirdAcre
        {
            get { return FRCNThirdAcre; }
        }
        public double HalfAcre
        {
            get { return FRCNHalfAcre; }
        }
        public double Acre
        {
            get { return FRCNAcre; }
        }
        public string TheSoil
        {
            get { return FSoil; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct DataStructLcluArea
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
        public DataStructLcluArea(string aUnitCode, int aYear, double turf, double woods, double openWater,
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
                int codeint = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                if (!isErr)
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
                                        string tempLCLU = clayStr;//, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            DataStructRCN AD = new DataStructRCN(tempOrder, tempSand, tempLoam,tempSCL,tempClay,tempLCLU);
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
                return (AD.LCLU == lclu) ;
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











    #region RCN
    public class DataClassLcluRCN
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        //const string FScodeFieldStr = "SC";
        //const string FSnameFieldStr = "SN";
        // EDIT QUAY 2/19/18
        // Added Region Filed strings and setup to use State or Region
        //const string FRnameFieldStr = "RN";
        //const string FRcodeFieldStr = "RC";

        //string FNameFieldStr = FRnameFieldStr;
        //string FCodeFieldStr = FRcodeFieldStr;
        //
        string FRCNTurfFieldStr = "Turf";
        string FRCNWoodsFieldStr = "Woods";
        string FRCNWaterFieldStr = "OpenWater";
        string FRCNPastureFieldStr = "Pasture";
        string FRCNRowCropsFieldStr = "RowCrops";
        string FRCNFallowFieldStr = "Fallow";
        string FRCNIndFieldStr = "Ind";
        string FRCNCommFieldStr = "Comm";
        string FRCNPavedFieldStr = "Paved";
        string FRCNEigthAcreFieldStr = "EigthAcre";
        string FRCNQuarterAcreFieldStr = "QuarterAcre";
        string FRCNThirdAcreFieldStr = "ThirdAcre";
        string FRCNHalfAcreFieldStr = "HalfAcre";
        string FRCNAcreFieldStr = "Acre";
        //
        string FsoilFieldStr = "TEXTURE";

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
        //
        double[] FSoilArray = null;

        List<DataStructLcluRCN> FLcluRCNDataList = new List<DataStructLcluRCN>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassLcluRCN(string DataDirectory, string Filename)
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
                throw new Exception("Error loading Runoff Curve Number Data. " + errMessage);
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
            //
            //
            FSoilArray = new double[arraysize];
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // EDIT QUAY 2/19/18
                // Setup to use region or state codes
                //string namestr = DR[FNameFieldStr].ToString();
                //string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                string texture = "A";
                bool pass = true;
                string rsoilstr = DR[FsoilFieldStr].ToString();

                if (pass)
                {
                   // string rsoilstr = DR[FsoilFieldStr].ToString();

                    string turffStr = DR[FRCNTurfFieldStr].ToString();
                    string woodsStr = DR[FRCNWoodsFieldStr].ToString();
                    string waterStr = DR[FRCNWaterFieldStr].ToString();
                    string pastureStr = DR[FRCNPastureFieldStr].ToString();
                    string cropsStr = DR[FRCNRowCropsFieldStr].ToString();
                    string fallowStr = DR[FRCNFallowFieldStr].ToString();
                    string indStr = DR[FRCNIndFieldStr].ToString();
                    string commStr = DR[FRCNCommFieldStr].ToString();
                    string pavedStr = DR[FRCNPavedFieldStr].ToString();
                    string eigthStr = DR[FRCNEigthAcreFieldStr].ToString();
                    string quarterStr = DR[FRCNQuarterAcreFieldStr].ToString();
                    string thirdStr = DR[FRCNThirdAcreFieldStr].ToString();
                    string halfStr = DR[FRCNHalfAcreFieldStr].ToString();
                    string acreStr = DR[FRCNAcreFieldStr].ToString();
                    //
                    //string rindacresstr = DR[FAcerageIndFieldStr].ToString();
                    //string rsoilstr = DR[FsoilFieldStr].ToString();

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
                                                                            string tempSoil = rsoilstr;
                                                                            if (!isErr)
                                                                            {
                                                                                // OK 
                                                                                DataStructLcluRCN AD = new DataStructLcluRCN(tempSoil, tempTurf, tempWoods, tempWater,
                                                                                    tempPasture, tempCrops, tempFallow, tempInd, tempComm, tempPaved, tempEigth, tempQuarter,
                                                                                    tempThird, tempHalf, tempAcre);
                                                                                FLcluRCNDataList.Add(AD);
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
        public double[] TurfRCN()
        {
            return GetDataArray(FRCNTurfFieldStr);
        }
        public double FastTurfRCN(string soil)
        {
            double temp = InvalidRate;
            DataStructLcluRCN TheData = FLcluRCNDataList.Find(delegate (DataStructLcluRCN AD)
            {
                bool v = AD.TheSoil == soil;
                return v;
            });
            if (TheData.TheSoil == soil)
            {
                temp = TheData.Turf;
            }
                return temp;
            
        }

    }
    #endregion RDN
    //=================================

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

        List<DataStructLcluArea> FLcluClassesDataList = new List<DataStructLcluArea>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassLcluArea(string DataDirectory, string Filename)
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
                                                                                    DataStructLcluArea AD = new DataStructLcluArea(codestr, tempYear, tempTurf, tempWoods, tempWater,
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
        public double[] TurfArea()
        {
            return GetDataArray(FAreaTurfFieldStr);
        }
        #endregion DataClass

        public double FastTurfArea(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataStructLcluArea TheData = FLcluClassesDataList.Find(delegate (DataStructLcluArea AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.Turf;
            }
            return temp;
        }
        public double FastTotalArea(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataStructLcluArea TheData = FLcluClassesDataList.Find(delegate (DataStructLcluArea AD)
            {
                return ((AD.TheYear == year) && (AD.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.TotalArea;
            }
            return temp;
        }


    }
}
