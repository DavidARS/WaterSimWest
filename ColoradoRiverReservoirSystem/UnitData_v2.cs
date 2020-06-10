using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data;
//using System.IO;
using UniDB;
//using WaterSimDCDC.Generic;

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
    public class UnitDataCO
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
        public UnitDataCO(string DataDirectory, string Filename)
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
    public struct ICSDataStruct
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;
        int FYear;
 
        double FICS_R;

        public ICSDataStruct(string aUnitName, string aUnitCode, int aYEAR, double aICS)
        {
            bool isErr = false;
            string errMsg = "";

            //FUnitName = aUnitName;
            FUnitCodeStr = aUnitCode;
            int temp = Tools.ConvertToInt32(FUnitCodeStr, ref isErr, ref errMsg);

            if (!isErr)
            {
                FUnitCode = temp;
            }
            else
            {
                FUnitCode = 0;// UDI.BadIntValue; // Connect this once the code is brought across
            }
            FUnitName = aUnitName;
            FICS_R = aICS;
            //
            FYear = aYEAR;
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
        public double ICS
        {
            get { return FICS_R; }
        }
        /// <summary>
        /// Returns the Data year
        /// </summary>
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
        const string FScodeFieldStr = "SC";
        const string FSnameFieldStr = "SN";
        // EDIT QUAY 2/19/18
        // Added Region Filed strings and setup to use State or Region
        const string FRnameFieldStr = "RN";
        const string FRcodeFieldStr = "RC";

        string FNameFieldStr = FRnameFieldStr;
        string FCodeFieldStr = FRcodeFieldStr;

        string FICSFieldStr = "ICS";
        //
        string FcurrentYearFieldStr = "YEAR";

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidData = -1;//double.NaN;

        double[] FRC_ICS = null;

        List<ICSDataStruct> FDataList = new List<ICSDataStruct>();
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
            FRC_ICS = new double[arraysize];

            foreach (DataRow DR in TheData.Rows)
            {
                // Get name and code
                // Setup to use region or state codes
                string namestr = DR[FNameFieldStr].ToString();
                string codestr = DR[FCodeFieldStr].ToString();
                // Decided not to use code in DataTable
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);

                if (!isErr)
                {
                    string rICS = DR[FICSFieldStr].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();
                    //
                    double TempICS = Tools.ConvertToDouble(rICS, ref isErr, ref errMessage);
                    if (!isErr)
                    {

                        int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            // OK 
                            ICSDataStruct ICS = new ICSDataStruct(namestr, codestr, TempYear, TempICS);
                            FDataList.Add(ICS);
                            //// add to dictionary 
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
        public double Fast_ICS(int Code, int year)
        {
            double temp = InvalidData;
            ICSDataStruct TheData = FDataList.Find(delegate (ICSDataStruct RC)
            {

                return RC.TheYear == year;
            });
            ICSDataStruct TheCode = FDataList.Find(delegate (ICSDataStruct TC) {
                    return TC.Code == Code;              
             });          
             temp = TheData.ICS;         
           return temp;
        }
        //
        public bool Fast_CODE(int Code)
        {
            bool temp = false;          
           foreach(ICSDataStruct ID in FDataList) {
                if (ID.Code == Code) temp = true;
                if(temp == true) break;
            }
            return temp;
        }
      

        //
    }
}
