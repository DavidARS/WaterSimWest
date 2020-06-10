using System;
using System.IO;
using ReadWriteCsv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsumerResourceModelFramework;
using WaterSimDCDC.America;


namespace WaterSimDCDC
{
    public class WaterSimAmerica
    {
        CRF_Network_WaterSim_America WSA;
        //
        internal StreamWriter sw;
        DateTime now = DateTime.Now;
        // http://waterdata.usgs.gov/fl/nwis/wu
        // Units
        // Million gallons per day (Mgd)--a rate of flow of water equal to 133,680.56 cubic feet per day, 
        // or 1.5472 cubic feet per second, or 3.0689 acre-feet per day. 
        // A flow of one million gallons per day for one year equals 1,120 acre-feet (365 million gallons).
        //
        // Version Control
        // 03.07.16, DAS, WaterSimAmerica_4.cs - Ag growth, changed Lake Water Management to work as desalination, added "invokePolicies"
        //
        //
        //
        //
        /// <summary>
        ///  Constructor for WaterSimAmerica
        /// </summary>
        #region Constructor
        static protected bool _isWaterForAmericaInstatiated = false;  // used to keep track if a WaterForAmerica object has been constructed
        CsvFileReader Reader;
        // QUAY EDIT 2/9/2016
        // string path = System.IO.Directory.GetCurrentDirectory();
        public WaterSimAmerica(string DataDirectoryName, string TempDirectoryName)
        {
            State = FStateName;
            StateData = "Just5StatesLakeNoRuralPower.csv";// "Just5StatesLakeNoRural.csv"; //"Just5StatesLake.csv";// "JustSmithStates.csv";// "All_50_states.csv";
            //String IndustryData = "FiveStateIndAnnualGrowth.csv";
            String IndustryData = "FiveStateIndGrowthAnnual.csv";
            try
            {
               // ReadTest(IndustryData);
                Reader = new CsvFileReader(DataDirectoryName + "\\" + IndustryData);
                int useYear = 2015;
                parseFromStringToArray(Reader, useYear);
                WSA = new CRF_Network_WaterSim_America(DataDirectoryName + "\\" + StateData, null, State);
                initialize_firstRun();
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
        internal void initialize_firstRun()
        {
            PopYearData[0] = FloridaPopYear;
            PopYearData[1] = IdahoPopYear;
            PopYearData[2] = IllinoisPopYear;
            PopYearData[3] = MinnesotaPopYear;
            PopYearData[4] = WyomingPopYear;
            //
            GPCDYearData[0] = FloridaGPCDYear;
            GPCDYearData[1] = IdahoGPCDYear;
            GPCDYearData[2] = IllinoisGPCDYear;
            GPCDYearData[3] = MinnesotaGPCDYear;
            GPCDYearData[4] = WyomingGPCDYear;
            //
            demand_total = 0;
            GPCD = 0;
            initProportions();
            //
            _population = oldPopulation = Population;
            maxReclaimed = 25;
        }

        // ============================================================================================================
        // New Access Fields
        /// <summary>
        /// State Indexing. This code grabs the string and the index for the State being examined.
        /// And, it resets the Network
        /// </summary>
        #region State Examined
            public const int FNumberOfStates = 5;
            string[] FStateNames = new string[FNumberOfStates] { "Florida", "Idaho", "Illinois", "Minnesota", "Wyoming" };

            public int FStateIndex = 0;
            public string FStateName = "Florida";
            public void seti_StateIndex(int value)
            {
                if ((value > -1) && (value < FNumberOfStates))
                {
                    FStateIndex = value;
                    FStateName = FStateNames[value];
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

        // ------------------------------------------------------
        //
        #region Reset Network and variables
            public void ResetNetwork()
            {
                seti_StateIndex(FStateIndex);
                ResetVariables();
            }
            public void ResetVariables()
            {
                oldPopulation = startPOP;
                int temp = 100;
                seti_PowerConservation(temp);
                seti_AgConservation(temp);
                seti_UrbanConservation(temp);
                seti_PopGrowthRate(temp);
                seti_GroundwaterControl(temp);
                seti_SurfaceWaterControl(temp);
                int zero = 0;
                seti_SurfaceWaterManagement(zero);
                seti_GroundwaterManagement(zero);
                seti_ReclaimedWaterManagement(zero);
                //
                oldPopulation = 0;
                seti_SurfaceWaterControl(temp);
                seti_GroundwaterControl(temp);
                InitialPower(FStateName);
                seti_PowerEnergy(initialPower);
            }
        #endregion
        //
        // ========================================================================================
        // MODEL
        //
        //
        public int runOneYear(int year)
        {
            currentYear = year;
            Model(year);
            return 0;
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
            void postProces(int yr)
            {
                int temp = 0;
                temp = Population;
                if (yr == startYear) startPop = temp;
                oldPopulation =temp;
            }
            internal void analyze_results()
            {
                int GPCDurban = urbanGPCD;
                int GPCDindustry = industrialGPCD;
                int GPCDag = agriculturalGPCD;
                int GPCDpower = powerGPCD;
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
            double temp = 0;
            int result = 0;
            temp = geti_SurfaceWaterFresh() * d_surfaceWaterControl * d_drought;
            result = Convert.ToInt32(temp);
            seti_SurfaceWaterFresh(result);
        }
        void surfaceSaline()
        {
            double temp = 0;
            int result = 0;
            temp = geti_SurfaceWaterSaline() * Desal;
            result = Convert.ToInt32(temp);
            result = (int)temp;
            seti_SurfaceWaterSaline(result);
        }
        void surfaceLake()
        {
            double temp = 0;
            int result = 0;
            temp = geti_SurfaceLake() * LWManagement;
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
            // Community Demand
            // -------------------------
            void Urban()
            {
                double temp = 0;
                int result = geti_Urban();
                if (invokePolicies)
                {
                    temp = weightedGrowth(geti_Urban());
                    result = ModifyDemand(temp, d_urbanConservation);
                }
                seti_Urban(result);
                i_demand_urban = result;
            }
            int urbanGPCD
            {
                get
                {
                    double temp = WSA.Urban.Demand * convertDemand;
                    double pop = Population;
                    double gpcd = 0;
                    if (0 < pop)gpcd = temp / pop;
                    // comparison estimates
                    //return Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Urban); }
                    return Convert.ToInt32(gpcd);
                }
            }
            // ----------------
            //
            // 
            //
            // AGRICULTURE PRODUCTION AND DEMAND
            //  
            //  Added Quay 3/3/16
            //#########################################################
            //
            # region agriculture
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
            double _agGrowthRate = 1.0;
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
                double CurrentRate = DampenRate(FAgRate / 100, Damper, period);
                // calculate the new $ production
                double NewProduction = Math.Round(FAgNet * Math.Pow(1 + (CurrentRate * AgGrowthRate), period));

                int temp = 0;
                try
                {
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

            public int Calc_AgDemand(int theCurrentYear, int theStartYear, double NewProduction)
            {
                int result = 0;
                // get the Initial Gallons per Dollar (ie assume it does not change; should insert some code to change this
                double InitialAGPD = AgricultureInitialGPDD;
                // Calculate new demand based on new production and AGPD
                // Adjust AGPCD
                double period = (theCurrentYear - theStartYear) + 1;
                double CurDamper = 1 + ((100.0 - (double)FAgTargetGPDDReduction) / 4000);
                FAdjAGPCD = DampenRate(InitialAGPD, CurDamper, period);

                double NewAgDemand = NewProduction * FAdjAGPCD;

                int temp = 0;
                try
                {
                    temp = Convert.ToInt32(NewAgDemand);
                    result = temp;
                }
                catch (Exception ex)
                {
                    // Ouch
                }
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
                //temp = weightedGrowth(geti_Agriculture());
                //result =ModifyDemand(temp,d_agConservation);
                // get Ag production
                FAgProduction = Calc_AgProduction(currentYear, startYear);
                // now estmate demand based on production
                result = Calc_AgDemand(currentYear, startYear, FAgProduction);
                // set the parameter for AgDemand
                seti_Agriculture(result);
                // Set the parameter for AgGPDD
                i_demand_ag = result;
            }


            int agriculturalGPCD
            {
                get
                {
                    double temp = WSA.Agriculture.Demand * convertDemand;
                    double pop = Population;
                    double gpcd = 0;
                    if(0 < pop) gpcd=temp / pop;
                    // For comparison
                    int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Ag);
                    return Convert.ToInt32(gpcd);
                }                          
            }
            #endregion
            // --------------------------------------------------------------------
  
            // ----------------
            // Power Water
            void PowerWater()
            {
                double temp = 0;
                int result = 0;
                temp = weightedGrowth(geti_PowerWater());
                result = ModifyDemand(temp, d_powerConservation);
                seti_PowerWater(result);
                i_demand_power = result;
            }
            int powerGPCD
            {
                get {
                    double temp = WSA.Power.Demand * convertDemand;
                    double pop = Population;
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
                temp = weightedGrowth(geti_PowerEnergy());
                result = Convert.ToInt32(temp);// ModifyDemand(temp, d_powerConservation);
                seti_PowerEnergy(result);
            }
 

            // ----------------
            // Industry
            void Industrial()
            {
                double temp = 0;
                int result = 0;
               // temp = weightedGrowth(geti_Industrial());
                temp = IndustryGrowth(geti_Industrial());
                //
                //temp = geti_Industrial();
                result = (int)temp;
                seti_Industrial(result);

                i_demand_industry = result;
            }
            int industrialGPCD
            {
                get {
                    double temp = WSA.Industrial.Demand * convertDemand;
                    double pop = Population;
                    double gpcd = 0;
                    if(0 < pop)gpcd=temp / pop;
                    // for comparison only
                    int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Industry);
                    return Convert.ToInt32(gpcd);
                }
            }
            // --------------
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
            // =========================================

            // ======================================================================================
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
        // sustainability
        //
        public int sustainability_surface_water;
        public int sustainability_groundwater;
        public int sustainability_economic;
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
                    if (0 < Population)
                        temp = (d_demand_total * MGDtogal) / Convert.ToDouble(Population);
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
            //internal double Proportion_Waterdemand_Rural
            //{
            //    get { return _proportion_waterdemand_rural; }
            //    set
            //    {
            //        double temp = value;
            //        if (0 < d_demand_total)
            //        {
            //            temp = Convert.ToDouble(geti_Rural()) / Convert.ToDouble(d_demand_total);
            //            temp = utilities.RoundToSignificantDigits(temp, 3);
            //        }
            //        _proportion_waterdemand_rural = temp;
            //    }
            //}
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
        #region Website directory faking
        private static string DataDirectoryName
        {
            get
            {
                return @"App_Data\";
            }
        }

        private static string TempDirectoryName
        {
            set
            {
                string dir = value;
                string.Concat(@"WaterSmith_Output\", dir);
            }
            get
            {
                // Make a common for testing
                return @"WaterSmith_Output\";
                // Make the temp directory name unique for each access to avoid client clashes
                //return +System.Guid.NewGuid().ToString() + @"\";
            }
        }
        private static void CreateDirectory(string directoryName)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        #endregion

        // -------------------------------------------------
        // Population
        // -------------------------------------------------
        #region Population
        // ------------
        // 
        int _oldPop = 0;
        int oldPopulation
        {
            set { _oldPop = value; }
            get { return _oldPop; }
        }
        int _population = 0;
        double _populationRatio = 0;
        // Temp Pop Data
        // Year	2015	2020	2025	2030	2035	2040	2045	2050	2055	2060	2065

        const int StartYear = 2015;
        const int EndYear = 2065;
        const int RawPopDataInc = 5; //Years
        const int NumberRawYears = ((EndYear - StartYear) / 5) + 1;

        int[] FloridaPopYear = new int[NumberRawYears] { 19815183, 21236667, 22478109, 23872566, 24939176, 26081392, 27400243, 28630776, 29861309, 31091843, 32322376 };
        int[] IdahoPopYear = new int[NumberRawYears] { 1656870, 1741333, 1852627, 1969624, 2057935, 2158202, 2258468, 2358735, 2459001, 2559268, 2659534 };
        int[] IllinoisPopYear = new int[NumberRawYears] { 12978800, 13129233, 13156436, 13263662, 13357779, 13432892, 13559122, 13659794, 13760465, 13861137, 13961808 };
        int[] MinnesotaPopYear = new int[NumberRawYears] { 5502386, 5677582, 5841619, 5982601, 6093729, 6175801, 6234930, 6281407, 6297125, 6297125, 6297125 };
        int[] WyomingPopYear = new int[NumberRawYears] { 587662, 616145, 640401, 665674, 691719, 717379, 743038, 768697, 794357, 820016, 845675 };

        int[][] PopYearData = new int[WaterSimManager.FNumberOfStates][];
        //

        public int Population
        {
            get
            {
                int temp = 0;
                temp = geti_pop();
                return temp;
            }
            set { _population = value; }
        }
        double _startPop = 0;
        double startPop
        {
            set { _startPop = value; }
            get { return _startPop; }
        }
        double populationRatio
        {
            set
            {
                double temp;
                temp = 0.0;
                if (0 < _oldPop)
                {
                    double pop = Population;
                    double old = _oldPop;
                   // if(startPop < old)
                      temp = ((pop - old) / old);
                }
                else
                {
                    temp = 0;
                }
                _populationRatio = temp;
            }
            get { return _populationRatio; }
        }
        double weightedGrowth(int institution)
        {
            double result = institution;
            result = institution + Convert.ToInt32(Convert.ToDouble(institution) * populationRatio);
            return result;
        }
        double IndustryGrowth(int institution)
        {
            double result = institution;
            double growth;
            int index = geti_YearIndex();
            //parseFromString(Reader, currentYear, FStateIndex, out growth);
            growth = Industry[index][FStateIndex + 1];
             result = institution + Convert.ToInt32(Convert.ToDouble(institution) * growth);
            return result;
        }

        // move this to WaterSim Manager
        internal int geti_pop()
        {
            return Get_PopYear(currentYear);
        }

        // QUAY EDIT 2/24/16
        //internal int Get_PopYear(int year)
        public int Get_PopYear(int year)
        // END QUAY EDIT
        {
            int start = 2015;
            int TempPop = 0;
            int ModYear = 0;
            int result = 0;
            bool bypassCode = true;
            double diffPop = 0;
            double startpop = 0;
            double endpop = 0;
            //
            if (year == 0) year = start; // year = 2015;// currentYear;
            ModYear = year % RawPopDataInc; ;
            if (ModYear == 0)
            {
                int yearIndex = (year - start) / RawPopDataInc;
                if (yearIndex == 0) startPOP = TempPop = PopYearData[FStateIndex][yearIndex];
            }
            else
            {
                if (bypassCode)
                {
                    int indexPOP = 10;
                    endpop = PopYearData[FStateIndex][indexPOP];
                    startpop = startPOP;
                    diffYear = endYear - StartYear;

                    initialSlope = (endpop - startpop) / diffYear;
                    endPOP = Convert.ToInt32(d_popGrowthRate * PopYearData[FStateIndex][indexPOP]);

                }
                else
                {
                    int lowyearindex = ((year - start) - ModYear) / RawPopDataInc;
                    int hiyearindex = lowyearindex + 1;
                    int lowpop = PopYearData[FStateIndex][lowyearindex];
                    int hipop = PopYearData[FStateIndex][hiyearindex];
                    int PopChangeByYear = (hipop - lowpop) / RawPopDataInc;
                    TempPop = lowpop + (PopChangeByYear * ModYear);
                }
            }
            int temp = TempPop;
            if (year != start)
            {
                // Set these values only once
                if (year == start + 1)
                {
                    // diffYear = (endYear - startYear);
                    if (0 < diffYear) slope = (endPOP - startPOP) / diffYear;
                    intercept = startPOP - Convert.ToInt32(initialSlope);
                }
                //
                // This needs updating annually
                double runningYear = diffYear - (endYear - currentYear) + 1;

                //Slope[0] = (endPOP - startPOP) / diffYear; ;
                //if (runningYear == 2) Slope[2] = (endPOP - startPOP) / diffYear;
                //if (runningYear == 3) Slope[3] = (endPOP - startPOP) / diffYear;
                //if (runningYear == 4) Slope[4] = (endPOP - startPOP) / diffYear;
                //if (runningYear == 5) Slope[5] = (endPOP - startPOP) / diffYear; 
                //startpop = Convert.ToDouble(startPOP);

                // Threshold check on population change
                if (0 < startpop) diffPop = ((endpop - startpop) / startpop) * 100;
                slope = Math.Max(0, slope);
                result = Convert.ToInt32(slope * runningYear + intercept);
            }
            else
            {
                result = startPOP;
            }
            //
            return result;
        }
        int _startPOP = 0;
        int startPOP
        {
            get { return _startPOP; }
            set { _startPOP = value; }
        }
        int _endPOP = 0;
        int endPOP
        {
            get { return _endPOP; }
            set { _endPOP = value; }
        }
        double _intercept = 0;
        double intercept
        {
            get { return _intercept; }
            set { _intercept = value; }
        }
        double _initialSlope = 0;
        double initialSlope
        {
            set { _initialSlope = value; }
            get { return _initialSlope; }
        }
        double _diffYear = 0;
        double diffYear
        {
            get { return _diffYear; }
            set { _diffYear = value; }
        }
        double _slope = 0;
        double slope
        {
            get { return _slope; }
            set { _slope = value; }
        }
        double[] Slope = new double[5];
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

        const int RawGPCDDataInc = 5; //Years
        const int NumberGPCDYears = ((EndYear - StartYear) / 5) + 1;

        int[] FloridaGPCDYear = new int[NumberGPCDYears] { 250, 240, 230, 220, 210, 200, 195, 190, 185, 180, 175 };
        int[] IdahoGPCDYear = new int[NumberGPCDYears] { 250, 240, 230, 220, 210, 200, 195, 190, 185, 180, 175 };
        int[] IllinoisGPCDYear = new int[NumberGPCDYears] { 250, 240, 230, 220, 210, 200, 195, 190, 185, 180, 175 };
        int[] MinnesotaGPCDYear = new int[NumberGPCDYears] { 250, 240, 230, 220, 210, 200, 195, 190, 185, 180, 175 };
        int[] WyomingGPCDYear = new int[NumberGPCDYears] { 250, 240, 230, 220, 210, 200, 195, 190, 185, 180, 175 };

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
        // 02.09.16
        internal int geti_gpcdTotal()
        {
            return GPCD;
        }
        #endregion
        //
        // =============================================================================================
        // Network Parameters

        // CRF_NETORK PARAMETERS
        // =================================================
        //--------------------------------------------------
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
                int tempint = Convert.ToInt32(Math.Abs(Math.Round(temp))+0 );
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
                int TempInt = Convert.ToInt32(Math.Abs(Math.Round(temp)) + 0 );
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
                WSA.Effluent.Limit = Math.Min(maxReclaimed, (double)value + effluentToAdd);

            }
        }
        public int geti_EffluentNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Effluent.Net)) + 0);
            return TempInt;
        }

        double _maxReclaimed = 0;
        const double maxReclaimedRatio = 0.95;
        const double consumptive = 0.86; // leaks http://www3.epa.gov/watersense/pubs/indoor.html
        const double indoor = 0.45;
        double maxReclaimed
        {
            set { _maxReclaimed = value; }
            get { return _maxReclaimed; }
        }
        double effluentToAdd
        {
            get
            {
                double temp = 0;
                temp = d_reclaimedWaterUse * WSA.Urban.Demand;
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
        ///// <summary>
        ///// Policy Start Year; starts the year in which any policy 
        ///// starts; valid are 2016 to 2060 (at present)
        ///// </summary>
        //int _policyStartYear = 2016;
        //public int geti_PolicyStartYear()
        //{
        //   return _policyStartYear;
        //}
        // public void seti_PolicyStartYear(int value)
        //{
        //    _policyStartYear = value;
        //}
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
        public double Desal
        {
            get
            {

                double temp = 1;

                if (invokePolicies)
                {
                    temp = 1 + _desalinization;
                }
                return temp;

            }
        }



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
        // =====================================================================
        // ============================================
        // Groundwater Management 
        //--------------------------------
        /// <summary>
        /// Seti GroundwaterManagement
        /// </summary>
        /// <returns>an int from zero to 100   . </returns>
        double d_groundwaterManagement = 0.00;
        public int geti_GroundwaterManagement()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_groundwaterManagement));
            return TempInt;
        }
        public int geti_GroundwaterControl()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_groundwaterControl));
            return TempInt;
        }

        ///------------------------------------------------------------------
        /// <summary>   Seti GroundwaterManagement.</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_GroundwaterManagement(int value)
        {

            //CheckBaseValueRange(eModelParam.epGroundwaterManagement, value);
            d_groundwaterManagement = value;
        }
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
        double d_surfaceWaterManagement = 0.00;
        public int geti_SurfaceWaterManagement()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_surfaceWaterManagement));
            return TempInt;
        }
        public int geti_SurfaceWaterControl()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_surfaceWaterControl));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Seti SurfaceWaterManagement. ONLY called 
        /// durring initialization</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_SurfaceWaterManagement(int value)
        {
            d_surfaceWaterManagement = value;
        }
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
        public double LWManagement
        {
            get
            {
                double temp = 1;

                if (invokePolicies)
                {
                    temp = 1 + d_lakeWaterManagement;
                }
                return temp;
            }
        }
        bool _invokePolicies = false;
        public bool invokePolicies
        {
            set { _invokePolicies = value; }
            get { return _invokePolicies; }
        }

        // =====================================================================
        //
        double d_drought = 1.0;
        public int geti_DroughtControl()
        {
            //int TempInt = Convert.ToInt32(Math.Round(d_drought));
            int TempInt = Convert.ToInt32(d_drought*100);
            return TempInt;
        }
        public void seti_DroughtControl(int value)
        {
            d_drought = ((double)value) / 100;
        }
        // ================================================================================
        // Sustainability
        //
        //Groundwater sustainability
        //double indicator_groundwater = 0;
        //public void seti_Sustainability_Groundwater(int value) { Sustainability_Groundwater = value; }
        //public int geti_Sustainability_Groundwater() { return Sustainability_Groundwater; }
        //public int Sustainability_Groundwater
        //{
        //    set
        //    {
        //        double temp = 0;
        //        int gw = geti_Groundwater();
        //        temp = (SafeYield / Convert.ToDouble(gw)) * 100;
        //        indicator_groundwater = temp;
        //    }
        //    get { return Convert.ToInt32(indicator_groundwater); }    // 
        //}
        ////
        //double _safeYield = 0;
        //double safeYield
        //{
        //    get { return _safeYield; }
        //    set { _safeYield = value; }

        //}
        //double SafeYield
        //{
        //    get
        //    {
        //        //Florida", "Idaho", "Illinois", "Minnesota", "Wyoming"
        //        double[] yield = { 202.80, 94.22, 34.23, 288.65, 113.07 };
        //        safeYield = yield[FStateIndex];
        //        return safeYield;
        //    }
        //}


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
        double[][] Industry = new double[51][];
        void parseFromString(CsvFileReader reader,int currentYear,int State, out double myDouble)
        {
            string myString = reader.ReadLine();
            if (currentYear == 2015)
            {
                myString = reader.ReadLine();
            }
            string[] parts = myString.Split(',');
            string Value= "";
            myDouble=0;
               Value = parts[State+1];
               myDouble = double.Parse(Value);
         }
        void parseFromStringToArray(CsvFileReader reader, int currentYear)
        {
            string myString = reader.ReadLine();
            double[] mydouble = new double[6];
            string[] Value = new string[6];
            string[] parts = myString.Split(',');
            for (int i = 0; i < 51; i++)
            {
                myString = reader.ReadLine();
                parts = myString.Split(',');

                 for (int b = 0; b < 6; b++)
                 {
                     Value[b] = parts[b];
                     mydouble[b] = double.Parse(Value[b]);
                 }
                 Industry[i] = new double[] { mydouble[0], mydouble[1], mydouble[2], mydouble[3], mydouble[4], mydouble[5] };
             }

            
        }

       



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
        }
    #endregion
}
