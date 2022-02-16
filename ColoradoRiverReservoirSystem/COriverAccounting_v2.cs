using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using UniDB;
using DCDC_Utilities;

// EDIT QUAY 9/10/20
// Restored CORiverModel namespace
namespace CORiverModel
//namespace WaterSimDCDC.Generic
// END EDit

{
    /// <summary>
    /// A class to act on CO river flows and demands
    /// </summary>
    public class COriverAccounting
    {
         //
        double _UBallotment = 0;
        USGSdataClass DC;
        COriverModel theModel;
         //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename">of the USGS data with Colorado River Water variables</param>
        public COriverAccounting(string DataDirectory, string Filename, COriverModel Amodel){
            DC = new USGSdataClass(DataDirectory, Filename);
            theModel = Amodel;
        }
        //
           internal double UBallotment
        {
            get { return _UBallotment; }
            set { _UBallotment = value; }
        }
        public double UBasinMGD(int year)
        {
            UBallotment =  Utilities.MAFtoMGD(theModel.GetPMupperBasinTotal(), year);
            return UBallotment;
        }
        // ==================================================================================
        //
        public bool CORassess(int year)
        {
            bool temp = false;
            try
            {
                double Temp = UBasinMGD(year);
                double urb = DC.FastUCOL(8);
                string str = DC.FastRNCOL(8);
                temp = true;
            }
            catch (Exception e)
            {
                throw new Exception("public bool CORassess", e);
            }
            return temp;
        }
        //
        //==============================================================================================================
        /// <summary>
        /// Raw USGS data
        /// </summary>
        public struct USGSData
        {
            //string USGSstate;
            //string USGSregion;
            readonly int USGSRC;
            readonly double FUCOL;
            readonly double FACOL;
            readonly double FPCOL;
            readonly double FICOL;
            readonly double FOCOL;
            readonly double FTCOL;
            readonly string RName;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Region"></param>
            /// <param name="Urban"></param>
            /// <param name="Ag"></param>
            /// <param name="Power"></param>
            /// <param name="Industry"></param>
            /// <param name="Other"></param>
            /// <param name="Total"></param>
            public USGSData(int Region, double Urban, double Ag, double Power, double Industry, double Other, double Total, string Rname)
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
                //USGSstate = State;
                //USGSregion = Region;
                USGSRC = Region;
                FUCOL = Urban;
                FACOL = Ag;
                FPCOL = Power;
                FICOL = Industry;
                FOCOL = Other;
                FTCOL = Total;
                RName = Rname;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Gets the name of the unit. </summary>
            ///
            /// <value> The name of the unit. </value>
            ///-------------------------------------------------------------------------------------------------

            ///
            //public string state
            //{
            //    get { return USGSstate; }
            //}
            //public string region
            //{
            //    get { return USGSregion; }
            //}
            public int Rc
            {
                get { return USGSRC; }
            }
            public double Urban
            {
                get { return FUCOL; }
            }
            public double Ag
            {
                get { return FACOL; }
            }
            public double Power
            {
                get { return FPCOL; }
            }
            public double Industry
            {
                get { return FICOL; }
            }
            public double Other
            {
                get { return FOCOL; }
            }
            public double Total
            {
                get { return FTCOL; }
            }
            public string RegionName
            {
                get { return RName; }
            }
        }
        // ==============================================================================================================================
        //
        /// <summary>
        /// class for the USGS data call separate from the CFR
        /// </summary>
        public class USGSdataClass
        {
            // DataTable Parameters
            DataTable TheData = null;
            readonly string FDataDirectory = "";
            readonly string FFilename = "";
            // EDIT end 2 13 18

            // string FNameFieldStr = FRnameFieldStr;

            readonly string FRnameFieldStr1 = "RC";
            readonly string FColoradoRiverUse_1 = "UCOL";
            readonly string FColoradoRiverUse_2 = "ACOL";
            readonly string FColoradoRiverUse_3 = "PCOL";
            readonly string FColoradoRiverUse_4 = "ICOL";
            readonly string FColoradoRiverUse_5 = "OCOL";
            readonly string FColoradoRiverUse_6 = "COL";
            readonly string FRnameFieldStr2 = "RN";
            //
            // Data Array Parameters

            readonly Dictionary<string, int> RegionCodes = new Dictionary<string, int>();
            const double InvalidRate = -1;//double.NaN;
            const string InvalidString = "-1";

            readonly string[] FString = null;
            readonly int[] FRegion = null;
            readonly double[] FCORuse_1 = null;
            readonly double[] FCORuse_2 = null;
            readonly double[] FCORuse_3 = null;
            readonly double[] FCORuse_4 = null;
            readonly double[] FCORuse_5 = null;
            readonly double[] FCORuse_6 = null;
            //

            List<USGSData> FDataList = new List<USGSData>();
            /// <summary>
            /// 
            /// </summary>
            /// <param name="DataDirectory"></param>
            /// <param name="Filename"></param>
            public USGSdataClass(string DataDirectory, string Filename)
            {
                string errMessage = "";
                bool isErr = false;
                FDataDirectory = DataDirectory ;
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

                FString = new string[arraysize];
                FRegion = new int[arraysize];
                FCORuse_1 = new double[arraysize];
                FCORuse_2 = new double[arraysize];
                FCORuse_3 = new double[arraysize];
                FCORuse_4 = new double[arraysize];
                FCORuse_5 = new double[arraysize];
                FCORuse_6 = new double[arraysize];
                                                                                 

                //int CodeI = 0;
                foreach (DataRow DR in TheData.Rows)
                {
                     // Decided not to use code in DataTable
                    // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                    if (!isErr)
                    {

                        string FRegion = DR[FRnameFieldStr1].ToString();
                        string rCORuse_1 = DR[FColoradoRiverUse_1].ToString();
                        string rCORuse_2 = DR[FColoradoRiverUse_2].ToString();
                        string rCORuse_3 = DR[FColoradoRiverUse_3].ToString();
                        string rCORuse_4 = DR[FColoradoRiverUse_4].ToString();
                        string rCORuse_5 = DR[FColoradoRiverUse_5].ToString();
                        string rCORuse_6 = DR[FColoradoRiverUse_6].ToString();
                        string FString = DR[FRnameFieldStr2].ToString();
                        //
                        
                        int Temp1 = Tools.ConvertToInt32(FRegion, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            double Temp2 = Tools.ConvertToDouble(rCORuse_1, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                double Temp3 = Tools.ConvertToDouble(rCORuse_2, ref isErr, ref errMessage);
                                if (!isErr)
                                {
                                    double Temp4 = Tools.ConvertToDouble(rCORuse_3, ref isErr, ref errMessage);
                                    if (!isErr)
                                    {

                                        double Temp5 = Tools.ConvertToDouble(rCORuse_4, ref isErr, ref errMessage);
                                        if (!isErr)
                                        {

                                            double Temp6 = Tools.ConvertToDouble(rCORuse_5, ref isErr, ref errMessage);
                                            if (!isErr)
                                            {

                                                double Temp7 = Tools.ConvertToDouble(rCORuse_6, ref isErr, ref errMessage);
                                                if (!isErr)
                                                {
                                                    // EDIT QUAY 9/10/20
                                                    // Do not need to convert string to striong
                                                    string Temp8 = FString;
                                                    //string Temp8 = Tools.ConvertToString(FString, ref isErr, ref errMessage);
                                                    //if (!isErr)
                                                    //{
                                                        // OK 
                                                        //string aUnitName, string aUnitCode, double anAcerageUrban, double anAcerageAg, double anAcerageInd, int aYear
                                                        USGSData USGSD = new USGSData(Temp1, Temp2, Temp3, Temp4, Temp5, Temp6, Temp7,Temp8);
                                                        FDataList.Add(USGSD);
                                                        //// add to dictionary 
                                                        ///
                                                    //}
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

            public double FastUCOL(int rc)
            {
                double temp = InvalidRate;
                USGSData TheData = FDataList.Find(delegate (USGSData FD) {
                    return ((FD.Rc == rc));
                });

                temp = TheData.Urban;
                return temp;
            }
            public string FastRNCOL(int rc)
            {
                string temp = InvalidString;
                USGSData TheData = FDataList.Find(delegate (USGSData FD) {
                    return ((FD.Rc == rc));
                });

                temp = TheData.RegionName;
                return temp;
            }


        }
        //
        // ==============================================================================================================================


    }
}
