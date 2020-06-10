using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UniDB;
using ConsumerResourceModelFramework;
using WaterSimDCDC.Documentation;
//using WaterSimDCDC.America;
//=======================================================================================
// LEAPING INDICATORS!
// version 2.3
//  2/16/18
// Status
//  
//  NOTES
//   

//   NASTY

namespace WaterSimDCDC
{
    //=============================================================================================
    //
    //  Indicator Data Class
    // 
    //  Retreieves Data for use by all Indicators
    // 
    //=============================================================================================

    #region Indicator Data
    /// <summary>   Indicator data class. </summary>
    /// <remarks> Retreives data for use by all indicators</remarks>
    public class IndicatorDataClass
    {
        DataTable TheData = null;
        string FDataDirectory = "";
        string FFilename = "IndicatorData.csv";
        string FScodeFieldStr = "SCODE";

        string FSafeYieldFieldStr = "SFYG";
        string FEcoRatioFieldStr = "ECOGPCD";
        string FEcoValueFieldStr = "ECOVALUE";
        string FSurRatioFieldStr = "SURRATIO";
        string FSurFlowFieldStr = "STRMFLOW";
        string FAGRateFieldStr = "AGRATE";
        string FAGNETFieldStr = "AGNET";
        string FWWSurfFieldStr = "WWSURF";
        string FWWAdjustFieldStr = "WWADJ";
        string FWWWeightFieldStr = "WWWGHT";
        string FBaseEnvIndicatorFieldStr = "ENVIND";

