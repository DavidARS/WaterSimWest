using System;
using System.IO;
//using ReadWriteCsv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsumerResourceModelFramework;
using WaterSimDCDC.America;
using UniDB;
using System.Data;

namespace WaterSimDCDC
{
    public class WaterSimAmerica
    {
        CRF_Network_WaterSim_America WSA;
        //
        RateDataClass RDC;
        //  RateDataProcess RDP;
        internal StreamWriter sw;
        DateTime now = DateTime.Now;
        //
        // http://waterdata.usgs.gov/fl/nwis/wu
        // Units
        // Million gallons per day (Mgd)--a rate of flow of water equal to 133,680.56 cubic feet per day, 
        // or 1.5472 cubic feet per second, or 3.0689 acre-feet per day. 
        // A flow of one million gallons per day for one year equals 1,120 acre-feet (365 million gallons).
        //
        //
        //
        /// <summary>
        ///  Constructor for WaterSimAmerica
        /// </summary>
        #region Constructor
        static protected bool _isWaterForAmericaInstatiated = false;  // used to keep track if a WaterForAmerica object has been constructed
        //  CsvFileReader Reader;
        // QUAY EDIT 2/9/2016
        // string path = System.IO.Directory.GetCurrentDirectory();
        public WaterSimAmerica(string DataDirectoryName, string TempDirectoryName)
        {
            State = FStateName;
            StateData = "Just11StatesLakeNoRuralPower.csv";// "Just5StatesLakeNoRural.csv"; //"Just5StatesLake.csv";// "JustSmithStates.csv";// "All_50_states.csv";
            //  String IndustryData = "ElevenStateAnnualIndGrowthRates.csv";
            string rates = "ElevenStateGrowthRates.csv";
            try
            {
                //StreamW(DataDirectoryName);
                RDC = new RateDataClass(DataDirectoryName, rates);
                WSA = new CRF_Network_WaterSim_America(DataDirectoryName + "\\" + StateData, null, State);

                initialize_FirstRun();
                WaterAmerica = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        protected bool WaterAmerica
        {
            get { return _isWaterForAmericaInstatiated; }
            set { _isWaterForAmericaInstatiated = value; }
        }
        //public void StreamW(string TempDirectoryName)
        //{
        //    string filename = string.Concat(TempDirectoryName + "\\" + "Output" + "\\" + "Output" + "_" + now.Month.ToString() + "-" 
        //        + now.Day.ToString() + "_" +  now.Hour.ToString() + "." +  now.Minute.ToString() + now.Second.ToString()
        //        + ".csv");
        //    sw = File.AppendText(filename);
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dispose of this object, cleaning up any resources it uses. </summary>
        ///
        /// <param name="disposing">    true if resources should be disposed, false if not. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void Dispose(bool disposing)
        {
            _isWaterForAmericaInstatiated = false;
            if (disposing)
            {
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Performs application-defined tasks associated with freeing, releasing, or
        ///             resetting unmanaged resources. </summary>
        ///
        /// <seealso cref="System.IDisposable.Dispose()"/>
        ///-------------------------------------------------------------------------------------------------

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //
        //public CRFSmith.CRF_Network_WaterSim_America TheCRFNetwork
        //public CRF_Network_WaterSim_America TheCRFNetwork
        //{
        //    get { return WSA; }
        //}
        public CRF_Network_WaterSim_America TheCRFNetwork
        {
            get { return WSA; }
        }
        // Default State
        // ----------------
        internal string _state = "Florida";
        public string State
        {

            get { return _state; }
            set { _state = value; }
        }
        //
        string _stateData = "";
        internal string StateData
        {
            get { return _stateData; }
            set { _stateData = value; }
        }
        #endregion
        // --------------------------------------------
        // ====================================================================================
        // ====================================================================================
        /// <summary>
        ///  Called at the start of every initialization
        /// </summary>
        //internal void initialize_firstRun()
        //{
        //    demand_total = 0;
        //    GPCD = 0;
        //    initProportions();
        //    //
        //    OldPopulation = population;
        //    maxReclaimed = 25;
        //}
        internal void initialize_FirstRun()
        {
            //
            MODdemand mod = new MODdemand();
            myConsumers(mod);
            demand_total = 0;
            GPCD = 0;
            initProportions();
            //
            OldPopulation = population;
            maxReclaimed = 25;
            populationRatio = 1;
        }
        // ====================================================================================
        // New Access Fields
        /// <summary>
        /// State Indexing.This code grabs the string and the index for the State being examined.
        /// And, it resets the Network
        /// </summary>
        #region State Examined
        //public const int FNumberOfStates = 5;
        //string[] FStateNames = new string[FNumberOfStates] { "Florida", "Idaho", "Illinois", "Minnesota", "Wyoming" };

        public int FStateIndex = 0;
        public string FStateName = "Florida";
        public void seti_StateIndex(int value)
        {
            if ((value > -1) && (value < WaterSimManager.FNumberOfStates))
            {
                FStateIndex = value;
                FStateName = WaterSimManager.FStateNames[value];
                WSA.StateName = FStateName;
            }
        }
        public int geti_StateIndex()
        {
            return FStateIndex;
        }


        #endregion
        //
        public int FYearIndex = 0;
        public void seti_YearIndex(int value)
        {
            if ((value > -1) && (value < 51))
            {
                FYearIndex = value;
            }
        }
        public int geti_YearIndex()
        {
            return FYearIndex;
        }

        // -------------------------
        //
        #region Reset Network and variables
        public void ResetNetwork()
        {
            seti_StateIndex(FStateIndex);
            ResetVariables();
        }
        public void ResetVariables()
        {
            // 
            int temp = 100;
            int zero = 0;
            //
            seti_PowerConservation(temp);
            seti_AgConservation(temp);
            seti_UrbanConservation(temp);
            seti_PopGrowthRate(temp);
            //
            seti_SurfaceWaterControl(temp);
            seti_GroundwaterControl(temp);
            seti_Effluent(temp);
            seti_LakeWaterManagement(temp);
            seti_Desalinization(zero);
            //
            seti_ReclaimedWaterManagement(zero);
            //
            OldPopulation = 0;
            InitialPower(FStateName);
            seti_PowerEnergy(initialPower);
            invokeEffluent = true;
        }
        #endregion
        //
        // ====================================================================================
        // MODEL
        //
        //
        public int runOneYear(int year)
        {
            currentYear = year;
            startUp(year);
            Model(year);
            cleanUp(year);
            return 0;
        }
        internal void startUp(int year)
        {
            // 09.12.16
            CalculateNewPopulation(year, StartYear);
        }
        internal void cleanUp(int year)
        {
            if (year == endYear)
            {
                int temp = geti_NetDemandDifference();
                tearDown(year);
            }
        }
        internal void tearDown(int year)
        {
            //sw.Flush();
            // sw.Close();
        }
        // ===========================
        //
        #region Model kernel Calls
        //
        int baseyear = 2015;
        internal void Model(int year)
        {
            seti_YearIndex(year - baseyear);
            //
            currentYear = year;
            initializeRun();
            preProcess(year);
            annual_delta();
            initialize_run_network();
            analyze_results();
            postProces(year);
            //            
        }
        void initializeRun()
        {
            populationRatio = 0;
            demand_total = 0;
            initProportions();
        }
        internal void annual_delta()
        {
            surfaceFresh();
            surfaceSaline();
            surfaceLake();
            groundwater();
            effluent();
        }
        internal void initialize_run_network()
        {
            Urban();
            Agriculture();
            PowerWater();
            PowerEnergy();
            Industrial();
        }
        // 12.19.16 das following code
        // ---------------------------
        void preProcess(int yr)
        {
            // 12.19.16 added
            if(yr == policyStartYear)
            invokePolicies = true;

        }
        // ===========================
        void postProces(int yr)
        {
            int temp = 0;
            temp = population;
            if (yr == startYear) startPop = temp;
            OldPopulation = temp;
            reset_Drivers(yr);
            //invokeEffluent = false; // One year - 2015, where the value of the added effluent is set

        }
        internal void analyze_results()
        {
            int GPCDurban = urbanGPCD;
            int GPCDindustry = industrialGPCD;
            int GPCDag = agriculturalGPCD;
            int GPCDpower = powerGPCD;
        }
        internal void reset_Drivers(int yr)
        {
            if (yr == policyStartYear)
            {
                int one = 100;
                int zero = 0;
                seti_SurfaceWaterControl(one);
                seti_GroundwaterControl(one);
                seti_Effluent(one);
                seti_LakeWaterManagement(one);
                seti_Desalinization(zero);
            }
        }

        #endregion
        //
        // =============================================================================================
        //
        // ---------------------------------------------------------------------------------------------
        // Resources - State definitions and management actions on resources
        // ==================================================================
        #region Resources
        void surfaceFresh()
        {
            int one = 100;
            double temp = 0;
            int result = 0;
            temp = geti_SurfaceWaterFresh() * d_surfaceWaterControl;
            if (startDroughtYear <= currentYear)
                temp = geti_SurfaceWaterFresh() * d_surfaceWaterControl * d_drought;

            result = Convert.ToInt32(temp);
            seti_SurfaceWaterFresh(result);
            if (startDroughtYear <= currentYear) seti_DroughtControl(one);
        }
        void surfaceSaline()
        {
            double temp = 0;
            int result = 0;
            temp = geti_SurfaceWaterSaline() * _desalinization;
            result = Convert.ToInt32(temp);
            result = (int)temp;
            seti_SurfaceWaterSaline(result);
        }
        void surfaceLake()
        {
            double temp = 0;
            int result = 0;
            //temp = geti_SurfaceLake() * LWManagement;
            temp = geti_SurfaceLake() * d_lakeWaterManagement;
            result = Convert.ToInt32(temp);
            seti_SurfaceLake(result);
        }
        void groundwater()
        {
            double temp = 0;
            int result = 0;
            temp = geti_Groundwater() * d_groundwaterControl;
            result = (int)temp;
            seti_Groundwater(result);
        }
        void effluent()
        {
            double temp = 0;
            int result = 0;
            temp = geti_Effluent();
            result = (int)temp;
            seti_Effluent(result);
        }
        //
        #endregion
        // ---------------------------------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------------------------------
        // Consumers - State definitions and Management Actions on the consumers
        // =====================================================================
        //
        const double convertDemand = 1000000;

        #region Water Demand
        // Community Demand UrbanConservation
        #region Community Demand
        // -------------------------
        void Urban()
        {
            double temp = 0;
            int result = geti_Urban();
            temp = weightedGrowthPopulation(result) * modifyDemandCF();
            if (invokePolicies)
            {
                temp = 0;
                try
                {
                    temp = alterGrowthConservation(geti_Urban(), UrbanConservation) * modifyDemandCF();
                }
                catch (Exception ex)
                {
                    // Ouch
                }
            }
            result = Convert.ToInt32(temp);
            seti_Urban(result);
            i_demand_urban = result;
        }
        int urbanGPCD
        {
            get
            {
                double temp = WSA.Urban.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                try
                {
                    if (0 < pop) gpcd = temp / pop;
                    // comparison estimates
                }
                catch (Exception ex) { }
                return Convert.ToInt32(gpcd);
            }
        }
        #endregion
        // ----------------

        // AGRICULTURE PRODUCTION AND DEMAND
        // ----------------
        # region Agriculture Demand
        //  
        //  Added Quay 3/3/16
        //#########################################################

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dampen rate of growth. </summary>
        /// <remarks>   This function reduces the annual rate of growth based on the period from start 
        ///             of growth to current year.  The dampening of the rate increases as the period increases</remarks>
        /// <param name="rate">     The base annual rate. </param>
        /// <param name="damper">   A Factor for the strength of the dampening.  1 is no dampening, 1.0001 is slight dampening
        ///                         1.1 is really fast, anything larger is insane. </param>
        /// <param name="period">   The period. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        double DampenRate(double rate, double damper, double period)
        {
            double NewRate = 0;
            NewRate = rate * Math.Pow(damper, -1 * period);
            return NewRate;
        }


        // need some default values here
        double FAgNet = 0;
        double FAgRate = 0;
        double FAgBaseGPDD = 0;
        double FAgTargetGPDDReduction = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture net. </summary>
        /// <remarks> This is the Base net Form income for the Should not vary from year to year</remarks>
        /// <value> The agriculture net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgricultureNet
        {
            get { return FAgNet; }
            set { FAgNet = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture growth rate. </summary>
        /// <remarks>  This is the annual rate of growth projected for agricultural net farm inccome.
        ///            This is as a percent ie 2 = 2% or a rate of .02</remarks>
        /// <value> The agriculture growth rate (percent). </value>
        ///-------------------------------------------------------------------------------------------------
        // AGCON is the policy control for agriculture, scaled from 50 to 100 (100 is no change)
        // 03.04.2016 DAS
        //
        public double AgricultureGrowthRate
        {
            get { return FAgRate; }
            set { FAgRate = value; }
        }
        // 03.04.26 DAS
        // need a separate rate controller that uses this variable, but cannot alter AgricultureGrowthRate
        // directly. Can be initialized and used in the model
        //
        double _agGrowthRate = 0.5;
        public int geti_AgGrowthRate()
        {
            int TempInt = Convert.ToInt32(Math.Round(_agGrowthRate * 100));
            return TempInt;
        }
        public void seti_AgGrowthRate(int value)
        {
            _agGrowthRate = (Double)value / 100;
        }
        //
        double AgGrowthRate
        {
            get { return _agGrowthRate; }

        }
        // -------------------------------------------------------------------------

        double _agProductionRate = 1.0;
        public int geti_AgProductionRate()
        {
            int TempInt = Convert.ToInt32(Math.Round(_agProductionRate * 100));
            return TempInt;
        }
        public void seti_AgProductionRate(int value)
        {
            _agProductionRate = (Double)value / 100;
        }
        double AgProductionRate
        {
            get { return _agProductionRate; }

        }
        //
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture gpdd. </summary>
        /// <remarks>  This is the initial base GPDD and should not change over time.
        ///            
        ///            </remarks>
        /// <value> The agriculture Gallons per One $ of Net Farm Income per Day (gpdd). </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgricultureInitialGPDD
        {
            get { return FAgBaseGPDD; }

            set { FAgBaseGPDD = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture target reduction. </summary>
        /// <remarks>  This is the Target annual GPDD and should not change over time.
        ///            This is a Percent, 100 is 100% or no change in GPDD            </remarks>
        ///
        /// <value> The agriculture target Percent reduction in Gallons per One $ of Net Farm Income per Day (gpdd) </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgricultureTargetGPDD
        {
            get { return FAgTargetGPDDReduction; }
            set { FAgTargetGPDDReduction = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag production. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        const double Damper = 1.01;

        public int Calc_AgProduction(int theCurrentYear, int theStartYear)
        {
            int result = 0;
            // calculate term
            double period = (theCurrentYear - theStartYear) + 1;
            // get adjusted growth rate
            //// dampen the rate
            double rate = AgricultureGrowthRate;
            // calculate the new $ production
            double AgNet = AgricultureNet;

            int temp = 0;
            try
            {
                double CurrentRate = DampenRate(AgricultureGrowthRate / 100, Damper, period);
                double NewProduction = Math.Round(AgNet * Math.Pow(1 + (CurrentRate * AgProductionRate), period));
                temp = Convert.ToInt32(NewProduction);
                result = temp;
            }
            catch (Exception ex)
            {
                // Ouch
            }
            return result;
        }

        // holder for GPDD
        double FAdjAGPCD = 0;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti ag demand. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        double _retainAg = 0;
        double RetainAgDemand
        {
            get { return _retainAg; }
            set { _retainAg = value; }
        }

        public int Calc_AgDemand(int theCurrentYear, int theStartYear, double NewProduction, double modDemand)
        {
            int result = 0;
            double final = 0;
            double temp = 0;
            // get the Initial Gallons per Dollar (ie assume it does not change; should insert some code to change this
            double InitialAGPD = AgricultureInitialGPDD;
            // Calculate new demand based on new production and AGPD
            // Adjust AGPCD
            double period = (theCurrentYear - theStartYear) + 1;
            if (theCurrentYear <= theStartYear) { RetainAgDemand = (WSA.Agriculture.Demand); }
            try
            {
                double CurDamper = 1 + ((100.0 - (double)FAgTargetGPDDReduction) / 4000);
                FAdjAGPCD = DampenRate(InitialAGPD, CurDamper, period);
                // das
                double ttemp = WSA.Agriculture.Demand;

                double NewAgDemand = NewProduction * FAdjAGPCD;
                double CurrentRate = DampenRate(FAgRate / 100, Damper, period);
                double newAgproduction = Math.Round(ttemp * Math.Pow(1 + (CurrentRate * AgGrowthRate), period));
                // NOTE I see on 01.03.17 that AgGrowthRate has no current effect on Agriculture
                // DAS- and newAgproduction is NOT being called - AgProductionRate would need to be modified
                // to have any growth effects on Agriculture (at this time)
                //
                double Difference = NewAgDemand - RetainAgDemand;// ttemp;
                //if (modDemand < 1) modDemand = correctMod(modDemand);
                //double temp = ttemp + (Difference * Math.Min(1.0, Math.Max(minMod, modDemand)));
                temp = ttemp + (Difference * modDemand);
                RetainAgDemand = NewAgDemand;
                if (theCurrentYear <= theStartYear) temp = NewAgDemand;

            }
            catch (Exception ex) { }
            //
            final = (temp);
            result = Convert.ToInt32(final);

            //sw.WriteLine(currentYear
            //+ ","

            //+ result
            //);
            return result;
        }

        // holder for ag production
        int FAgProduction = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agricuture production. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AgricutureProduction()
        {
            return FAgProduction;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agriculture gpdd. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Agriculture_GPDD()
        {
            int result = 0;
            try
            {
                int tempi = Convert.ToInt32(FAdjAGPCD);
                result = tempi;
            }
            catch (Exception ex)
            {
                // ouch
            }
            return result;
        }

        /// <summary>   Estimate Agriculture demand </summary>
        void Agriculture()
        {
            int result = 0;
            double temp = 0;
            // get Ag production
            FAgProduction = Calc_AgProduction(currentYear, startYear);
            // now estmate demand based on production
            temp = Calc_AgDemand(currentYear, startYear, FAgProduction, AgConservation) * modifyDemandCF();
            result = (int)temp;
            // set the parameter for AgDemand
            seti_Agriculture(result);
            // Set the parameter for AgGPDD
            i_demand_ag = result;
            //
            //sw.WriteLine(currentYear
            //+ ","
            //+ result
            //);
        }


        int agriculturalGPCD
        {
            get
            {
                double temp = WSA.Agriculture.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                try
                {
                    if (0 < pop) gpcd = temp / pop;
                    // For comparison
                    int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Ag);
                }
                catch (Exception ex) { }
                return Convert.ToInt32(gpcd);
            }
        }
        #endregion
        // ----------------

        // ----------------
        // Power Water
        # region Power Demand
        void PowerWater()
        {
            double temp = 0;
            int result = 0;
            temp = weightedGrowthPopulation(geti_PowerWater()) * modifyDemandCF();
            //
            if (invokePolicies)
            {
                temp = 0;
                try
                {
                    temp = alterGrowthConservation(geti_PowerWater(), PowerConservation) * modifyDemandCF();
                }
                catch (Exception ex)
                {
                    // opps
                }
            }
            result = (int)temp;
            seti_PowerWater(result);
            i_demand_power = result;
        }
        int powerGPCD
        {
            get
            {
                double temp = WSA.Power.Demand * convertDemand;
                double pop = population;
                double gpcd = temp / pop;

                int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Power);
                return Convert.ToInt32(gpcd);
            }
        }
        // ----------------
        // Power Energy
        void PowerEnergy()
        {
            double temp = 0;
            int result = 0;
            temp = weightedGrowthPopulation(geti_PowerEnergy()) * modifyDemandCF();
            result = Convert.ToInt32(temp);
            seti_PowerEnergy(result);
        }
        #endregion
        // ----------------

        // ----------------
        // Industry
        #region Industry Demand

        // NEW CODE AS OS 09.06.16 das 
        double _indProduction;
        double IndProduction
        {
            get { return _indProduction; }
            set { _indProduction = value; }
        }
        //
        // need some default values here
        double FIndRate = 0;
        // double IndDamper = 1.2;
        const double DamperF = 0.9;
        public double IndustrialGrowthRate
        {
            get { return RDC.INDRate(FStateIndex); }
            set { FIndRate = value; }

        }
        internal double correctMod(double dataIn)
        {
            double temp = 0;
            double result = 0;
            const double slope = -0.267;
            const double intercept = 1.265;
            temp = slope * dataIn + intercept;
            result = temp * dataIn;
            return result;

        }
        const double minMod = 0.5;
        public int Calc_IndDemand(int theCurrentYear, int theStartYear, double modDemand)
        {
            int result = 0;
            int Industry2015 = 0;

            double final = 0;
            double IndustrialDamper = 1;
            double DampenTheRate = 1;
            double useValue = 1.2;
            double modValue = 0.53;
            double minTheRate = 1.15;
            double temp = 0;
            try
            {
                IndustrialDamper = Math.Max(IndustrialGrowthRate * modValue, useValue);
                DampenTheRate = Math.Min(IndustrialGrowthRate / 2, minTheRate);
                if (theCurrentYear <= theStartYear) { Industry2015 = geti_Industrial(); }

                double period = (theCurrentYear - theStartYear) + 1;
                double dampedRate = DampenRate(IndustrialGrowthRate / 100, IndustrialDamper, period);
                double ttemp = Convert.ToInt32(Math.Round(WSA.Industrial.Demand));
                // Generic code from here down
                double newDemand = Math.Round(ttemp * Math.Pow(1 + (dampedRate * DampenTheRate), period));
                double Difference = newDemand - ttemp;
                if (modDemand < 1) modDemand = correctMod(modDemand);
                temp = ttemp + (Difference * Math.Min(1.0, Math.Max(minMod, modDemand)));
                if (theCurrentYear <= theStartYear) temp = newDemand;
            }
            catch (Exception ex) { }
            final = (temp);
            result = Convert.ToInt32(final);
            //

            //sw.WriteLine(currentYear
            // + ","
            // + ttemp
            // + ","
            // + newDemand
            // + ","
            // + indDifference
            // + ","
            // + indDifference * modDemand
            // + ","
            // + result
            // );
            return result;
        }
        // End Of New Code 09.06.16

        void Industrial()
        {
            double temp = 0;
            int result = 0;
            double preInvoke = 1.0;
            temp = Calc_IndDemand(currentYear, startYear, preInvoke);
            result = (int)temp;
            if (invokePolicies)
            {
                temp = Calc_IndDemand(currentYear, startYear, IndustryConservation);
            }
            result = Convert.ToInt32(temp);
            seti_Industrial(result);
            i_demand_industry = result;
        }
        int industrialGPCD
        {
            get
            {
                double temp = WSA.Industrial.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                if (0 < pop) gpcd = temp / pop;
                // for comparison only
                int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Industry);
                return Convert.ToInt32(gpcd);
            }
        }
        #endregion
        // ----------------

        // ----------------
        #region GPCD and modify Demand
        internal int geti_gpcd()
        {
            return urbanGPCD;
        }
        internal int geti_gpcdAg()
        {
            return agriculturalGPCD;
        }
        internal int geti_gpcdOther()
        {
            return powerGPCD + industrialGPCD;
        }
        // -------------------------------------------------------------
        int ModifyDemand(double demand, double parm)
        {
            int result = Convert.ToInt32(demand);
            const double b = 0.98;
            if (0.5 <= parm)
            {
                double DifYear = (endYear - startYear);
                double temp = 0;
                double a = 1 / Math.Sqrt(parm);
                if (0 < DifYear)
                {
                    double touch = a * b * ((1 - parm) / DifYear);
                    temp = demand - (demand * touch);
                    result = Convert.ToInt32(Math.Round(temp));
                }
            }
            return result;
        }
        // -------------------------------------------

        // -------------------------------------------------------------
        int ModifyDemandIndustry(double demand, double parm)
        {
            int result = 0;




            return result;
        }
        // -------------------------------------------
        internal struct MODdemand
        {
            public string urban;
            public string power;
            public string agriculture;
            public string industry;
        }
        internal static void myConsumers(MODdemand m)
        {
            m.urban = "Urban";
            m.power = "Power";
            m.agriculture = "Ag";
            m.industry = "Industry";
        }

        int _startYear = 0;
        public int startYear
        {
            set { _startYear = value; }
            get { return _startYear; }
        }
        //
        int _Sim_CurrentYear = 0;
        public int currentYear
        {
            set { _Sim_CurrentYear = value; }
            get { return _Sim_CurrentYear; }
        }
        int _endYear = 0;
        public int endYear
        {
            set { _endYear = value; }
            get { return _endYear; }
        }
        int _policyStartYear = 2015;
        public int policyStartYear
        {
            set { _policyStartYear = value; }
            get { return _policyStartYear; }
        }
        //

        // =========================================
        // -------------------------------------------------------------
        double modifyDemandCF()
        {
            double result = 1;
            double cf = geti_DroughtControl();
            if (cf * 0.01 < 1)
            {
                result = utilities.hyperbola(cf);
            }
            return result;
        }
        #endregion
        // ----------------

        //
        // ------------------------
        // use lower case
        // derived
        // send to WaterSimith Manager
        public int i_demand_urban;
        public int i_demand_rural;
        public int i_demand_ag;
        public int i_demand_power;
        public int i_demand_industry;
        //
        #endregion
        // 
        //public int sustainability_surface_water;
        //public int sustainability_groundwater;
        //public int sustainability_economic;
        // -------------------------------------------------
        // proportions
        // -------------------------------------------------
        #region Proportional demand
        //
        double _proportion_waterdemand_urban;
        double _proportion_waterdemand_ag;
        double _proportion_waterdemand_industry;
        double _proportion_waterdemand_power;
        //
        // ======================================================================================
        // May want to use the parameter that Ray created...
        //  ParamManager.AddParameter(new ModelParameterClass(eModelParam.epTotalDemand, "Total Demand", "TD", geti_TotalDemand));
        double d_demand_total = 0;
        double demand_total
        {
            get
            {
                return d_demand_total;
            }
            set
            {
                double temp = value;
                temp = (geti_Urban() + geti_Agriculture() + geti_PowerWater() + geti_Industrial());
                d_demand_total = temp;
            }
        }

        int _gpcd = 0;
        const double MGDtogal = 1000000;
        public int GPCD
        {
            set
            {
                double temp = value;
                if (0 < population)
                    temp = (d_demand_total * MGDtogal) / Convert.ToDouble(population);
                _gpcd = Convert.ToInt32(temp);
            }
            get { return _gpcd; }
        }

        // ======================================================================================
        //
        void initProportions()
        {
            Proportion_Waterdemand_Urban = 0;
            //Proportion_Waterdemand_Rural = 0;
            Proportion_Waterdemand_Ag = 0;
            Proportion_Waterdemand_Power = 0;
            Proportion_Waterdemand_Industry = 0;
        }
        //
        internal double Proportion_Waterdemand_Urban
        {
            get { return _proportion_waterdemand_urban; }
            set
            {
                double temp = value;
                if (0 < d_demand_total)
                {
                    temp = Convert.ToDouble(geti_Urban()) / Convert.ToDouble(d_demand_total);
                    temp = utilities.RoundToSignificantDigits(temp, 3);
                }
                //value = temp;
                _proportion_waterdemand_urban = temp;
            }
        }
        internal double Proportion_Waterdemand_Ag
        {
            get { return _proportion_waterdemand_ag; }
            set
            {
                double temp = value;
                if (0 < d_demand_total)
                {
                    temp = Convert.ToDouble(geti_Agriculture()) / Convert.ToDouble(d_demand_total);
                    temp = utilities.RoundToSignificantDigits(temp, 3);
                }
                _proportion_waterdemand_ag = temp;
            }
        }
        internal double Proportion_Waterdemand_Industry
        {
            get { return _proportion_waterdemand_industry; }
            set
            {
                double temp = value;
                //temp = 1 - (Proportion_Waterdemand_Urban + Proportion_Waterdemand_Rural + Proportion_Waterdemand_Ag + Proportion_Waterdemand_Power);
                temp = 1 - (Proportion_Waterdemand_Urban + Proportion_Waterdemand_Ag + Proportion_Waterdemand_Power);
                _proportion_waterdemand_industry = temp;
            }
        }
        internal double Proportion_Waterdemand_Power
        {
            get { return _proportion_waterdemand_power; }
            set
            {
                double temp = value;
                if (0 < d_demand_total)
                {
                    temp = Convert.ToDouble(geti_PowerWater()) / Convert.ToDouble(d_demand_total);
                    temp = utilities.RoundToSignificantDigits(temp, 3);
                }
                _proportion_waterdemand_power = temp;
            }
        }
        #endregion
        // -------------------------------------------------
        // Directory Control
        // -------------------------------------------------
        //#region Website directory faking
        //private static string DataDirectoryName
        //{
        //    get
        //    {
        //        return @"App_Data\";
        //    }
        //}

        //private static string TempDirectoryName
        //{
        //    set
        //    {
        //        string dir = value;
        //        string.Concat(@"WaterSmith_Output\", dir);
        //    }
        //    get
        //    {
        //        // Make a common for testing
        //        return @"WaterSmith_Output\";
        //        // Make the temp directory name unique for each access to avoid client clashes
        //        //return +System.Guid.NewGuid().ToString() + @"\";
        //    }
        //}
        //private static void CreateDirectory(string directoryName)
        //{
        //    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
        //    if (!dir.Exists)
        //    {
        //        dir.Create();
        //    }
        //}
        //#endregion

        // -------------------------------------------------
        // Population
        // -------------------------------------------------
        #region Population
        // ------------


        // NEW CODE AS OF 09.08.16 das 
        //
        double PopAdj = 1.4;
        double PopDamper = 1.01;
        double FPopRate = 0;
        public double PopulationGrowthRate
        {
            get { return RDC.POPRate(FStateIndex); }
            set { FPopRate = value; }
        }

        public int population
        {
            get { return Convert.ToInt32(Math.Round(WSA.Population.CurrentState)); }
            set { WSA.Population.CurrentState = value; }
        }
        int _initialPopulation = 0;
        int Pop2015
        {
            set { _initialPopulation = value; }
            get { return _initialPopulation; }

        }
        double _holdPopulation = 0;
        double PopRunning
        {
            set { _holdPopulation = value; }
            get { return _holdPopulation; }

        }

        public int CalculateNewPopulation(int theCurrentYear, int theStartYear)
        {
            double final = 0;
            int result = 0;
            //
            if (theCurrentYear <= theStartYear) { Pop2015 = population; PopRunning = Pop2015; }
            double period = (theCurrentYear - theStartYear) + 1;
            try
            {
                double dampedRate = DampenRate((PopulationGrowthRate / 100), PopDamper, period);
                double newPop = Math.Round(Pop2015 * Math.Pow(1 + (dampedRate * PopAdj), period));
                double popDifference = newPop - PopRunning;
                PopRunning = newPop;
                double temp = population + (popDifference * AdjustPopulation);
                if (theCurrentYear <= theStartYear) temp = newPop;
                final = (temp);
            }
            catch (Exception ex) { }
            //
            result = Convert.ToInt32(final);
            population = result;
            return result;
        }
        int _popOld = 0;
        public int OldPopulation
        {
            get { return _popOld; }
            set { _popOld = value; }
        }

        //
        // End New CODE As Of 09.08.16 das
        double _populationRatio = 0;
        double _startPop = 0;
        double startPop
        {
            set { _startPop = value; }
            get { return _startPop; }
        }
        double weightedGrowthPopulation(int institution)
        {
            double temp = 0;
            double result = institution;
            try
            {
                temp = institution + Convert.ToInt32(Convert.ToDouble(institution) * populationRatio);
                result = temp;
            }
            catch (Exception ex) { }
            return result;
        }
        double alterGrowthConservation(double institution, double ModifyGrowth)
        {
            double temp = 0;
            double result = institution;
            try
            {
                 temp = institution + Convert.ToInt32(Convert.ToDouble(institution) * populationRatio * ModifyGrowth);
                result = temp;
            }
            catch (Exception ex) { }
            return result;
        }
        double populationRatio
        {
            set
            {
                double temp;
                temp = 0.0;
                if (0 < OldPopulation)
                {
                    double pop = population;
                    double old = OldPopulation;// _oldPop;
                    // if(startPop < old)
                    try
                    {
                        temp = ((pop - old) / old);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    temp = 0;
                }
                _populationRatio = temp;
            }
            get { return _populationRatio; }
        }
        public int Get_PopYear(int year)
        // END QUAY EDIT
        {
            int result = 0;
            result = population;
            return result;
        }
        #endregion
        // -------------------------------------------------
        // Gallon Per Capita Per Day
        // -------------------------------------------------
        #region _GPCD
        int _urban_gpcd = 0;
        public virtual int gpcd
        {
            get { return geti_gpcd(); }
            set { _urban_gpcd = value; }
        }
        const int StartYear = 2015;
        const int EndYear = 2050;

        const int RawGPCDDataInc = 5; //Years
        const int NumberGPCDYears = ((EndYear - StartYear) / 5) + 1;

        int[][] GPCDYearData = new int[WaterSimManager.FNumberOfStates][];

        internal int Get_GPCDYear(int year)
        {
            int TempGPCD = 0;
            if (year == 0) year = StartYear;
            int ModYear = year % RawGPCDDataInc; ;
            if (ModYear == 0)
            {
                int yearIndex = (year - StartYear) / RawGPCDDataInc;
                TempGPCD = GPCDYearData[FStateIndex][yearIndex];
            }
            else
            {
                int lowyearindex = ((year - StartYear) - ModYear) / RawGPCDDataInc;
                int hiyearindex = lowyearindex + 1;
                int lowgpcd = GPCDYearData[FStateIndex][lowyearindex];
                int higpcd = GPCDYearData[FStateIndex][hiyearindex];
                int GPCDChangeByYear = (higpcd - lowgpcd) / RawPopDataInc;
                TempGPCD = lowgpcd + (GPCDChangeByYear * ModYear);


            }
            return Convert.ToInt32(TempGPCD * d_urbanConservation);
        }
        const int RawPopDataInc = 5;
        // 02.09.16
        internal int geti_gpcdTotal()
        {
            return GPCD;
        }
        #endregion
        //
        // =================================================
        // Network Parameters

        // CRF_NETWORK PARAMETERS
        // =================================================
        // Population for 2010 adjusted to 2015
        //--------------------------------------------------
        #region Population
        // This is the method being called by The WaterSim parameter manager
        public int geti_NewPopulation()
        {
            int TempInt = 0;
            TempInt = Convert.ToInt32(Math.Round(WSA.Population.CurrentState));

            return TempInt;
        }
        public int seti_NewPopulation
        {
            set { WSA.Population.CurrentState = value; }
        }
        #endregion

        // Resources
        // -------------------------------------------------
        #region Resources
        //----------------------------------------------
        //  SUrface Water Fresh 
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti surface water fresh. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SurfaceWaterFresh()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.SurfaceFresh.Limit));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti surface water fresh. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SurfaceWaterFresh(int value)
        {
            WSA.SurfaceFresh.Limit = value;
        }
        //

        public int geti_SurfaceWaterFreshNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.SurfaceFresh.Net)) + 0);
            return TempInt;
        }


        // -------------------------------------------------------------------------------------------------
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti surface lake From CRF_Network. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SurfaceLake()
        {
            int result = 0;
            double temp = TheCRFNetwork.SurfaceLake.Limit;
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
        public void seti_SurfaceLake(int value)
        {
            WSA.SurfaceLake.Value = value;
        }
        public int geti_SurfaceLakeNet()
        {
            int result = 0;
            //double temp = WSmith.TheCRFNetwork.SurfaceLake.Limit;
            double temp = TheCRFNetwork.SurfaceLake.Net;
            double temp2 = TheCRFNetwork.SurfaceLake.Limit;
            //
            try
            {
                int tempint = Convert.ToInt32(Math.Abs(Math.Round(temp)) + 0);
                result = tempint;
            }
            catch (Exception ex)
            {
                // ouch
            }
            return result;
        }

        //----------------------------------------------
        // Surface Water Saline
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti surface water saline. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        // Changed to mimic Surface Lake on 03.10.16 DAS
        //
        public int geti_SurfaceWaterSaline()
        {
            int result = 0;
            double temp = WSA.SurfaceSaline.Limit;
            try
            {
                int tempint = Convert.ToInt32(Math.Round(temp));
                result = tempint;
            }
            catch (Exception ex)
            {
                // ouch
            }
            //return result;
            //int TempInt = Convert.ToInt32(Math.Round(WSA.SurfaceSaline.Limit));
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti surface water saline. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SurfaceWaterSaline(int value)
        {
            WSA.SurfaceSaline.Limit = value;
        }
        //
        public int geti_SurfaceWaterSalineNet()
        {

            int result = 0;
            double temp = WSA.SurfaceSaline.Net;
            double temp2 = WSA.SurfaceSaline.Limit;
            try
            {
                int TempInt = Convert.ToInt32(Math.Abs(Math.Round(temp)) + 0);
                result = TempInt;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        //----------------------------------------------
        // Groundwater
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti groundwater. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Groundwater()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Groundwater.Limit));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti groundwater. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Groundwater(int value)
        {
            WSA.Groundwater.Limit = value;
        }
        //
        public int geti_GroundwaterNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Groundwater.Net)) + 0);
            return TempInt;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti effluent. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        //----------------------------------------------
        // Effluent
        //-----------------------------------------------

        public int geti_Effluent()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Effluent.Limit));

            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti effluent. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Effluent(int value)
        {
            WSA.Effluent.Limit = value;
            if (invokePolicies)
            {
                maxReclaimed = MaxReclaimed();
                // "value" is the default amount for each state. Added is the user-defined
                // request, balanced by the reasonable amount as defined by indoor water use,
                // allowing for some lost due to leaks and efficiencies of production and use
                WSA.Effluent.Limit = Math.Min(maxReclaimed, (double)value + effluentToAdd);
                // Added here from elsewhere on 12.14.16 DAS
                // At present "staticEffluentAdd" is not used- 12.19.16
                staticEffluentAdd = effluentToAdd;
            }
        }
        public int geti_EffluentNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Effluent.Net)) + 0);
            return TempInt;
        }

        double _maxReclaimed = 0;
        const double maxReclaimedRatio = 0.95;
        //const double consumptive = 0.86; // leaks http://www3.epa.gov/watersense/pubs/indoor.html
        // Need more flexibility- it is a GAME, and we need a response for reclaimed (recycled) water
        const double consumptive = 0.97; // leaks 
        //const double indoor = 0.45;
        const double indoor = 0.65;
        double maxReclaimed
        {
            set { _maxReclaimed = value; }
            get { return _maxReclaimed; }
        }
        double staticEffluent = 0;
        double staticEffluentAdd
        {
            get
            {
                return staticEffluent;
            }
            set
            {
                staticEffluent = value;
            }
        }


        double effluentToAdd
        {
            get
            {
                double temp = 0;
                temp = (d_reclaimedWaterUse) * WSA.Urban.Demand;
                return Math.Min(maxReclaimed, temp);
            }
        }
        public double MaxReclaimed()
        {
            double temp = 0;
            temp = maxReclaimedRatio * consumptive * ((WSA.Urban.Demand) * indoor);
            return temp;
        }
        // ============================================
        // Gets ans Sets to the AReclaimed Water Management   
        //--------------------------------
        /// <summary>
        /// Seti ReclaimedWaterUse
        /// </summary>
        /// <returns>an int from zero to 100   . </returns>
        double d_reclaimedWaterUse = 0.00;
        public int geti_ReclaimedWaterManagement()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_reclaimedWaterUse * 100));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti ReclaimedWaterUse.</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_ReclaimedWaterManagement(int value)
        {
            d_reclaimedWaterUse = (Double)value / 100;
        }
        // ============================================
        public int geti_TotalSupplies()
        {
            int result = 0;
            double temp = TheCRFNetwork.SurfaceLake.Limit;
            temp += TheCRFNetwork.SurfaceFresh.Limit;
            temp += TheCRFNetwork.SurfaceSaline.Limit;
            temp += TheCRFNetwork.Groundwater.Limit;
            temp += TheCRFNetwork.Effluent.Limit;
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

        // ========================================================================================
        #endregion Resources
        //--------------------------------------------------
        // Consumers
        // -------------------------------------------------
        # region Consumers
        /// <summary>
        /// Total Demand
        /// </summary>
        /// <returns></returns>
        double getd_TotalDemand()
        {
            double Temp = WSA.Urban.Demand + WSA.Industrial.Demand + WSA.Agriculture.Demand + WSA.Power.Demand;
            return Temp;
        }
        double getd_TotalNet()
        {
            double Temp = WSA.Urban.Net + WSA.Industrial.Net + WSA.Agriculture.Net + WSA.Power.Net;
            return Temp;
        }
        public int geti_NetDemandDifference()
        {
            //int TotalSupplies = geti_TotalSupplies();
            int tempInt = 0;
            double temp = 0;
            //
            double demand = getd_TotalDemand();
            double net = getd_TotalNet();
            temp = Math.Min(100,Math.Max(0, (net / demand) * 100));
            return tempInt = Convert.ToInt32(Math.Round(temp));

        }
        // =================================================================================================

        //----------------------------------------------
        // Urban
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti urban. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Urban()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Urban.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti urban. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Urban(int value)
        {
            WSA.Urban.Demand = value;
            i_demand_urban = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti urban net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Urban_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Urban.Net)));
            return TempInt;

        }


        //----------------------------------------------
        // Agriculture
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti agriculture. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Agriculture()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Agriculture.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti agriculture. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Agriculture(int value)
        {
            WSA.Agriculture.Demand = value;
            i_demand_ag = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti agriculture net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Agriculture_Net()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Agriculture.Net)));
            return TempInt;

        }
        // -------------------------------------------------------------------------------------------------



        // =================================================================================================

        //----------------------------------------------
        // Industrial
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti industrial. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Industrial()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Industrial.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti insustrial. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Industrial(int value)
        {
            WSA.Industrial.Demand = value;
            i_demand_industry = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti industrial net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Industrial_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Industrial.Net)));
            return TempInt;

        }

        //----------------------------------------------
        // Power
        //-----------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti power. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PowerWater()
        {
            int TempInt = Convert.ToInt32(Math.Round(WSA.Power.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti power. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_PowerWater(int value)
        {
            WSA.Power.Demand = value;
            i_demand_power = value;
        }

        public int geti_PowerWater_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Power.Net)));
            return TempInt;

        }
        // ------------------------------------------------------------------
        /// <summary>
        /// The Power production
        /// </summary>
        /// <returns></returns>

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti power. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------
        //
        int _initialPower = 0;
        void InitialPower(string state)
        {
            int temp = WSA.InitialPowerGenerated(state);
            initialPower = temp;
        }
        internal int initialPower
        {
            get { return _initialPower; }
            set { _initialPower = value; }
        }
        int _powerEnergy = 0;
        public int geti_PowerEnergy()
        {
            int temp = _powerEnergy;
            return temp;
        }
        public void seti_PowerEnergy(int value)
        {
            _powerEnergy = value;
        }



        //----------------------------------------------------------------------------------------------------------------------------
        #endregion Consumers
        // -------------------------------------------------
        // Policies to Implement
        // -------------------------------------------------
        #region Policy Controls
        //
        int i_droughtYear = 2015;
        public int startDroughtYear
        {
            get
            {
                return i_droughtYear;
            }
            set
            {
                i_droughtYear = value;
            }
        }
        ///// <summary>

        // External Drivers 
        //--------------------------------------------------------------------------------------------
        // =================================================================
        // Population Growth Rate Adjustment
        //-----------------------------------------------

        ///-----------------------------------------------------------------
        /// <summary>   Gets the geti population growth rate. </summary>
        ///
        /// <returns>an int from zero to 150 </returns>
        ///-----------------------------------------------------------------
        double d_popGrowthRate = 1.00;
        public int geti_PopGrowthRate()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_popGrowthRate * 100));
            return TempInt;
        }

        ///----------------------------------------------------------------
        /// <summary>   Seti population growth rate. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///----------------------------------------------------------------
        //public void seti_PopGrowthRate(int value)
        //{
        //    d_popGrowthRate = (Double)value / 100;
        //}
        public void seti_PopGrowthRate(int value)
        {
            d_popGrowthRate = ((Double)value / 100);
        }
        // ========================================
        public double AdjustPopulation
        {
            get { return d_popGrowthRate; }
        }
        // ========================================
        // Drought Impacts on Rivers/Lakes
        //--------------------------------

        ///--------------------------------------------------------------
        /// <summary>   Gets the geti DroughtImpacts. </summary>
        ///
        /// <returns>an int from zero to 150   . </returns>
        ///--------------------------------------------------------------
        //double d_drought = 1.00;
        double d_droughtManagement = 0.00;
        public int geti_DroughtImpacts()
        {
            int TempInt = Convert.ToInt32(d_droughtManagement);
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti DroughtImpacts. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///------------------------------------------------------------------
        public void seti_DroughtImpacts(int value)
        {
            d_droughtManagement = value;
        }
        // =======================================================================
        //
        // User Policies - 
        // =======================================================================
        // Urban Water Conservation
        //--------------------------------

        ///--------------------------------------------------------------
        /// <summary>   Gets the geti UrbanConservation. </summary>
        ///
        /// <returns>an int from zero to 100   . </returns>
        ///--------------------------------------------------------------
        double d_urbanConservation = 1.00;
        //double return_urban = 100;
        public int geti_UrbanConservation()
        {
            int temp = Convert.ToInt32(Math.Round(d_urbanConservation * 100));
            return temp;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti UrbanConservation.</summary>
        ///
        /// <param name="value">    The value. </param>
        ///------------------------------------------------------------------
        public void seti_UrbanConservation(int value)
        {
            d_urbanConservation = (Double)value / 100;
        }
        public double UrbanConservation
        {
            get { return d_urbanConservation; }
        }
        //
        // Desalinaiton
        double _desalinization = 0.0;
        public int geti_Desalinization()
        {
            int TempInt = Convert.ToInt32(Math.Round(_desalinization * 100));
            return TempInt;
        }
        public void seti_Desalinization(int value)
        {
            _desalinization = (Double)value / 100;
        }
        //
        //public double Desal
        //{
        //    get
        //    {

        //        double temp = 1;

        //        if (invokePolicies)
        //        {
        //            temp = 1 + _desalinization;
        //        }
        //        return temp;

        //    }
        //}



        // =====================================================================
        // ============================================
        // Agricultural Water Conservation
        //--------------------------------

        ///--------------------------------------------------------------
        /// <summary>   Gets the geti AgConservation.</summary>
        ///
        /// <returns>an int from zero to 100   . </returns>
        ///--------------------------------------------------------------
        double d_agConservation = 1.00;
        public int geti_AgConservation()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_agConservation * 100));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti UrbanConservation.</summary>
        ///
        /// <param name="value">    The value. </param>
        ///------------------------------------------------------------------
        public void seti_AgConservation(int value)
        {
            d_agConservation = (Double)value / 100;
        }
        public double AgConservation
        {
            get { return d_agConservation; }
        }
        // =====================================================================
        // ============================================
        // Power (utilities) Water Conservation
        //--------------------------------

        ///--------------------------------------------------------------
        /// <summary>   Gets the geti AgConservation.</summary>
        ///
        /// <returns>an int from zero to 100   . </returns>
        ///--------------------------------------------------------------
        double d_powerConservation = 1.00;
        public int geti_PowerConservation()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_powerConservation * 100));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti UrbanConservation.</summary>
        ///
        /// <param name="value">    The value (0 to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_PowerConservation(int value)
        {
            d_powerConservation = (Double)value / 100;
        }
        public double PowerConservation
        {
            get { return d_powerConservation; }
        }
        // =====================================================================
        // ============================================
        double d_industryConservation = 1.00;
        public int geti_IndustryConservation()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_industryConservation * 100));
            return TempInt;
        }
        double IndustryConservation
        {
            get { return d_industryConservation; }
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti UrbanConservation.</summary>
        ///
        /// <param name="value">    The value (0 to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_IndustryConservation(int value)
        {
            d_industryConservation = (Double)value / 100;
        }

        // Groundwater Management 
        //--------------------------------
        /// <summary>
        /// Seti GroundwaterManagement
        /// </summary>
        /// <returns>an int from zero to 100   . </returns>
        //int d_groundwaterManagement = 1;
        //public int geti_GroundwaterManagement()
        //{
        //    int TempInt = d_groundwaterManagement;
        //    return TempInt;
        //}
        public int geti_GroundwaterControl()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_groundwaterControl*100));
            return TempInt;
        }

        ///------------------------------------------------------------------
        /// <summary>   Seti GroundwaterManagement.</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        //public void seti_GroundwaterManagement(int value)
        //{

        //    //CheckBaseValueRange(eModelParam.epGroundwaterManagement, value);
        //    d_groundwaterManagement = value;
        //}
        double d_groundwaterControl = 1.0;
        public void seti_GroundwaterControl(int value)
        {
            d_groundwaterControl = ((double)value) / 100;
        }
        //
        // =====================================================================
        // ============================================
        // SurfaceWater Management 
        //--------------------------------
        /// <summary>
        /// Seti SurfaceWaterManagement
        /// </summary>
        /// <returns>an int from zero to 100   . </returns>
        //int i_surfaceWaterManagement = 1;
        //public int geti_SurfaceWaterManagement()
        //{
        //    int TempInt = i_surfaceWaterManagement;
        //    return TempInt;
        //}
        public int geti_SurfaceWaterControl()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_surfaceWaterControl* 100));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti SurfaceWaterManagement. ONLY called 
        /// durring initialization</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        //public void seti_SurfaceWaterManagement(int value)
        //{
        //    i_surfaceWaterManagement = value;
        //}
        double d_surfaceWaterControl = 1.0;
        public void seti_SurfaceWaterControl(int value)
        {
            d_surfaceWaterControl = ((double)value) / 100;
        }
        // =====================================================================
        // ===================================
        // Lake Water Management
        double d_lakeWaterManagement = 0.00;
        public int geti_LakeWaterManagement()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_lakeWaterManagement * 100));
            return TempInt;
        }
        // ------------------------------------------------
        public void seti_LakeWaterManagement(int value)
        {
            d_lakeWaterManagement = (double)value / 100;

        }
        //public double LWManagement
        //{
        //    get
        //    {
        //        double temp = 1;

        //        if (invokePolicies)
        //      //  if(invokeLakeWaterManagement)
        //        {
        //            //temp = 1 + d_lakeWaterManagement;
        //            temp = d_lakeWaterManagement;
        //        }
        //        return temp;
        //    }
        //}
        bool _invokePolicies = false;
        public bool invokePolicies
        {
            set { _invokePolicies = value; }
            get { return _invokePolicies; }
        }
        bool _invokeEffluent = false;
        public bool invokeEffluent
        {
            set { _invokeEffluent = value; }
            get { return _invokeEffluent; }
        }
        bool _invokeLakeWaterManagement;
        public bool invokeLakeWaterManagement
        {
            set { _invokeLakeWaterManagement = value; }
            get { return _invokeLakeWaterManagement; }
        }


        // =====================================================================
        //
        double d_drought = 1.0;
        public int geti_DroughtControl()
        {
            int TempInt = Convert.ToInt32(d_drought * 100);
            return TempInt;
        }
        public void seti_DroughtControl(int value)
        {
            d_drought = ((double)value) / 100;
        }
        // ================================================================================
        // Sustainability
        //
        // ================================================================================

        /// <summary>
        /// Retain a memory of what the flow modifyer was, so we can revert it back
        /// after the endYear is reached
        /// </summary>
        double _initialFlowSurface = 0;
        double initialFlowSurface
        {
            get { return _initialFlowSurface; }
            set { _initialFlowSurface = value; }
        }
        double _initialFlowModGW = 0;
        double initialFlowModGW
        {
            get { return _initialFlowModGW; }
            set { _initialFlowModGW = value; }
        }


        #endregion
        //    
    }
    #region Utilities
    static class utilities
    {
        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        //If, as in your example, you really want to truncate, then you want:
        static double TruncateToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1 - digits);
            return scale * Math.Truncate(d / scale);
        }
        public static double DoubleBack(double In)
        {
            int Temp = 0;
            double temp = 0;
            Temp = Convert.ToInt32(In * 100);
            temp = (double)Temp / 100;

            return temp;
        }
        public static double hyperbola(double droughtFactor)
        {
            double b = 1.18;
            double a = -17;
            double temp = 1;
            const double minDF = 50;
            if (0 < droughtFactor)
            {
                if (50 <= droughtFactor)
                {
                    if (droughtFactor <= 100)
                        temp = droughtFactor / (a + b * droughtFactor);
                }
                else
                {
                    temp = droughtFactor / (a + b * minDF);
                }
            }
            return temp;
        }
    }

    #endregion
}
