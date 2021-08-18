using System;
using System.Collections.Generic;
using WaterSimDCDC.Generic;
using System.Linq;
using System.Data;
using UniDB;
using System.Text;

namespace WaterSim_West_v_1
{
    public class StormWater
    {
        public DataClassLcluClasses RCNclasses;
        public DataClassLcluRCN RCN;
        /// <summary>
        /// The data file that holds the area for each LCLU class for each state and county
        /// </summary>
        string LcluClassesDataFilename = "LCLUclassArea.csv";
        /// <summary>
        /// Runoff Curve numbers for each LCLU class
        /// </summary>
        string LcluClassesRCNDataFilename = "LCLUrcn.csv";

        public StormWater(DataClassLcluClasses DC, DataClassLcluRCN rcn)
        {
            RCNclasses = DC;
            RCN = rcn;
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


    }
    #region strut(s)
    public struct DataLcluRCN
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;

        int FYear;
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
        public DataLcluRCN(string aUnitCode, int aYear, double turf, double woods, double openWater,
                     double pasture, double rowCrops, double fallow, double industrial, double commercial,
                     double paved, double eigthAcre, double quarterAcre, double thirdAcre, double halfAcre, double acre, string aUnitName)
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
        public int TheYear
        {
            get { return FYear; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct DataLcluClasses
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
        /// <param name="aUnitName"></param>
        public DataLcluClasses(string aUnitCode, int aYear, double turf, double woods, double openWater,
                            double pasture, double rowCrops, double fallow, double industrial, double commercial,
                            double paved, double eigthAcre, double quarterAcre, double thirdAcre, double halfAcre, double acre, string aUnitName)
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
    }
    #endregion struct
    #region DataClasses
    // ================================
    public class DataClassLcluRCN
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

        List<DataLcluRCN> FLcluRCNDataList = new List<DataLcluRCN>();
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
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // END EDIT

                // Decided not to use code in DataTable
                int codeint = Tools.ConvertToInt32(codestr, ref isErr, ref errMessage);
                if (!isErr)
                {

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
                    string rsoilstr = DR[FsoilFieldStr].ToString();

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
                                                                            int tempSoil = Tools.ConvertToInt32(rsoilstr, ref isErr, ref errMessage);
                                                                            if (!isErr)
                                                                            {
                                                                                // OK 
                                                                                DataLcluRCN AD = new DataLcluRCN(codestr, tempSoil, tempTurf, tempWoods, tempWater,
                                                                                    tempPasture, tempCrops, tempFallow, tempInd, tempComm, tempPaved, tempEigth, tempQuarter,
                                                                                    tempThird, tempHalf, tempAcre, namestr);
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


    }

        //=================================
        public class DataClassLcluClasses
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
        //
        double[] FYearArray = null;

        List<DataLcluClasses> FLcluClassesDataList = new List<DataLcluClasses>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassLcluClasses(string DataDirectory, string Filename)
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
                                                                                // OK 
                                                                                DataLcluClasses AD = new DataLcluClasses(codestr, tempYear, tempTurf, tempWoods, tempWater,
                                                                                    tempPasture, tempCrops, tempFallow, tempInd, tempComm, tempPaved, tempEigth, tempQuarter,
                                                                                    tempThird, tempHalf, tempAcre, namestr);
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

        //public double FastUrbanAcres(string UnitName, int year)
        //{
        //    double temp = InvalidRate;
        //    DataLCLU TheData = FRuralDataList.Find(delegate (DataLCLU AD)
        //    {
        //        return ((AD.TheYear == year) && (AD.UnitName == UnitName));
        //    });

        //    if (TheData.UnitName == UnitName)
        //    {
        //        temp = TheData.AcerageUrban;
        //    }
        //    return temp;
        //}
    }
}
