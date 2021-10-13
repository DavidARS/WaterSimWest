using System;
using System.Collections.Generic;
using System.Linq;
using UniDB;
using System.Data;
using System.Text;

namespace DCDC_Utilities
{
    public static class Utilities
    {
        //EDIT QUAY 10//10/10
        // ADDED THese, also in WaterSim Unitcode UDI
        // Values to use as place holders for bad values
        public static int BadIntValue = int.MinValue;
        public static double BadDoubleValue = double.NaN;
        //End Edit
        /// <summary>
        ///  This constant is multiplied against gallons to obtain liters
        /// </summary>
        public const double convertGallonsToLiters = 3.785411784;

        #region Power Curve
        public static double loadSensitivity(double TavHistorical, double TavScenario)
        {
            double temp = 0;
            const double asymptote = 3.692; // 3.5 is actual asymptote - Load sensitivity asymptote
            const double scaler = 0.1174;
            const double t0 = 20; // Celsius
            double difference = 0;
            double T = 0;
            //
            difference = Math.Max(0, TavScenario - TavHistorical);
            T = TavHistorical + difference;
            //
            if (20 <= T)
            {
                temp = (asymptote * (1 - Math.Exp(-scaler * (T - t0))) * 1/100.0 );
            }
            return temp;

        }

        //        Source DF Sum of Squares Mean Square F Value Approx
        //Pr > F
        //Model 2 102.9 51.4498 10214.8 <.0001 
        //Error 10 0.0504 0.00504     
        //Uncorrected Total 12 103.0       

        // approximate r2 = 0.999

        //Parameter Estimate Approx Std Error Approximate 95% Confidence Limits
        //a= 3.6892 0.0504 (3.5768 3.8016 )
        //k= 0.1174 0.00484 (0.1066 0.1282 )



        //Approximate Correlation Matrix
        //  a k
        //a 1.0000000 -0.8591191 
        //k -0.8591191 1.0000000 



        #endregion Power Curve





        #region TemperatureEffects
        /// <summary>
        ///  This is a function to increase water demand based on air temperature; Although the model was generated using Average monthly data, 
        ///  we will have to use it on annual data. The regression was based on the City of Phoenix water use data. One year of monthly data at the
        ///  blockgroup level. Data were originally used by Darren Ruddell [post Doc for DCDC back in 2010-2011]
        ///  
        /// I used nonlinear precedures in SAS using a segmented modeling structure.
        ///  parameters are statistically significant
        /// </summary>
        /// <param name="population"></param> 
        /// <param name="time"></param> Place holder
        /// <param name="Tav"></param> Maximum annual, temperature
        /// <param name="Tmin></param>
        /// <param name="Tav"></param> Average temperature (Tav + Tmin)/2
        /// <param name="Demandadd"></param> Liters per Capita Per Day to add to existing demand
        /// <returns></returns>
        public static double TemperatureFunction_AddDemand(double population, int time, double TavHistorical, double TavScenario)
        {
            double result = 1;
            double T = 1;
            double TemperatureBreak = 25.8;
            //double Intercept = -1299.7 - LPCDo;
            // double Intercept = -455.4 - LPCD;
            //double slope = 109.7 - LPCDo;
            double slope = 39.455;
            //double constant = 1553.7240 - LPCDo;
            // double constant=562.549 - LPCD;
            double Tscaler = 0;
            double temp = 0;
            //
            double difference = 0;
            difference = Math.Max(0, TavScenario - TavHistorical);
            //
            if (TavHistorical != -999) { T = TavHistorical + difference; }
            if(2063 < time)
            {

                bool stop=true;
            }

            if (T >= TemperatureBreak)
            {
                Tscaler = T - TemperatureBreak;
            }
            //
            temp = (Tscaler * slope )* 1 / convertGallonsToLiters; // LPCD proportion of change to galllons
            //result = Math.Round(temp * population * daysInAYear(time) ) ;
            result = Math.Round(temp * population); // units are already gallons per day
            //
            return result;
            // equation : LPCD = Intercept + slope * T;
            //
        }
        // 05.18.18 David Arthur Sampson
        //
        //Source DF Sum of Squares Mean Square F Value Approx Pr > F
        //Model 1 3978875 3978875 23.91 <.0001 
        //Error 193 32120119 166425     
        //Corrected Total 194 36098994       



        //Parameter Estimate ApproxStd Error Approximate 95% Confidence Limits
        //alpha -455.4 230.7 -910.5 -0.3452 
        //beta 39.4554 8.0693 23.5400 55.3707 

        // Balling and Gober 2007 - 60.7 LPCD per 1 degree Celsius (increase in urban water demand)
        // 
        #endregion
        /// <summary>
        ///  A function to determine the number of days in a year
        ///  written on 05.17.2018 by David Arthur Sampson
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static double daysInAYear(double year)
        {
            double result = 365;
            double Leapyear = (Convert.ToInt32((year + 1) / 4) * 4 - 1) + 1;
            if (year == Leapyear)
            {
                result = 366;
            }
            return result;
        }
        static double Dampen(double rate, double damper, double period)
        {
            double NewRate = 0;
            NewRate = rate * Math.Pow(damper, period);
            return NewRate;
        }

