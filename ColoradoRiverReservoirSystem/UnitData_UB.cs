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
    public struct UBData
    {
      //  string FUnitName;
       // string FUnitCodeStr;
       // int FUnitCode;

        int FYear;
        double FCOUBasin_1;
        double FCOUBasin_2;
        double FCOUBasin_3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UB_1"></param>
        /// <param name="UB_2"></param>
        /// <param name="aYear"></param>
        public UBData(double UB_1, double UB_2, double UB_3, int aYear)
        {
            bool isErr = false;
           /// string errMsg = "";

            if (!isErr)
            {
            }
            else
            {
                // FUnitCode = UDI.BadIntValue;
            }
            FCOUBasin_1 = UB_1;
            FCOUBasin_2 = UB_2;
            FCOUBasin_3 = UB_3;

            FYear = aYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the unit. </summary>
        ///
        /// <value> The name of the unit. </value>
        ///-------------------------------------------------------------------------------------------------

        //public string UnitName
        //{
        //    get { return FUnitName; }
        //}

        /// <summary>   Gets the ag rate. </summary>
        ///
        /// <value> The ag rate. </value>
        ///-------------------------------------------------------------------------------------------------
        public double COUpperBasin_1
        {
            get { return FCOUBasin_1; }
        }
        public double COUpperBasin_2
        {
            get { return FCOUBasin_2; }
        }
        public double COUpperBasin_3
        {
            get { return FCOUBasin_3; }
        }

        public int TheYear
        {
            get { return FYear; }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class UnitData2
    {
        // DataTable Parameters
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "";
        // EDIT end 2 13 18

        // string FNameFieldStr = FRnameFieldStr;
        const string FRnameFieldStr = "UB1";
        string FColoradoUBasinData1 = "UB1";
        string FColoradoUBasinData2 = "UB2";
        string FColoradoUBasinData3 = "UB3";

        string FNameFieldStr = FRnameFieldStr;
        string FcurrentYearFieldStr = "Year";


        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FCOUBasin_1 = null;
        double[] FCOUBasin_2 = null;
        double[] FCOUBasin_3 = null;

        List<UBData> FDataList = new List<UBData>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public UnitData2(string DataDirectory, string Filename)
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
            FCOUBasin_1 = new double[arraysize];
            FCOUBasin_2 = new double[arraysize];
            FCOUBasin_3 = new double[arraysize];
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

                    string rCOUB_1 = DR[FColoradoUBasinData1].ToString();
                    string rCOUB_2 = DR[FColoradoUBasinData2].ToString();
                    string rCOUB_3 = DR[FColoradoUBasinData3].ToString();

                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double TempUB1 = Tools.ConvertToDouble(rCOUB_1, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempUB2 = Tools.ConvertToDouble(rCOUB_2, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double TempUB3 = Tools.ConvertToDouble(rCOUB_3, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    // OK 
                                    //string aUnitName, string aUnitCode, double anAcerageUrban, double anAcerageAg, double anAcerageInd, int aYear
                                    UBData UD = new UBData(TempUB1, TempUB2, TempUB3, TempYear);
                                    FDataList.Add(UD);
                                    //// add to dictionary 
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

        public double FastUB1(int year)
        {
            double temp = InvalidRate;
            UBData TheData = FDataList.Find(delegate (UBData FD) {
                return ((FD.TheYear == year));
            });

                temp = TheData.COUpperBasin_1;
            return temp;
        }
        public double FastUB2(int year)
        {
            double temp = InvalidRate;
            UBData TheData = FDataList.Find(delegate (UBData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.COUpperBasin_2;
            return temp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public double FastUB3(int year)
        {
            double temp = InvalidRate;
            UBData TheData = FDataList.Find(delegate (UBData FD) {
                return ((FD.TheYear == year));
            });

            temp = TheData.COUpperBasin_3;
            return temp;
        }

    }




}
