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




}