        //
        // ===========================
        // Sampson edits 07.08.2018
        public static double indoorGPCD(double ppl_HH, double rate, double damper, double period)
        {
            double result = 0;
            double indoorGPCD = 0;
            const double slope = -21.9;
            const double intercept = 76.416;
            try
            {
                if (0 < ppl_HH && ppl_HH <= 10)
                {
                    indoorGPCD = (slope * Math.Log(ppl_HH) + intercept) * Dampen(rate, damper, period);
                }
                result = indoorGPCD;
            }
            catch
            {

            }
            return result;
        }

        public static double KieferKrentz(double ppl_HH, double unitsPerAcre, double rate, double damper, double period)
        {
            double result = 0;
            double gpud = 0;
            double gpcd = 0;
            const double slope = -71.23;
            const double intercept = 379.44;
            if (0 < unitsPerAcre)
            {
                gpud = intercept + slope * Math.Log(unitsPerAcre);
                gpcd = gpud * (1 / ppl_HH) * Dampen(rate, damper, period);
            }
            return result;
        }
        // End Sampson edits 07.08.2018
        // ===============================

        // edits 06.12.20 das
        const double galAF = 325851.43326;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datain"> available in million acre-feet</param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static double MAFtoMGD(double datain, int year)
        {
            double dataOut = 0;
            //dataOut = (((datain * 1000000) * galAF ) / 1000000 ) / daysInAYear(year);
            dataOut = ((datain) * galAF) / daysInAYear(year);
            return dataOut;
        }
        // end edits 06.12.20 das
    }
    public struct DataTemperature
    {
        string FUnitName;
        string FUnitCodeStr;
        int FUnitCode;

        int FYear;
        double FTav;
        double FContemporary;
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aUnitName"></param>
        /// <param name="aUnitCode"></param>
        /// <param name="aTav"></param>
        /// <param name="aContemporary"></param>
        /// <param name="aYear"></param>
        public DataTemperature(string aUnitName, string aUnitCode, double aTav, double aContemporary, int aYear)
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
                FUnitCode = Utilities.BadIntValue;
            }
            FTav = aTav;
            FContemporary = aContemporary;
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the ag rate. </summary>
        ///
        /// <value> The ag rate. </value>
        ///-------------------------------------------------------------------------------------------------
        public double Tav
        {
            get { return FTav; }
        }
        public double Tcontemporary
        {
            get { return FContemporary; }
        }
        public int TheYear
        {
            get { return FYear; }
        }



    }

    /// 
    /// </summary>
    public class DataClassTemperature
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

        string FMaximumTempFieldStr = "TAV";
        string FContemporaryTempFieldStr = "TC";
        string FcurrentYearFieldStr = "YEAR";

        // Data Array Parameters

        Dictionary<string, int> StateCodes = new Dictionary<string, int>();
        const double InvalidRate = -1;//double.NaN;

        double[] FTavArray = null;
        double[] FContemporaryArray = null;
        double[] FYearArray = null;

        List<DataTemperature> FTemperatureDataList = new List<DataTemperature>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataDirectory"></param>
        /// <param name="Filename"></param>
        public DataClassTemperature(string DataDirectory, string Filename)
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
            FTavArray = new double[arraysize];
            FContemporaryArray = new double[arraysize];
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
                // int codeint = Tools.ConvertToInt32(codestr,ref isErr,ref errMessage);


                if (!isErr)
                {

                    string maximumtempstr = DR[FMaximumTempFieldStr].ToString();
                    string contemporarytempstr = DR[FContemporaryTempFieldStr].ToString();
                    string ryearsstr = DR[FcurrentYearFieldStr].ToString();

                    double TempTav = Tools.ConvertToDouble(maximumtempstr, ref isErr, ref errMessage);
                    if (!isErr)
                    {
                        double TempTC = Tools.ConvertToDouble(contemporarytempstr, ref isErr, ref errMessage);
                        if (!isErr)
                        {
                            int TempYear = Tools.ConvertToInt32(ryearsstr, ref isErr, ref errMessage);
                            if (!isErr)
                            {
                                // OK 
                                DataTemperature DT = new DataTemperature(namestr, codestr, TempTav, TempTC, TempYear);
                                FTemperatureDataList.Add(DT);
                                //// add to dictionary 
                            }


                        }
                    }
                }
            }

        }
        // ==============================================================


        // ==============================================================


        public double FastTav(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataTemperature TheData = FTemperatureDataList.Find(delegate (DataTemperature DT)
            {
                return ((DT.TheYear == year) && (DT.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.Tav;
            }
            return temp;
        }
        public double FastTC(string UnitName, int year)
        {
            double temp = InvalidRate;
            DataTemperature TheData = FTemperatureDataList.Find(delegate (DataTemperature DT)
            {
                return ((DT.TheYear == year) && (DT.UnitName == UnitName));
            });

            if (TheData.UnitName == UnitName)
            {
                temp = TheData.Tcontemporary;
            }
            return temp;
        }


        // =============================================================================================================
    }   // End of Class DataClass



}