        public IndicatorDataClass(string DataDirectory, string Filename)
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
                throw new Exception("Error loading Indicator Data. " + errMessage);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Base envirionment indicator. </summary>
        ///
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double BaseEnvirionmentIndicator(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[ FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FBaseEnvIndicatorFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastewater surface flow. </summary>
        ///
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterSurfaceFlow(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FWWSurfFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastewater weight. </summary>
        ///
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterWeight(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FWWWeightFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Wastewater adjustment. </summary>
        ///
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterAdjustment(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FWWAdjustFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Surface ratio. </summary>
        /// <remarks>   This is the Base Ratio of USGS Urban Surface Water WithDrawl to Total Stream Flow as a Percent.</remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SurfaceRatio(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FSurRatioFieldStr].ToString();
                         
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Surface flows. </summary>
        /// <remarks>   Total of mean Flows of surface waters at intake points for ubran water treatment plants, MGD</remarks>
        /// <param name="State">    The state. </param>
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double SurfaceFlows(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FSurFlowFieldStr].ToString();

                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Safe yield goal. </summary>
        ///<remarks> Should be MGD units</remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        public int SafeYieldGoal(int State)
        {
            int result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FSafeYieldFieldStr].ToString();
                        temp = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = temp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Economic ratio. </summary>
        ///<remarks>   No Units Ratio of Local water used for Economic Production by Total NonSaline Water Used</remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double EconomicGPCD(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FEcoRatioFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Eco value. </summary>
        /// <remarks>  Units is $ dollars per gallon of water used for local production</remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int EcoValue(int State)
        {
            int result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FEcoValueFieldStr].ToString();
                        temp = Tools.ConvertToInt32(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = temp;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ag rate. </summary>
        /// <remarks> Annual Growth Rate of Agricuture production as percent </remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double AGRate(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FAGRateFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ag net. </summary>
        /// <remarks> Net Annual Total Farm Income in Millions of Dollars</remarks>
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public double AGNet(int State)
        {
            double result = -1;
            bool iserr = true;
            string errMessage = "";
            foreach (DataRow DR in TheData.Rows)
            {
                string statecode = DR[FScodeFieldStr].ToString();
                int temp = Tools.ConvertToInt32(statecode, ref iserr, ref errMessage);
                if (!iserr)
                {
                    if (temp == State)
                    {
                        string valstr = DR[FAGNETFieldStr].ToString();
                        double tempDbl = Tools.ConvertToDouble(valstr, ref iserr, ref errMessage);
                        if (!iserr)
                        {
                            result = tempDbl;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }

    #endregion
    //##########################################################################
    //  
    //  INDICATORS
    //  
    //   
    //##########################################################################

    #region Indicators

    public class WebIndicator
    {
        protected WaterSimManager FWsim = null;
        protected IndicatorDataClass FIData = null;

    }
    // =================================================================================
    // 
    //   Groundwater Indicator Classes
    // 
    //  ==============================================================================. 
    #region Groundwater Indicator


    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A gw indicator process. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GWIndicatorProcess : AnnualFeedbackProcess
    {
        int FStateCode = 0;
        int FTotalSYDeficit = 0;
        int FGoal = 0;
        double FMaxWithdraw = 0;
        int FMaxWithdrawAsInt = 0;
        int FBaseWithdrawal = 0;
        int FAnnualRatio = 0;
        int FCumulativeRatio = 0;

        IndicatorDataClass FIData = null;
          ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///  <remarks> It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		   pre and post process calls</remarks>
        ///-------------------------------------------------------------------------------------------------

        public GWIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            FIData = iData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Self Yield Goal Tracker Process: This process keeps track of the cumaltive Safe Yield Deficit based on the state's Safe Yield goal";
            FProcessCode = "CSY";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   method that is called right before the first year of a simulation is called. </summary>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            FTotalSYDeficit = 0;
            // get the state code for this run
            int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            FStateCode = statecode;
            // get the gola for this state
            FGoal = FIData.SafeYieldGoal(statecode);
            // get the initial withdrawal
            FBaseWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epGroundwater).Value;
            // calulate SafeYield Withdrawal
            FMaxWithdraw = FBaseWithdrawal - FGoal;
            try
            {
                FMaxWithdrawAsInt = Convert.ToInt32(FMaxWithdraw);
            }
            catch (Exception ex)
            {
                // OUCH!!
            }

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. Error should be placed in FErrorMessage. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // get the current total GW withdrawal
            int GWWithdrawal = WSim.ParamManager.Model_Parameter(eModelParam.epGroundwater).Value;
            // Calculate cumulative net SY defecit
            int AnnualNet = GWWithdrawal - FMaxWithdrawAsInt;
            FTotalSYDeficit += AnnualNet;
            // setup for rations
            double GW = GWWithdrawal;
            double Goal = FGoal;
            double CumulativeAnnual = FTotalSYDeficit;
            // calcualte ratios
            double SYAnnualRatio = 0;
            double SYCumulativeRatio = 0;
            if (GW != 0)
            {
                SYAnnualRatio = FMaxWithdraw / GW;//Goal / GW;
                SYCumulativeRatio = CumulativeAnnual / GW;
            }
            // do double to int conversion as percent
            int tempAnnualRatio = 0;
            int tempCumulativeRatio = 0;
            try
            {
                tempAnnualRatio = Convert.ToInt32(SYAnnualRatio*100);
            }
            catch
            {
                tempAnnualRatio = 100;
            }
            try
            {
                tempCumulativeRatio = Convert.ToInt32(SYCumulativeRatio*100);
            }
            catch
            {
                tempCumulativeRatio = 100;
            }
            // save these values check for max
            if (tempCumulativeRatio > 100)
            {
                FCumulativeRatio = 100;
            }
            else
            {
                if (tempCumulativeRatio < 0)
                {
                    FCumulativeRatio = 0;
                }
                else
                {

                    FCumulativeRatio = tempCumulativeRatio;
                }
            }
            if (tempAnnualRatio > 100)
            {
                FAnnualRatio = 100;
            }
            else
            {
                FAnnualRatio = tempAnnualRatio;
            }

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the goal. </summary>
        ///
        /// <value> The goal. </value>
        ///-------------------------------------------------------------------------------------------------

        public int Goal
        {
            get { return FGoal; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the annual ratio. </summary>
        ///
        /// <value> The annual ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int AnnualRatio
        {
            get { return FAnnualRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cummulative ratio. </summary>
        ///
        /// <value> The cummulative ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public int CummulativeRatio
        {
            get { return 100-FCumulativeRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cumulative safe yield net. </summary>
        ///
        /// <value> The cumulative safe yield net. </value>
        ///-------------------------------------------------------------------------------------------------

        public int CumulativeSafeYieldNet
        {
            get { return FTotalSYDeficit; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the safe yield. </summary>
        ///
        /// <value> The safe yield. </value>
        ///-------------------------------------------------------------------------------------------------

        public int SafeYield
        {
            get { return FMaxWithdrawAsInt; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state code. </summary>
        ///
        /// <value> The state code. </value>
        ///-------------------------------------------------------------------------------------------------

        public int StateCode
        {
            get { return FStateCode; }
        }
 
    }
    //=================================================================================================
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A web indicator ground water. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class WebIndicator_GroundWater : WebIndicator
    {
        const int eGWAnnual_Indicator = 1;
        const int eGWCumulative_Indicator = 2;
        const int eGWCumulative_Deficit = 3;
        const int eGWAnnual_SafeYield_Deficit = 4;
        const int eGWSafeYield_Reduction = 5;
        const int eGWSafeYield = 6;

  //      int FBaseParam = 0;
        GWIndicatorProcess FGWProcess = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="BaseEModelParam">  The base model parameter. </param>
        /// <param name="WSim">             The simulation. </param>
        /// <param name="iData">            The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public WebIndicator_GroundWater(WaterSimManager WSim, IndicatorDataClass iData)
        {

            FWsim = WSim;
            FGWProcess = new GWIndicatorProcess("Safe Yield Indicator",FWsim, iData );
            FWsim.ProcessManager.AddProcess(FGWProcess);
            CreateModelParameters();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti annual indicator. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AnnualIndicator()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.AnnualRatio;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti cumulative indicator. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_CumulativeIndicator()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.CummulativeRatio;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti cumulative safe yield net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_CumulativeSafeYieldNet()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.CumulativeSafeYieldNet;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti safe yield reduction goal. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SafeYield_Reduction_Goal()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.Goal;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti safe yield. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SafeYield()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.SafeYield;
            }
            return value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the state code. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int StateCode()
        {
            int value = 0;
            if (FGWProcess != null)
            {
                value = FGWProcess.CumulativeSafeYieldNet;
            }
            return value;
            
        }


        /// <summary>   Creates the model parameters. </summary>
        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWsim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_Annual_Indicator, "Annual Safe Yield Indicatior", "GWSYA", geti_AnnualIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_Annual_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the annual ground water withdrawal, 0 means safe yield achived, 100 is max value", "%", "Percent", "Groundwater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_Cumulative_Indicator, "Scenario Safe Yield Indicatior", "GWSYC",geti_CumulativeIndicator));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_Cumulative_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the cumaulative difference between Safe Yield Withdrawal and annual ground water withdrawal, 0 means safe yield balance is achieved, 100 is max value", "%", "Percent", "Groundwater Cumulative Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_Cumulative_SafeYieldNet, "Safe Yield Cumulative Net", "GWSYN", geti_CumulativeSafeYieldNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_Cumulative_SafeYieldNet, "The total cumulative overdraft of groundwater based on the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield and annual ground water withdrawal, 0 means safe yield balance is achieved, 100 is max value", "mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_SafeYield_Reduction_Goal, "Safe Yield Reduction Goal", "GWSYG", geti_SafeYield_Reduction_Goal));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_SafeYield_Reduction_Goal, "The amount of reduction in annual groundwater based on the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield","mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            FPM.AddParameter(new ModelParameterClass(eModelParam.epGW_SafeYield, "Groundwater Safe Yield", "GWSY",geti_SafeYield));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_SafeYield, "The amount of annual groundwater withdrawal that represents safe yield or a balance with the amount of annual hgroundwater recharge", "mgd", "Millions of Gallons Per Day", "Groundwater Cumulative Overdraft", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    
    #endregion

    //=================================================================================
    //
    //  Economic Indicator Classes
    // 
    // ==============================================================================
    
    #region Economic Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Economic indicator process. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class EconomicIndicatorProcess : AnnualFeedbackProcess
    {
        int FStateCode = 0;
        WaterSimManager FWsim = null;
        IndicatorDataClass FIData = null;
        double FEcoGPCY= 0;
        int FEcoValue = 0;
        int FBaseTotalDemand = 0;
        int FBasePop = 0;
        int FBaseEcoDemand = 0;
        double FBaseRatio = 0;
        double FBaseGPCY = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///  <remarks> It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		   pre and post process calls</remarks>
        ///-------------------------------------------------------------------------------------------------

        public EconomicIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            FIData = iData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Economic Ratio and Value Tracker Process: This process keeps track economic impact of reduce water use";
            FProcessCode = "ERT";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   method that is called right before the first year of a simulation is called. </summary>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.ProcessStarted(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // get the state code for this run
            int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            FStateCode = statecode;
            // get the data
            FEcoGPCY = FIData.EconomicGPCD(FStateCode);
            FEcoValue = FIData.EcoValue(FStateCode);
            FBaseTotalDemand = (WSim as WaterSimManager).TotalDemand;
//            FBasePop = WSim.ParamManager.Model_Parameter(eModelParam.epMagicPop).Value;
            FBasePop = WSim.ParamManager.Model_Parameter(eModelParam.epPopulation).Value;
            FBaseEcoDemand = Convert.ToInt32(FEcoGPCY * (double)FBasePop);
            FBaseGPCY = WSim.ParamManager.Model_Parameter(eModelParam.epTotalGPCY).Value;
            //FBaseRatio = FEcoGPCY / FBaseGPCY;
            if (0 < FBaseGPCY) { FBaseRatio = FEcoGPCY / FBaseGPCY; }
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before after each annual run. </summary>
        ///
        /// <param name="year"> The year just run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. Error should be placed in FErrorMessage. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PostProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the economic gpcd. </summary>
        ///
        /// <value> The economic gpcd. </value>
        ///-------------------------------------------------------------------------------------------------

        public double EconomicGPCY
        {
            get { return FEcoGPCY; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base ratio. </summary>
        ///
        /// <value> The base ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public double BaseRatio
        {
            get { return FBaseRatio; }
        }
    }
    //==================================================
    //
    //  ECONOMIC INIDCATOR
    // --========================================================
    public class WebIndicator_Economic : WebIndicator
    {
        EconomicIndicatorProcess FEIP = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="BaseEModelParam">  The base model parameter. </param>
        /// <param name="WSim">             The simulation. </param>
        /// <param name="iData">            The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public WebIndicator_Economic( WaterSimManager WSim, IndicatorDataClass iData)
        {

            FWsim = WSim;
//            FBaseParam = BaseEModelParam;
            FIData = iData;
            FEIP = new EconomicIndicatorProcess("Economic Indicator Tracking", WSim, FIData);
            FWsim.ProcessManager.AddProcess(FEIP);
            CreateModelParameters();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti eco demand. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_EcoDemand()
        {
//            int Pop = FWsim.ParamManager.Model_Parameter(eModelParam.epMagicPop).Value;
            int Pop = FWsim.ParamManager.Model_Parameter(eModelParam.epPopulation).Value;
            int EcoDemand = Convert.ToInt32(FEIP.EconomicGPCY * (double)Pop) / 1000000;
            return EcoDemand;
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti eco ratio. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int Geti_EcoRatio()
        {
            double AnnualGPCY = FWsim.TotalGPCY;
            double EcoGPCY = FEIP.EconomicGPCY;
            double CurrentRatio = 0;
            if(0 < AnnualGPCY)CurrentRatio=EcoGPCY / AnnualGPCY;
            double StressRatio = 0;
            if(0 < CurrentRatio)StressRatio=FEIP.BaseRatio / CurrentRatio;
            StressRatio = StressRatio / 1.3;
            int TempInt = Convert.ToInt32(StressRatio*100);
            return TempInt;
        }

        /// <summary>   Creates the model parameters. </summary>
        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWsim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            FPM.AddParameter(new ModelParameterClass(eModelParam.epECODemand, "Demand for Economic Production", "ECOD",Geti_EcoDemand));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epECODemand, "The volume of water needed to meet local water needs for economic production to support the population", "mgd", "Million Gallans Per Day", "Economic Demand", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epECORatio, "Econmic Water Stress", "ECOR", Geti_EcoRatio));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epECODemand, "The ratio of Needed Gallons Per Capita Per Day to Annual Gallons Per Capita Per Day", "%", "Percent", "Economic Water Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
        }
    }
    #endregion

    //=========================================================================================
    //  
    //   Surface Water Indicator
    //    
    //=========================================================================================
    #region Surface Water

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Software indicator process. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class SWIndicatorProcess : AnnualFeedbackProcess
    {
        int FStateCode = 0;
        double FBaseRatio = 0;
        double FStreamFlow = 0;

        IndicatorDataClass FIData = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///  <remarks> It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		   pre and post process calls</remarks>
        ///-------------------------------------------------------------------------------------------------

        public SWIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            FIData = iData;
        }

        protected override void BuildDescStrings()
        {
            FProcessDescription = "";
            FProcessLongDescription = "Surface Water Tracker Process: Keeps track of initial base surface water indicator parameters";
            FProcessCode = "SWP";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base ratio. </summary>
        ///
        /// <value> The base ratio. </value>
        ///-------------------------------------------------------------------------------------------------

        public double BaseRatio
        {
            get { return FBaseRatio; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the stream flow. </summary>
        ///
        /// <value> The stream flow. </value>
        ///-------------------------------------------------------------------------------------------------

        public double StreamFlow
        {
            get { return FStreamFlow; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the started. </summary>
        ///
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            // get the state code for this run
            int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            FStateCode = statecode;

            FBaseRatio = FIData.SurfaceRatio(FStateCode);
            FStreamFlow = FIData.SurfaceFlows(FStateCode);
            
            return true;
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Surface water indicator. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterIndicator: WebIndicator
    {
        SWIndicatorProcess SWIP = null;
        public SurfaceWaterIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            FWsim = WSim;
            FIData = iData;
            SWIP = new SWIndicatorProcess("Surface Water Indicator Tracking", WSim, FIData);
            FWsim.ProcessManager.AddProcess(SWIP);
            CreateModelParameters();
        }

        int FMaxRange = 6;

        public int geti_SurfaceChangeIndicator()
        {
            int result = 0;
            // get current surfacewater withdrawal for urban
            double UrbanSW = FWsim.ParamManager.Model_Parameter(eModelParam.epUrbanSurfacewater).Value;
            // get what we are using for stream flow
            double BaseFlow = SWIP.StreamFlow;
            //// get what we are using for the intial ration of surfacewithdrawal and strema flow
            //double BaseRatio = SWIP.BaseRatio;
            //// calcualte the current ratio
            //double CurrentRatio = (UrbanSW / BaseFlow)*100;
            //// indicator is how mcuh indicator has changed. ie current ratio divided by baseratio, >100 not good < 100 good
            ////double ChangeIndicator = (BaseRatio / CurrentRatio)*100;

            //double ChangeIndicator = (Math.Log10((BaseRatio / CurrentRatio) * 1000)/FMaxRange)+100;
            double ChangeIndicator = (1-(Math.Log10((UrbanSW / BaseFlow) * 1000000)/FMaxRange))*100;
            //
            try
            {
                result = Convert.ToInt32(ChangeIndicator);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWsim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epSWIndicator, "Surfacewater Indicator", "SWI", geti_SurfaceChangeIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSWIndicator, "Change in impact on Surface Water", "?", "?", "Surfacewater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    #endregion

    //=========================================================================================
    //  
    //   Agriculture Indicator
    //    
    //=========================================================================================

    #region Agriculture Indicator

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ag indicator process. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class AGIndicatorProcess : AnnualFeedbackProcess
    {
        int FStateCode = 0;
        IndicatorDataClass FIData = null;

        double FBaseAgDemand = 0;
        double FAGRate = 0.0;
        double FBaseAGNet = 0.0;
        double FBaseAGGallonPerDollar = 0.0;
        double FBasePercent = 0.0;
        public const double AG_National_MAX_GPDD = 20.0;
        public const double AG_National_Shift = 1.2;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor </summary>
        ///
        /// <param name="aName"> The name of the process </param>
        /// <param name="WSim"> The WaterSimManager Using the Process. </param>
        ///  <remarks> It should not be assumed that the WaterSimManager value being passed is the WaterSimManager that will make'
        /// 		   pre and post process calls</remarks>
        ///-------------------------------------------------------------------------------------------------

        public AGIndicatorProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            FIData = iData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "Gallons per Agriculture $ Tracker";
            FProcessLongDescription = "Gallons Per  Agriculture DollarTracker Process: This process keeps track of the annual changes in the annual Gallons per Agricultual Dollar of production";
            FProcessCode = "AGPD";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the started. </summary>
        ///
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            // get the state code for this run
            int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            FStateCode = statecode;
            // Get the indicator data
            // Growth Rate
            FAGRate = FIData.AGRate(FStateCode);
            // Net Farm Income
            FBaseAGNet = FIData.AGNet(FStateCode);
            // Get initial AG Water Demand
            FBaseAgDemand = WSim.ParamManager.Model_Parameter(eModelParam.epAgriculture).Value;
            // Calcualte Base $ per Gallon
            FBaseAGGallonPerDollar = FBaseAgDemand / FBaseAGNet;

            FBasePercent = (FBaseAGGallonPerDollar / AG_National_MAX_GPDD)*AG_National_Shift;

            // WEST QUAY EDIT 1/31/17 BEGIN
            (WSim as WaterSimManager).WaterSimModel.AgricultureGrowthRate = FAGRate;
            (WSim as WaterSimManager).WaterSimModel.AgricultureInitialGPDD = FBaseAGGallonPerDollar;
            (WSim as WaterSimManager).WaterSimModel.AgricultureNet = FBaseAGNet;

            
            //(WSim as WaterSimManager_SIO).WaterSimAmericaModel.AgricultureGrowthRate = FAGRate;
            //(WSim as WaterSimManager_SIO).WaterSimAmericaModel.AgricultureInitialGPDD = FBaseAGGallonPerDollar;
            //(WSim as WaterSimManager_SIO).WaterSimAmericaModel.AgricultureNet = FBaseAGNet ;
            // WEST QUAY EDIT 1/31/17 END
            
            return true;
        }

        int FLowerValue = 0;
        int FUpperValue = 0;
        internal void buildIndicatorArray()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Method that is called before each annual run. </summary>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. Error should be placed in FErrorMessage. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.PreProcess(int,WaterSimManagerClass)"/>
        ///-------------------------------------------------------------------------------------------------

        public override bool PreProcess(int year, WaterSimManagerClass WSim)
        {
           //int newDemand = FWsim.geti_AgDemand();
           //FWsim.seti_NetworkAgricultureDemand(newDemand);

            return true;
        }
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the rate of growth. </summary>
        ///
        /// <value> The rate of growth. </value>
        ///-------------------------------------------------------------------------------------------------

        public double RateOfGrowth
        {
            get { return FAGRate; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base net. </summary>
        ///
        /// <value> The base net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double BaseNet
        {
            get { return FBaseAGNet; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the base gallon per dollar. </summary>
        ///
        /// <value> The base gallon per dollar. </value>
        ///-------------------------------------------------------------------------------------------------

        public double BaseGallonPerDollar
        {
            get { return FBaseAGGallonPerDollar; }
        }

        public double BasePercent
        {
            get { return FBasePercent; }
        }

        public double National_Max_GPDD
        {
            get { return AG_National_MAX_GPDD; }
        }


    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Surface water indicator. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.WebIndicator"/>
    ///-------------------------------------------------------------------------------------------------

    public class AgricultureIndicator : WebIndicator
    {
        AGIndicatorProcess AGIP = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="WSim">     The simulation. </param>
        /// <param name="iData">    The data. </param>
        ///-------------------------------------------------------------------------------------------------

        public AgricultureIndicator(WaterSimManager WSim, IndicatorDataClass iData)
        {
            FWsim = WSim;
            FIData = iData;
            AGIP = new AGIndicatorProcess("Agriculture Water Indicator Tracking", WSim, FIData);
            FWsim.ProcessManager.AddProcess(AGIP);
            CreateModelParameters();
        }

        //double[] AgScale = new double[10] { 0.02, 0.04, 0.1, 0.5, 1, 5, 10, 20, 30, 45 };
        //double[] InitialAgIndicatorScale = new double[10] { 0.04, 0.10, 0.25, 0.50, 0.75, 1.00, 2.00, 5.00, 10.00, 30.00 };

        const double FAdjustFactor = 1.2;//  this is a 80% change with a 25% change 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag indicator. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AgIndicator()
        {
            int result = 0;

            double CurrentDemand = FWsim.ParamManager.Model_Parameter(eModelParam.epAgriculture).Value; //.epAgricultureDemand).Value;
            double CurremtProduction = FWsim.ParamManager.Model_Parameter(eModelParam.epAgricultureProduction).Value; ;
            double CurrentAGPD = CurrentDemand / CurremtProduction;
            double RealPercent = CurrentAGPD / AGIP.National_Max_GPDD;
            double AdjustedPercent = AGIP.BasePercent + ((RealPercent - AGIP.BasePercent)*FAdjustFactor);
            int temp = 0;
            try
            {
                //temp = 100 - Convert.ToInt32(AdjustedPercent*100);
                temp =  Convert.ToInt32(AdjustedPercent * 100);
                result = temp;
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti base a gpd 100. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_BaseAGPD100()
        {
            int result = 0;

            int temp = 0;
            double BaseAGPD100 = AGIP.BaseGallonPerDollar * 100;
            try
            {
                temp = Convert.ToInt32(BaseAGPD100);
                result = temp;
            }
            catch (Exception ex)
            {
                // ouch
            }

            return result;
        }

        /// <summary>   Creates the model parameters. </summary>
        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWsim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            FPM.AddParameter(new ModelParameterClass(eModelParam.epAgricultureBaseGPD100, "Ag Base GPD", "BAGPD100", geti_BaseAGPD100));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Initial 100 Gallons per Dollar", "GP$", "100 Gallons Per Dollar", "Initial GPD", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epAgricultureIndicator, "Ag Indicator", "AGIND", geti_AgIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Efficiency of Ag Water Use", "?", "?", "Ag Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

//            FPM.AddParameter(new ModelParameterClass(eModelParam.epSWIndicator, "Surfacewater Indicator", "SWI", geti_SurfaceChangeIndicator));
  //          ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSWIndicator, "Change in impact on Surface Water", "?", "?", "Surfacewater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }
    #endregion

    //==========================================================================================
    //  
    //  Urban Efficienct Indicator
    //   
    //==========================================================================================

    #region Urban Efficiency Indicator

    /// <summary>   Urban efficiency indicator. </summary>
    public class UrbanEfficiencyIndicator
    {
        IndicatorDataClass FData = null;
        WaterSimManager FWSim = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="TheData">  Access to the Idicator Data. </param>
        ///-------------------------------------------------------------------------------------------------

        public UrbanEfficiencyIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
        {
            FData = TheData;
            FWSim = TheWSManager;
            CreateModelParameters();
        }

        double Under100Factor = 3.3;
        double Over100Divisor = 41;
        double Over100Beta = 99;
        double SweetSpotLow = 83;
        double SweetSpotHigh = 115;
        double SweetSpotIndicator = 38;
        double SweetSpotAdjust = 65;

        public int geti_UrbanEfficiencyIndicator()
        {
            int result = 0;
            double UrbanDemand = FWSim.ParamManager.Model_Parameter(eModelParam.epUrban).Value;
            double POP = FWSim.ParamManager.Model_Parameter(eModelParam.epPopulation).Value;
            double CALCGPCD = (UrbanDemand * 1000000) / POP;
            double CurrentGPCD = FWSim.ParamManager.Model_Parameter(eModelParam.epGPCD_urban).Value;
            // Green spot of inmdicator is centerd on a value of 38 with 0 the short red end and 100 the long red end
            // the sweet spot (center of green) is intended to be 95 to 105;
            double IndicatorGPCD = SweetSpotIndicator;
            if (CALCGPCD < SweetSpotLow)
            {
                // if under 95 then declines faster as number gets smaller
                IndicatorGPCD = 100 / ((100 - CALCGPCD) / Under100Factor);
            }
            else
            {
                if (CALCGPCD > SweetSpotHigh)
                {
                    // if over 105 then increases slower as number gets bigger
                    IndicatorGPCD = Math.Log10(CALCGPCD - Over100Beta) * Over100Divisor;

                }
                else
                {
                    IndicatorGPCD = CALCGPCD - SweetSpotAdjust;
                }
            }
            int tempint = 0;
            try
            {
                tempint = Convert.ToInt32(IndicatorGPCD);
                result = tempint;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epUrbanEfficiencyIndicator, "Urban Water Efficiency Stress", "UEF", geti_UrbanEfficiencyIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Urban Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Urban Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }

   
    #endregion Urban Efficiency

    //==========================================================================================
    //  
    //  Environment Indicator
    //   
    //==========================================================================================

    #region Environment Indicator

    public class EnvironmentIndicator
    {
        IndicatorDataClass FData = null;
        WaterSimManager FWSim = null;
        EnvironmentProcess FEP = null;
        double FBaseDilution = 0.0;
        public EnvironmentIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
        {
            FWSim = TheWSManager;
            FData = TheData;
            FEP = new EnvironmentProcess("Wastewater Tracker", FWSim, FData);
            FWSim.ProcessManager.AddProcess(FEP);
            CreateModelParameters();
        }

        const double DilutionRange = 1900; // Values should range from 0 to 1900


        public int geti_Environment()
        {
            int result = 0;
            //// get estimate of wastewater flow
            double TotWWFlow = FWSim.Wastewater();
            // adjust this
            
            // get UrbanSurfacewater
            double USurf = FWSim.ParamManager.Model_Parameter(eModelParam.epUrbanSurfacewater).Value;
            // OK Adjust the, if Reclaimed is less than ocaen discharge, adjust difference, if Reclaimed is larger use difference
            double Reclaimed = FWSim.ParamManager.Model_Parameter(eModelParam.epEffluent).Value;
            double OceanLakeDischarge = FEP.WastewaterOceanLakeDischarge;
            double Adjustment = 0.0;
            if (Reclaimed > OceanLakeDischarge)
            {
                Adjustment = Reclaimed - OceanLakeDischarge;
            }
            else
            {
                Adjustment = OceanLakeDischarge - Reclaimed;
            }
            // Estimate Discharged
            double Discharged =TotWWFlow - Adjustment;

            if (Discharged < 0)
            {
                Discharged = 0;
            }
            ////// calculate the Dilution Factor
            //double CurrentDilutionFactor = FEP.CalcDilutionFactor(TotWWFlow, Reclaimed);
            
            //double Change = FEP.BaseDilutionFactor / CurrentDilutionFactor;
//            double NewIndicatorScore = FEP.BaseIndicator / Change;
 

//            double NewIndicatorScore = FEP.CurrentIndicatorScore;
            double NewIndicatorScore = FEP.CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(FEP.WastewaterSurfaceFlow, Discharged, USurf);
           // NewIndicatorScore
            try
            {
                // now estmate its range compared to all others Range 0 to 4;
                int TempInt = Convert.ToInt32(NewIndicatorScore);
                result = TempInt;
            }
            catch (Exception ex)
            {
                // ouch
            }
//            double Pop = FWSim.ParamManager.Model_Parameter(eModelParam.epPopulation).Value;
//            double EperP = EFfluent / Pop;
            return result;
        }
 
        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epENVIndicator, "Environment Indicators", "ENVIND", geti_Environment));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epENVIndicator, "Impact of Influent Reductions on Changes in Surface Water Flows", "0-100", "", "Environment Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Environment process. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess"/>
    ///-------------------------------------------------------------------------------------------------

    public class EnvironmentProcess : AnnualFeedbackProcess
    {
        IndicatorDataClass FIData = null;
        double FWWFlow = 0.0;
        double FWWAdjust = 0.0;
        double FWWeight = 0.0;
        double FResetFactor = 0.0;
        double FInitialDilutionFDactor = 0.0;
        double FInitialIndicator = 0.0;
        bool FFactorsReady = false;
        double FCurrentIndicatorScore = 0.0;
        public EnvironmentProcess(string aName, WaterSimManager WSim, IndicatorDataClass iData)
        {
            BuildDescStrings();
            Fname = aName;
            this.Name = this.GetType().Name;
            FWsim = WSim;
            FIData = iData;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the description strings. </summary>
        ///
        /// <seealso cref="WaterSimDCDC.AnnualFeedbackProcess.BuildDescStrings()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void BuildDescStrings()
        {
            FProcessDescription = "Effluent Tracker";
            FProcessLongDescription = "Tracks gallons of effluent discharged";
            FProcessCode = "EFD";
        }

        double fMaxLogValue = 4;

        public double CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(double SurfaceFlow, double WWDischarge, double UrbanSurfWithdrawal)
        {
            // Ratio = Totalsurface flow divide by surface Water withdrawn
            // TotalSurface flow is (UrbanRiverFlow + UrbanWastewaterDischarge)
            // UrbanWastewaterDischarged is weight based on INdicator data from 1 to 100;  
            // Indicator is Log10 of Ration Divide by MaxLogValue from % states
            double result = ((Math.Log10((SurfaceFlow + (WWDischarge*FWWeight)) / UrbanSurfWithdrawal) / fMaxLogValue) * 100)-FResetFactor;
            return result;
        }

        public double CalcDilutionFactor(double WWFlow, double Reclaimed)
        {
            double result = 0;
            if (FFactorsReady)
            {
                // to estimate how much of this is discharged 
                // adjust this for ocean lake outfalls and amount being reclaimed

                double OceanLake = FWWAdjust;
                double Adjustment = 0;

                if (Reclaimed > OceanLake)
                {
                    Adjustment = Reclaimed;
                }
                else
                {
                    Adjustment = OceanLake - Reclaimed;
                }
                // Estimate Discharged
                double Discharged = WWFlow - Adjustment;

                // calculate the Dilution Factor
                double DilutionFactor = (Discharged + FWWFlow) /  Discharged;
                result = DilutionFactor;
            }
            return result;

        }

        const double FDilutionRange = 4000;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Process the started. </summary>
        ///
        /// <remarks>   Mcquay, 2/25/2016. </remarks>
        ///
        /// <param name="year"> The year about to be run. </param>
        /// <param name="WSim"> The WaterSimManager that is making call. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool ProcessStarted(int year, WaterSimManagerClass WSim)
        {
            // zero out cumulatives
            // get the state code for this run
            int statecode = WSim.ParamManager.Model_Parameter(eModelParam.epState).Value;
            // Get the indicator data

            FWWFlow = FIData.WastewaterSurfaceFlow(statecode);
            FWWAdjust = FIData.WastewaterAdjustment(statecode);
            FInitialIndicator = FIData.BaseEnvirionmentIndicator(statecode);
            FWWeight = FIData.WastewaterWeight(statecode);
            // initialize the reset factor 
            FResetFactor = 0.0;
            FFactorsReady = true;

            double TotWWFlow = (WSim as WaterSimManager).Wastewater();
            double Reclaimed = WSim.ParamManager.Model_Parameter(eModelParam.epEffluent).Value;
            // to estimate how much of this is discharged 
            // adjust this for ocean lake outfalls and amount being reclaimed
            double OceanLake = FWWAdjust;
            double Adjustment = 0;

            if (Reclaimed > OceanLake)
            {
                Adjustment = Reclaimed;
            }
            else
            {
                Adjustment = OceanLake - Reclaimed;
            }
            // calculate the Dilution Factor
            double Discharged = TotWWFlow - Adjustment;
            double USurf = WSim.ParamManager.Model_Parameter(eModelParam.epUrbanSurfacewater).Value;
            double newIndicator = CalcSurfaceFlowToSurfaceWithdrawRatioIndicator(FWWFlow, Discharged, USurf);
            FResetFactor = newIndicator - FInitialIndicator; 
            return true;
        }

        public override bool PostProcess(int year, WaterSimManagerClass WSim)
        {
            // get estimate of wastewater flow
            double TotWWFlow = (WSim as WaterSimManager).Wastewater();
            double Reclaimed = WSim.ParamManager.Model_Parameter(eModelParam.epEffluent).Value;
            //// calculate the Dilution Factor
            double CurrentDilutionFactor = CalcDilutionFactor(TotWWFlow, Reclaimed);
            if (year == WSim.Simulation_Start_Year)
            {
                FInitialDilutionFDactor = CurrentDilutionFactor;
            }
            double Change =   CurrentDilutionFactor / BaseDilutionFactor;

            FCurrentIndicatorScore = BaseIndicator / Change;

            return true;
        }

        
        public double CurrentIndicatorScore
        {
            get { return FCurrentIndicatorScore; }
        }

        public double BaseDilutionFactor
        {
            get { return FInitialDilutionFDactor; }
        }

        public double BaseIndicator
        {
            get { return FInitialIndicator; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the wastewater surface flow. </summary>
        /// <remarks>   Total mean flow of Rivers that have wastewater effluent discharge</remarks>
        /// <value> The wastewater surface flow. (mgd)</value>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterSurfaceFlow
        {
            get { return FWWFlow; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the wastewater discharged to lakes and oceansadjust. </summary>
        /// <remarks>   Some wastewater effluent is discharged to Lakes and Ocean, this is the adjustment for that</remarks>
        /// <value> The wastewater adjust. (mgd) </value>
        ///-------------------------------------------------------------------------------------------------

        public double WastewaterOceanLakeDischarge
        {
            get { return FWWAdjust; }
        }

    }

    #endregion Environment Indicator

    //==========================================================================================
    //  
    //  Power Indicator
    //   
    //==========================================================================================

    #region Power Efficiency Indicator

    /// <summary>   Urban efficiency indicator. </summary>
    public class PowerEfficiencyIndicator
    {
        IndicatorDataClass FData = null;
        WaterSimManager FWSim = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="TheData">  Access to the Idicator Data. </param>
        ///-------------------------------------------------------------------------------------------------

        public PowerEfficiencyIndicator(WaterSimManager TheWSManager, IndicatorDataClass TheData)
        {
            FData = TheData;
            FWSim = TheWSManager;
            CreateModelParameters();
        }

        double Under100Factor = 3.3;
        double Over100Divisor = 41;
        double Over100Beta = 99;
        double SweetSpotLow = 15;
        double SweetSpotHigh = 45;

        double MaxGPMW = 250000;
        double SweetSpotIndicator = 25;
        double SweetSpotAdjust = 0;

        public int geti_PowerEfficiencyIndicator()
        {
            int result = 0;
            // get values
            double PowerDemand = FWSim.ParamManager.Model_Parameter(eModelParam.epPower).Value;
            double PowerGen = FWSim.ParamManager.Model_Parameter(eModelParam.epPowerEnergy).Value;
            //double PowerSaline = FWSim.ParamManager.Model_Parameter(eModelParam.epPowerSaline).Value;
            // OK estimate how much of PowerGen is Saline
            //double PowerGenSaline = PowerGen * (PowerSaline / PowerDemand);
            // OK just want to use NON Saline water for GPMW
            //double PowerGenNonSaline = PowerGen - PowerGenSaline;
            //double CALCGPMW = 0;
            //if (PowerGenNonSaline > 0)
            //{
            //    CALCGPMW = (PowerDemand - PowerSaline) / PowerGenNonSaline;
            //}
            double CALCGPMW = 0.0;
            if (PowerGen > 0)
            {
                CALCGPMW = (((PowerDemand * 1000000) / PowerGen) / MaxGPMW) * 100;
            } // Green spot of inmdicator is centerd on a value of 38 with 0 the short red end and 100 the long red end
            // the sweet spot (center of green) is intended to be 95 to 105;
        //    double IndicatorGPCD = SweetSpotIndicator;
            double IndicatorGPCD = CALCGPMW;
            //if (CALCGPMW < SweetSpotLow)
            //{
            //    // if under 95 then declines faster as number gets smaller
            //    IndicatorGPCD = 100 / ((100 - CALCGPMW) / Under100Factor);
            //}
            //else
            //{
            //    if (CALCGPMW > SweetSpotHigh)
            //    {
            //        // if over 105 then increases slower as number gets bigger
            //        IndicatorGPCD = Math.Log10(CALCGPMW - Over100Beta) * Over100Divisor;

            //    }
            //    else
            //    {
            //        IndicatorGPCD = CALCGPMW - SweetSpotAdjust;
            //    }
            //}
            int tempint = 0;
            try
            {
                tempint = Convert.ToInt32(IndicatorGPCD);
                result = tempint;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public void CreateModelParameters()
        {
            ParameterManagerClass FPM = FWSim.ParamManager;
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;

            //FPM.AddParameter(new ModelParameterClass(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "PEF", geti_PowerEfficiencyIndicator));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Power Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

        }
    }


    #endregion Power Efficiency

    //---------------------------------------------------
    #endregion Indicators


    //#######################################################################
    //  
    //   WaterSimManager  Partial Classes
    //    
    //##########################################################################
    #region WaterSimManager Classes
    /// <summary>   Model parameter. </summary>
    public static partial class eModelParam
    {
        public const int epGWIndicatorBase = 200;
        public const int epGW_Annual_Indicator = 201;
        public const int epGW_Cumulative_Indicator = 202;
        public const int epGW_Cumulative_SafeYieldNet = 203;
        public const int epGW_SafeYield_Reduction_Goal = 204;
        public const int epGW_SafeYield = 205;

        public const int epECOIndicatorBase = 210;
        public const int epECODemand = 211;
        public const int epECORatio = 212;

        public const int epSWIndicatorBase = 220;
        public const int epSWIndicator = 221;

        public const int epENVIndicatorBase = 230;
        public const int epENVIndicator = 231;

        public const int epUEFIndicatorBase = 240;
        public const int epUrbanEfficiencyIndicator = 241;

        public const int epAEFIndicatorBase = 250;
        public const int epAgricutureGallonsPerDollar = 251;

        public const int epAgricultureBaseGPD100 = 254;
        public const int epAgricutureRateAdjust = 255;
        public const int epAgricultureIndicator = 257;

        public const int epPEFIndicatorBase = 260;
        public const int epPowerEfficiency = 261;
     

        public const int epTotalDemand = 291;
        public const int epTotalDemandNet = 292;
        public const int epTotalGPCY = 293;
        //public const int epMagicPop = 294;
        //public const int epUrbanSurfacewater = 295;
        //public const int epSurfaceLake = 296;
        //public const int epPowerSurfacewater = 297;
        //public const int epPowerSaline = 298;
        //public const int epPowerGW = 299;


    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for water simulations. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public partial class WaterSimManager : WaterSimManagerClass
    {
        protected IndicatorDataClass IndicatorData;
        protected WebIndicator_GroundWater GWIndicator;
        protected WebIndicator_Economic ECOINdicator;
        protected SurfaceWaterIndicator SURIndicator;
        protected AgricultureIndicator AGIndicator;
        protected UrbanEfficiencyIndicator UEFIndicator;
        protected EnvironmentIndicator ENVIndicator;
        protected PowerEfficiencyIndicator POWIndicator;
        //---------------------------------------------------
        // Total Demand
        // ------------------------------------------------
        protected int geti_TotalDemand()
        {
 //           int TempInt = Convert.ToInt32(Math.Round (WSmith.TheCRFNetwork.Urban.Demand + WSmith.TheCRFNetwork.Agriculture.Demand + WSmith.TheCRFNetwork.Rural.Demand + WSmith.TheCRFNetwork.Industrial.Demand + WSmith.TheCRFNetwork.Power.Demand));
            int TempInt = Convert.ToInt32(Math.Round(WSmith.TheCRFNetwork.Urban.Demand + WSmith.TheCRFNetwork.Agriculture.Demand + WSmith.TheCRFNetwork.Industrial.Demand + WSmith.TheCRFNetwork.Power.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti total demand net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        protected int geti_TotalDemandNet()
        {
            //int TempInt = Convert.ToInt32(Math.Round(WSmith.TheCRFNetwork.Urban.Net + WSmith.TheCRFNetwork.Agriculture.Net + WSmith.TheCRFNetwork.Rural.Net + WSmith.TheCRFNetwork.Industrial.Net + WSmith.TheCRFNetwork.Power.Net));
            int TempInt = Convert.ToInt32(Math.Round(WSmith.TheCRFNetwork.Urban.Net + WSmith.TheCRFNetwork.Agriculture.Net + WSmith.TheCRFNetwork.Industrial.Net + WSmith.TheCRFNetwork.Power.Net));
            return TempInt;
        }

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Gets the geti magic pop. </summary>
        ///// <remarks> this is replacing the current geti_pop which only works, after the annual run, not before</remarks>
        ///// <returns>   . </returns>
        /////-------------------------------------------------------------------------------------------------

                //protected int geti_MagicPop()
        //{
        //    int year = Sim_CurrentYear;
        //    int pop = WSmith.Get_PopYear(year);
        //    return pop;
        //}

        public double Wastewater()
        {
            return WSmith.MaxReclaimed();
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total demand. </summary>
        ///
        /// <value> The total number of demand. </value>
        ///-------------------------------------------------------------------------------------------------

        public int TotalDemand
        {
            get { return geti_TotalDemand(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti total gpcd. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        protected int geti_TotalGPCY()
        {
            double TheTotaldemand = (double)TotalDemand * 1000000.0;
            double POP = ParamManager.Model_Parameter(eModelParam.epPopulation).Value; //.epMagicPop).Value;
            // 07.19.16 DAS stop division by zero
            double TempGPCD = 0;
            if(0 < POP) TempGPCD=(TheTotaldemand / POP);
            int TempInt = Convert.ToInt32(TempGPCD);
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the total gpcd. </summary>
        ///
        /// <value> The total number of gpcd. </value>
        ///-------------------------------------------------------------------------------------------------

        public int TotalGPCY
        {
            get { return geti_TotalGPCY(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated. </summary>
        ///
        /// <remarks>   Mcquay, 3/15/2016. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="Res">  The resource. </param>
        /// <param name="Cons"> The cons. </param>
        ///
        /// <returns>   The flux allocated. </returns>
        ///-------------------------------------------------------------------------------------------------

        int getFluxAllocated(ConsumerResourceModelFramework.CRF_Resource Res, ConsumerResourceModelFramework.CRF_Consumer Cons)
        {
            int result = 0;
            ConsumerResourceModelFramework.CRF_Flux TheFlux = Res.ToFluxs.FindTarget(Cons.Name);
            if (TheFlux != null)
            {
                double value = TheFlux.Allocated();
                try
                {
                    int valueInt = Convert.ToInt32(value);
                    result = valueInt;
                }
                catch (Exception ex)
                {
                    // ouch
                    throw new Exception("Error Getting Flux " + Res.Name + " to " + Cons.Name + " with value " + value.ToString());
                }
            }
            return result; 
        }

        //int getFluxAllocated(string ResourceStr, string ConsStr)
        //{
        //    int result = 0;
        //    ConsumerResourceModelFramework.CRF_Flux TheFlux = Res.ToFluxs.FindTarget(Cons.Name);
        //    if (TheFlux != null)
        //    {
        //        double value = TheFlux.Allocated();
        //        try
        //        {
        //            int valueInt = Convert.ToInt32(value);
        //            result = valueInt;
        //        }
        //        catch (Exception ex)
        //        {
        //            // ouch
        //            throw new Exception("Error Getting Flux " + Res.Name + " to " + Cons.Name + " with value " + value.ToString());
        //        }
        //    }
        //    return result;
        //}


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti urban surface water. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------


        public int geti_UrbanSurfaceWater()
        {
            int result = 0;
            double UrbanSW = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Resource Res in CRFNetwork.Resources)
            {
                if (Res.Name == StateData.SurfaceWaterFieldname)
                {
                    ConsumerResourceModelFramework.CRF_Flux UrbanConFlux = Res.ToFluxs.FindTarget(StateData.UrbanWaterFieldname);
                    if (UrbanConFlux != null)
                    {
                        UrbanSW = UrbanConFlux.Allocated();
                    }
                }
            }
            try
            {
                result = Convert.ToInt32(UrbanSW);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti power saline. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PowerSaline()
        {
            int result = 0;
            double PowerSal = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Resource Res in CRFNetwork.Resources)
            {
                if (Res.Name == StateData.SalineWaterFieldname)
                {
                    ConsumerResourceModelFramework.CRF_Flux PowerConFlux = Res.ToFluxs.FindTarget(StateData.PowerWaterFieldname);
                    if (PowerConFlux != null)
                    {
                        PowerSal = PowerConFlux.Allocated();
                    }
                }
            }
            try
            {
                result = Convert.ToInt32(PowerSal);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti surface lake From CRF_Network. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SurfaceLake()
        {
            int result = 0;
            double temp = WSmith.TheCRFNetwork.SurfaceLake.Limit;
            try
            {
                int tempint = Convert.ToInt32(Math.Round(temp));
                result = tempint;
            }
            catch (Exception ex)
            {
                // ouch
            }
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti agriculture demand. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_NetWorkAgricultureDemand()
        {
            int result = 0;
            double AgDemand = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;
            foreach (ConsumerResourceModelFramework.CRF_Consumer Con in CRFNetwork.Consumers)
            {
                if (Con.Name == StateData.AgricultureFieldname)
                {
                            AgDemand = Con.Demand;
                            break;
                }
            }
            try
            {
                result = Convert.ToInt32(AgDemand);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }
        public int geti_Population()
        {
            int result = 0;
            double Pop2010 = 0;
            ConsumerResourceModelFramework.CRF_Network CRFNetwork = WSmith.TheCRFNetwork;

            //ConsumerResourceModelFramework.CRF_Flux Population = Res.ToFluxs.FindTarget(StateData.PopulationFieldname);
            //if (Population != null)
            //{
            //    Pop2010 = UrbanConFlux.Allocated();
            //}



            try
            {
                result = Convert.ToInt32(Pop2010);
            }
            catch (Exception ex)
            {
                // Ouch!!
            }
            return result;
        }
     

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dampen rate. </summary>
        ///
        /// <param name="rate">     The rate. </param>
        /// <param name="damper">   Gets the geti ag production. </param>
        /// <param name="period">   The period. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        double DampenRate(double rate, double damper, double period )
        {
            double NewRate = 0;
            NewRate = rate * Math.Pow(damper, -1 * period);
            return NewRate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag production. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        const double Damper = 1.01;


        // Agriculture Target Efficiency Gallons per 1000 Dollars
        int FAgTargetEfficieny = 100;

        //double[] FAgEfficiencyAdjustment;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag target efficiency. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        int geti_agTargetEfficiency()
        {
            int result = FAgTargetEfficieny; 
            
            return result;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti ag target efficiency. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        void seti_agTargetEfficiency(int value)
        {
            FAgTargetEfficieny = value;
            WaterSimModel.AgricultureTargetGPDD = value;
        }

        //public int AgDemand
        //{
        //    get { return 0; }
        //    set { }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the initial power generated. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int InitialPowerGenerated()
        {
            int result = 0;
            int StateCode = ParamManager.Model_Parameter(eModelParam.epState).Value;
            string Statename = FStateNames[StateCode];
            result = WSmith.TheCRFNetwork.InitialPowerGenerated(Statename);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <returns>   The flux allocated, 0 if not found. </returns>
        ///-------------------------------------------------------------------------------------------------

        //public int GetFluxAllocated(string ResField, string ConsField)
        //{
        //    int result = 0;
        //    CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField,ConsField);
        //    if (theFlux != null)
        //    {
        //        double value = theFlux.Allocated();
        //        try
        //        {
        //            int tempint = Convert.ToInt32(value);
        //            result = tempint;
        //        }
        //        catch (Exception ex)
        //        {
        //            // ouch
        //        }
        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <param name="aValue">       The value. </param>
        ///-------------------------------------------------------------------------------------------------

        //public void SetFluxAllocated(string ResField, string ConsField, double aValue)
        //{
        //    CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField, ConsField);
        //    if (theFlux != null)
        //    {
        //        theFlux.SetAllocation(aValue);
        //    }
        //}

        //public int geti_SUR_PD()
        //{
        //    int result = GetFluxAllocated("SUR", "PTOT");
        //    return result;
        //}
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the indicators. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool initializeIndicators()
        {
            bool result = true;
            try
            {
                Extended_Parameter_Documentation ExtendDoc = ParamManager.Extended;

                // New Parameters
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epTotalDemand, "Total Demand", "TD", geti_TotalDemand));
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epTotalDemandNet, "Total Demand (Net)", "TDN", geti_TotalDemandNet));
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epTotalGPCY, "Total GPCY", "TGPCY", geti_TotalGPCY));
//                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epMagicPop, "Magic Population", "MPOP", geti_MagicPop));
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epUrbanSurfacewater, "Urban Surfacewater", "USUR", geti_UrbanSurfaceWater));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epUrbanSurfacewater, "Urban Surfacewater: Amount of Surface water used to meet Urban Demand", "MGD", "Million Gallons Per Day", "Urban Surfacewater", new string[] { "No Change", "Moderate", "Extreme" }, new int[] { 100, 70, 30 }, new ModelParameterGroupClass[] { }));

                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epPowerSaline, "Power Saline", "PSAL", geti_PowerSaline));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epUrbanSurfacewater, "Power Saline: Amount of Saline water used to meet Power Demand", "MGD", "Million Gallons Per Day", "Urban Surfacewater", new string[] { "No Change", "Moderate", "Extreme" }, new int[] { 100, 70, 30 }, new ModelParameterGroupClass[] { }));

 //               ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricultureDemand, "Agriculture Demand", "ADP", geti_AgDemand));
//                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricultureProduction, "Agriculture Net $", "ANP", geti_AgProduction));

                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricutureRateAdjust, "Ag Efficiency", "AGCON", rangeChecktype.rctCheckRange, 10, 100, geti_agTargetEfficiency , seti_agTargetEfficiency, RangeCheck.NoSpecialBase));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricutureRateAdjust, "Adjust Ag Efficiency", "%", "Percent of Projected", "Adjust Ag Efficiemcy", new string[] { "No Change", "Moderate", "Extreme" }, new int[] { 100, 70, 30 }, new ModelParameterGroupClass[] { }));

                //ParamManager.AddParameter(new ModelParameterClass(eModelParam.epSurfaceLake, "Surface Lake Water", "SURL", geti_SurfaceLake));
                //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceLake, "Surface Lake Water", "mgd", "Million Gallons Per Day", "Lake Water", new string[] {}, new int[] { }, new ModelParameterGroupClass[] { }));
                
                // Load the data
                try
                {
                  //  IndicatorData = new IndicatorDataClass(DataDirectory, "IndicatorData.csv");
                    IndicatorData = new IndicatorDataClass(DataDirectory, "ElevenStateIndicatorData.csv");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                // I changed the use of this control switch to allow the model to run two years - 2015 and 2016 - before a policy in implemented
                // so that the default values from just11StatesNoPower show up in the outputs (I want to see the effect take place)
                // 12.19.16
                //WSmith.invokePolicies = true;

                // create the base indicator classes and create the Indicator parameters 

                // GroundWater Indicator
                GWIndicator = new WebIndicator_GroundWater(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epGW_Annual_Indicator, "Annual Safe Yield Indicatior", "GWSYA", GWIndicator.geti_AnnualIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGW_Annual_Indicator, "The ratio of the amount of reduction in 2010 groundwater withdrawal needed to achieve Safe Yield to the annual ground water withdrawal, 0 means safe yield achived, 100 is max value", "%", "Percent", "Groundwater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Economic Indicator
                ECOINdicator = new WebIndicator_Economic(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epECORatio, "Econmic Water Stress", "ECOR", ECOINdicator.Geti_EcoRatio));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epECODemand, "The ratio of Needed Gallons Per Capita Per Day to Annual Gallons Per Capita Per Day", "%", "Percent", "Economic Water Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Urban Surface Water Indicator
                SURIndicator = new SurfaceWaterIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epSWIndicator, "Surfacewater Indicator", "SWI", SURIndicator.geti_SurfaceChangeIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSWIndicator, "Change in impact on Surface Water", "?", "?", "Surfacewater Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Agricutural Indicator
                AGIndicator = new AgricultureIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epAgricultureIndicator, "Ag Indicator", "AGIND", AGIndicator.geti_AgIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Efficiency of Ag Water Use", "?", "?", "Ag Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Urban Efficiency Indicator
                UEFIndicator = new UrbanEfficiencyIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epUrbanEfficiencyIndicator, "Urban Water Efficiency Stress", "UEF", UEFIndicator.geti_UrbanEfficiencyIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureBaseGPD100, "Urban Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Urban Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Environment Indicator
                ENVIndicator = new EnvironmentIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epENVIndicator, "Environment Indicators", "ENVIND", ENVIndicator.geti_Environment));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epENVIndicator, "Impact of Influent and Withdrawls on Changes in Total Surface Water Flows", "0-100", "", "Environment Indicator", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

                // Power Efficiency Indicator
                POWIndicator = new PowerEfficiencyIndicator(this, IndicatorData);
                ParamManager.AddParameter(new ModelParameterClass(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "PEF", POWIndicator.geti_PowerEfficiencyIndicator));
                ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPowerEfficiency, "Power Water Efficiency Stress", "0-100", "Indicator Stress 0 - 100", "Power Efficiency Stress", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            }
            catch (Exception ex)
            {
                string Mesg = ex.Message;
                // ouch need error trapping
            }

           
            return result;
        }
    }
#endregion

}
