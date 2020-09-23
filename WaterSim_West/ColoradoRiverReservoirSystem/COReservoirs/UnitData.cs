using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using UniDB;

namespace WaterSimDCDC.Generic
{
    public struct FlowData
    {
        string FUnitName;
       // string FUnitCodeStr;
       // int FUnitCode;

        int FYear;
        double FCOflows;
        double FCOInFlows;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitName"></param>
        /// <param name="FlowData"></param>
        /// <param name="aYear"></param>
        public FlowData(string aUnitName, double FlowData, double InFlowData, int aYear)
        {
            bool isErr = false;
           // string errMsg = "";

            FUnitName = aUnitName;

            if (!isErr)
            {
            }
            else
            {
                // FUnitCode = UDI.BadIntValue;
            }
            FCOflows = FlowData;
            FCOInFlows = InFlowData;
            FYear = aYear;
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

        /// <summary>   Gets the ag rate. </summary>
        ///
        /// <value> The ag rate. </value>
        ///-------------------------------------------------------------------------------------------------
        public double COriverFlows
        {
            get { return FCOflows; }
        }
        public double COriverInFlows
        {
            get { return FCOInFlows; }
        }

        public int TheYear
        {
            get { return FYear; }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class UnitData
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        // string FNameFieldStr = FRnameFieldStr;
        const string FRnameFieldStr = "COflows";
        string FColoradoRiverFlowData = "COflows";
        string FColoradoRiverInFlowData = "COinflows";
        string FNameFieldStr = FRnameFieldStr;
        string FcurrentYearFieldStr = "YR";


        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FCOriverFlows = null;
        double[] FCOriverInFlows = null;
        List<FlowData> FDataList = new List<FlowData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public UnitData(string DataDirectory, string Filename)
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
            FCOriverFlows = new double[arraysize];
            FCOriverInFlows = new double[arraysize];
            //int CodeI = 0;
            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // Setup to use region or state codes
                string namestr = DR[FNameFieldStr].ToString();

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                if (!isErr)
                {

                    string rCOriverFlows = DR[FColoradoRiverFlowData].ToString();
                    string rCOriverInFlows = DR[FColoradoRiverInFlowData].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double TempCO = Tools.ConvertToDouble(rCOriverFlows, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempCOinFlow = Tools.ConvertToDouble(rCOriverInFlows, ref isErr, ref errMessage);
                        if (!isErr)
                        {

                            int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                // OK 
                                //string aUnitName, string aUnitCode, double anAcerageUrban, double anAcerageAg, double anAcerageInd, int aYear
                                FlowData UD = new FlowData(namestr, TempCO, TempCOinFlow, TempYear);
                                FDataList.Add(UD);
                                //// add to dictionary 
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
        //

        public double FastFlow(int year)
        {
            double temp = InvalidRate;
            FlowData TheData = FDataList.Find(delegate (FlowData FD) {
                return ((FD.TheYear == year));
            });

                temp = TheData.COriverFlows;
            return temp;
        }
        public double FastInFlow(int year)
        {
            double temp = InvalidRate;
            FlowData TheData = FDataList.Find(delegate (FlowData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.COriverInFlows;
            return temp;
        }


    }
    // ========================================================================
    //
    public struct ICSData
    {
       // string FUnitName;
 
        int FYear;
        double FICS_AZ;
        double FICS_CA;
        double FICS_IID;
        //double FICS_MWD;
        double FICS_NV;
        double FICS_SNWA;
        double FICS_NVOther;
        double FICS_TON;
        double FICS_GILA;


        public ICSData(double aAZ, double aCA, double aIID, double aNV, double aSNWA, 
            double aNVother, double aTON, double aGILA, int aYear)
        {
            bool isErr = false;
            // string errMsg = "";

            //FUnitName = aUnitName;

            if (!isErr)
            {
            }
            else
            {
            }
            FICS_AZ=aAZ;
            FICS_CA=aCA;
            FICS_IID=aIID;
           // FICS_MWD=aMWD;
            FICS_NV=aNV;
            FICS_SNWA=aSNWA;
            FICS_NVOther=aNVother;
            FICS_TON=aTON;
            FICS_GILA=aGILA;
            //
            FYear = aYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the unit. </summary>
        ///
        ///  <value> The name of the unit. </value>
        ///-------------------------------------------------------------------------------------------------

        //public string UnitName
        //{
        //    get { return FUnitName; }
        //}

        /// <summary>   Gets the ag rate. </summary>
        ///
        /// <value> The ag rate. </value>
        ///-------------------------------------------------------------------------------------------------
        public double ICS_AZ
        {
            get { return FICS_AZ; }
        }
        public double ICS_CA
        {
            get { return FICS_CA; }
        }
        public double ICS_IID
        {
            get { return FICS_IID; }
        }
        //public double ICS_MWD
        //{
        //    get { return FICS_MWD; }
        //}
        public double ICS_NV
        {
            get { return FICS_NV; }
        }
        public double ICS_SNWA
        {
            get { return FICS_SNWA; }
        }
        public double ICS_NVOther
        {
            get { return FICS_NVOther; }
        }
        public double ICS_TON
        {
            get { return FICS_TON; }
        }
        public double ICS_GILA
        {
            get { return FICS_GILA; }
        }
        public int TheYear
        {
            get { return FYear; }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class UnitData_ICS
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        // string FNameFieldStr = FRnameFieldStr;
        //const string FRnameFieldStr = "ICS";

        string FICS_AZ = "Arizona";
        string FICS_CA = "California";
        string FICS_IID = "IID";
        string FICS_NV = "Nevada";
        string FICS_SNWA = "SNWA";
        string FICS_NVOther = "NVOther";
        string FICS_TON = "TON";
        string FICS_GILA = "Gila";
        //
      //  string FNameFieldStr = FRnameFieldStr;
        string FcurrentYearFieldStr = "Year";


        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FAZ_ICS= null;
        double[] FCA_ICS = null;
        double[] FIID_ICS = null;
        double[] FNV_ICS = null;
        double[] FSNWA_ICS = null;
        double[] FNVother_ICS = null;
        double[] FTON_ICS = null;
        double[] FGILA_ICS = null;
        List<ICSData> FDataList = new List<ICSData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public UnitData_ICS(string DataDirectory, string Filename)
        {
            string errMessage = "";
            bool isErr = false;
            FDataDirectory = DataDirectory;
            FFilename = Filename;
            UniDbConnection DbCon = new UniDbConnection(SQLServer.stText, "", FDataDirectory, "", "", "")
            {
                UseFieldHeaders = true
            };
            DbCon.Open();
            TheData = Tools.LoadTable(DbCon, FFilename, ref isErr, ref errMessage);
            if (isErr)
            {
                throw new Exception("Error loading Rate Data. " + errMessage);
            }
            // build data arrays
            int arraysize = TheData.Rows.Count;
            //
            FAZ_ICS = new double[arraysize];
            FCA_ICS = new double[arraysize];
            FIID_ICS = new double[arraysize];
            FNV_ICS = new double[arraysize];
            FSNWA_ICS = new double[arraysize];
            FNVother_ICS = new double[arraysize];
            FTON_ICS = new double[arraysize];
            FGILA_ICS = new double[arraysize];

            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // Setup to use region or state codes
               // string namestr = DR[FNameFieldStr].ToString();

                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                if (!isErr)
                {

                    string rICS_AZ = DR[FICS_AZ].ToString();
                    string rICS_CA = DR[FICS_CA].ToString();
                    string rICS_IID = DR[FICS_IID].ToString();
                    string rICS_NV = DR[FICS_NV].ToString();
                    string rICS_SNWA = DR[FICS_SNWA].ToString();
                    string rICS_NVother = DR[FICS_NVOther].ToString();
                    string rICS_TON = DR[FICS_TON].ToString();
                    string rICS_GILA = DR[FICS_GILA].ToString();
                    //
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double TempAZ = Tools.ConvertToDouble(rICS_AZ, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempCA = Tools.ConvertToDouble(rICS_CA, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double TempIID = Tools.ConvertToDouble(rICS_IID, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double TempNV = Tools.ConvertToDouble(rICS_NV, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    double TempSNWA = Tools.ConvertToDouble(rICS_SNWA, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {
                                        double TempNVother = Tools.ConvertToDouble(rICS_NVother, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {
                                            double TempTON = Tools.ConvertToDouble(rICS_TON, ref isErr, ref errMessage);
                                            if (!isErr)
                                            {
                                                double TempGILA = Tools.ConvertToDouble(rICS_GILA, ref isErr, ref errMessage);
                                                if (!isErr)
                                                {

                                                    int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                                                    if (!isErr)
                                                    {
                                                        // OK 
                                                        ICSData ICS = new ICSData(TempAZ, TempCA, TempIID, TempNV, TempSNWA, TempNVother, TempTON, TempGILA, TempYear);
                                                        FDataList.Add(ICS);
                                                        //// add to dictionary 
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
        //

        public double FastAZ_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_AZ;
            return temp;
        }
        public double FastCA_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_CA;
            return temp;
        }
        public double FastIID_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_IID;
            return temp;
        }
        public double FastNV_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_NV;
            return temp;
        }
        public double FastSNWA_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_SNWA;
            return temp;
        }
        public double FastNVOther_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_NVOther;
            return temp;
        }
        public double FastTON_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_TON;
            return temp;
        }
        public double FastGILA_ICS(int year)
        {
            double temp = InvalidRate;
            ICSData TheData = FDataList.Find(delegate (ICSData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.ICS_GILA;
            return temp;
        }



    }



}
