using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsumerResourceModelFramework;
using WaterSim_Base;
using DemandModel_Base;
using UniDB;
using System.Data;

// WATERSIMDCDC GENERIC 
// VERSION 5_0_0
// This is the core WaterSim Consumer Resource Framewoirk Model
// Notes
// 2 13 18  QUAY Chnaged name of default data file name
// 2 16 18  QUAY Deleted references to WaterSimAmerica instatiated
// 2 18 18  Rebuilt Agriculture
//  
// Version 3_0_0
// 2 29 28  Created Generic Consumer growth and demand routines
// 3  1 18   Applied generic Consumer Growth and demand to Agriculture
// 3 28 18   Added drought 
// 3 29 18   Fixed katency errors for web controls
//
// VERSION 4_0_0
// 
// 9 11 18  Added colord River Resource
//
// VERSION 5_0_0
// 9 13 18 Merged Colorado Code with New LandUse/Greywater Model

// 
//  RULES/CONVENTIONS
//  1) All aspects of the WaterSimCRF model should be done inside the model() method.  the runoneYear() method
//  calls model().  In the runoneYear() method, actions before or after model() should only be system level activities
//  and not something that will inclfuence the function of model().  Classes using the runoneYear() method directly 
//  should set any parameters that will affect function before runoneYear() and should access model data only after
//  runoneYear() has completed. 
//  2) Before runoneyear() can be called for the first time, several things must be done 
//    a) the StartYear and EndYear for the simulation must be set.  The model will use these, not setting these will
//       result in bad results.
//    b) ResetNetwork() should be called.  This will call ResetVariables().
//  3) Parameters of the model that will be set to create a scenario, should be set before a call to runoneYear() is made,
//     but after the above pre tasks for the first call to runoneYear 



namespace WaterSimDCDC.Generic
{

    //==========================================================================================================================
    //  MODEL CALL BACK HANDLERS 
    //==========================================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Executes the run model handler action. </summary>
    ///
    /// <param name="year"> The year. </param>
    ///-------------------------------------------------------------------------------------------------

    public delegate void OnRunModelHandler(int year);

    //==========================================================================================================================
    //  WaterSim CRF Model 
    //==========================================================================================================================

    /// <summary>   Water simulation crf model. </summary>
    public class WaterSimCRFModel
    {

        /// <summary> The unit network.</summary>
        // QUAY EDIT
        // Changed to West_CRF_UnitNetwork to add COlorado
        West_CRF_Unit_Network UnitNetwork;
        //CRF_Unit_Network UnitNetwork;
        // END EDIT

        WaterSimManager WS;
        /// <summary> The frdc.</summary>
        /// <remarks>  This is the rate class used to retrieve data from the growth data file</remarks>
        //  RateDataClass FRDC;
        public RateDataClass FRDC;

        public DataClassLCLU FDClclu;

        public DataClassTemperature FDCtemperature;
        /// <summary> Information describing the unit.</summary>
        /// <remarks>  This is the unit Data class used to retrieve USGS combined and summary data by region </remarks>
        UnitData FUnitData = null;

        // ===========================
        // Demand Classes
        // ==============
        //
        // base class models
        //
        //DemandModel UrbanDEM;
        DemandModel UrbanDEM;
        DemandModel AgDEM;
        DemandModel IndustrialDEM;
        DemandModel PowerDEM;

        // Direct access models
        UrbanDemand_GPCD UD;
        //UrbanDemand_GPCDa UDA;
        AgriculturalDemand_income ADI;
        PowerDemand_wp PD;
        IndustryDemand_employee ID;
        //
        RuralDemand_LCLU_urban UDR;
        RuralDemand_LCLU_ag ADR;
        RuralDemand_LCLU_industry IDR;
        //============================
        DataClassTemperature DataClassT;
        /// <summary>
        /// 
        ///
        /// </summary>
        // These are used if WaterSimCRFModel is used as a singe model using first constructor
        //string FUnitName = "";
        public string FUnitName = "";
        int FUnitCode = 0;
        string FComment = "";


        internal StreamWriter sw;
        DateTime now = DateTime.Now;
        /// <summary> The run call back.</summary>
        /// <remarks> This is the internal field for the Run Call back event handler, is called in run year if not null</remarks>
        /// <seealso cref="OnRunHandler"/>
        /// 
        OnRunModelHandler FRunCallBack = null;

        // http://waterdata.usgs.gov/fl/nwis/wu
        // Units
        // All USGS data for consumers, demands, and fluxes is Million Gallons Per Day.
        // 
        // Million gallons per day (Mgd)--a rate of flow of water equal to 133,680.56 cubic feet per day, 
        // or 1.5472 cubic feet per second, or 3.0689 acre-feet per day. 
        // A flow of one million gallons per day for one year equals 1,120 acre-feet (365 million gallons).

        ///  Constructor for CRF Models, two constructir, one uses default rate file and one uses specified files while one uses an existing set of data structure and a unit name
        #region Constructors

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/6/2017. </remarks>
        /// <remarks>   This constructor can be used to run a model using USGS State level data, however, UID nees to change unitname to state fields 
        ///             3/1/2018   Quay, I have not tested to see if this actually works !!!!
        ///             </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="DataDirectoryName">    Pathname of the data directory. </param>
        /// <param name="TempDirectoryName">    Pathname of the temporary directory. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel(string DataDirectoryName, string TempDirectoryName)
        {

            FUnitName = UDI.DefaultUnitName;

            string rates = "ElevenStateGrowthRates3.csv";
            //string rates = "WestRegionRates_2.csv";

            try
            {
                FRDC = new RateDataClass(DataDirectoryName, rates);
                string StateDataPath = DataDirectoryName + "//" + UDI.USGSDataFilename;
                FUnitData = new UnitData(StateDataPath);
                // QUAY EDIT 9/11/18
                // Changed to West_CRF_Unit_Network to add colorado
                // 
                UnitNetwork = new West_CRF_Unit_Network(FUnitData, FUnitName);
                //UnitNetwork = new CRF_Unit_Network(FUnitData, FUnitName);
                // END EDIT
                //
                  StreamW(TempDirectoryName);

                //
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Gets all the base data POpRate, Initialpop, Agrate , AgNet etc. 
            SetBaseValues();

        }
        public void StreamW(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Output" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw = File.AppendText(filename);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   2/6/2017. </remarks>
        ///
        /// <param name="TheUnitData">  Information describing the unit. </param>
        /// <param name="TheRateData">  Information describing the rate. </param>
        /// <param name="TheUnitName">  Name of the unit. </param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel(UnitData TheUnitData, RateDataClass TheRateData, string TheUnitName)
        {
            FUnitName = TheUnitName;
            string errMsg = "";
            FRDC = TheRateData;
            FUnitData = TheUnitData;
            if (FUnitData.GetValue(TheUnitName, UDI.UnitCodeField, out FUnitCode, out errMsg))
            {
                // All good
            }
            else
            {
                // Not So Good 
                FComment = "Unit Code : " + errMsg;
            }
            // QUAY EDIT 9/11/18
            // Changed to West_CRF_UNit_Network to add Colorado
            UnitNetwork = new West_CRF_Unit_Network(TheUnitData, TheUnitName);

            //UnitNetwork = new CRF_Unit_Network(TheUnitData, TheUnitName);
            // EDN EDIT


            // Gets all the base data POpRate, Initialpop, Agrate , AgNet etc. 
            SetBaseValues();

            // This should be used to initialize variables that will not change from one simulation to another
            // This should include intitial values retreived from the database
            // As opposed to resetVariables() that set defaults for each simulation
            Initialize_Variables();
            //initialize_FirstRun();  

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TheUnitData"></param>
        /// <param name="TheRateData"></param>
        /// <param name="DataLCLU"></param>
        /// <param name="TheUnitName"></param>
        public WaterSimCRFModel(UnitData TheUnitData, RateDataClass TheRateData, DataClassLCLU DataLCLU, string TheUnitName)
        {
            FUnitName = TheUnitName;
            string errMsg = "";
            FRDC = TheRateData;
            FUnitData = TheUnitData;
            FDClclu = DataLCLU;
            //
            if (FUnitData.GetValue(TheUnitName, UDI.UnitCodeField, out FUnitCode, out errMsg))
            {
                // All good
            }
            else
            {
                // Not So Good 
                FComment = "Unit Code : " + errMsg;
            }
            // EDIT QUAY 9/13/18
            UnitNetwork = new West_CRF_Unit_Network(TheUnitData, TheUnitName);
            // END EDIT

            // Gets all the base data POpRate, Initialpop, Agrate , AgNet etc. 
            SetBaseValues();

            // This should be used to initialize variables that will not change from one simulation to another
            // This should include intitial values retreived from the database
            // As opposed to resetVariables() that set defaults for each simulation
            Initialize_Variables();
            //initialize_FirstRun();

            //  isInitialized = true;
            //


        }
        ///-------------------------------------------------------------------------------------------------
        public WaterSimCRFModel(UnitData TheUnitData, RateDataClass TheRateData, DataClassLCLU DataLCLU, DataClassTemperature Tav ,string TheUnitName)
        {
            FUnitName = TheUnitName;
            string errMsg = "";
            FRDC = TheRateData;
            FUnitData = TheUnitData;
            FDClclu = DataLCLU;
            FDCtemperature = Tav;
            //
            if (FUnitData.GetValue(TheUnitName, UDI.UnitCodeField, out FUnitCode, out errMsg))
            {
                // All good
            }
            else
            {
                // Not So Good 
                FComment = "Unit Code : " + errMsg;
            }
            // ESIT QUAY 9/13/18
            UnitNetwork = new West_CRF_Unit_Network(TheUnitData, TheUnitName);
            // END EDIT

            // Gets all the base data POpRate, Initialpop, Agrate , AgNet etc. 
            SetBaseValues();

            // This should be used to initialize variables that will not change from one simulation to another
            // This should include intitial values retreived from the database
            // As opposed to resetVariables() that set defaults for each simulation
            Initialize_Variables();
            //initialize_FirstRun();
            string Filename = "Compare.txt";

          //  sw = new System.IO.StreamWriter(Filename);

            //  isInitialized = true;
          //  UD = new UrbanDemand_GPCD(this);
          ////  UDA = new UrbanDemand_GPCDa(this);
          //  UDR = new RuralDemand_LCLU_urban(this, TheRateData, DataLCLU);
          //  ADR = new RuralDemand_LCLU_ag(this, TheRateData, DataLCLU);
          //  ADI = new AgriculturalDemand_income(this);
          //  PD = new PowerDemand_wp(this);
          //  IDR = new RuralDemand_LCLU_industry(this, TheRateData, DataLCLU);
          //  ID = new IndustryDemand_employee(this);
            //


        }

        /// 
        /// 
        /// 
        /// <summary>   Dispose of this object, cleaning up any resources it uses. </summary>
        /// <remarks> Modified 2/18/18 Quay </remarks>
        /// <param name="disposing">    true if resources should be disposed, false if not. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void Dispose(bool disposing)
        {
            //_isWaterForAmericaInstatiated = false;
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the crf network for this model. </summary>
        ///
        /// <value> the crf network. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network TheCRFNetwork
        {
            get { return UnitNetwork; }
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
        /// <summary>   Gets the unit code. </summary>
        ///
        /// <value> The unit code. </value>
        ///-------------------------------------------------------------------------------------------------

        public int unitCode
        {
            get { return FUnitCode; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets the on run handler. </summary>
        ///
        /// <value> The on run handler. </value>
        ///-------------------------------------------------------------------------------------------------

        public OnRunModelHandler OnRunHandler
        {
            set { FRunCallBack = value; }
        }
        //
        // ===================================
        // Class Assignemnts for Property
        // =====
        /// <summary>
        /// 
        /// </summary>
        public DemandModel URBAN
        {
            get { return UrbanDEM; }
            set { UrbanDEM = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DemandModel AG
        {
            get { return AgDEM; }
            set { AgDEM = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DemandModel INDUSTRY
        {
            get { return IndustrialDEM; }
            set { IndustrialDEM = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DemandModel POWER
        {
            get { return PowerDEM; }
            set { PowerDEM = value; }
        }
        // ==============================================================================
        /// <summary>
        /// A Method and property to get added water demand due to temperature increases
        /// into the demand models via the CRF model. 07.09.2018
        /// </summary>
        /// <param name="DCT"></param>
        /// <param name="CRF"></param>
        //
        public void TemperatureData(DataClassTemperature DCT, WaterSimCRFModel CRF)
        {
            double population = CRF.population;
            int theyear = CRF.currentYear;
            double temp = Utilities.TemperatureFunction_AddDemand(population, theyear, DCT.FastTav(CRF.FUnitName, theyear), DCT.FastTC(CRF.FUnitName, theyear));
            TheAddedDemand = temp;

            LoadSensitivityToTemperature(DCT, CRF);
            

        }
        double _addedDemand = 0;
        public double TheAddedDemand
        {
            set { _addedDemand = value; }
            get { return _addedDemand; }
        }
        //
        // ==================================
        //
        public void LoadSensitivityToTemperature(DataClassTemperature DCT, WaterSimCRFModel CRF)
        {
            double temp = Utilities.loadSensitivity(DCT.FastTav(CRF.FUnitName, CRF.currentYear), DCT.FastTC(CRF.FUnitName, CRF.currentYear));
            TheAddedPowerPCT = temp ;

        }
        double _addedPower = 0;
        public double TheAddedPowerPCT
        {
            set { _addedPower = value; }
            get { return _addedPower; }

        }
        //
        // ============================================================================
        //
        #endregion

        // ====================================================================================
        // Model Control
        // These routines control the model
        // ====================================================================================
        #region Model_Control

        //-------------------------------------------------
        // MODEL CONSTANTS
        // ------------------------------------------------

        // This is the default goal for Urban GPCD 
        const double _DefaultUrGPCDAdjust = 0.99;
        // This is the default goal for Ag GPDD 
        const double _DefaultAgGPDDAdjust = 0.99;
        // The default goal for Power GPWD
        const double _DefaultPwGPWDAdjust = 0.99;
        // The Default goal for industry GPED
        const double _DefaultIndGPEDAdjust = 0.99;
        // The default value for Modifying the Growth Rate
        const double _DefaultPopGrowthMode = 1.0;
        // this is thedefault for the POPulation Growth damper
        const double _UrbanPOPAdjustDamper = .9;
        //  This is as low as Urban GPCD should be allowed to go
        const double _MinimumUrbanGPCD = 30.0;

        // This is the coeffecient to convert USGS MGD Consumer/resource numbers to gallons
        public const double convertDemand = 1000000;
        // This is the coeffecient to convert USGS MegaWatts MW Consumer numbers to watts
        public const double convertPower = 1000000;
        // This is the coeffecient to convert employee to persons
        public const double convertEmployee = 1000;
        // This is the coeffecient to convert pop to persons
        public const double convertPopulation = 1000;
        /// <summary> The convert ag production to Dollars.</summary>
        public const double convertAgProduction = 1000;
        ///// <summary>
        /////  This constant is multiplied against gallons to obtain liters
        ///// </summary>
        //public const double convertGallonsToLiters = 3.785411784;

        //-------------------------------------------------
        // MODEL FIELDS
        // ------------------------------------------------

        /// <summary> The base population, thisis read from the USGS data file.</summary>
        double FBasePopulation = 0;
        /// <summary>   The pop growth rate, this is read from the Rate file. </summary>
        double FPopRate = 0;
        /// <summary>   The Basse value for Fame Income, this is read from Rate FIle. </summary>
        double FAgNet = 0;
        /// <summary>   The ag rate growth rate. This is read from the Rate file </summary>
        double FAgRate = 0;

        /// <summary>   The ind rate. This is read from the Rate FIle</summary>
        double FIndRate = 0;
        //  This is the initial GPCD value, the 120 is a default, this is calclated based on data from the USGS data file
        double FBaseUrbanGPCD = 120;
        // This is the initial GPDD value for agriculture, calculated based on FAGNet and data readt from USGS Data file
        double FAgBaseGPDD = 0;

        double _agAdjustProductionRate = 1;
        const double convertFarmIncome = 1000;
        //

        //
        // EDIT QUAY 3/4/18
        // used to create Change Coeeficients

        // Urban
        double FMinUrbanGPCDPercent = _MinimumUrbanGPCD / 120;
        double FUrbanGPCDChangeCoef = 1;

        // ****************************
        // Sampson edits 06.27.18
        double FMinUrbanLCLU = 0;
        double FUrbanLCLUChangeCoef = 1;
        // End Sampson Edits 06.27.18

        // ====================================================
        //

        // AG
        double FMinAgGPDDPercent = 0.10;
        double AgGPDDChangeCoef = 1;

        // *************************
        // Sampson edits 06.27.18
        double FMinAgLCLU = 0;
        double FAgLCLUChangeCoef = 1;
        // End Sampson Edits 06.27.18
        // ====================================================

        // Power
        // Intital Gallons per mega watt of power per day
        double FInitialGPMWD = 0;
        // Minimum Gallons Per Mega Watt per day
        double FMinPwGPMWDPercent = 0.1;
        double FPwGPMWChangeCoef = 1;

        // Initial watt per person 
        public double FInitialWattPerPerson = 0;
        double FMinWattPerPersonPercent = 0.50;
        double FWattPerPersonChangeCoef = 1;

        // END EDIT 3/4/18. 

        // EDIT QUAY 3/11/18
        // Industry
        // Number of Industrial Employees
        double FIndEmployees = 0;
        // The Initial number of employees
        double FIndInitialEmploy = 0;
        // Indistrial Change Coef for Employee Growth
        double FindEmpChangeCoef = 1;
        // Initial Industrial Galloans per Employee per day
        double FIndInitialGPED = 0;
        // Industrial Gallons per Employee per Day
        double FIndGPED = 0;
        // indistrial Minimum percent decline in GPED
        double FIndMinGPEDPercent = 0.10;
        // Indiustrial GPEDCHangeCoef
        double FindGPEDChangeCoef = 1;
        // END EDIT 3/11

        // *************************
        // Sampson edits 06.27.18
        double FMinIndustryLCLU = 0;
        double FIndustryLCLUChangeCoef = 1;
        // End Sampson Edits 06.27.18
        // Sampson edits 07.02.18
        int FDemandModelUrbanIndex = 1;
        int FDemandModelAgIndex = 1;
        int FDemandModelIndIndex = 1;
        // End Sampson Edits 07.02.18
        //
        // ====================================================



        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets base values at the beginning of a model run.</summary>
        /// <remarks> Quay, 3/5/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        internal void SetBaseValues()
        {
            // Get Population
            FBasePopulation = UnitNetwork.InitialPopulation;
            // Set up Consumner DEmand Fields
            SetInitialDemandFactors();
            // Get the rates
            SetInitialRates();
        }

        /// <summary>   Sets the initial rates. </summary>
        /// <remarks> Quay 2/6/17  This gets the initial rates from the RateData class</remarks>
        void SetInitialRates()
        {
            FAgRate = FRDC.FastAgRate(FUnitName);
            FIndRate = FRDC.FastIndRate(FUnitName);
            FPopRate = FRDC.FastPopRate(FUnitName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets initial demand factors.</summary>
        ///-------------------------------------------------------------------------------------------------

        void SetInitialDemandFactors()
        {
            // get Agricuture Farm Income
            FAgNet = FRDC.FastAgNet(FUnitName);
            // Initialize Base GPDD, Use Network Demand as of NOW!
            double AgWater = TheCRFNetwork.Agriculture.Demand;
            // Calculate Production Efficiency Coeficient
            AgricultureInitialGPDD = AgWater / FAgNet;
            // Intialize base Urban GOCD, Use Netwrok Demand as of NOW!
            double UrbanWater = TheCRFNetwork.Urban.Demand;
            // Calculate GPCD
            if (FBasePopulation > 0)
            {
                // demand is million and pop 1000, no need to convert
                FBaseUrbanGPCD = (UrbanWater * convertDemand) / (FBasePopulation);
            }
        }
        #region CapAndReviseResources
        //====================================================================
        // Resource Capping and Control Revisions
        // ===================================================================


        // This bool will triger a call for all resources to CapLimitToDemand() if true
        const bool FDefaultCapLimitsToDemand = true;
        bool FCapLimitsToDemand = FDefaultCapLimitsToDemand;
        // This bool will trigger a revision of the Surfacewater and groundwater wwb controls based on values needed to acheive final capped limit.
        const bool FDefaultReviseResources = true;
        bool FReviseResources = FDefaultReviseResources;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the do revise resources at end of model run.</summary>
        /// <remarks> the ReviseResources methods will only be called if this and DoCapLimits is true </remarks>
        /// <value> True if do revise resources, false if not.</value>
        ///-------------------------------------------------------------------------------------------------

        public bool DoReviseResources
        {
            get { return FReviseResources; }
            set { FReviseResources = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti do revise resources.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <seealso cref="DoReviseResources"/>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DoReviseResources()
        {
            if (DoReviseResources)
                return 1;
            else
                return 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti do revise resources.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <seealso cref="DoReviseResources"/>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DoReviseResources(int value)
        {
            if (value == 0)
            {
                DoReviseResources = false;
            }
            else
            {
                DoReviseResources = true;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the do cap limits to demand each year</summary>
        /// <value> True if do capability limits, false if not.</value>
        ///-------------------------------------------------------------------------------------------------

        public bool DoCapLimits
        {
            get { return FCapLimitsToDemand; }
            set { FCapLimitsToDemand = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti for DoCaplimits.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <seealso cref="DoCapLimits"/>
        /// <returns> An int  0=DoCaplimts = false;  1= DoCaplimits = true.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DoCapLimits()
        {
            if (DoCapLimits)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti for Docaplimits.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <seealso cref="DoCapLimits"/>
        /// <param name="value"> The value. 0=DoCaplimts = false;  any thing else= DoCaplimits = true.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DoCapLimits(int value)
        {
            if (value == 0)
            {
                DoCapLimits = false;
            }
            else
            {
                DoCapLimits = true;
            }
        }

        //================================================================================

        #endregion CapAndReviseResources


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes the Base Variables on Objest Creation.</summary>
        /// <remarks> Quay 2/18/18 This method is only called when the unitmodel is constructed, It should only be used 
        ///           to initialize variables that will not change between scenarios.  Anything that needs to be initialized 
        ///           for each scenario should be initialized elsewhere </remarks>
        ///-------------------------------------------------------------------------------------------------


        // QUAY EDIT 3/9/18
        // OK, I fially renamed this to be called more what it is doing            
        //internal void initialize_FirstRun()
        // END EDIT 3/9/18
        internal void Initialize_Variables()
        {
            //QUAY EDIT 3/30/18
            // DISCONNECTING Population Proportion Code, this was part of it though demand_total is not be referenced for a get by any code, old or new
            //demand_total = 0;
            // End Edit
            GPCD = 0;
            // QUAY EDIT 3/30/18
            // Disconnectimng Population Proportions Code, not currently being used
            //initProportions();
            //  OldPopulation = population;
            // END EDIT
            // This is not being used anymore, references are in methods not being used
            //maxReclaimed = 25;
            // NOTE 3/1/18 QUAY ======================================
            // This reset will also be called by WaterSimManager via WaterSim_Model when simulkation is initialized
            // If you are using the model independet of WaterSimManager you will need to do this ebfore each simulation
            // 02.01.17 added reset below
            ResetVariables();

            // QUAY EDIT 3/30/18
            // Disconnectimng Population Proportions Code, not currently being used
            // populationRatio = 1;
            // END EDIT
            // ================================================

            Desalinization = 1;

            // EDiT 3/5/18 QUAY
            // THis was all moved to an annual basis/ bacuase Conservation values could be change inbeteen years
            // Hoever, MinUrbanGPCDPercent has to stay here becuase it uses the initial urban GPC and is a static value
            //// EDIT QUAY 3/1/2018
            //// Changed to use Change COef
            //// AgricultureTargetGPDD = 1;
            //// This calcuates Change Coef for Urban GPCD changes (ie conservation)
            //double UrbanGPCDTargetReduction = UrbanConservation;
            //double YearsToTarget = (EndYear - StartYear);

            FMinUrbanGPCDPercent = (_MinimumUrbanGPCD / urbanGPCD);
            //UrbanGPCDChangeCoef = EstimateChangeCoef(UrbanGPCDTargetReduction, MinUrbanGPCDPercent, YearsToTarget);
            //// This calculates Change Coef for Ag GPCD
            //double AgGPDDTargetReduction = AgConservation;
            //YearsToTarget = (EndYear - StartYear);
            //AgGPDDChangeCoef = EstimateChangeCoef(AgGPDDTargetReduction, MinAgGPDDPercent, YearsToTarget);
            //// 03.12.17 das added because the dampener was not working. Note this has been changed
            //// END EDIT 3/1/18
            // END EDIT 3/5/18


            // power 
            // Set up the Power Base cooeffcients

            // Set Initial Power
            GetInitialPower();
            // Intital Gallons per mega watt of power per day
            // Got to check this!
            if (initialPower > 0)
            {
                double Temp = UnitNetwork.Power.Demand;
                FInitialGPMWD = (UnitNetwork.Power.Demand * convertDemand) / (initialPower);
                // probably could get by with out this multipliction, but is only done when obejct created so better to be safe than fast
            }

            // OK give the model this initial value
            seti_GallonsPerMegaWattPerDay((int)FInitialGPMWD);

            // Initial watt per person 
            // Got to Check this!
            if (population > 0)
            {
                FInitialWattPerPerson = (initialPower * convertPower) / population;
            }
            // ok give the model this initial value
            seti_WattsPerPerson((int)FInitialWattPerPerson);


            // power
            // NOTE QUAY 3/11/18
            // We do not have industrial employees yet, this is a proxy for Industrial employees
            // using total demand / 100 for total employees assuming that Gallons Per Employee per Day is 100.
            // There is one of the west regions that has 0 MGD demand for  Industry.  
            // So it will have 0 employees, will have to watch for this
            //
            FIndEmployees = (UnitNetwork.Industrial.Demand * convertDemand) / (100.0 * convertEmployee);
            FIndInitialEmploy = FIndEmployees;
            // calculate IndGPED
            if (FIndEmployees > 0) FIndGPED = (UnitNetwork.Industrial.Demand * convertDemand) / (FIndEmployees * convertEmployee);
            // Set Initial Industrial Galloans per Employee per day
            FIndInitialGPED = FIndGPED;

            // EDIT QUAY 4/2/18
            // This is the BaseSurface water used to allocate annual amounts of surface water
            FBaseSurfaceWater = UnitNetwork.SurfaceFresh.Limit;
            // This is the Base ground water used to allocate annual amounts of groundwater
            FBaseGW = UnitNetwork.Groundwater.Limit;

            // EDIT QUAY 9/12/18
            // Adding for colorado
            // 
            FBaseColorado = UnitNetwork.Colorado.Limit;
            // end edit

        }


        // QUAY EDIT 3/18/18
        // These methods and properties are not being used.

        ///// <summary> Zero-based index of the year.</summary>
        //public int FYearIndex = 0;

        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Seti year index.</summary>
        ///// <param name="value"> The value.</param>
        /////-------------------------------------------------------------------------------------------------

        //public void seti_YearIndex(int value)
        //{
        //    if ((value > -1) && (value < 51))
        //    {
        //        FYearIndex = value;
        //    }
        //}

        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Geti year index.</summary>
        ///// <returns> An int.</returns>
        /////-------------------------------------------------------------------------------------------------

        //public int geti_YearIndex()
        //{
        //    return FYearIndex;
        //}
        // END EDIT 3/18/18

        // -------------------------
        // Reset Network and variables
        #region Reset Network and variables

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the network.</summary>
        /// <remarks> This routine is called during the Simulation_Inityialize method.  IT should be used to prepare the
        ///           model for a simulation run, right nowit does the following
        ///             1) resets all dynamic model variables to their default vaules,  
        ///             2) calls the RunCallback routine to inidcate the model has been reset  
        ///             
        ///           NOTE: The generic model does not keep track of this.  It assumes that all variables have been reset before first run year,
        ///           this is done in WaterSimManager.Simulation_Initialize(). 
        ///           As long as the WaterSimManger SimulationInitialize, SimulaytionRun, and SimulationStop are being used this is the case.
        ///           However, if the model is being used as a seperate independent model, then this method should be called before each new run 
        ///           of a siumulation using the model occurs
        ///           </remarks>
        /// <seealso cref="WaterSimManager.Simulation_Initialize()"/>
        ///-------------------------------------------------------------------------------------------------

        public void ResetNetwork()
        {
            // QUAY EDIT 2 19 18
            // The State Index is no longer being set dynamically, if the model is used as an isolated model, ie not with WaterSim_Model
            // Then this should be explicitely should be called after the Unitname or state index is changed.  This routin should be used to reset the 
            // model, not be the way to assign the state, so I have removed that
            // NOTE:  HOWEVER!!!!! The Network does need to be rebuilt, and changing the Unitname does that, and it is the only way to do that without
            // recreating the whole object.  We should have a method in the CRFNetwork  than will rebuild the model, using its base configuration.  
            // For now I am leaving the ChangeUnit Here here becuase it must be done before each simulation

            UnitNetwork.ChangeUnit(FUnitName);
            // seti_StateIndex(FStateIndex);
            // End EDIT 2 19 18

            ResetVariables();
            if (FRunCallBack != null)
            {
                FRunCallBack(-1);


            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the variables.</summary>
        /// <remarks> This is called by Newtwork reset, should initialize all values to default and startup values.
        ///           This occurs at SimuationInitialize, called only once for each model
        ///           
        ///           </remarks>
        /// <seealso cref="ResetNetwork()"/>
        ///-------------------------------------------------------------------------------------------------

        public void ResetVariables()
        {
            // 
            int Hundred = 100;
            int zero = 0;
            //
            // QUAY EDIT 3/5/2018
            // CHANGED THIS TO USE A DEAFULT REDUCTION OF 1% per year, ie 0.99
            // seti_PowerConservation(Hundred);
            //seti_AgConservation(Hundred);

            // seti_UrbanConservation(temp);
            // seti_AgConservation(temp);
            seti_AgConservation((int)(_DefaultAgGPDDAdjust * 100));
            seti_UrbanConservation((int)(_DefaultUrGPCDAdjust * 100));
            seti_PowerConservation((int)(_DefaultPwGPWDAdjust * 100));
            // END EDIT 3/5/2018
            // QUAY 3/29/18
            // OPPS forgot industry
            seti_IndustryConservation((int)(_DefaultIndGPEDAdjust * 100));
            // END EDIT 3/29/18

            // QUAY EDIT 3/30/28
            // YIKES MISSED THIS ONE AS WELL
            seti_PopGRateModifier((int)_DefaultPopGrowthMode * 100);
            // END EDIT 3/30/18

            // QUAY EDIT 3/5/2018
            // OUCH DO NOT DO THIS HERE, this gets set at object creation
            // Will need to think about this. deleted for now
            //seti_PopGrowthRate(temp);
            // END EDIT
            //
            seti_SurfaceWaterControl(Hundred);
            seti_GroundwaterControl(Hundred);

            // EDIT QUAY 3/22/18
            // Why is this being done, this is a resource.  Need to leave this alone
            // I commented this out
            //seti_Effluent(zero);
            // END EDIT 3/22/18

            // EDIT QUAY 3/22/18
            // Lets not set this to zero, we need to get an initial State for based on potential reclaimed the initial values in Effluent.Limit
            // seti_ReclaimedWaterManagement(zero));
            double PercentReclaimed = 0;
            double TheMaxReclaimed = MaxReclaimed();
            if (TheMaxReclaimed > 0) PercentReclaimed = UnitNetwork.Effluent.Limit / MaxReclaimed();
            //
            defaultPCTReclaimed = 0;
            defaultPCTReclaimed = PercentReclaimed*100;
            seti_ReclaimedWaterManagement((int)(PercentReclaimed * 100));
           // sampson edits.. I do not know why Ray is doing this.. need to straighten this out.
           // seti_ReclaimedWaterManagement((int)(zero));
            // END EDIT 3/22/18
            // 
            seti_LakeWaterManagement(Hundred);
            seti_Desalinization(Hundred);
            //
            // NOTE QUAY 3/9/18
            // I left this in here, but I do not think it is being used now
            // 08.03.17 das
            OldPopulation = 0;
            // EDN NOTE
            // 

            // QUAY EDIT 3/9/18
            // Moved this to firstpreprocess()
            //InitialPower(FUnitName);
            // END EDIT 3/9/18
            seti_PowerEnergy(initialPower);
            invokeEffluent = true;

            // EDIT QUAY 3/5/18
            // MOVED THIS, initialize base fields does not need to be done with each reset, just when model object is created
            // QUAY 2/6/17
            //SetInitialRates();
            //SetInitialDemandFactors();
            // END EDIT

            // Reset Drought to defaults
            FDroughtDepth = DefaultDroughtDepth;
            FDroughtLength = DefaultDroughtEnd - DefaultDroughtStart;
            FDroughtStartYear = DefaultDroughtStart;
            FDroughtActive = DefaultDroughtActive;

            // Reset Caplimits and resources reset to defaults
            FCapLimitsToDemand = FDefaultCapLimitsToDemand;
            FReviseResources = FDefaultReviseResources;

            // QUAY EDIT 9/13/18
            // ===================================================================
            // CLIMATE CHANGE
            // ===================================================================
            // 
            FCCChangeCoefficient = DefaultCCChangeCoefficient;

            // this is the field used for change coeeficient, and is the Max or Min values based on desired goal
            FCCChangeLimit = DefaultCCChangeLimit;

            FCCChangeTarget = DefaultCCChangeTarget;
            FCCYearTarget = DefaultCCYearTarget;
            // END EDIT

            // sampson edits 09.06.18
            seti_GrayWaterManagement(zero);
            // end sampson edits 09.06.18
        }
        double _defaultPCTreclaimed = 0;
        double defaultPCTReclaimed
        {
            get { return _defaultPCTreclaimed; }
            set { _defaultPCTreclaimed = value; }

        }
        #endregion
        //
        // ====================================================================================
        // MODEL
        //

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes the one year operation of the model.</summary>
        /// <param name="year"> The year.</param>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int runOneYear(int year)
        {
            currentYear = year;

            // Run the model for one year based on current factors
            Model(year);
            // Model clean up, memory , handles etc.
            cleanUp(year);
            // Has run one year, CallBack if set
            if (FRunCallBack != null)
            {
                FRunCallBack(year);
            }
            
            return 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Stets up for a year run</summary>
        /// <remarks>  This method should forecast production evels for this year</remarks>
        /// <param name="year"> The year.</param>
        ///-------------------------------------------------------------------------------------------------

        internal void startUpProduction(int year)
        {
            // EDIT 3/1/18 QUAY
            // Modified to set population here instead of in Calcualte
            // I did this becuase both urban and power use these pop numbers, moved to here so it would be 
            // independent of both of these consumers
            // 09.12.16
            // CalculateNewPopulation(year, StartYear);
            population = CalculateNewPopulation(year, StartYear);
            // END EDIT
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Clean up at the end of end of each year run.</summary>
        /// <remarks> Does not do anything now except at the last year, but anything that has to be disposed or flushed at the
        ///           end of year model run should be done here</remarks>
        /// <param name="year"> The year.</param>
        ///-------------------------------------------------------------------------------------------------

        internal void cleanUp(int year)
        {
            if (year == endYear)
            {
                //int temp = geti_NetDemandDifference();
                tearDown(year);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Tear down at end of year run.</summary>
        /// <remarks> Does not do anything now, but anything that has to be disposed or flushed at the
        ///           end of the last year, or end of simulation should be done here</remarks>
        /// <param name="year"> The year.</param>
        /// <seealso cref="cleanUp(int)"/>
        ///-------------------------------------------------------------------------------------------------

        internal void tearDown(int year)
        {
            //sw.Flush();
            // sw.Close();
        }
        // ===========================
        //
        #region Model kernel Calls

        //  Pretty Obvious
        // CLEAN UP int baseyear = 2015;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Model Kernal</summary>
        /// <remarks>  This is the kernal of the model, all functions of the model are called in this mehod for each year.</remarks>
        /// <param name="year"> The year.</param>
        ///-------------------------------------------------------------------------------------------------

        internal void Model(int year)
        {
            // QUAY EDIT 3/18/18
            // This is not doing anything or used by any method, several methods and properties also commented out
            //seti_YearIndex(year - baseyear);

            // Also , restrucured this code to reflectnew system, renamed soem functions to reflect what they are doing, added comments
            // End Edit

            currentYear = year;

            // EDIT QUAY 3/30/18
            // Now that the proportional population methods have been disconnected  this method does not do anything
            // AND both of these IntializeRUn and preProcess basically are things done before the model runs so they both are not needed.
            // So commented this out
            //initializeRun();
            // END EDIT

            // This method is to prepare vaious coefficients that change each year.  
            // In this case there are growth factors be calculated for this period
            preProcess(year);

            // EDIT QUAY 4/2/18
            // OK, the original method of changing resources levels is no longer going work given how the web interface is
            // working.  We can not just let the Resource  
            // going to work.  There has to be a Resource potential, that can potentially be allocaed each year
            // This method manages the resources for this period
            annual_Resources();

            // This runs the consumer routines to resestimate production and demand
            annual_Consumers();

            // QUAY EDIT 2/18/18
            // Added this optimization, needs to be done after resource sna consumers have been modified
            // to optmize the network, this can effect both resources, fluxes, and consumers
            UnitNetwork.optimize();
            // END EDIT 2/18/18

            // EDIT QUAY 3/31/18
            // This is not doing anything now sdo I removed it
            // Analyzing results should probably be donw at the end of the runoneyear() method
            // analyze_results();
            // END EDIT 3/31/18

            // This metyhod does tasks that need done after the resources, consumers, and network optimization has occured.
            postProces(year);

            // EDIT QUAY 3/31/18
            // I reorganized and remaned these method calls
            // currentYear = year;
            // initializeRun();
            // preProcess(year);
            // annual_delta();

            // initializeNetwork();
            // analyze_results();
            // postProces(year);
            //  End Edit 3/31/18           
        }

        // EDIT QUAY 3/30/18
        // Removed this routine as reducndant, pre model task are done in preprocess()
        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Initializes the Model run for a year.</summary>
        /////-------------------------------------------------------------------------------------------------

        //void initializeRun()
        //{
        //    //QUAY EDIT 3/30/18
        //    // DISCONNECTING Population Proportion Code, demand_total is not part of  that but it is not referenced for a get by any code, old or new
        //    //populationRatio = 0;
        //    //demand_total = 0;
        //    //initProportions();
        //    // End Edit
        //}
        // EDN EDIT 3/30/18

        // EDIT QUAY 3/31/18
        // I renamed this in order to have the name convey waht is happening in this methpod
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Annual Resource Management</summary>
        /// <remarks>  This calls the resource routines to manage water supply
        ///            if a water supply is added , a call to its manager must be placed here
        ///            </remarks>
        /// <seealso cref="surfaceFresh()"/>
        /// <seealso cref="surfaceSaline()"/>
        /// <seealso cref="surfaceLake()"/>
        /// <seealso cref="groundwater()"/>
        /// <seealso cref="effluent()"/>
        ///-------------------------------------------------------------------------------------------------

        //internal void annual_delta()
        internal void annual_Resources()

        {
            surfaceFresh();
            surfaceSaline();
            surfaceLake();
            groundwater();
            effluent();
        }
        // EDIT QUAY 3/31/18
        // I renamed this in order to have the name convey waht is happening in this methpod
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Does Consumer Management for this period.</summary>
        /// <remarks>  This calls the consumer methods to manage water demand
        ///            if a water consumer is added , a call to its manager must be placed here
        ///            </remarks>
        /// <seealso cref="Urban()"/>
        /// <seealso cref="Agriculture()"/>
        /// <seealso cref="Power()"/>
        ///
        /// <seealso cref="Industrial()"/>
        ///-------------------------------------------------------------------------------------------------

        //internal void Run_Network_Model()
        internal void annual_Consumers()
        {
            TemperatureData(FDCtemperature, this);
            Urban();
            calculateGrayWater();
            //
            Agriculture();
            // EDIT QUAY 3/9/18
            // This is now unified in a single methods, similar to other consumers
            //PowerWater();
            //PowerEnergy();
            Power();
            // END EDIT 3/9/18
            Industrial();
            //

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Pre process tasks.</summary>
        /// <remarks>  Sets up fields that may change each year that are need for the consumer and resource management and
        ///            other model functions</remarks>
        /// <param name="yr"> The year.</param>
        /// <see cref="Urban()"/>
        /// <see cref="Agriculture()"/>
        /// <see cref="Industrial()"/>
        /// <see cref="Power()"/>
        /// <see cref="surfaceFresh()"/>
        /// <see cref="groundwater()"/>
        ///-------------------------------------------------------------------------------------------------
        // 12.19.16 das following code
        // ---------------------------
        void preProcess(int yr)
        {

            // EDIT QUAY 4/3/18
            // This was moved here to enforce a concept.  The model() method shold be the model.
            // Nothing considered part of the model should be outside the model() method.  This should be 
            // the first thing the model does, but it should be inside the model method.

            // Estimate production levls for this year
            startUpProduction(yr);

            // EDIT QUAY 3/5/2018
            // This was moved to here becuase these could change each year
            // This calcuates Change Coef for Urban GPCD changes (ie conservation)
            // NOTE:  This was set to be from Start to Endyear, So Technically if the
            // Conservation goals are not changed between years of a simulation, these values 
            // should be exactly the same for every annula run.  However, if they are changed between
            // years, new changecoef will be calculated.  The FLAW in this is that the changecoef
            // will bedesigned for a curve from StartYear to EndYear.  If it changes midway, the result values 
            // could be radically different resulting a abrupt change in the year the conservation or Gallons per
            // are changed, that could be a problem.  One way would be to change the BaseValue of the Gallons Per UNit
            // factor to be the actual Gallons Per unit in the year of change.  But right now, that is not folded into the
            // set methods for the conservation factors.
            // 

            //---------------------------------------------------
            // This caluclates change coef for Urban GOCD
            double YearsToTarget = (EndYear - startYear);
            double UrbanGPCDTargetReduction = UrbanConservation;
            //FUrbanGPCDChangeCoef = EstimateChangeCoef(UrbanGPCDTargetReduction, FMinUrbanGPCDPercent, YearsToTarget);
            FUrbanGPCDChangeCoef = utilities.ExponentialDecayCoef(UrbanGPCDTargetReduction, 1, FMinUrbanGPCDPercent, 0, YearsToTarget);

            //---------------------------------------------------
            // This calculates Change Coef for Ag GPCD
            double AgGPDDTargetReduction = AgConservation;
            YearsToTarget = (EndYear - StartYear);
            //AgGPDDChangeCoef = EstimateChangeCoef(AgGPDDTargetReduction, FMinAgGPDDPercent, YearsToTarget);
            AgGPDDChangeCoef = utilities.ExponentialDecayCoef(AgGPDDTargetReduction, 1, FMinAgGPDDPercent, 0, YearsToTarget);

            //---------------------------------------------------
            // This calculates Change Coef for Industry
            double IndGPEDTargetReduction = IndustryConservation;
            YearsToTarget = (EndYear - StartYear);
            //FindGPEDChangeCoef = EstimateChangeCoef(IndGPEDTargetReduction, FIndMinGPEDPercent, YearsToTarget);
            FindGPEDChangeCoef = utilities.ExponentialDecayCoef(IndGPEDTargetReduction, 1, FIndMinGPEDPercent, 0, YearsToTarget);

            //---------------------------------------------------
            // this calcualted Change Coef for Power
            double PwGPMWDTargetReduction = PowerConservation;
            YearsToTarget = (EndYear - StartYear);
            //FPwGPMWChangeCoef = EstimateChangeCoef(PwGPMWDTargetReduction, FMinPwGPMWDPercent, YearsToTarget);
            FPwGPMWChangeCoef = utilities.ExponentialDecayCoef(PwGPMWDTargetReduction, 1, FMinPwGPMWDPercent, 0, YearsToTarget);
            // END EDIT

            // Edit Sampson
            // 06.27.18
            //---------------------------------------------------
            // This caluclates change coef for Urban LCLU demand
            YearsToTarget = (EndYear - startYear);
            double UrbanLCLUTargetReduction = UrbanConservation;
            FUrbanLCLUChangeCoef = utilities.ExponentialDecayCoef(UrbanLCLUTargetReduction, 1, FMinUrbanLCLU, 0, YearsToTarget);

            //---------------------------------------------------
            // This caluclates change coef for Ag LCLU demand
            YearsToTarget = (EndYear - startYear);
            double AgLCLUTargetReduction = AgConservation;
            FAgLCLUChangeCoef = utilities.ExponentialDecayCoef(AgLCLUTargetReduction, 1, FMinAgLCLU, 0, YearsToTarget);

            //---------------------------------------------------
            // This caluclates change coef for Ag LCLU demand
            YearsToTarget = (EndYear - startYear);
            double IndustryLCLUTargetReduction = IndustryConservation;
            FIndustryLCLUChangeCoef = utilities.ExponentialDecayCoef(IndustryLCLUTargetReduction, 1, FMinIndustryLCLU, 0, YearsToTarget);
            // End Edit Sampson 06.27.18


            // EDIT QUAY 4/2/18
            //---------------------------------------------------
            // Calculates the Surface Water Change Coefficients
            YearsToTarget = (EndYear - StartYear);
            double TheSurfaceGoal = (double)geti_SurfaceWaterControl() / 100;

            // set the surface change limit need for ChangeINcrement method
            // Essentially if declining (d) then lower limit (l) and if increasing )i) set upper limit (u)
            // v
            // v uuuuuuuuuuuuuuuuuuuuuuiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii
            // v                i
            // v        i
            // v   i 
            // v x 
            // v    d
            // v         d
            // v                 d
            // v lllllllllllllllllllllldddddddddddddddddddddddddddddddddddd
            // v 
            // 
            if (TheSurfaceGoal <1)
            {
                FSurfaceChangeLimit = MinSurfaceChange;
            }
            else
            {
                FSurfaceChangeLimit = MaxSurfaceChange;
            }
            // Calculate the Coef
            // FSurfaceChangeCoefficient = EstimateChangeCoef(TheSurfaceGoal, FSurfaceChangeLimit, YearsToTarget);
            FSurfaceChangeCoefficient = utilities.ExponentialDecayCoef(TheSurfaceGoal, 1, FSurfaceChangeLimit, 0, YearsToTarget);

            //---------------------------------------------------
            // Calculates the groundwater change coefficients
            YearsToTarget = (EndYear - StartYear);
            double TheGWGoal = (double)geti_GroundwaterControl() / 100;

            // set the surface change limit need for ChangeINcrement method
            if (TheGWGoal < 1)
            {
                FGWChangeLimit = MinGWChange;
            }
            else
            {
                FGWChangeLimit = MaxGWChange;
            }
            // Calculate the Coef

            //FGWChangeCoefficient = EstimateChangeCoef(TheGWGoal, FGWChangeLimit, YearsToTarget);
            FGWChangeCoefficient = utilities.ExponentialDecayCoef(TheGWGoal, 1, FGWChangeLimit, 0, YearsToTarget);
            // END EDIT

            // EDIT QUAY 9/13/18
            //---------------------------------------------------
            // Calculates CLIMATE CHANGE coeeficients

            if (FCCChangeTarget < 1)
            {
               FCCChangeLimit =  MinCCChange;
            }
            else
            {
                FCCChangeLimit = MaxCCChange;
            }
            YearsToTarget = (FCCYearTarget - StartYear);
            // Change Coeficient
            FCCChangeCoefficient = utilities.ExponentialDecayCoef(FCCChangeTarget, 1, FCCChangeLimit, 0, YearsToTarget);
            // END EDIT


            // EDIT QUAY 3/31/18
            // We should develop a more reobust way to do this for multiple policies each with different triggers and actions
            // For now I commented this out, along with other policy code.
            //// 
            //// 12.19.16 added
            //if (yr == policyStartYear)
            //    invokePolicies = true;
            // END EDIT

            // QUAY EDIT 4/2/18
            // Here is a start on this issue of how policies need to be implemented

         }
        //

        // =====================================================================================================================
        // =====================================================================
        // Properties to pass variables to other Classes for Demand Estimation
        //
        //
        //

        // Added on 06.14.18 by DAS. To pass to the UrbanDemand_GPCD class
        // =====================================================================
        /// <summary>
        /// 
        /// </summary>
        public double PUrbanGPCDChangeCoef
        {
            get { return FUrbanGPCDChangeCoef; }
        }
        // ---------------------------
        // das 06.13.18
        /// <summary>
        /// 
        /// </summary>
        public double PMinUrbanGPCDPercent
        {
            get { return FMinUrbanGPCDPercent; }

        }
        public double PFBaseUrbanGPCD
        {
            get { return FBaseUrbanGPCD; }

        }
        //
        // Added on 06.14.18 by DAS. To pass to the AgriculturalDemand_income class
        // ========================================================================

        public double PAgGPDDChangeCoef
        {
            get { return AgGPDDChangeCoef; }
        }


        // Added on 06.14.18 by DAS. To pass to the PowerDemand_wp
        // ========================================================================
        public double PPwGPMWChangeCoef
        {
            get { return FPwGPMWChangeCoef; }
        }
        //
        // NOT SURE THIS WORKS
        public double PGallonsPerMegaWattPerDay
        {
            get { return FGallonsPerMegaWattPerDay; }

        }
        // Added on 06.15.18 by DAS. To pass to the IndustryDemand_employees class
        // ========================================================================
        //
        public double PIndustrialEmployees
        {
            get { return IndustrialEmployees; }
        }
        //
        public double PFIndInitialEmploy
        {
            get { return FIndInitialEmploy; }
        }
        // Initial Variable
        public double PFIndInitialGPED
        {
            get { return FIndInitialGPED; }
        }
        // 
        // FindGPEDChangeCoef 
        public double PFindGPEDChangeCoef
        {
            get { return FindGPEDChangeCoef; }
        }
        // =====================================================================
        // Sampson Edits 06.27.18
        public double PUrbanLCLUChangeCoef
        {
            get { return FUrbanLCLUChangeCoef; }
        }
        public double PAgLCLUChangeCoef
        {
            get { return FAgLCLUChangeCoef; }
        }
        public double PIndustryLCLUChangeCoef
        {
            get { return FIndustryLCLUChangeCoef; }
        }



        public double PminUrbanLCLU
        {
            get { return FMinUrbanLCLU; }
        }
        public double PminAgLCLU
        {
            get { return FMinAgLCLU; }
        }
        public double PminIndustryLCLU
        {
            get { return FMinIndustryLCLU; }
        }
        // End Sampson Edits 06.27.18

        // 
        // =====================================================================================================================


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Post process tasks.</summary>
        /// <remarks>  This method is used to conduct tasks that must be done before the end of the simulation for this
        ///            year, and before a postProcess() of the Simulation.</remarks> 
        /// <param name="yr"> The year.</param>
        ///-------------------------------------------------------------------------------------------------

        void postProces(int yr)
        {
            // EDIT QUAY 4/3/18
            // OK, first we need to Cap the Resource Limits in case more resources
            // was allocated than is needed
            if (FCapLimitsToDemand)
            {
                TheCRFNetwork.CapLimitToDemand();

                // NOW The control that over allocated this resource for this year, is still set to its goal.
                // We do not want to change that goal and let it try and allocate more next year until it reaches its
                // goal.  However, at the end of the simulation we do not want to return this value, if the goal was not reached.
                // So on the last year, we want to see what the goal would have been, check if that was allocated, if not
                // then change this to what goal (percent) was achieved. Thus if the model is run again, with this new
                // modified goal, it will achive the same value, rather than a different value becuase other parameters influenced it ability
                // to achive that goal.  
                //  How the hell do we do that?
                //  THis could be done at the end of the runoneyear() method, philosophy wise when we leave the model() method,
                //  the model should be done, and all values final.
                //  
                //  So I am considering the resetting of the resource control parameters as part of the model, and not a post model process
                //  by testing for yr, we can have the last action performed by the model after  the last year
                //
                // This code will revise the resource control values to macth the last year end state values
                if ((FDefaultReviseResources) && (yr == EndYear))
                {
                    // OK we are in the last year

                    // do surface water control
                    ReviseSurfaceControl();

                    // do groundwater control
                    ReviseGroundwaterControl();

                    // No need to do reclaimed!
                }
            }
            // QUAY EDIT 3/30/18
            // Removed the proprtion population references
            //int temp = 0;
            //temp = population;
            //if (yr == startYear)
            //{

            //    //   startPop = temp;
            //}
            //OldPopulation = temp;
            // EDN EDIT 3/30/18

            // EDIT QUAY 3/30/18
            // THis was being used with policies, should develop a more robust sstem for policies
            //reset_Drivers(yr);
            // END EDIT 3/30/18
            //invokeEffluent = false; // One year - 2015, where the value of the added effluent is set

        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Analyze results.</summary>
        /// <remarks> Grabs results</remarks>
        ///-------------------------------------------------------------------------------------------------

        internal void analyze_results()
        {
            // EDIT QUAY 3/30/18
            // Disconnecting Proportional Population Code
            // These fields are not referenced in any old or new code, the GOCD function setting these is part of the Proportional Population Code
            //int GPCDurban = urbanGPCD;
            //int GPCDindustry = industrialGPCD;
            //int GPCDag = agriculturalGPCD;
            //int GPCDpower = powerGPCD;
            // END EDIT
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the drivers if policy start years</summary>
        /// <param name="yr"> The year.</param>
        ///-------------------------------------------------------------------------------------------------

        internal void reset_Drivers(int yr)
        {
            if (yr == policyStartYear)
            {
                int one = 100;
                int zero = 0;
                seti_SurfaceWaterControl(one);
                seti_GroundwaterControl(one);
                // EDIT QUAY 3/22/18
                // SAME WHY IS THIS BEUNG DONE THIS IS A RESOURCE
                // I commented this out
                //seti_Effluent(zero);
                // END EDIT 3/22/18
                seti_LakeWaterManagement(one);
                seti_Desalinization(one);
            }
        }

        #endregion

        #endregion Model_Control

        //
        // =============================================================================================
        //
        // ---------------------------------------------------------------------------------------------
        // Resources - State definitions and management actions on resources
        // ==================================================================
        #region Resources

        // EDIT QUAY 4/2/18
        // A New System for managing Surface Water and Groundwater was developed.  It is based on the same approach 
        // that consumers are managed.  A base resource is identified at the beginning of the scenario.  This base resource 
        // remains the same throughout all years of the scenario.  Each years resource Limit that is available is calculated 
        // by modifying the base resource using the values in the resource controls as goals.  Similar to the consumers
        // chnage coefficients are estimated each year using the resource control as a goal.  These change coeficients are then multiplied 
        // by the base value.
        // Lots of changes, I did not mark most of these.


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface freshwater Management .</summary>
        ///-------------------------------------------------------------------------------------------------
        // The Surface change coefficient
        double FSurfaceChangeCoefficient = 1;

        // The min and max changes allowed for surface water
        const double MaxSurfaceChange = 2.0;
        const double MinSurfaceChange = 0.10;

        // this is the field used for change coeeficient, and is the Max or Min values based on desired goal
        double FSurfaceChangeLimit = 1;

        // this is the base surface water value used to allocate annual surface water
        double FBaseSurfaceWater = 0.0;
        // EDIT 9/12/18
        // Adding this for Colorado
        double FBaseColorado = 0.0;
        // END EDIT

        // this is used by the drought formulas to recover from drought
        double InitialSUrfaceAtDrought = 0;

        // QUAY EDIT 3/23/18
        // Modified Drought

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surfacewater management.</summary>
        ///
        /// <remarks> This method manages the annual allocation of surface water.</remarks>
        ///-------------------------------------------------------------------------------------------------

        void surfaceFresh()
        {
// EDIT QUAY 9/13/18
// This was confusing, this code does not have climate change
// O modeved the code with climate change in here, but saved by commenting out the old code
/*
            double NewSurfaceLimit = 0;
            int result = 0;
            // EDIT QUAY 4/2/18
            // This is the change in how the resources are managed.  Each policy should have its own set of factors
            // this one is for the surface water manaagement control
            double tempBase = 0;
            int SurfacePeriod = currentYear - startYear;
            //double annualFactor =  ChangeIncrement(1, SurfacePeriod, FSurfaceChangeCoefficient, FSurfaceChangeLimit);
            double annualFactor = AnnualExponentialChange(1, SurfacePeriod, FSurfaceChangeCoefficient, FSurfaceChangeLimit);
            tempBase = annualFactor * FBaseSurfaceWater;
            //int one = 100;
            //tempBase = geti_SurfaceWaterFresh() * d_surfaceWaterControl;
            // end edit

            // EDIT QUAY
            // THis is the new drought management
            NewSurfaceLimit = tempBase;
            if (FDroughtActive > 0)
            {
                if (FDroughtStartYear <= currentYear)
                {
                    if (FDroughtStartYear == currentYear)
                    {
                        InitialSUrfaceAtDrought = tempBase;
                    }
                    int period = currentYear - FDroughtStartYear;
                    int StartPeriod = FDroughtStartYear - StartYear;
                    int SimulationYears = EndYear - startYear;
                    double DroughtEffect = DroughtFunction(period, StartPeriod, SimulationYears, FDroughtLength, FDroughtDepth);
                    NewSurfaceLimit = InitialSUrfaceAtDrought * DroughtEffect;
                }
            }
*/


            double NewSurfaceLimit = 0;
            int result = 0;
            // EDIT QUAY 4/2/18
            // This is the change in how the resources are managed.  Each policy should have its own set of factors


            // This is the cumulative effect variable
            double tempBase = 0;
            // This is how far into the simulation we are
            int SurfacePeriod = currentYear - startYear;

            // this one is for the surface water manaagement control
            //double annualFactor =  ChangeIncrement(1, SurfacePeriod, FSurfaceChangeCoefficient, FSurfaceChangeLimit);
            double annualFactor = AnnualExponentialChange(1, SurfacePeriod, FSurfaceChangeCoefficient, FSurfaceChangeLimit);
            tempBase = annualFactor * FBaseSurfaceWater;

            //int one = 100;
            //tempBase = geti_SurfaceWaterFresh() * d_surfaceWaterControl;
            // end edit
           

            // This one is the climate change Management
            // Climate Change is affects the values after the surface management has been applied
            if (FCCChangeTarget!=1.0)
            {
                double CCannualFactor = AnnualExponentialChange(1, SurfacePeriod, FCCChangeCoefficient, FCCChangeLimit);
                tempBase = tempBase * CCannualFactor;
            }

            // THis is the new drought management
            // drought is applied on top of Climate Change and Surface ater control.
            if (FDroughtActive > 0)
            {
                if (FDroughtStartYear <= currentYear)
                {
                    int period = currentYear - FDroughtStartYear;
                    int StartPeriod = FDroughtStartYear - StartYear;
                    int SimulationYears = EndYear - startYear;
                    double DroughtEffect = DroughtFunction(period, StartPeriod, SimulationYears, FDroughtLength, FDroughtDepth);
                    tempBase = tempBase * DroughtEffect;
                }
            }
// END EDIT 9/13/18

            // OK, after cascading imapcts, finally set the new surface water limit
            NewSurfaceLimit = tempBase;


            //if (startDroughtYear <= currentYear)
            //    temp = geti_SurfaceWaterFresh() * d_surfaceWaterControl * d_drought;
            // End EDIT

            result = Convert.ToInt32(NewSurfaceLimit);
            seti_SurfaceWaterFresh(result);
            //if (startDroughtYear <= currentYear) seti_DroughtControl(one);
        }

 // QUAY EDIT 9/12/18
        // Add Colordo Water

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface colorado.</summary>
        ///
        /// <remarks> Quay, 9/12/2018.
        ///           This manages annually the Colorado Surface Water.
        ///           This should inlcude special shortage conditions, normal drought, and climate change
        ///           Right now it is responding excatly as surface water does
        ///           </remarks>
        ///-------------------------------------------------------------------------------------------------

        void surfaceColorado()
        {
            double NewSurfaceLimit = 0;
            int result = 0;

            // This is the cumulative effect variable
            double tempBase = 0;
            // This is how far into the simulation we are
            int SurfacePeriod = currentYear - startYear;

            // this one is for the surface water manaagement control
            // We will need to decide what to use here?
            // I am temprorarily NOT increasing this base on the control
            //double annualFactor = AnnualExponentialChange(1, SurfacePeriod, FSurfaceChangeCoefficient, FSurfaceChangeLimit);
            //tempBase = annualFactor * FBaseColorado; // FBaseSurfaceWater;

            tempBase = FBaseColorado; // FBaseSurfaceWater;


            // This one is the climate change Management
            // Climate Change is affects the values after the surface management has been applied
            if (FCCChangeTarget != 1.0)
            {
                double CCannualFactor = AnnualExponentialChange(1, SurfacePeriod, FCCChangeCoefficient, FCCChangeLimit);
                tempBase = tempBase * CCannualFactor;
            }

            // THis is the new drought management
            // drought is applied on top of Climate Change and Surface ater control.
            if (FDroughtActive > 0)
            {
                if (FDroughtStartYear <= currentYear)
                {
                    int period = currentYear - FDroughtStartYear;
                    int StartPeriod = FDroughtStartYear - StartYear;
                    int SimulationYears = EndYear - startYear;
                    double DroughtEffect = DroughtFunction(period, StartPeriod, SimulationYears, FDroughtLength, FDroughtDepth);
                    tempBase = tempBase * DroughtEffect;
                }
            }

            // OK, after cascading imapcts, finally set the new surface water limit
            NewSurfaceLimit = tempBase;

            result = Convert.ToInt32(NewSurfaceLimit);
            seti_SurfaceColorado(result);

        }

        // END EDIT 9/12
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface saline water management.</summary>
        ///-------------------------------------------------------------------------------------------------

        void surfaceSaline()
        {
            double temp = 0;
            int result = 0;
            //temp = geti_SurfaceWaterSaline() * _desalinization;
            temp = geti_SurfaceWaterSaline() * Desalinization;
            result = Convert.ToInt32(temp);
            result = (int)temp;
            seti_SurfaceWaterSaline(result);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Surface lake water management</summary>
        ///-------------------------------------------------------------------------------------------------

        void surfaceLake()
        {
            double temp = 0;
            int result = 0;
            //temp = geti_SurfaceLake() * LWManagement;
            temp = geti_SurfaceLake() * d_lakeWaterManagement;
            result = Convert.ToInt32(temp);
            seti_SurfaceLake(result);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Groundwater management.</summary>
        ///-------------------------------------------------------------------------------------------------

        // The Surface change coefficient
        double FGWChangeCoefficient = 1;

        // The min and max changes allowed for surface water
        const double MaxGWChange = 2.0;
        const double MinGWChange = 0.10;

        // this is the field used for change coeeficient, and is the Max or Min values based on desired goal
        double FGWChangeLimit = 1;

        // this is the base surface water value used to allocate annual surface water
        double FBaseGW = 0.0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Groundwater management</summary>
        /// <remarks>  This method manages the annual allocation of groundwater</remarks>
        ///-------------------------------------------------------------------------------------------------

        void groundwater()
        {
            int result = 0;
            // EDIT QUAY 4/2/18
            // This is the change in how the resources are managed.  Each policy should have its own set of factors
            // this one is for the groundwater water manaagement control
            double tempBase = 0;
            int GWPeriod = currentYear - startYear;
            //double annualFactor = ChangeIncrement(1, GWPeriod, FGWChangeCoefficient, FGWChangeLimit);
            double annualFactor = AnnualExponentialChange(1, GWPeriod, FGWChangeCoefficient, FGWChangeLimit);
            tempBase = annualFactor * FBaseGW;
            result = (int)tempBase;

            // double temp = 0;
            // temp = geti_Groundwater() * d_groundwaterControl;
            // result = (int)temp;
            // end edit

            seti_Groundwater(result);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Effluent management</summary>
        /// <remarks> This gets called each year to establish how much Reclaimed water is available this year</remarks>
        ///-------------------------------------------------------------------------------------------------

        void effluent()
        {

            // QUAY EDIT 3/22/18
            // This was modified to increase relcaimed if 1( effluent is aviable, and 2) The Effluent Managemnt value >0
            double temp = 0;
            //int result = 0;
            temp = AvailableReclaimed();

            //temp = geti_Effluent();
            //result = (int)temp;
            //seti_Effluent(result);
            //seti_Effluent(temp);
            SetReclaimed(temp);
        }
        //
        #endregion
        // ---------------------------------------------------------------------------------------------
        //
        // ---------------------------------------------------------------------------------------------
        // Consumers - State definitions and Management Actions on the consumers
        // =====================================================================
        //

        #region Water Demand


        //=============================================================================
        // Generic Demand Model
        // These routines provide methods that can be used for any Consumer or Resource
        #region Generic_Change_Rate_Models
        // Concepts
        //    Each consumer has a basic unit of measure of consumers (CU), Urban is people, Ag is fram income, 
        //          power is mgeawatts, industry is employees.
        //    Each consumer has an efficiency cooefficient gallons per consumer unit per time (GCU), gallons per capita, g
        //          allons per doallar, gallons per megawatt, gallons per employee
        //    A model is used to forecast chnage over time of consumer units. (CUT)  
        //    A model is used to forecast change in gallons per consumer unit (GCUT)
        //    Demand over time (CDT) is then CUT * GCUT.
        //    A user definable coeefieicnt (ACUT) is used to adjust the projection of CUT
        //    A user definable coorefficeint (AGCUT) is used to adjust the project of GCUT.
        //    Damper functions and ChangeCoef are defined to phase out ACUT and phase in AGCUT, slowly build up over time a phase 
        //        out of ACUT or sloowly build over time a Phase in of AGCUT 
        // =========================================================================================

        /// <summary>
        /// This code returns a number between "depth" and one using a decrementing logistic
        /// curve followed by an incrementing logistic curve;
        /// </summary>
        /// <remarks> DAS 3/23/18 </remarks>
        /// <param name="time">A value between 1 and Totalyears</param>
        /// <param name="StartDate">A value that determines the start date (in years [not othe year, but
        /// the number of years from the start of the simulation] which determinines the beginning of the 
        /// decrement in the scale from one to "depth" using an inverse logistic curve)</param>
        /// <param name="TotalYears">Total number of years of the simulation</param>
        /// <param name="length">The length of the drought (in number of years)</param>
        /// <param name="depth">The depth of the drought- the reduction in flows associate with
        /// the function return value</param>
        /// <returns>final</returns>
        public double DroughtFunction(int time, int StartDate, int TotalYears, int length, double depth)
        {
            double result = 1;
            double span = length - 10;
            double YearsToInflection = StartDate + 5;
            double final = 0;
            double Inverse = 0;
            double Increment = 0;
            //
            if (YearsToInflection >= TotalYears / 2 - 10)
            {
                YearsToInflection = TotalYears / 2 - 10;
            }
            double a = 1 - depth;
            double c = 0.5;
            double b = c * YearsToInflection;
            // Inflection point = b / c  
            // 2.718 is the approximate exponent
            Inverse = 1 - (a / (1 + Math.Exp(b - c * time)));
            final = Inverse;
            if (time > YearsToInflection + span / 2)
            {
                b = c * (span + YearsToInflection);
                Increment = a / (1 + Math.Exp(b - c * time)) + 1 - a;
                final = Increment;
            }
            //
            result = final;
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Dampen rate of growth. </summary>
        /// <remarks>   This function reduces the annual rate of growth based on the period from start 
        ///             of growth to current year.  
        ///             The dampening of the rate increases or decreases based on the damper factor.
        ///             Values less than 1 decrease the rate, values larger than 1 will increase the rate.
        ///             To estmate a damper factor given a desired percent change over a set period use the DampenRate method</remarks>
        /// <param name="rate">     The base annual rate. </param>
        /// <param name="damper">   A Factor for the strength of the dampening.  1 is no dampening, 1.0001 is slight dampening
        ///                         1.1 is really fast, anything larger is insane. </param>
        /// <param name="period">   The period. </param>
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        double Dampen(double rate, double damper, double period)
        {
            double NewRate = 0;
            NewRate = rate * Math.Pow(damper, period);
            return NewRate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Dampen rate.</summary>
        /// <remarks> Quay, 3/4/2018.</remarks>
        /// <remarks>  This estimates a Damper factor to use with the Dampen function
        ///            This will return a Damper factor that will acheive the Percentchange in periods time.</remarks>
        /// <param name="PercentChange"> The percent change.</param>
        /// <param name="periods">       The periods.</param>
        /// <seealso cref="Dampen(double, double, double)"/>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double DampenRate(double PercentChange, double periods)
        {
            double DR = Math.Pow(PercentChange, 1 / periods);
            return DR;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Estimate change coef.</summary>
        ///
        /// <remarks> Quay, 3/5/2018.</remarks>
        /// <remarks> This method generates a coeefieicnt that can be used with ChangeIncrement(), Target is a goal and this method generates an estimate to achive target in periodsToTarget.
        ///           which will flatten out at Limit.  It is an estimate and has a tendency to achive target faster than Periods.</remarks>
        /// <param name="TargetPercentGoal">        Target for the change expressed as a percent in decimal, 1 = no change, .80 is a tweny percent reduction, 1.2 is a 20 percent increase.</param>
        /// <param name="Limit">         The limit in terms of percent change, if Target >1 this is the upper limit, if Target is less than 1 this is lower limit>.</param>
        /// <param name="PeriodsToTarget"> The periods desired to achive goal years to target.</param>
        ///
        /// <returns> A double.</returns>
        /// <see cref="EstimateChangeCoef(double, double, double)"/>
        ///-------------------------------------------------------------------------------------------------

        public double EstimateChangeCoef_old(double TargetPercentGoal, double Limit, double PeriodsToTarget)
        {
            // if Target = 1 then coef should be 1;
            double Coef = 1;
            if (TargetPercentGoal != 1)
            {
                // OK target does not = 1, but can not be lower that Limit
                if (TargetPercentGoal < Limit) TargetPercentGoal = Limit;
                // OK the goal is the gap between Target and limit, but can not be 0, so we add a low number
                double DampenGoal = (TargetPercentGoal - Limit) + .001;
                Coef = Math.Abs(Math.Pow(DampenGoal, 1 / PeriodsToTarget));

            }
            return Coef;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetPeriods"></param>
        /// <param name="Limit"></param>
        /// <param name="DesiredPCT"></param>
        /// <returns></returns>
        public double EstimateGoalFromDesiredValue(double TargetPeriods, double Limit, double DesiredPCT)
        {
            double Factor1 = Math.Pow((DesiredPCT - Limit) / (1 - Limit), 1 / TargetPeriods);
            double Goal = Math.Pow(Factor1, TargetPeriods) + 1;
            return Goal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetPercentGoal"></param>
        /// <param name="GoalPeriods"></param>
        /// <returns></returns>
        public double EstimateExpRate(double TargetPercentGoal, double GoalPeriods)
        {
            double result = Math.Pow(TargetPercentGoal, 1 / GoalPeriods) - 1;
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Estimate change coef. </summary>
        ///
        /// <remarks>   5/2/2018. </remarks>
        ///
        /// <param name="TargetPercentGoal">
        ///     Target for the change expressed as a percent in decimal, 1 = no change, .80 is a tweny
        ///     percent reduction, 1.2 is a 20 percent increase.
        /// </param>
        /// <param name="Limit">
        ///     The limit in terms of percent change, if Target >1 this is the upper limit, if Target is
        ///     less than 1 this is lower limit>.
        /// </param>
        /// <param name="PeriodsToTarget">      The periods desired to achive goal years to target. </param>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TargetPercentGoal"></param>
        /// <param name="Limit"></param>
        /// <param name="PeriodsToTarget"></param>
        /// <returns></returns>
        public double EstimateChangeCoef(double TargetPercentGoal, double Limit, double PeriodsToTarget)
        {
            // if Target = 1 then coef should be 1;
            double Coef = 1;
            if (TargetPercentGoal != 1)
            {
                double FactorA = TargetPercentGoal;
                double LimitA = Limit;
                // OK target does not = 1, but can not be lower that Limit
                if (TargetPercentGoal < Limit) { FactorA = Limit; LimitA = TargetPercentGoal; }
                // OK the goal is the gap between Target and limit, but can not be 0, so we add a low number
                double DampenGoal = (FactorA - LimitA) + .001;
                Coef = Math.Abs(Math.Pow(DampenGoal, 1 / PeriodsToTarget));

            }
            return Coef;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Exponential decay coef.</summary>
        ///
        /// <remarks> Quay, 4/16/2018.</remarks>
        ///
        /// <param name="TargetValue">  Target value.</param>
        /// <param name="StartValue">   The start value.</param>
        /// <param name="Limit">        The limit in terms of percent change, if Target >1 this is the
        ///                             upper limit, if Target is less than 1 this is lower limit>.</param>
        /// <param name="StartPeriod">  The start period.</param>
        /// <param name="TargetPeriod"> Target period.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        //public double ExponentialDecayCoef(double TargetValue, double StartValue, double Limit, double StartPeriod, double TargetPeriod)
        //{
        //    double result = 0;

        //    // OK, Check Target
        //    if (TargetValue < StartValue)
        //    {
        //        // Declining, Target can not be =< Limit, if it is adjust to be slightly above limit
        //        if (TargetValue <= Limit) TargetValue = Limit *1.01 ;
        //    }
        //    else
        //    {
        //        // Increasing Declining, Target can not be >= Limit, if it is adjust to be slightly below limit
        //        if (TargetValue >= Limit) TargetValue = Limit * .99; 
        //    }
        //    // OK, if Target is greater than Start and Target is Less than Limit (increasing) or
        //    //        Target os less that Start and Target is Greater than Limit (decreasing)
        //    if (((TargetValue > StartValue) && (Limit >= TargetValue))||((TargetValue<StartValue)&&(TargetValue>=Limit)))
        //    {
        //        double LimitStart = Limit - StartValue;
        //        double LimitTarget = Limit - TargetValue;
        //        double Ratio = LimitStart / LimitTarget;
        //        double Periods = TargetPeriod - StartPeriod;
        //        double LN = Math.Log(Ratio);
        //        double Value = LN / Periods;

        //        result = Math.Log((Limit - StartValue) / (Limit - TargetValue)) / (TargetPeriod-StartPeriod);

        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Annual exponential change.</summary>
        ///
        /// <remarks> Quay, 4/16/2018.</remarks>
        ///
        /// <param name="StartValue">      The start value.</param>
        /// <param name="period">          The period.</param>
        /// <param name="ExponentialCoef"> The exponential coef.</param>
        /// <param name="Limit">            The limit in terms of percent change, if Target >1 this is
        ///                                 the upper limit, if Target is less than 1 this is lower
        ///                                 limit>.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double AnnualExponentialChange(double StartValue, double period, double ExponentialCoef, double Limit)
        {
            double result = 1;
            double Fuck1 = (StartValue - Limit);
            double Fusk2 = (-1 * ExponentialCoef) * period;
            double Fuck3 = Math.Exp(Fusk2);
            double Fuck4 = Fuck3 * Fuck1;
            result = Limit + ((StartValue - Limit) * Math.Exp((-1 * ExponentialCoef) * period));
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Change increment.</summary>
        ///
        /// <remarks> Quay, 3/5/2018.</remarks>
        ///
        /// <param name="Target">      The value.</param>
        /// <param name="period">      The period number in a series of periods</param>
        /// <param name="ChangeCoef">  The change coef generated with EstmateChangeCoef(). Target and Limit need to be the same as was used in EstimateChangeCoef ,
        ///                            Otherwise strange things happen, interesting but not what you would expect.</param>
        /// <param name="Limit">        The limit in terms of percent change, if Value is Target >1 this is the
        ///                             upper limit, if Target is less than 1 this is lower limit>.</param>
        /// <returns> A double.</returns>
        /// <see cref="EstimateChangeCoef(double, double, double)"/>
        ///-------------------------------------------------------------------------------------------------

        public double ChangeIncrement(double Target, double period, double ChangeCoef, double Limit)
        {
            if (ChangeCoef != 1)
            {
                double P = Math.Pow(ChangeCoef, period);
                double NewValue = ((P * (Target - Limit)) + Limit);
                return NewValue;
            }
            else
            {
                return Target;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ChangeCoef"></param>
        /// <param name="period"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        public double ChangeIncrementExp(double ChangeCoef, double period, double Limit)
        {
            double result = Math.Pow(1 * ChangeCoef, period);
            if (result < 1)
            {
                if (result < Limit) result = Limit;
            }
            else
            {
                if (result > Limit) result = Limit;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Forecast cu.</summary>
        /// <remarks> Quay, 3/3/2018.</remarks>
        /// <remarks>  This is based on the formula Forecast = Base_CU * ((1+GrowthRate)^(term))
        ///            The growth rate is adjusted based on a dampen function 
        ///
        /// <param name="Base_CU">         The base cu.</param>
        /// <param name="GrowthRateof_CU"> The growth rateof cu.</param>
        /// <param name="AdjustGrowth_CU"> The adjust growth cu.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double Forecast_CU(double Base_CU, double GrowthRateof_CU, double GrowthDamper, double period, double AdjustGrowth_CU, double AdjustDamper)
        {
            double result = 0;
            double rate = GrowthRateof_CU;

            try
            {
                // Dampem the growth rate so it slows over time, dividing by 100 becasue GrpwthRate is in Percent ie 1% = .01
                double CurrentRate = Dampen(rate / 100, GrowthDamper, period);
                // Apply Adjustment to Production Growth if not 1
                if (AdjustGrowth_CU != 1)
                {
                    // Dampen the effect of AdjustGrowth so its impact increases over time.
                    double AdjustProductionGrowth = Dampen(AdjustGrowth_CU, AdjustDamper, period);
                    CurrentRate = CurrentRate * AdjustProductionGrowth;
                }
                double P = Math.Pow(1 + CurrentRate, period);
                // Calculates F in Future = Present * A 

                result = Base_CU * P;
            }
            // END EDIT 3/2/18

            catch (Exception ex)
            {
                // Ouch Only thing going here is Math.Pow or Dampen Functions
            }
            return result;
        }
        //
        //


        /////-------------------------------------------------------------------------------------------------
        ///// <summary> Estimate conumer demand.</summary>
        /////
        ///// <remarks> Quay, 3/3/2018.</remarks>
        ///// <remarks> This using dammpner values</remarks>
        ///// <param name = "ConsumerUnits" > The consumer units.</param>
        ///// <param name = "GallonsPerUnit" > The gallons per unit.</param>
        ///// <param name = "AdjustEfficiency" > The adjust efficiency.</param>
        ///// <param name = "AdjustDamper" > The damper factor fr Asjusting effciency</param>
        ///// <param name = "period" > The period.</param>
        ///// <returns> A double.</returns>
        /////-------------------------------------------------------------------------------------------------

        //public double EstimateConsumerDemand(double ConsumerUnits, double GallonsPerUnit, double AdjustEfficiency, double AdjustDamper, double period)
        //{
        //    double result = 0;
        //    try
        //    {
        //        // modify the efficiency
        //        double DampenValue = Dampen(1, AdjustDamper, period);

        //        double ModifyRate = AdjustEfficiency * DampenValue;
        //        if (AdjustEfficiency != 1)
        //        {
        //            result = ConsumerUnits * (GallonsPerUnit * ModifyRate);
        //        }
        //        else
        //        {
        //            result = ConsumerUnits * GallonsPerUnit;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ouch Only thing going here is the Dampen Function
        //    }
        //    return result;
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Estimate consumer demand 2. </summary>
        ///
        /// <remarks>   3/5/2018. </remarks>
        /// <remarks>   This usies ChangeCoefficients</remarks>
        /// <param name="ConsumerUnits">    . </param>
        /// <param name="GallonsPerUnit">   . </param>
        /// <param name="AdjustEfficiency"> . </param>
        /// <param name="AdjustDamper">     . </param>
        /// <param name="MinValue">         The minimum value. </param>
        /// <param name="period">           The period. </param>
        ///
        /// <remarks>  This method estimates consumer demand based on units of Consumers (ConsumerUnits) and 
        ///            Gallons per Consumer Unit (GallonsPer Unit).  Actually this does not need to be water, but
        ///            any resource and in fact in this code it is used to generate power per people.
        ///            The demand is adjusted by AdjustEfficiency which is a coefficient deigned to slowly reeduce the 
        ///            effect of GallonsPerUnit in a hacked hyperbolic curve.  This coefficient is calculated by the 
        ///            method EstimateChangeCoef() and is then used by the method ChangeIncrement(). 
        ///            These methods work in pair.  Refer to the documentation of these methods. </remarks>
        /// <see cref="EstimateChangeCoef(double, double, double)"/>
        /// <see cref="ChangeIncrement(double, double, double, double)"/>
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double EstimateConsumerDemand(double ConsumerUnits, double GallonsPerUnit, double AdjustEfficiency, double AdjustDamper, double MinValue, double period)
        {
            double result = 0;
            try
            {
                // Get the modify factor to apply for this period using the coefficient AdjustDamper
                //double ModifyRate = ChangeIncrement(1, period, AdjustDamper, MinValue);
                double ModifyRate = AnnualExponentialChange(1, period, AdjustDamper, MinValue);
                // only use the Modifyrate if your target change is not 1.
                if (AdjustEfficiency != 1)
                {
                    // demand = units * a modified resource per unit value
                    result = ConsumerUnits * (GallonsPerUnit * ModifyRate);
                }
                else
                {
                    // staright forward resources = units * resources per unit
                    result = ConsumerUnits * GallonsPerUnit;
                }
            }
            // Why is this here?  Good question, ChangeIncrement uses the Math.POW() method which can
            // throw an exception.  Hopefully that does not happen and if it does we will just use a 
            // zero as the default value
            catch (Exception ex)
            {
                // Ouch Only thing going here is the Change Increment Function
            }
            return result;
        }


        #endregion Generic_Demand_Model

        //=================================================================================
        // Community Demand Manager
        // ===========================================================================
        #region Community Demand

        // EDIT QUAY 3/1/18
        // This is the original urban method, I replaced it with a new urban method below
        // which uses the generic production and demand methods.  This is also kind of messy and needs cleaned up
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Urban   calculates new Urban Demand</summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        void Urban_old()
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
            // QUAY EDIT 2 18 18
            // These are not be used, I removed them
            // i_demand_urban = result;
            // END EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Urban Demand Managememt </summary>
        /// <remarks>   This uses the genric production and demand methods
        ///             This routine uses the </remarks>
        ///             777
        ///-------------------------------------------------------------------------------------------------
        // david ars
        void Urban()
        {
            double period = (currentYear - startYear) + 1;
            //
           
            // Estimate demand
           // double NewDemand = EstimateConsumerDemand(population, FBaseUrbanGPCD, UrbanConservation, FUrbanGPCDChangeCoef, FMinUrbanGPCDPercent, period);
           //// NewDemand += this.TheAddedDemand;
           // // 

           // double Atemp = UD.GetDemand(currentYear);
           // double temp = UDR.GetDemand(currentYear);

           // double Mytemp = UD.GetDemand(currentYear);
           // Mytemp+= this.TheAddedDemand;
            //
           
            // Is this the NEW structure?
            // 07.10.18 das
            double Demand = this.URBAN.GetDemand(currentYear);
           // Demand += this.TheAddedDemand;
            // 
            // 
            //// Sampson  Edits - 05.18.18

            // ok this is now in gallons, convert it to MGD
           // double NewDemandMGD = NewDemand / convertDemand;
            //double NewDemandMGD = Demand / convertDemand;
            double NewDemandMGD = Demand ;
            // Set value for parameter
            seti_Urban((int)NewDemandMGD);
            //sw.WriteLine(currentYear
            //        + ","
            //        + NewDemandMGD
            //     + ","
            //     + Atemp / convertDemand
            //     + ","
            //     + temp
            //     + ","
            //     + Mytemp
            //        );
        }
        // END EDIT 3/1/18


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the urban gpcd.</summary>
        ///
        /// <value> The urban gpcd.</value>
        ///-------------------------------------------------------------------------------------------------

        //int urbanGPCD
        public int urbanGPCD
        {
            get
            {
                //double temp = WSA.Urban.Demand * convertDemand;
                double temp = UnitNetwork.Urban.Demand * convertDemand;

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
        //
        //public int UrbanGPCD
        //{
        //    get { return urbanGPCD; }
        //}

        //
        #endregion


        // 07.02..18
        // Edits Sampson
        // Index for the Demand Model to Use

        public int geti_aDemandModelUrbanIndex()
        {
            int TempInt = FDemandModelUrbanIndex;
            return TempInt;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag growth rate.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------
        int _anIndexUrban = 1;
        public void seti_aDemandModelUrbanIndex(int value)
        {
            FDemandModelUrbanIndex = value;
        }
        //public int aDemandModelUrbanIndex
        //{
        //    get { return _anIndexUrban; }
        //    set { _anIndexUrban = value; }
        //}
        public int geti_aDemandModelAgIndex()
        {
            int TempInt = FDemandModelAgIndex;
            return TempInt;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag growth rate.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------
        int _anIndexAg = 1;
        public void seti_aDemandModelAgIndex(int value)
        {

            FDemandModelAgIndex = value;
        }
        //


        public int geti_aDemandModelIndIndex()
        {
            int TempInt = FDemandModelIndIndex;
            return TempInt;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag growth rate.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------
        public void seti_aDemandModelIndIndex(int value)
        {

            FDemandModelIndIndex = value;
        }




        // End Edits Sampson



        // =============================================================
        // AGRICULTURE DEmand Manager
        // ----------------
        #region Agriculture Demand
        //  
        //#########################################################

        //-----------------------------------------------
        // QUAY 2/22/18
        // This is the first model I edited.  Converted it to a standard production - demand format.
        // This is pretty messy and I should come back in and clean up
        // I think I edited out some variables that were not necessary to do so
        // ------------------------------------------------------------------------------


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the agriculture net. </summary>
        /// <remarks> This is the Base net Form income for the Should not vary from year to year</remarks>
        /// <value> The agriculture net. </value>
        ///-------------------------------------------------------------------------------------------------

        public double AgricultureAGNet
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti ag growth rate.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AgGrowthRate()
        {
            int TempInt = Convert.ToInt32(Math.Round(FAgRate * 100));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag growth rate.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_AgGrowthRate(int value)
        {
            FAgRate = (Double)value / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti ag production rate.</summary>
        ///
        /// <remarks> Quay, 3/4/2018.</remarks>
        /// <remarks> This is used to adjust the GallonsPerDollar value of AG over time</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_agAdjustProductionRate()
        {
            int TempInt = Convert.ToInt32(Math.Round(_agAdjustProductionRate * 100));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti ag production rate.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_agAdjustProductionRate(int value)
        {
            _agAdjustProductionRate = (Double)value / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the ag production rate.</summary>
        /// <value> The ag production rate.</value>
        ///-------------------------------------------------------------------------------------------------

        //double AgAdjustProductionRate
        public double AgAdjustProductionRate
        {
            get { return _agAdjustProductionRate; }
        }

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
        /// <summary> Geti agriculture initial gpdd.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AgricultureInitialGPDD()
        {
            return (int)FAgBaseGPDD;
        }




        /// <summary>   The minimum ag gpdd percent change. </summary>
        /// <remarks>   This is used to create a Change Coeeficient for AG GPDD </remarks>
        const double MinimumAgGPDDPercentChange = 0.20;
        const double Damper = 1.01;
        const double AgGrowthDamper = 0.980;
        const double AgAdjustDamper = 0.990;
        const double AgEfficiencyDamper = 0.98;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Calculates the ag production.</summary>
        ///
        /// <remarks> Quay, 3/4/2018.</remarks>
        ///
        /// <param name="theCurrentYear"> the current year.</param>
        /// <param name="theStartYear">   the start year.</param>
        ///
        /// <returns> The calculated ag production.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double Calc_AgProduction(int theCurrentYear, int theStartYear)
        {
            double period = (theCurrentYear - StartYear) + 1;
            double Production = Forecast_CU(AgricultureAGNet, AgricultureGrowthRate, AgGrowthDamper, period, AgAdjustProductionRate, AgAdjustDamper);
            return Production;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Calculates the ag demand.</summary>
        ///
        /// <remarks> Quay, 3/4/2018.</remarks>
        ///
        /// <param name="theCurrentYear"> the current year.</param>
        /// <param name="theStartYear">   the start year.</param>
        /// <param name="NewProduction">  The new production.</param>
        /// <param name="AdjustEfficiency">  The modifier of efficiency.</param>
        ///
        /// <returns> The calculated ag demand.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Calc_AgDemand(int theCurrentYear, int theStartYear, double NewProduction, double AdjustEfficiency)
        {
            double period = (theCurrentYear - theStartYear) + 1;
            // Have to remember this is MGD not gallons per day (probably should relable this field
            double AgDemand = EstimateConsumerDemand(NewProduction, AgricultureInitialGPDD, AdjustEfficiency, AgGPDDChangeCoef, FMinAgGPDDPercent, period);
            return (int)AgDemand;
        }


        //    ///-------------------------------------------------------------------------------------------------
        //    /// <summary>   Calculates the ag production. </summary>
        //    ///
        //    /// <param name="theCurrentYear">   the current year. </param>
        //    /// <param name="theStartYear">     the start year. </param>
        //    ///
        //    /// <returns>   The calculated ag production. </returns>
        //    ///-------------------------------------------------------------------------------------------------

        //    public int Calc_AgProduction_orginal(int theCurrentYear, int theStartYear)
        //{
        //    int result = 0;
        //    // calculate term
        //    double period = (theCurrentYear - theStartYear) + 1;
        //    // get adjusted growth rate
        //    //// dampen the rate
        //    double rate = AgricultureGrowthRate;

        //    // EDIT QUAY 3/2/18
        //    // Rate can be negative  can test here with
        //    // rate = rate * -1;
        //    // ------------------------

        //    // calculate the new $ production

        //    // get the base agProduction ($)
        //    double AgNet = AgricultureNet;

        //    int temp = 0;
        //    try
        //    {
        //        // Dampem the growth rate so it slows over time, dividing by 100 becasue GrpwthRate is in Percent ie 1% = .01
        //        double CurrentRate = DampenRate(rate / 100, Damper, period);
        //        // Caluclates A in Future  = Present  * A where A = (1+Growthrate)^Term
        //        // I think that AgProductionRatio is a way to alter the GrowthRate by a certain percent
        //        // with out altering the ariginal growth rate, perhaps to be set by a feedback process.
        //        double AgPower = Math.Pow(1 + (CurrentRate * AgProductionRate),period);
        //        // Calculates F in Future = Present * A 
        //        double NewProduction = Math.Round(AgNet * AgPower);
        //        // (int) is faster, Convert will throeugh Exceptions
        //        //temp = Convert.ToInt32(NewProduction);
        //        temp = (int)NewProduction;
        //        result = temp;
        //    }
        //    // END EDIT 3/2/18

        //    catch (Exception ex)
        //    {
        //        // Ouch Only thing going here is Math.Pow
        //    }
        //    return result;
        //}



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

        /////-------------------------------------------------------------------------------------------------
        ///// <summary>   Calculates the ag demand. </summary>
        /////
        ///// <param name="theCurrentYear">   the current year. </param>
        ///// <param name="theStartYear">     the start year. </param>
        ///// <param name="NewProduction">    The new production, ie production from last year. </param>
        ///// <param name="modDemand">        The modifier demand. Not currently used! </param>
        /////
        ///// <returns>   The calculated ag demand. </returns>
        /////-------------------------------------------------------------------------------------------------

        //public int Calc_AgDemand_original(int theCurrentYear, int theStartYear, double NewProduction, double modDemand)
        //{
        //    // QUAY 3 /3 18
        //    // This code is now working but has been substantially changed 
        //    // 
        //    int result = 0;
        //    double NewAgDemand = 0;
        //    //double final = 0;
        //    //double temp = 0;
        //    // get the Initial Gallons per Dollar (ie assume it does not change; should insert some code to change this
        //    double InitialAGPD = AgricultureInitialGPDD;
        //    // Calculate new demand based on new production and AGPD
        //    // Adjust AGPCD
        //    double period = (theCurrentYear - theStartYear) + 1;
        //    //double AgNetworkDemand = UnitNetwork.Agriculture.Demand;
        //    //if (theCurrentYear <= theStartYear) { RetainAgDemand = AgNetworkDemand; }
        //    //if (theCurrentYear <= theStartYear) { RetainAgDemand = (WSA.Agriculture.Demand); }
        //    try
        //    {

        //        double CurDamper = 1 + ((100.0 - (double)FAgTargetGPDDReduction) / 4000);
        //        FAdjAGPCD = DampenRate(InitialAGPD, CurDamper, period);
        //        // das
        //        //double ttemp = WSA.Agriculture.Demand;
        //        //double ttemp = UnitNetwork.Agriculture.Demand;

        //        NewAgDemand = NewProduction * FAdjAGPCD;
        //        //temp = NewAgDemand;
        //        ////double CurrentRate = DampenRate(FAgRate / 100, Damper, period);
        //        //double newAgproduction = Math.Round(ttemp * Math.Pow(1 + (CurrentRate * AgGrowthRate), period));
        //        //// NOTE I see on 01.03.17 that AgGrowthRate has no current effect on Agriculture
        //        //// DAS- and newAgproduction is NOT being called - AgProductionRate would need to be modified
        //        //// to have any growth effects on Agriculture (at this time)
        //        ////
        //        //double Difference = NewAgDemand - RetainAgDemand;// ttemp;
        //        ////if (modDemand < 1) modDemand = correctMod(modDemand);
        //        ////double temp = ttemp + (Difference * Math.Min(1.0, Math.Max(minMod, modDemand)));
        //        //temp = ttemp + (Difference * modDemand);
        //        //RetainAgDemand = NewAgDemand;
        //        //if (theCurrentYear <= theStartYear) temp = NewAgDemand;

        //    }
        //    catch (Exception ex) { }
        //    //
        //    //final = (temp);
        //    //result = Convert.ToInt32(final);

        //    //sw.WriteLine(currentYear
        //    //+ ","

        //    //+ result
        //    //);
        //    //return result;
        //    return (int)NewAgDemand;
        //}

        //======================================================================
        // NOTES ON AGRICUTURE
        // 2/22/18
        // QUAY
        // After much debugging and tracing the code, I was unable to get this code to work correctly.  I really could not figure out what was going on (ie understand why
        // and what the code was doing)  Also their is a disconnect between what is occuring here and what is going on in the Ag Indicator.  The Ag model code gets data 
        // from the rate file and the ag indicator gets what is suppose to be the same data from the indictaor file.  So rather than spend a lot more
        // time trying to figure it out, I rewrote it and the indicator code based on the following
        // 1) AGNet is the Anual Net Farm Income.  This is estinated for each region based on USDA Net Farm Income data for each state, and then proportional
        //    allocated to each region based on each regions share of ag total water demand for the state.  If we can find county specific data this can be modified.
        //    The units here are Thousands of dollars
        // 2) AgRate the Annual Rate of Net Farm income, it is calculated based on USDA Net Farm income from 1996 to 2016 for each state and taking the anualized 
        //    rate of growth over this time period.  All regions is a state currently have the same rate.  If we can find county specific data this can be modified. 
        // 3) A metric of Gallons Per Dollar Net Farm Income (GFIY) can be calcualted by divding Total Ag Demand in the first year of the model by AgNet.
        // 4) Ag Farm Income (AFIY) is estimnated each year after is based on AgNet * ((1+Agrate)^(Current year - Start Year))
        // 5) Ag Water Demand each year is based on Ag Farm income (AFIY) * the Gallons per Dollar or Famr INcome Metric (GFIY)
        // 6) Ag water efficiency is a function of the GFIY.  Increased efficiency can lower this value so fewer gallons are needed per dollar of Farm Income.  Same Farm Income with
        //    less water use.
        // 7) One could also lower farm demand by altering AgRate to be lower which would slow growth of Farm Income.  Same farn efficienc=y but less growth in Farm Income.  
        //     This could be considered an economic stress (will have to think about that)   
        // 8) The indicator is based on the following concept  
        //  
        //    During the first year, the indictor process will save a base AGNET value .  

        //  9) The indicator launches a process to grab these initial state values.  The problem is how does the Model communicate with the indicator.  Well it does not.
        //     The indicators are self contained, they can use model parameters to talk to the model, but the model can not talk to the indicators.
        //     So a model parameter has been created for AgNetBase and AgRateBase.  WaterSimManager loads the rate data, and create thsese parameters.  The indicator or other controls can
        //     Use these values or set the values.  The model will use these parameters each year, what ever there values are, to estimate Farm Income and ag water demand, and
        //     sets up parameters for these.  The indicator will use these values on start up to create a base. 
        //
        //     Thi means that AGNET and AGRATE values for each region are in the Rate data file, and the indicator will ignore anything in the indicator data file





        // holder for ag production
        double FAgProduction = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agricuture production. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_AgricutureProduction()
        {
            return (int)FAgProduction;
        }


        /// <summary>   Estimate Agriculture demand </summary>
        void Agriculture()
        {
            int result = 0;
            double temp = 0;
            // get Ag production
            //FAgProduction = Calc_AgProduction(currentYear, startYear);
            //// now estmate demand based on production
            //// EDIT QUAY 3/3/2018
            //// QUESTION  Why are we modify agproduction if there is a drought?
            //// This is wnat modifyDemand does.  I removerd for now
            //// temp = Calc_AgDemand(currentYear, startYear, FAgProduction, AgConservation) * modifyDemandCF();
            //temp = Calc_AgDemand(currentYear, startYear, FAgProduction, AgConservation);
            //// END EDIT 3/3/2018
            ////
            ////double Mytemp = ADI.GetDemand(currentYear);
            //double Mytemp = ADR.GetDemand(currentYear);
            ////
            //this.AG.GetDemand(currentYear);
            //
            // This is the new object for Ag demand
            //AG.GetDemand(currentYear);
            temp = this.AG.GetDemand(currentYear);
            result = (int)temp;
            //result = (int)Mytemp;
            // set the parameter for AgDemand
            seti_Agriculture(result);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agricultural gpcd. </summary>
        ///
        /// <value> The agricultural gpcd. </value>
        ///-------------------------------------------------------------------------------------------------

        int agriculturalGPCD
        {
            get
            {
                double temp = UnitNetwork.Agriculture.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                try
                {
                    if (0 < pop) gpcd = temp / pop;
                    // For comparison
                    // EDIT QUAY 3/30/18
                    // The new systems donot use the population proportion methods, Modified this to just calcuate GPCD
                    // Also Not this was not returnng the t anyways, not sure if I made this change early in the conversion and did 
                    // not makr it or if this was done a long time ago.
                    // Note that the new agriculture metric of Gallons Per CDollar Farm Income is a better efficiecny measure
                    //int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Ag);
                    // END EDIT
                }
                catch (Exception ex) { }
                return Convert.ToInt32(gpcd);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the agriculture Gallons per Dollar of Farm Income per Day.</summary>
        ///
        /// <value> The agriculture gpdd.</value>
        ///-------------------------------------------------------------------------------------------------

        public int agricultureGPDD
        {
            get
            {
                double AgDemand = UnitNetwork.Agriculture.Demand * convertDemand;
                double FarmIncome = geti_AgricutureProduction() * convertFarmIncome;
                double GPDD = 0;
                if (FarmIncome > 0)
                {
                    GPDD = AgDemand / FarmIncome;
                }
                return (int)GPDD;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti agriculture Gallons per Dollar of Farm Income per Day.</summary>
        ///
        /// <remarks> Quay, 3/4/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_agricultureGPDD()
        {
            return agricultureGPDD;
        }
        #endregion
        // ----------------

        // =============================================
        // Power Water
        // =============================================
        # region Power Demand

        //-------------------------------------------------------------------------------------------------
        // Power Water Demand
        //
        // Quay, 3/9/2018.  I modified this to follow the generic prodiuction and demand approach
        //           Power is the production. it responds to population in this approach but could have its own growth curve.
        //           For the west model we will need to look into this, growth of power by region, could use the USGS data for that.
        //           Power (mega wats) then creates the demand for water based on an efficiency coefficient of megawats per power.  
        //           drive water demand.
        //-------------------------------------------------------------------------------------------------


        /// <summary> The gallons per watt per day.</summary>
        double FGallonsPerMegaWattPerDay = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti gallons per watt per day.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_GallonsPerMegaWattPerDay()
        {
            return (int)FGallonsPerMegaWattPerDay;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti gallons per watt per day.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_GallonsPerMegaWattPerDay(int value)
        {
            FGallonsPerMegaWattPerDay = value;
        }


        /// <summary> The watts per person per day.</summary>
        double FWattsPerPersonPerDay = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti watts per person.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_WattsPerPerson()
        {
            return (int)FWattsPerPersonPerDay;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti watts per person.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_WattsPerPerson(int value)
        {
            FWattsPerPersonPerDay = value;
        }
        //
        public double PWattsPerPersonPerDay
        {

            get { return FWattsPerPersonPerDay; }
        }

        //
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Power.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <remarks> This method estmates power produced and then based on power produced estimates water demand</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Power()
        {
            //double PowerProduced = PowerProduction(currentYear, startYear);
            //seti_PowerEnergy((int)PowerProduced);
            //double PowerWater = PowerDemand(currentYear, startYear, PowerProduced, PowerConservation);
            //// 07.10.18 das edits
            //PowerWater += PowerWater * TheAddedPowerPCT;
            //// end edits das 07.10.18
            //seti_PowerWater((int)PowerWater);

            //double temp= PD.GetDemand(currentYear);
            double temp = POWER.GetDemand(currentYear);
            seti_PowerWater((int)temp);
            //seti_PowerWater((int)PowerWater);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Power production.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        ///  <remarks> This method estimates power produced
        ///            currently that is done based on population using population, power per population , and energy use per person
        ///            </remarks>
        /// <note>     This ignores exported and imported power and assumes all power produced is related to population use.
        ///            This is highly error prone at the region level.  Need to collect data on exported, imported, and used data
        ///            We can then devise a strategy for power (which here is power produced, not consumed)
        ///            Allso may want to include power export, import , and consumed into model.</note>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double PowerProduction(int Year, int FirstYear)
        {
            double result = 0;
            double period = (Year - FirstYear) + 1;
            // population is actually the production, which has already been done, but need to convert to watts, which is an internmediate demand
            double temp = EstimateConsumerDemand(population, FWattsPerPersonPerDay, FMinWattPerPersonPercent, FWattPerPersonChangeCoef, FMinWattPerPersonPercent, period);
            // Note that this produdes Total watts and must be converted to MegaWatts
            result = temp / convertPower;
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Power demand.</summary>
        /// <remarks> Quay, 3/9/2018.</remarks>
        /// <remarks> This is based on power produced and a gallons per megawatt coefficient</remarks>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double PowerDemand(int Year, int FirstYear, double PowerProduced, double PowerConservationGoal)
        {
            double result = 0;
            double period = (Year - FirstYear) + 1;
            double Temp = EstimateConsumerDemand(PowerProduced, FGallonsPerMegaWattPerDay, PowerConservation, FPwGPMWChangeCoef, FMinPwGPMWDPercent, period);
            // OK, we need to remeber that this is Gallons per Mega Watt, need to convert to MGD. 
            result = Temp / convertDemand;
            return result;
        }

        // NOTE QUAY 3/9/18
        // I LEFT THE OLD METHOD CODE HERE, though it is not connected right now.
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Power water old.</summary>
        ///-------------------------------------------------------------------------------------------------
        void PowerWater_Old()
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
            // QUAY EDIT 2 18 18
            // This are not be used, I removed them
            //i_demand_power = result;
            // END EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the power gpcd.</summary>
        ///
        /// <value> The power gpcd.</value>
        ///-------------------------------------------------------------------------------------------------

        int powerGPCD
        {
            get
            {
                double temp = UnitNetwork.Power.Demand * convertDemand;
                //double temp = WSA.Power.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                if (0 < pop)
                {
                    gpcd = temp / pop;
                }
                // EDIT QUAY 3/9/18
                // Not sure what this was for, not being used, commented this out
                //int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Power);
                // END EDIT 3/9/18
                return Convert.ToInt32(gpcd);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Power Gallons Per MegaWatt per Day.</summary>
        /// <remarks> Quay, 3/20/2018.</remarks>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double PowerGPMWD()
        {
            double temp = UnitNetwork.Power.Demand * convertDemand;
            double MEGAWATTS = _powerEnergy;
            double gpMWd = 0;
            if (0 < MEGAWATTS)
            {
                gpMWd = temp / MEGAWATTS;
            }
            // EDIT QUAY 3/9/18
            // Not sure what this was for, not being used, commented this out
            //int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Power);
            // END EDIT 3/9/18
            return gpMWd;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti power gpmwd.</summary>
        /// <remarks> Quay, 3/20/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PowerGPMWD()
        {
            return (int)PowerGPMWD();
        }

        // NOTE QUAY 3/9/18
        // I LEFT THE OLD METHOD CODE HERE, though it is not connected right now.
        //  Power_energy is set in the new power method
        //  ----------------
        // Power Energy
        void PowerEnergy()
        {
            double temp = 0;
            int result = 0;
            temp = weightedGrowthPopulation(geti_PowerEnergy()) * modifyDemandCF();
            result = Convert.ToInt32(temp);
            seti_PowerEnergy(result);
        }
        // ------------------------------------

        #endregion
        // ----------------
        // Industry  
        // ----------------

        #region Industry Demand

        // NEW CODE AS OS 09.06.16 das 

        // Production of Industry which is based on employees
        double _indProduction;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the ind production.</summary>
        /// <value> The ind production.</value>
        ///-------------------------------------------------------------------------------------------------

        double IndProduction
        {
            get { return _indProduction; }
            set { _indProduction = value; }
        }
        //

        // EDIT QUAY 3/4/18
        // Moved this to intgialize base fields
        // need some default values here
        // //double FIndRate = 0;
        // EnN EDIT
        // 
        // double IndDamper = 1.2;
        const double DamperF = 0.9;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the industrial growth rate.</summary>
        /// <value> The industrial growth rate.</value>
        ///-------------------------------------------------------------------------------------------------

        public double IndustrialGrowthRate
        {
            //get { return RDC.INDRate(FStateIndex); }
            get { return FRDC.FastIndRate(FUnitName); }
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Calculates the industrial demand.</summary>
        /// <param name="theCurrentYear"> the current year.</param>
        /// <param name="theStartYear">   the start year.</param>
        /// <param name="modDemand">      The modifier demand.</param>
        /// <returns> The calculated ind demand.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int Calc_IndDemand(int theCurrentYear, int theStartYear, double modDemand)
        {
            int result = 0;
            int Industry2015 = 0;

            double final = 0;
            double IndustrialDamper = 0.9;
            double DampenTheRate = 1;
            double useValue = 1.2;
            double modValue = 0.53;
            double minTheRate = 1.15;
            double temp = 0;
            try
            {
                // EDIT QUAY 3/3/2018
                // Chamged how dampen function works see dampen()
                // IndustrialDamper = Math.Max(IndustrialGrowthRate * modValue, useValue);
                // END EDIT 3/3/2018
                DampenTheRate = Math.Min(IndustrialGrowthRate / 2, minTheRate);
                if (theCurrentYear <= theStartYear) { Industry2015 = geti_Industrial(); }

                double period = (theCurrentYear - theStartYear) + 1;
                double dampedRate = Dampen(IndustrialGrowthRate / 100, IndustrialDamper, period);
                double ttemp = Convert.ToInt32(Math.Round(UnitNetwork.Industrial.Demand));
                //                double ttemp = Convert.ToInt32(Math.Round(WSA.Industrial.Demand));
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industrial /summary>
        ///-------------------------------------------------------------------------------------------------
        /* double FIndEmployees = 0;
        // Indistrial Change Coef for Employee Growth
        double FindEmpChangeCoef = 1;

        // Industrial Gallons per Employee per Day
        double FIndGPED = 0;
        // indistrial Minimum percent decline in GPED
        double FIndMinGPEDPercent = 0;
        // Indiustrial GPEDCHangeCoef
        double FindGPEDChangeCoef = 1;
        */

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industrial</summary>
        ///
        /// <remarks> Quay, 3/11/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        void Industrial()
        {
            int result = 0;
            double temp = 0;
            //double indEmp = Industrial_Employees(currentYear, StartYear);
            //FIndEmployees = indEmp;
            //double indDemand = Industrial_Demand(currentYear, StartYear, indEmp);
            //seti_Industrial((int)indDemand);

            //double temp= ID.GetDemand(currentYear);


            
            //double myTemp = IDR.GetDemand(currentYear);
            //this.INDUSTRY.GetDemand(currentYear);
            temp = this.INDUSTRY.GetDemand(currentYear);
            seti_Industrial((int)temp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industrial employees.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <param name="year">      The year.</param>
        /// <param name="startyear"> The start year.</param>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double Industrial_Employees(int year, int startyear)
        {
            double period = (year - startyear) + 1;
            double result = 0.0;
            // we are not adjusting the rate of growth of employees just yet. so these values are 1
            result = Forecast_CU(FIndInitialEmploy, IndustrialGrowthRate, FindEmpChangeCoef, period, 1, 1);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industrial demand.</summary>
        ///
        /// <remarks> Quay, 3/11/2018.</remarks>
        ///
        /// <param name="year">      The year.</param>
        /// <param name="startyear"> The start year.</param>
        /// <param name="Employees"> The employees.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double Industrial_Demand(int year, int startyear, double Employees)
        {
            double result = 0.0;
            double period = (year - startyear) + 1;
            double temp = EstimateConsumerDemand((Employees * convertEmployee), FIndInitialGPED, IndustryConservation, FindGPEDChangeCoef, FIndMinGPEDPercent, period);

             // this is gallons convert to MGD
            result = temp / convertDemand;
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industrial old.</summary>
        ///-------------------------------------------------------------------------------------------------

        void Industrial_old()
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
            // QUAY EDIT 2 18 18
            // These are not be used, I removed them
            //i_demand_industry = result;
            // END EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the industrial gpcd.</summary>
        /// <value> The industrial gpcd.</value>
        ///-------------------------------------------------------------------------------------------------

        int industrialGPCD
        {
            get
            {
                double temp = UnitNetwork.Industrial.Demand * convertDemand;
                //double temp = WSA.Industrial.Demand * convertDemand;
                double pop = population;
                double gpcd = 0;
                if (0 < pop) gpcd = temp / pop;
                // for comparison only
                // EDIT QUAY 3/30/18
                // The new systems donot use the population proportion methods, Modified this to just calcuate GPCD
                // Also Not this was not returnng the t anyways, not sure if I made this change early in the conversion and did 
                // not makr it or if this was done a long time ago.
                // Note that the new Industrial metric of Gallons Per Industrial Employee is a better efficiecny measure
                // int t = Convert.ToInt32(Convert.ToDouble(GPCD) * Proportion_Waterdemand_Industry);
                // END EDIT
                return Convert.ToInt32(gpcd);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> geti for Industry Gallons Per Employee per Day</summary>
        /// <remarks> Quay, 3/20/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_IndustrialGPED()
        {
            return (int)IndustryGPED();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Industry gped.</summary>
        /// <remarks> Quay, 3/20/2018.</remarks>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double IndustryGPED()
        {
            double temp = UnitNetwork.Industrial.Demand * convertDemand;
            double emp = IndustrialEmployees;
            double gped = 0;
            if (0 < emp)
            {
                gped = temp / emp;
            }
            return gped;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the industrial employees.</summary>
        ///
        /// <value> The industrial employees.</value>
        ///-------------------------------------------------------------------------------------------------

        public double IndustrialEmployees
        {
            get { return FIndEmployees; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti industry employees.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_IndustryEmployees()
        {
            return (int)FIndEmployees;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti industry employees.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <remarks> Setting this has no effects</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_IndustryEmployees(int value)
        {
            FIndEmployees = value;
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
        //internal struct MODdemand
        //{
        //    public string urban;
        //    public string power;
        //    public string agriculture;
        //    public string industry;
        //}
        //internal static void myConsumers(MODdemand m)
        //{
        //    m.urban = "Urban";
        //    m.power = "Power";
        //    m.agriculture = "Ag";
        //    m.industry = "Industry";
        //}

        int _startYear = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the start year.</summary>
        ///
        /// <value> The start year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int startYear
        {
            set { _startYear = value; }
            get { return _startYear; }
        }
        //
        int _Sim_CurrentYear = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the current year.</summary>
        ///
        /// <value> The current year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int currentYear
        {
            set { _Sim_CurrentYear = value; }
            get { return _Sim_CurrentYear; }
        }
        int _endYear = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the end year.</summary>
        ///
        /// <value> The end year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int endYear
        {
            set { _endYear = value; }
            get { return _endYear; }
        }
        int _policyStartYear = 2015;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the policy start year.</summary>
        ///
        /// <value> The policy start year.</value>
        ///-------------------------------------------------------------------------------------------------

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
        //  QUAY EDIT 2 18 18
        // These are not be used, I removed them
        //// ------------------------
        //// use lower case
        //// derived
        //// send to WaterSimith Manager
        //public int i_demand_urban;
        //public int i_demand_rural;
        //public int i_demand_ag;
        //public int i_demand_power;
        //public int i_demand_industry;
        //END EDIT 2 18 18
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
        //  This is currently not being used by any code for a GET
        double d_demand_total = 0;
        //double demand_total
        //{
        //    get
        //    {
        //        return d_demand_total;
        //    }
        //    set
        //    {
        //        double temp = value;
        //        temp = (geti_Urban() + geti_Agriculture() + geti_PowerWater() + geti_Industrial());
        //        d_demand_total = temp;
        //    }
        //}

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

        // NEW CODE AST OF 3/4/18
        // SEE NEW COMMUNITY/PEOPLE WATER USING GENERIC CONSUMEr

        // NEW CODE AS OF 09.08.16 das 
        //
        double PopAdj = 1.4;
        // QUAY EDIT 3/3/2018
        // Changed how dampen function works, see dampen()
        //double PopDamper = 1.0
        double PopDamper = 0.90;
        // END EDIT 3/3/2018

        // EDIT 3/4/18
        // Moved this to initialize base fields section
        // double FPopRate = 0;
        // END EDIT

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the population growth rate.</summary>
        /// <value> The population growth rate.</value>
        ///-------------------------------------------------------------------------------------------------

        public double PopulationGrowthRate
        {
            // EDIT 3/4/18 QUAY
            // Change this to referene FPopRate, which is initialized on construction
            //get { return FRDC.FastPopRate(FUnitName); }
            get { return FPopRate; }
            // END EDIT
            set { FPopRate = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets population growth rate.</summary>
        ///
        /// <remarks> Quay, 3/19/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PopulationGrowthRate()
        {
            return (int)(FPopRate * 100);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets population growth rate.</summary>
        ///
        /// <remarks> Quay, 3/19/2018.</remarks>
        ///
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_PopulationGrowthRate(int value)
        {
            FPopRate = (double)value / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the population.</summary>
        /// <value> The population.</value>
        ///-------------------------------------------------------------------------------------------------
        // EDIT 2/28/18 QUAY
        // Edited methods below to use unit name instaed of state
        // -------------------------

        public int population
        {
            get { return Convert.ToInt32(Math.Round(UnitNetwork.Population.CurrentState)); }
            //get { return Convert.ToInt32(Math.Round(WSA.Population.CurrentState)); }
            set { UnitNetwork.Population.CurrentState = value; }
            //set { WSA.Population.CurrentState = value; }
        }

        /// <summary> The initial population.</summary>
        int _initialPopulation = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the pop 2015.</summary>
        /// <value> The pop 2015.</value>
        ///-------------------------------------------------------------------------------------------------

        int Pop2015
        {
            set { _initialPopulation = value; }
            get { return _initialPopulation; }

        }

        /// <summary> The hold population.</summary>
        double _holdPopulation = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the pop running.</summary>
        /// <value> The pop running.</value>
        ///-------------------------------------------------------------------------------------------------

        double PopRunning
        {
            set { _holdPopulation = value; }
            get { return _holdPopulation; }

        }

        // EDIT 3/1/18 QUAY
        // I replaced this with new method that uses the genric production estimate methods, see below
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Calculates the new population.</summary>
        /// <param name="theCurrentYear"> the current year.</param>
        /// <param name="theStartYear">   the start year.</param>
        /// <returns> The calculated new population.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int CalculateNewPopulation_old(int theCurrentYear, int theStartYear)
        {
            double final = 0;
            int result = 0;
            //
            if (theCurrentYear <= theStartYear) { Pop2015 = population; PopRunning = Pop2015; }
            double period = (theCurrentYear - theStartYear) + 1;
            try
            {
                double dampedRate = Dampen((PopulationGrowthRate / 100), PopDamper, period);
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Calculates the new population.</summary>
        ///
        /// <remarks> Quay, 3/5/2018.</remarks>
        /// <remarks> Modified to use generic consumer forecast routine </remarks>
        ///
        /// <param name="theCurrentYear"> the current year.</param>
        /// <param name="theStartYear">   the start year.</param>
        ///
        /// <returns> The calculated new population.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int CalculateNewPopulation(int theCurrentYear, int theStartYear)
        {
            double BasePOP = FBasePopulation;
            double POPGrowthRate = FPopRate;
            double POPGrowDamper = 0.99;
            double Period = theCurrentYear - theStartYear;
            double POPAdjustGrowth = d_popGrowthRateMod; // d_popGrowthRate;
            double POPAdjustDamper = 1;
            double NewPopulation = Forecast_CU(BasePOP, POPGrowthRate, POPGrowDamper, Period, POPAdjustGrowth, POPAdjustDamper);
            return (int)NewPopulation;

        }

        //EDIT END 3/1/18


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
            catch (Exception ex) { throw ex; }
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
        //public int Get_PopYear(int year)
        //// END QUAY EDIT
        //{
        //    int result = 0;
        //    result = population;
        //    return result;
        //}
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

        // QUAY EDIT 3/30/18
        // This is referencing old single model and is not being used, 
        // commented it out to disconnect
        // const int RawGPCDDataInc = 5; //Years
        // const int NumberGPCDYears = ((EndYear - StartYear) / 5) + 1;

        
        // int[][] GPCDYearData = new int[WaterSimManager.FNumberOfStates][];
        // 
        //internal int Get_GPCDYear(int year)
        //{
        //    int TempGPCD = 0;
        //    if (year == 0) year = StartYear;
        //    int ModYear = year % RawGPCDDataInc; ;
        //    if (ModYear == 0)
        //    {
        //        int yearIndex = (year - StartYear) / RawGPCDDataInc;
        //        TempGPCD = GPCDYearData[FStateIndex][yearIndex];
        //    }
        //    else
        //    {
        //        int lowyearindex = ((year - StartYear) - ModYear) / RawGPCDDataInc;
        //        int hiyearindex = lowyearindex + 1;
        //        int lowgpcd = GPCDYearData[FStateIndex][lowyearindex];
        //        int higpcd = GPCDYearData[FStateIndex][hiyearindex];
        //        int GPCDChangeByYear = (higpcd - lowgpcd) / RawPopDataInc;
        //        TempGPCD = lowgpcd + (GPCDChangeByYear * ModYear);


        //    }
        //    return Convert.ToInt32(TempGPCD * d_urbanConservation);
        //}
        // END EDIT
         
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> This is the method being called by The WaterSim parameter manager to get population.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_NewPopulation()
        {
            int TempInt = 0;
            TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Population.CurrentState));
            //TempInt = Convert.ToInt32(Math.Round(WSA.Population.CurrentState));

            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets the seti new population for this modleunit in the netowrk Other parameters.</summary>
        /// <Note> Quay : Not sure why I started keeping this inthe netowkr, perhaps so we could do rations using it to do water allocations?
        ///        Perhaps should discuss why this is? </Note>
        /// <value> The seti new population.</value>
        ///-------------------------------------------------------------------------------------------------

        public int seti_NewPopulation
        {
            set { UnitNetwork.Population.CurrentState = value; }
            //set { WSA.Population.CurrentState = value; }
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.SurfaceFresh.Limit));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.SurfaceFresh.Limit));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti surface water fresh. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SurfaceWaterFresh(int value)
        {
            UnitNetwork.SurfaceFresh.Limit = value;
            //WSA.SurfaceFresh.Limit = value;
        }
        //

        public int geti_SurfaceWaterFreshNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(UnitNetwork.SurfaceFresh.Net)) + 0);
            //int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.SurfaceFresh.Net)) + 0);
            return TempInt;
        }

        // QUAY EDIT 9/12/18
        // Add for Colorado
        //--------------------------------------------------------------------------
        // Colorado
        // 

        public int geti_SurfaceColorado()
        {
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Colorado.Limit));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti surface colorado.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SurfaceColorado(int value)
        {
            UnitNetwork.Colorado.Limit = value;
        }

        public int geti_SurfaceColoradoNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(UnitNetwork.Colorado.Net)) + 0);
            return TempInt;
        }


        // EDN EDIT

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
            UnitNetwork.SurfaceLake.Value = value;
            //WSA.SurfaceLake.Value = value;
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
            double temp = UnitNetwork.SurfaceSaline.Limit;
            //double temp = WSA.SurfaceSaline.Limit;
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
            UnitNetwork.SurfaceSaline.Limit = value;
            //WSA.SurfaceSaline.Limit = value;
        }
        //
        public int geti_SurfaceWaterSalineNet()
        {

            int result = 0;
            double temp = UnitNetwork.SurfaceSaline.Net;
            double temp2 = UnitNetwork.SurfaceSaline.Limit;
            //double temp = WSA.SurfaceSaline.Net;
            //double temp2 = WSA.SurfaceSaline.Limit;
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Groundwater.Limit));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.Groundwater.Limit));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti groundwater. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Groundwater(int value)
        {
            UnitNetwork.Groundwater.Limit = value;
            //WSA.Groundwater.Limit = value;
        }
        //
        public int geti_GroundwaterNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(UnitNetwork.Groundwater.Net)) + 0);
            //int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Groundwater.Net)) + 0);
            return TempInt;
        }

        // ////////////////////////////////////////////////////////////////////////////////
        // 
        // Effluent And Reclaimed
        // 
        // ////////////////////////////////////////////////////////////////////////////////



        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the effluent. </summary>
        ///<remarks>     This is a bit deceiving, this is not effluent, it is the amount of reclaimed
        ///              water available to be use. Should fix this identifer to reflect this, but is a 
        ///              massive effort 
        ///              
        ///             I am glad that this variable is available! Sampson edits 08.07.18 - check this to
        ///             be certain.....
        /// </remarks>
        ///              
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Effluent()
        {
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Effluent.Limit));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.Effluent.Limit));

            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti effluent. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Effluent(int value)
        {
            SetReclaimed(value);
            // QUAY EDIT 3/22/18
            // Modified this code to be simpler
            // Also there is a naming issue here, this is not efflunet, rather effluent reused
            //UnitNetwork.Effluent.Limit = value;
            //WSA.Effluent.Limit = value;
            //if (invokePolicies)
            //{

            //maxReclaimed = MaxReclaimed();
            // "value" is the default amount for each state. Added is the user-defined
            // request, balanced by the reasonable amount as defined by indoor water use,
            // allowing for some lost due to leaks and efficiencies of production and use
            //WSA.Effluent.Limit = Math.Min(maxReclaimed, (double)value + effluentToAdd);
            //UnitNetwork.Effluent.Limit = Math.Min(maxReclaimed, effluentToAdd);
            //WSA.Effluent.Limit = Math.Min(maxReclaimed, effluentToAdd);
            // Added here from elsewhere on 12.14.16 DAS
            // At present "staticEffluentAdd" is not used- 12.19.16
            //staticEffluentAdd = effluentToAdd;
            //}
            // End Edit
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets an effluent. </summary>
        /// <remarks>   3/22/2018. </remarks>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetReclaimed(double value)
        {
            if (invokePolicies)
            {
                // AvailableReclaimed looks at how much reclaimed water is available given eligible effluent produced and policy on reuse
                UnitNetwork.Effluent.Limit = AvailableReclaimed();
            }
            else
            {
                // There is no policy on reuse so just brute force set this, map have no bsis in reality
                UnitNetwork.Effluent.Limit = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti effluent net. </summary>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_EffluentNet()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(UnitNetwork.Effluent.Net)) + 0);
            //int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Effluent.Net)) + 0);
            return TempInt;
        }

        // EDIT QUAY 3/22/18
        // NEW CODE
        // The Base (first year) 
        // double FBasePctEffluentUsed = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Available reclaimed. </summary>
        ///  <remarks>  Returns the amount of real effluent that can be used for reclaimed</remarks>
        /// <remarks>   3/22/2018. </remarks>
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        //double AvailableReclaimed()
        public double AvailableReclaimed()
        {
            double result = 0;
            // Calculate the portion of the Theorit ical Amount of effluent that can be reclaimed
            // MaxReclaimed() returns the Theoretical portion of effluent that potentially be used
            // FPctEffluentUsed is the amunt allowed to be used
            double MaxAvailableReclaimed = MaxReclaimed() * d_reclaimedWaterUse;
            // See which is bigger, the amount now in the Effluent Resource or the theorotical amount

            result = Math.Max(MaxAvailableReclaimed, UnitNetwork.Effluent.Limit);
            // sampson edits 09.08.18
            // Gray Water Use was invoked. Must subtract gray water from waste water to update available reclaimed
            if (grayWaterEffInvoked)
            {
                result = UnitNetwork.Effluent.Limit;

            }
            // end sampson edits 09.08.18

            //result = Math.Max(MaxAvailableReclaimed, UnitNetwork.Effluent.Limit);
            //
            return result;
        }
        double _maxReclaimable = 0;
        double maxReclaimable
        {
            get { return _maxReclaimable; }
            set { _maxReclaimable = value; }
        }
        // END EDIT 3/22/18

       // double _maxReclaimed = 0;
        //QUAY EDIT 3/22/18
        // Modified this to set up ability to assign these factors to each region
        // Kept some structure so there will not be a need for a lot of code rewrite

        // This is not being used anymore so I did not expand this. All references are to code that is not used.
        const double maxReclaimedRatio = 0.95;
        //const double consumptive = 0.86; // leaks http://www3.epa.gov/watersense/pubs/indoor.html
        // Need more flexibility- it is a GAME, and we need a response for reclaimed (recycled) water
        const double consumptive = 0.97; // leaks 
                                         //const double indoor = 0.45;
                                         //const double indoor = 0.65;

        // Sampson edits - 08.14.18
        // NOT BEING USED at the moment
        //public double AvailableGray()
        //{
        //    double result = 0;
        //    // Calculate the portion of the Theoritical Amount of effluent that can be reclaimed
        //    // MaxReclaimed() returns the Theoretical portion of effluent that potentially be used
        //    // FPctEffluentUsed is the amunt allowed to be used
        //    double MaxAvailableGray = MaxGrayWater() * d_grayWaterUse;
        //    // See which is bigger, the amount now in the Effluent Resource or the theorotical amount
        //    result = Math.Max(MaxAvailableGray, UnitNetwork.Effluent.Limit);
        //    return result;
        //}

        const double grayWater = 0.76;
        const double maxGrayWaterRatio = 0.95;
        //
        double _grayWaterUsed = 0;
        double grayWaterUsed
        {
            set { _grayWaterUsed = value; }
            get { return _grayWaterUsed; }
        }
        // 09.07.18 sampson edits
        void calculateGrayWater()
        {
            bool alterDemand = false;
            int i_gwm = this.geti_GrayWaterManagement();
            int i_rwm = this.geti_ReclaimedWaterManagement();
            int i_gwFlow = this.geti_GrayWaterFlow();
            int i_eff = this.geti_Effluent();
            // int i_effMax = 0;
            // =============================================

            int year = this.currentYear;
            // ====

            // if the user has selected to not use gray water then this method is null and void
            if (i_gwm == 0) { }
            // if the user as NOT selected to use waste water then we can use Gray Water if, and only if, the region does not have
            // effluent in the input file. PERHAPS this needs changing.. i.e., ignore the effluent in the USGS data file and only
            // consider whether or not they are using the reclaimed water management option...NEED to run this by Ray.
            // 09.06.2018 david arthur sampson. 
            else
            {
                alterDemand = true;
                //
                double d_gray = i_gwm;
                double d_gwFlow = maxGrayWater; // i_gwFlow;
                double d_eff = i_eff;
                double available = 0;
                double diffEffluent = 0;
                double newEffluent = 0;
                double d_rwm = i_rwm;
                //
                available = d_gwFlow * d_gray / 100;
                double temp = maxReclaimed; //  
                double Temp = 0;
                //defaultPCTReclaimed
                //if (i_eff > 0) // Have effluent in their USGS starting point - 2010
                if(defaultPCTReclaimed > 0)
                {
                    if (i_rwm > 0) // either default estimate or reclaimed water management has been invoked - all providers have access to reclaimed
                    {
                        grayWaterEffInvoked = true;
                        diffEffluent = temp - available;
                        Temp = Math.Min(diffEffluent, d_eff);
                        newEffluent = Temp;
                        seti_Effluent((int)newEffluent);
                      }
                    //
                    grayWaterUsed = available;
                    //
                }
                else // Have no effluent listed in the USGS data
                {
                    if (i_rwm > 0) // reclaimed water management has been invoked - all providers have access to reclaimed
                    {
                        // this could be a problem
                        grayWaterEffInvoked = true;
                        newEffluent = Math.Max(0,(d_rwm/100 * maxReclaimed) - available);
                        seti_Effluent((int)newEffluent);
                    }
                    else // no change in effluent (reclaimed) - just demand
                    {
                    }
                    //
                    grayWaterUsed = available;
                    //
                }
            }
            //
            if (alterDemand)
            {
                double temp = 0;
                int urban = geti_Urban();
                double newDemand = Math.Max(0,urban - grayWaterUsed);
                seti_Urban((int)newDemand); // set the reduced water demand by the urban sector
                GrayWaterFlow(); // reset the graywater flow because demand changed
                int unitcode = this.FUnitCode; // for a debug break point
            }

        }
        // 09.07.18 end sampson edits

        // sampson edits - 09.08.18
        bool _grayWaterInvoked = false;
        public bool grayWaterEffInvoked
        {
            get { return _grayWaterInvoked; }
            set { _grayWaterInvoked = true; }
        }
        // end sampson edits 09.08.18

        //---------------------------------------------
        // Water Consumption Ratio
        // This is the amount of indoor water use that is actually consumed and does not go down the drain
        // 
        //----------------------------------------------

        const double defaultConsumption = 0.95;
        internal double Fconsumption = defaultConsumption;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the consumption Ratio, amount of indoor water consumed.</summary>
        /// <value> The consumption.</value>
        ///-------------------------------------------------------------------------------------------------

        public double consumption
        {
            get { return Fconsumption; }
            set { Fconsumption = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti consumption ratio.</summary>
        /// <remarks> This is the ratio of indoor water that us actually consumed and not returned through 
        ///           a sewer or septic system</remarks>
        ///           <remarks> Quay, 3/22/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_consumptionRatio()
        {
            return (int)(Fconsumption * 100);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti consumption ratio.</summary>
        /// <remarks> This is the ratio of indoor water that us actually consumed and not returned through 
        ///           a sewer or septic system</remarks>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_consumptionRatio(int value)
        {
            Fconsumption = (double)value / 100;
        }

        //------------------------------------------------------------------
        // 
        // Indoor ratio
        // 
        // The ration of indoor water use to total urban water use
        // This is a pretty iffy number, this is ration of total urban, As acommnity has more non landscape orieted 
        // commercial and industrial the more this is high value in general
        // This ratio in warmer climates is lower, however this is now setup so we can estimate a better ration for each
        // region
        // -------------------------------------------
        // 
        const double defaultIndoor = 0.65;
        internal double Findoor = defaultIndoor;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the indoor ration of urban water use.</summary>
        /// <value> The indoor.</value>
        ///-------------------------------------------------------------------------------------------------

        public double indoor
        {
            get { return Findoor; }
            set { Findoor = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti indoor ratio of total urban Water use.</summary>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_indoorRatio()
        {
            return (int)(Findoor * 100);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti indoor ratio.</summary>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_indoorRatio(int value)
        {
            Findoor = (double)value / 100;
        }
        //-----------------------------------------------------------------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the maximum reclaimed.</summary>
        /// <value> The maximum reclaimed.</value>
        /// <remarks> This is not being used.  Existing code references are in routines
        ///           not be used </remarks>
        ///-------------------------------------------------------------------------------------------------

        //public double maxReclaimed
        //{
        //    set { _maxReclaimed = value; }
        //    get { return _maxReclaimed; }
        //}
        double staticEffluent = 0;

        // This is not beging used 
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


        // NOTE QUAY 3/22/18
        // This is no longer being used
        //double effluentToAdd
        //{
        //    get
        //    {
        //        double temp = 0;
        //        temp = (d_reclaimedWaterUse) * UnitNetwork.Urban.Demand;
        //        //temp = (d_reclaimedWaterUse) * WSA.Urban.Demand;
        //        return Math.Min(maxReclaimed, temp);
        //    }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Wastewater flow  from urban area.</summary>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        public double WasteWaterFlow()
        {
            double temp = 0;
            temp = consumptive * ((UnitNetwork.Urban.Demand) * indoor);
            //temp = maxReclaimedRatio * consumptive * ((WSA.Urban.Demand) * indoor);

            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti waste water flow generated by Urban Indoor Water use.</summary>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_WasteWaterFlow()
        {
            return (int)WasteWaterFlow();
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Maximum reclaimed.</summary>
        /// <remarks> Quay 3/22/18 Modified this to use Wastwwater management</remarks>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------
        double _maxReclaimed = 0;
        public double MaxReclaimed()
        {
            double temp = 0;
            temp = maxReclaimedRatio * WasteWaterFlow();
            maxReclaimed = temp;
            //
            // 08.28.18 das
            double Temp = 0;
            Temp = temp * grayWater; // GrayWaterFlow(); // need to reset temp after passing through GrayWaterFlow()
            maxGrayWater = Temp;
            //temp = consumptive * ((UnitNetwork.Urban.Demand) * indoor);
            return temp;
        }
        double maxReclaimed
        {
            get { return _maxReclaimed; }
            set { _maxReclaimed = value; }
        }
        // ============================================
        // Gets and Sets to the Reclaimed Water Management   
        //--------------------------------
        /// <summary>
        /// Seti ReclaimedWaterUse
        /// </summary>
        /// <returns>an int from zero to 100   . </returns>
        double d_reclaimedWaterUse = 0.00;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti reclaimed water management.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

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
        // =================================================================================================
        //

   


        // Sampson edits 08.14.18 for Gray Water - for use as an indicator (Framework #6, Specified #2)
        //                                          indicator number 137
        // NOT IN THE PARAMETER MANAGER
        // ----------------------------
        public double MaxGrayWater()
        {
            double temp = 0;
            temp = maxGrayWaterRatio * GrayWaterFlow();

            //temp = consumptive * ((UnitNetwork.Urban.Demand) * indoor);
            return temp;
        }

        double d_grayWaterUse = 0.00;
        //
        /// <summary>
        ///  Get the int for Gray Water Use
        /// </summary>
        /// <returns></returns>
        public int geti_GrayWaterManagement()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_grayWaterUse * 100));
            return TempInt;
        }
        ///------------------------------------------------------------------
        /// <summary>   Set the double for Gray Water Management</summary>
        ///
        /// <param name="value"> The value (zero to 1.00). </param>
        ///------------------------------------------------------------------
        public void seti_GrayWaterManagement(int value)
        {
            d_grayWaterUse = (Double)value / 100;
        }

        /// <summary>
        ///  and estimate of the total amount of Gray Water Produced
        /// </summary>
        /// <returns></returns>
        public double GrayWaterFlow()
        {
            double temp = 0;
            //bool alterDemand = false;
            temp = maxReclaimedRatio* (consumptive * ((UnitNetwork.Urban.Demand) * indoor * grayWater));

            return temp;
        }
        /// <summary>
        /// Get the Gray Water Produced
        /// </summary>
        /// <returns></returns>
        public int geti_GrayWaterFlow()
        {
            return (int)GrayWaterFlow();
        }
        double _maxGrayWater = 0;
        double maxGrayWater
        {
            get { return _maxGrayWater; }
            set { _maxGrayWater = value; }
        }

        //public void set_GrayWaterPotential()
        //{
        //    //
        //    int ModelCount = 24;
        //    int[] RecToUrban = new int[ModelCount];
        //    int[] multiply = new int[ModelCount];
        //    int[] gray = new int[ModelCount];

        //    RecToUrban =  this.geti_Effluent();
        //    multiply = this.geti_GrayWaterManagement();
        //    gray = this.geti_GrayWaterFlow();
        //    grayWaterPotential(RecToUrban, gray, multiply);

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="baseRecUrban"></param>
        ///// <param name="GrayWater"></param>
        ///// <param name="management"></param>
        //public void grayWaterPotential(int[] baseRecUrban, int[] GrayWater, int[] management)
        //{
        //    //
        //    int[] iGray = new int[ModelCount];
        //    double G = 0;
        //    for (int i = 0; i < ModelCount; i++)
        //    {
        //        int result = 0;
        //        double Temp = 0;
        //        double gray = 0;
        //        G = (double)GrayWater[i];
        //        if (baseRecUrban[i] <= 2)
        //            gray = management[i] * G;
        //        else
        //            Temp = baseRecUrban[i];
        //        if (gray > 0) { result = (int)gray; }
        //        else result = (int)Temp;
        //        iGray[i] = result;
        //    }

        //    // ModelParameterClass MP = FWSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent);
        //    // MP.ProviderProperty.setvalues(In);
        //    this.seti_Effluent(iGray);
        //    //  this.MYWaterSimModel.Effluent.setvalues(In);

        //}
        // =================================================================================================

        // /////////////////////////////////////////////////////////////////////////////////////////////


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti total supplies.</summary>
        ///
        /// <remarks> Quay, 3/22/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

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
            double Temp = UnitNetwork.Urban.Demand + UnitNetwork.Industrial.Demand + UnitNetwork.Agriculture.Demand + UnitNetwork.Power.Demand;
            //double Temp = WSA.Urban.Demand + WSA.Industrial.Demand + WSA.Agriculture.Demand + WSA.Power.Demand;
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Getd total net.</summary>
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double getd_TotalNet()
        {
            double Temp = UnitNetwork.Urban.Net + UnitNetwork.Industrial.Net + UnitNetwork.Agriculture.Net + UnitNetwork.Power.Net;
            //double Temp = WSA.Urban.Net + WSA.Industrial.Net + WSA.Agriculture.Net + WSA.Power.Net;
            return Temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the total demand ie sum of all consumers demand.</summary>
        /// <remarks> Quay 3/28/18  HOw did we miss this for this long?</remarks>
        /// <value> The total number of demand.</value>
        ///-------------------------------------------------------------------------------------------------

        public double TotalDemand
        {
           get { return getd_TotalDemand(); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the total net demand for all consumers.</summary>
        /// <remarks> Quay 3/28/18  HOw did we miss this for this long?</remarks>
        /// <value> The total number of net.</value>
        ///-------------------------------------------------------------------------------------------------

        public double TotalNet
        {
            get { return getd_TotalNet();  }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the total Net demand.</summary>
        ///
        /// <value> The total net demand for all consumers.</value>
        ///-------------------------------------------------------------------------------------------------

        public int geti_TotalDemandNet()
        {
            return (int)getd_TotalNet(); 
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti total demand.</summary>
        ///
        /// <remarks> Quay, 3/28/2018.</remarks>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_TotalDemand()
        {
            return (int)getd_TotalDemand();
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti net demand difference.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_NetDemandDifference()
        {
            //int TotalSupplies = geti_TotalSupplies();
            int tempInt = 0;
            double temp = 0;
            //
            double demand = getd_TotalDemand();
            double net = getd_TotalNet();
            if (0 < demand)
            {
                temp = Math.Min(100, Math.Max(0, (net / demand) * 100));
            }
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Urban.Demand));
            // int TempInt = Convert.ToInt32(Math.Round(WSA.Urban.Demand));
             return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti urban. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Urban(int value)
        {
            UnitNetwork.Urban.Demand = value;
            //WSA.Urban.Demand = value;
            // QUAY EDIT 2 18 18
            // These are not be used, I removed them
            //i_demand_urban = value;
            // End EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti urban net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Urban_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(UnitNetwork.Urban.Net)));
            //int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Urban.Net)));
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Agriculture.Demand));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.Agriculture.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti agriculture. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Agriculture(int value)
        {
            UnitNetwork.Agriculture.Demand = value;
            //WSA.Agriculture.Demand = value;
            //  QUAY EDIT 2 18 18
            // These are not be used, I removed them
            // i_demand_ag = value;
            // EDN EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti agriculture net. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Agriculture_Net()
        {
            int TempInt = Convert.ToInt32(Math.Abs(Math.Round(UnitNetwork.Agriculture.Net)));
            //int TempInt = Convert.ToInt32(Math.Abs(Math.Round(WSA.Agriculture.Net)));
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Industrial.Demand));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.Industrial.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti insustrial. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Industrial(int value)
        {
            UnitNetwork.Industrial.Demand = value;
            //WSA.Industrial.Demand = value;
            //  QUAY EDIT 2 18 18
            // These are not be used, I removed them
            //i_demand_industry = value;
            // EDN EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti industrial net. </summary>
        ///
        /// 
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_Industrial_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(UnitNetwork.Industrial.Net)));
            //int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Industrial.Net)));
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
            int TempInt = Convert.ToInt32(Math.Round(UnitNetwork.Power.Demand));
            //int TempInt = Convert.ToInt32(Math.Round(WSA.Power.Demand));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti power. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_PowerWater(int value)
        {
            UnitNetwork.Power.Demand = value;
            //WSA.Power.Demand = value;
            //  QUAY EDIT 2 18 18
            // These are not be used, I removed them
            //i_demand_power = value;
            // EDN EDIT 2 18 18
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti power water net.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PowerWater_Net()
        {
            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(UnitNetwork.Power.Net)));
            //            int TempInt = Convert.ToInt32(Math.Round(Math.Abs(WSA.Power.Net)));
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


        void GetInitialPower()
        {
            int temp = UnitNetwork.InitialPowerGenerated;
            initialPower = temp;
        }
        // QUAY EDIT 3/9/18
        // OPPS MISSED THIS ONE,not using state indexes anymore
        //void InitialPower(string state)
        // {

        //     //int temp = UnitNetwork.InitialPowerGenerated(state);
        //     int temp = UnitNetwork.InitialPowerGenerated;
        //     //int temp = WSA.InitialPowerGenerated(state);
        //     initialPower = temp;
        // }
        // END EDIT 3/9/18

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the initial power.</summary>
        ///
        /// <value> The initial power.</value>
        ///-------------------------------------------------------------------------------------------------

        internal int initialPower
        {
            get { return _initialPower; }
            set { _initialPower = value; }
        }


        int _powerEnergy = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti power energy.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_PowerEnergy()
        {
            int temp = _powerEnergy;
            return temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti power energy.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

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
        //int i_droughtYear = 2015;
        //public int startDroughtYear
        //{
        //    get
        //    {
        //        return i_droughtYear;
        //    }
        //    set
        //    {
        //        i_droughtYear = value;
        //    }
        //}
        ///// <summary>

        // External Drivers 
        // =================================================================
        // Population Growth Rate 
        //-----------------------------------------------

        ///-----------------------------------------------------------------
        /// <summary>   Gets the geti population growth rate. </summary>
        ///
        /// <returns>an int from zero to 150 </returns>
        ///-----------------------------------------------------------------
        double d_popGrowthRateMod = 1.00;
        public int geti_PopGRateModifier()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_popGrowthRateMod * 100));
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
        public void seti_PopGRateModifier(int value)
        {
            d_popGrowthRateMod = ((Double)value / 100);
        }
        // ========================================
        public double AdjustPopulations
        {
            get { return d_popGrowthRateMod; }
        }

        //--------------------------------------------------------------------------------------------
        // =================================================================
        // Population Growth Rate 
        //-----------------------------------------------

        double d_popGrowthRate = 1.00;
        ///-----------------------------------------------------------------
        /// <summary>   Gets the geti population growth rate. </summary>
        ///
        /// <returns>an int from zero to 150 </returns>
        ///-----------------------------------------------------------------

        public int geti_PopGrowthRate()
        {
            // Edit Quay 3/19/18
            // This has been confusing, end up with two sets of population variables
            // Modifying to work with my new code, which may not be the best code
            //int TempInt = Convert.ToInt32(Math.Round(d_popGrowthRate * 100));
            int TempInt = (int)(FPopRate * 100);
            // END EDIT 3/19/18

            return TempInt;
        }

        ///----------------------------------------------------------------
        /// <summary>   Seti population growth rate. </summary>
        ///
        /// <param name="value">    The value. </param>
        ///----------------------------------------------------------------
        public void seti_PopGrowthRate(int value)
        {
            // Edit Quay 3/19/18
            // This has been confusing, end up with two sets of population variables
            // Modifying to work with my new code, which may not be the best code
            //d_popGrowthRate = ((Double)value / 100);
            FPopRate = ((Double)value / 100);
            // END EDIT 3/19/18
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
        // EDIT QUAY 3/5/2018
        // CHNAGED THIS TO BE A 1% decline per year
        // Addded a Minimum GPCD
        //double d_urbanConservation = 1.00;
        double d_urbanConservation = 0.99;
        // CLEAN UP double FMinimumUrbanGOCD = 30;
        // END EDIT

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
        double _desalinization = 1.0;
        public int geti_Desalinization()
        {
            int TempInt = Convert.ToInt32(Math.Round(_desalinization * 100));
            return TempInt;
        }
        public void seti_Desalinization(int value)
        {
            _desalinization = (Double)value / 100;
        }
        public double Desalinization
        {
            set { _desalinization = value; }
            get { return _desalinization; }
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
        /// <summary>   the Power Conservation Goal.</summary>
        ///
        /// <returns>an int from zero to 100   . </returns>
        ///--------------------------------------------------------------
        double d_powerConservation = 1.00;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti power conservation.</summary>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the power conservation.</summary>
        ///
        /// <value> The power conservation.</value>
        ///-------------------------------------------------------------------------------------------------

        public double PowerConservation
        {
            get { return d_powerConservation; }
        }
        // =====================================================================


        /// <summary> The industry conservation Goal.</summary>
        double d_industryConservation = 1.00;
        //public double IndustryConservation
        //{
        //    get { return d_industryConservation; }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti industry conservation goal.</summary>
        /// <returns> An int 0 - 100</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_IndustryConservation()
        {
            int TempInt = Convert.ToInt32(Math.Round(d_industryConservation * 100));
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the industry conservation.</summary>
        ///
        /// <value> The industry conservation Goal 0 - 100.</value>
        ///-------------------------------------------------------------------------------------------------

        public double IndustryConservation
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
            int TempInt = Convert.ToInt32(Math.Round(d_groundwaterControl * 100));
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Revise groundwater control.</summary>
        /// <remarks> Quay, 4/3/2018.</remarks>
        /// <remarks>  This method will rectify the grounwater control; goal with what the
        ///            current allocation of water is.  If the current allocation is less than
        ///            the goal, then the groundwater control value is set to what actual goal was acheived at this point
        ///            </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void ReviseGroundwaterControl()
        {
            double FinalAllocation = TheCRFNetwork.Groundwater.Allocated;
            double FinalRatio = 1;
            double FinalGoal = FBaseGW * d_groundwaterControl;
            if (FinalGoal > 0)
            {
                FinalRatio = FinalAllocation / FBaseGW;
            }
            bool TestBalance = true;
            if (TheCRFNetwork.Consumers.Demand > 0)
            {
                TestBalance = ( TheCRFNetwork.Consumers.PosNet / TheCRFNetwork.Consumers.Demand) < ReviseTolerance ; // CRF_Utility.BalanceTolerance;
            }
            if (TestBalance)
            {
                if (d_groundwaterControl > FinalRatio)
                {
                    double RatioDif = Math.Abs(d_groundwaterControl - FinalRatio);
                    if (RatioDif > CRF_Utility.BalanceTolerance)
                    {
                        d_groundwaterControl = FinalRatio;
                    }
                        //if (FinalRatio < 1)
                        //{
                        //    d_groundwaterControl = FinalRatio / FudgeFactorLow;
                        //}
                        //else
                        //{
                        //    d_groundwaterControl = FinalRatio / FudgeFactorLow;

                        //}
                }
            }
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
            int TempInt = Convert.ToInt32(Math.Round(d_surfaceWaterControl * 100));
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti surface water control.</summary>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SurfaceWaterControl(int value)
        {
            d_surfaceWaterControl = ((double)value) / 100;
        }

        double FudgeFactorHigh = 0.99;
        double FudgeFactorLow = 0.92;

        double ReviseTolerance = .02;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Revise surface control.</summary>
        /// <remarks> Quay, 4/3/2018.</remarks>
        /// <remarks>  This method will rectify the surfacewater control; goal with what the
        ///            current allocation of water is.  If the current allocation is less than
        ///            the goal, then the surfacewater control value is set to what actual goal was acheived at this point
        ///            </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void ReviseSurfaceControl()
        {
            // get the amount allocated this year
            double FinalAllocation = TheCRFNetwork.SurfaceFresh.Allocated;
            double FinalRatio = 1;
            
            // FBaseSurface was set during Initialize Variables and represents the initial state of the model
            if (FBaseSurfaceWater > 0)
            {
                // Metric for how different the current state is from the initial state (less than or greater than 1;
                FinalRatio = FinalAllocation / FBaseSurfaceWater;
            }
            bool TestBalance = true;
            // If there is demand
            if (TheCRFNetwork.Consumers.Demand > 0)
            {
                // test how big the deficit is compared to the demand,  Use the balance tolerance as the metric
                TestBalance = (TheCRFNetwork.Consumers.PosNet / TheCRFNetwork.Consumers.Demand) < CRF_Utility.BalanceTolerance;
            }
            if (TestBalance)
            {
                // ok if the deficit was relatively big and the surfacewater control has a percentage change bigger the the current state
                if (d_surfaceWaterControl > FinalRatio)
                {
                    // see how big is the difference
                    if (Math.Abs(d_surfaceWaterControl - FinalRatio) > ReviseTolerance); //       CRF_Utility.BalanceTolerance)
                    {
                        d_surfaceWaterControl = FinalRatio;
                        // if big enough estimate what the value of the control should be
                        // d_surfaceWaterControl = EstimateGoalFromDesiredValue(endYear - startYear, 2, FinalRatio);
                        //if (FinalRatio < 1)
                        //{
                        //    d_surfaceWaterControl = FinalRatio / FudgeFactorLow;
                        //}
                        //else
                        //{
                        //    d_surfaceWaterControl = FinalRatio / FudgeFactorLow;
                        //}
                    }
                }
            }
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
        // DROUGHT EFFECT
        // =====================================================================

        //const int DefaultDroughtStart = 2015;
        //const int DefaultDroughtEnd = 2100;
        //const double DefaultDroughtDepth = 0.60;
        //const int DefaultDroughtActive = 0;

        //int FDroughtStartYear =  DefaultDroughtStart;
        //int FDroughtLength = DefaultDroughtEnd - DefaultDroughtStart;
        //int FDroughtActive = DefaultDroughtActive;
        //double FDroughtDepth =DefaultDroughtDepth;

         
        int FDroughtStartYear = 2015;
        int FDroughtLength = 9999;
        int FDroughtActive = 0;
        double FDroughtDepth = 0.0;
        // this is the drought control, will set up for scenarios
        double d_drought = 1.0;

        // default drought factors
        int _DefaultDroughtStart = 2015;
        int _DefaultDroughtEnd = 2100;
        double _DefaultDroughtDepth = 0.60;
        int _DefaultDroughtActive = 0;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default start drought year.</summary>
        /// <value> The default start drought.</value>
        ///-------------------------------------------------------------------------------------------------

        public int DefaultDroughtStart
        {
            get { return _DefaultDroughtStart; }
            set
            {
                _DefaultDroughtStart = value;
                ////WSmith.startDroughtYear = value;
                //WestModel.StartDroughtYear = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default end year for drought.</summary>
        /// <value> The default end drought.</value>
        ///-------------------------------------------------------------------------------------------------

        public int DefaultDroughtEnd
        {
            get { return _DefaultDroughtEnd; }
            set { _DefaultDroughtEnd = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default drought depth.</summary>
        /// <value> The default drought depth.</value>
        ///-------------------------------------------------------------------------------------------------

        public double DefaultDroughtDepth
        {
            get { return _DefaultDroughtDepth;  }
            set { _DefaultDroughtDepth = value;  }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the default drought active state, 0 = is no drought, drought off  1 = active drought.</summary>
        /// <value> The default drought active.</value>
        ///-------------------------------------------------------------------------------------------------

        public int DefaultDroughtActive
        {
            get { return _DefaultDroughtActive; }
            set { _DefaultDroughtActive = value; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti drought control. </summary>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DroughtControl()
        {
            int TempInt = Convert.ToInt32(d_drought * 100);
            return TempInt;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti drought control. </summary>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtControl(int value)
        {
            d_drought = ((double)value) / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought depth.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DroughtDepth()
        {
            return (int)(FDroughtDepth * 100);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drough depth.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtDepth(int value)
        {
            FDroughtDepth = (double)value / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drough start year.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughStartYear(int value)
        {
            FDroughtStartYear = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought sart year.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DroughtSartYear()
        {
            return FDroughtStartYear;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought length.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtLength(int value)
        {
            FDroughtLength = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought length.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DroughtLength()
        {
            return FDroughtLength;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought active.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="value"> The value.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtActive(int value)
        {
            FDroughtActive = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti droughtactive.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_DroughtActive()
        {
            return FDroughtActive;
        }


        // ===================================================================
        // CLIMATE CHANGE
        // ===================================================================

        const double DefaultCCChangeTarget = 1.0;
        const double DefaultCCYearTarget = 2100;
        const double DefaultCCChangeCoefficient = 1;
        const double DefaultCCChangeLimit = 1;


        // Used to create a curve of climate change effect
        double FCCChangeCoefficient = DefaultCCChangeCoefficient;

        // The min and max changes allowed for climnate change
        const double MaxCCChange = 2.0;
        const double MinCCChange = 0.10;

        // this is the field used for change coeeficient, and is the Max or Min values based on desired goal
        double FCCChangeLimit = DefaultCCChangeLimit;

        // This is the climate change target value for amount of change by target year
        double FCCChangeTarget = DefaultCCChangeTarget;
        // this is the target year for climate change to be fully realized
        double FCCYearTarget = DefaultCCYearTarget;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the climate change target. </summary>
        ///
        /// <value> The climate change target. </value>
        ///-------------------------------------------------------------------------------------------------

        public double ClimateChangeTarget
        {
            get { return FCCChangeTarget; }
            set { FCCChangeTarget = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the climate change target year. </summary>
        ///
        /// <value> The climate change target year. </value>
        ///-------------------------------------------------------------------------------------------------

        public double ClimateChangeTargetYear
        {
            get { return FCCYearTarget; }
            set { FCCYearTarget = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target. </summary>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTarget(int value)
        {
            FCCChangeTarget = (double)value / 100;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target. </summary>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_ClimateChangeTarget()
        {
            return (int)(FCCChangeTarget * 100);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target year. </summary>
        /// <param name="value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTargetYear(int value)
        {
            FCCYearTarget = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target year. </summary>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_ClimateChangeTargetYear()
        {
            return (int)FCCYearTarget;
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
        // ==============================================================================
        //#region DenverWaterDemandModel
        //// 
        //#region Inputs
        //    // =======================================================================
        //    // Dwelling Units Per Acre
        //    //--------------------------------
        //    double d_dwellingUnitsAcre = 0;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <returns></returns>
        //    public void Seti_DwellingUnitsAcre_DM(int value)
        //    {
        //        d_dwellingUnitsAcre = ((Double)value / 100);     
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <returns></returns>
        //    public int Geti_DwellingUnitsAcre_DM()
        //    {
        //        int temp = Convert.ToInt32(d_dwellingUnitsAcre);
        //        return temp;
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double DwellingUnitsAcre_dm
        //    {
        //        get { return d_dwellingUnitsAcre; }
        //        set { d_dwellingUnitsAcre = value; }
        //    }
        //    //
        //#endregion Inputs
        //// =======================================================================
        //// Outputs David
        //#region Outputs
        //// =======================================================================
        //// Total Water Demand
        ////--------------------------------
        //double d_totalWaterDemand = 0;

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        public int Geti_TotalWaterDemand_DM()
        //        {
        //            int temp = Convert.ToInt32(d_totalWaterDemand);
        //            return temp;
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public double TotalWaterDemand_dm
        //        {
        //            get { return d_totalWaterDemand; }
        //        }
        //        //
        //        // =======================================================================
        //        // Indoor Water Demand
        //        //--------------------------------
        //        double d_indoorWaterDemand = 0;
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        public int Geti_IndoorWaterDemand_DM()
        //        {
        //            int temp = Convert.ToInt32(d_indoorWaterDemand);
        //            return temp;
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public double IndoorWaterDemand_dm
        //        {
        //            get { return d_indoorWaterDemand; }
        //        }
        //        // =======================================================================

        //        // =======================================================================
        //        // Outdoor Water Demand
        //        //--------------------------------
        //        double d_outdoorWaterDemand = 0;
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        public int Geti_OutdoorWaterDemand_DM()
        //        {
        //            int temp = Convert.ToInt32(d_outdoorWaterDemand);
        //            return temp;
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public double OutdoorWaterDemand_dm
        //        {
        //            get { return d_outdoorWaterDemand; }
        //        }
        //        // =======================================================================

        //        // =======================================================================
        //        // Total Process Water Demand
        //        //--------------------------------
        //        double d_totalProcessWaterDemand = 0;
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        public int Geti_TotalProcessWaterDemand_DM()
        //        {
        //            int temp = Convert.ToInt32(d_totalProcessWaterDemand);
        //            return temp;
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public double TotalProcessWaterDemand_dm
        //        {
        //            get { return d_totalProcessWaterDemand; }
        //        }
        //        // =======================================================================

        //        // =======================================================================
        //        // Total Number of Agents
        //        //--------------------------------
        //        double d_totalAgents = 0;
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <returns></returns>
        //        public int Geti_TotalAgents_DM()
        //        {
        //            int temp = Convert.ToInt32(d_totalAgents);
        //            return temp;
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public double TotalAgents_dm
        //        {
        //            get { return d_totalAgents; }
        //        }
        //    // =======================================================================

        //    // =======================================================================
        //    // Total Number of Agents
        //    //--------------------------------
        //    double d_gallonsUnitYear = 0;
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <returns></returns>
        //    public int Geti_GallonUnitYear_DM()
        //    {
        //        int temp = Convert.ToInt32(d_totalAgents);
        //        return temp;
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public double GallonsUnitYear_dm
        //    {
        //        get { return d_gallonsUnitYear; }
        //    }
        //    // =======================================================================

        //    #endregion Outputs

        //#endregion DenverWaterDemandModel
        // FLUXES
        #region fluxes
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <returns>   The flux allocated, 0 if not found. </returns>
        ///-------------------------------------------------------------------------------------------------
        public int GetFluxAllocated(string ResField, string ConsField)
        {
            int result = 0;
            CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField, ConsField);
            if (theFlux != null)
            {
                double value = theFlux.Allocated();
                try
                {
                    int tempint = Convert.ToInt32(value);
                    result = tempint;
                }
                catch (Exception ex)
                {
                    // ouch
                }
            }
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets flux allocated value. </summary>
        /// <param name="ResField">     The resource field. </param>
        /// <param name="ConsField">    The cons field. </param>
        /// <param name="aValue">       The value. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SetFluxAllocation(string ResField, string ConsField, double aValue)
        {
            CRF_Flux theFlux = TheCRFNetwork.FindFlux(ResField, ConsField);
            if (theFlux != null)
            {
                theFlux.SetAllocation(aValue);
            }
        }

        //======================================
        // Gets and Sets for SUR_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SUR_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SUR_UD()
        {
            int result = 0;
            result = GetFluxAllocated("SUR", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SUR_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SUR_UD(int value)
        {
            SetFluxAllocation("SUR", "UTOT", value);
        }

        //======================================
        // Gets and Sets for SUR_AD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SUR_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SUR_AD()
        {
            int result = 0;
            result = GetFluxAllocated("SUR", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SUR_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SUR_AD(int value)
        {
            SetFluxAllocation("SUR", "ATOT", value);
        }

        //======================================
        // Gets and Sets for SUR_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SUR_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SUR_ID()
        {
            int result = 0;
            result = GetFluxAllocated("SUR", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SUR_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SUR_ID(int value)
        {
            SetFluxAllocation("SUR", "ITOT", value);
        }

        //======================================
        // Gets and Sets for SUR_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SUR_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SUR_PD()
        {
            int result = 0;
            result = GetFluxAllocated("SUR", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SUR_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SUR_PD(int value)
        {
            SetFluxAllocation("SUR", "PTOT", value);
        }

        //======================================
        // Gets and Sets for SURL_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SURL_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SURL_UD()
        {
            int result = 0;
            result = GetFluxAllocated("SURL", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SURL_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SURL_UD(int value)
        {
            SetFluxAllocation("SURL", "UTOT", value);
        }

        //======================================
        // Gets and Sets for SURL_AD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SURL_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SURL_AD()
        {
            int result = 0;
            result = GetFluxAllocated("SURL", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SURL_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SURL_AD(int value)
        {
            SetFluxAllocation("SURL", "ATOT", value);
        }

        //======================================
        // Gets and Sets for SURL_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SURL_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SURL_ID()
        {
            int result = 0;
            result = GetFluxAllocated("SURL", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SURL_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SURL_ID(int value)
        {
            SetFluxAllocation("SURL", "ITOT", value);
        }

        //======================================
        // Gets and Sets for SURL_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SURL_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SURL_PD()
        {
            int result = 0;
            result = GetFluxAllocated("SURL", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SURL_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SURL_PD(int value)
        {
            SetFluxAllocation("SURL", "PTOT", value);
        }

        //======================================
        // Gets and Sets for GW_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get GW_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_GW_UD()
        {
            int result = 0;
            result = GetFluxAllocated("GW", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set GW_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_GW_UD(int value)
        {
            SetFluxAllocation("GW", "UTOT", value);
        }

        //======================================
        // Gets and Sets for GW_AD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get GW_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_GW_AD()
        {
            int result = 0;
            result = GetFluxAllocated("GW", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set GW_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_GW_AD(int value)
        {
            SetFluxAllocation("GW", "ATOT", value);
        }

        //======================================
        // Gets and Sets for GW_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get GW_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_GW_ID()
        {
            int result = 0;
            result = GetFluxAllocated("GW", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set GW_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_GW_ID(int value)
        {
            SetFluxAllocation("GW", "ITOT", value);
        }

        //======================================
        // Gets and Sets for GW_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get GW_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_GW_PD()
        {
            int result = 0;
            result = GetFluxAllocated("GW", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set GW_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_GW_PD(int value)
        {
            SetFluxAllocation("GW", "PTOT", value);
        }

        //======================================
        // Gets and Sets for REC_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get REC_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_REC_UD()
        {
            int result = 0;
            result = GetFluxAllocated("REC", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set REC_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_REC_UD(int value)
        {
            SetFluxAllocation("REC", "UTOT", value);
        }

        //======================================
        // Gets and Sets for REC_AD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get REC_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_REC_AD()
        {
            int result = 0;
            result = GetFluxAllocated("REC", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set REC_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_REC_AD(int value)
        {
            SetFluxAllocation("REC", "ATOT", value);
        }

        //======================================
        // Gets and Sets for REC_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get REC_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_REC_ID()
        {
            int result = 0;
            result = GetFluxAllocated("REC", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set REC_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_REC_ID(int value)
        {
            SetFluxAllocation("REC", "ITOT", value);
        }

        //======================================
        // Gets and Sets for REC_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get REC_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_REC_PD()
        {
            int result = 0;
            result = GetFluxAllocated("REC", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set REC_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_REC_PD(int value)
        {
            SetFluxAllocation("REC", "PTOT", value);
        }

        //======================================
        // Gets and Sets for SAL_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SAL_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SAL_UD()
        {
            int result = 0;
            result = GetFluxAllocated("SAL", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SAL_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SAL_UD(int value)
        {
            SetFluxAllocation("SAL", "UTOT", value);
        }

        //======================================
        // Gets and Sets for SAL_AD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SAL_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SAL_AD()
        {
            int result = 0;
            result = GetFluxAllocated("SAL", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SAL_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SAL_AD(int value)
        {
            SetFluxAllocation("SAL", "ATOT", value);
        }

        //======================================
        // Gets and Sets for SAL_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SAL_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SAL_ID()
        {
            int result = 0;
            result = GetFluxAllocated("SAL", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SAL_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SAL_ID(int value)
        {
            SetFluxAllocation("SAL", "ITOT", value);
        }

        //======================================
        // Gets and Sets for SAL_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get SAL_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_SAL_PD()
        {
            int result = 0;
            result = GetFluxAllocated("SAL", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set SAL_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_SAL_PD(int value)
        {
            SetFluxAllocation("SAL", "PTOT", value);
        }

        // EDIT QUAY 9/12/18 Late at night
        // Add all the Colorado Fluxes here

        //======================================
        // Gets and Sets for COLAD
        //======================================


        ///-----------------------------------------------------------
        ///  <summary>  get COL_AD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_COL_AD()
        {
            int result = 0;
            result = GetFluxAllocated("COL", "ATOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set COL_AD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_COL_AD(int value)
        {
            SetFluxAllocation("COL", "ATOT", value);
        }

        //======================================
        // Gets and Sets for GW_UD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get COL_UD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_COL_UD()
        {
            int result = 0;
            result = GetFluxAllocated("COL", "UTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set COL_UD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_COL_UD(int value)
        {
            SetFluxAllocation("COL", "UTOT", value);
        }


        //======================================
        // Gets and Sets for COL_ID
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get COL_ID value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_COL_ID()
        {
            int result = 0;
            result = GetFluxAllocated("COL", "ITOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set COL_ID value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_COL_ID(int value)
        {
            SetFluxAllocation("COL", "ITOT", value);
        }

        //======================================
        // Gets and Sets for COL_PD
        //======================================

        ///-----------------------------------------------------------
        ///  <summary>  get COL_PD value </summary>
        /// <returns>  the value allocated for this flux. </returns>
        ///-----------------------------------------------------------
        public int geti_COL_PD()
        {
            int result = 0;
            result = GetFluxAllocated("COL", "PTOT");
            return result;
        }

        ///-----------------------------------------------------------
        ///  <summary>  set COL_PD value </summary>
        /// <param name="value">    The value to Allocate to the Flux Parameter. </param>
        ///-----------------------------------------------------------
        public void seti_COL_PD(int value)
        {
            SetFluxAllocation("COL", "PTOT", value);
        }

        // Field used to test if parameters loaded
        // CLEAN UP private bool FFluxParametersReady = false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the flux parameters ready. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        #endregion fluxes

    }
    #region Utilities
    static class utilities
    {
        /// <summary>
        ///  This constant is multiplied against gallons to obtain liters
        /// </summary>
        public const double convertGallonsToLiters = 3.785411784;


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
        /// <param name="Tmax"></param> Maximum annual, temperature
        /// <param name="Tmin></param>
        /// <param name="Tav"></param> Average temperature (Tmax + Tmin)/2
        /// <param name="Demandadd"></param> Liters per Capita Per Day to add to existing demand
        /// <returns></returns>
        //public static double TemperatureFunction_AddDemand(double population, int time, double TavScenario, double TavHistorical)
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


            if (T >= TemperatureBreak)
            {
                Tscaler = T - TemperatureBreak;
            }
            //
            temp = Tscaler * slope * 1 / convertGallonsToLiters; // LPCD proportion of change to galllons
            result = temp * population * daysInAYear(time);
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

        public static double AnnualExponentialChange(double StartValue, double period, double ExponentialCoef, double Limit)
        {
            double result = 1;
            double Fuck1 = (StartValue - Limit);
            double Fusk2 = (-1 * ExponentialCoef) * period;
            double Fuck3 = Math.Exp(Fusk2);
            double Fuck4 = Fuck3 * Fuck1;
            result = Limit + ((StartValue - Limit) * Math.Exp((-1 * ExponentialCoef) * period));
            return result;
        }
        #region Generic Functions
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

        #endregion
        public static double ExponentialDecayCoef(double TargetValue, double StartValue, double Limit, double StartPeriod, double TargetPeriod)
        {
            double result = 0;

            // OK, Check Target
            if (TargetValue < StartValue)
            {
                // Declining, Target can not be =< Limit, if it is adjust to be slightly above limit
                if (TargetValue <= Limit) TargetValue = Limit * 1.01;
            }
            else
            {
                // Increasing Declining, Target can not be >= Limit, if it is adjust to be slightly below limit
                if (TargetValue >= Limit) TargetValue = Limit * .99;
            }
            // OK, if Target is greater than Start and Target is Less than Limit (increasing) or
            //        Target os less that Start and Target is Greater than Limit (decreasing)
            if (((TargetValue > StartValue) && (Limit >= TargetValue)) || ((TargetValue < StartValue) && (TargetValue >= Limit)))
            {
                double LimitStart = Limit - StartValue;
                double LimitTarget = Limit - TargetValue;
                double Ratio = LimitStart / LimitTarget;
                double Periods = TargetPeriod - StartPeriod;
                double LN = Math.Log(Ratio);
                double Value = LN / Periods;

                result = Math.Log((Limit - StartValue) / (Limit - TargetValue)) / (TargetPeriod - StartPeriod);

            }
            return result;
        }
        public static double Forecast_CU(double Base_CU, double GrowthRateof_CU, double GrowthDamper, double period, double AdjustGrowth_CU, double AdjustDamper)
        {
            double result = 0;
            double rate = GrowthRateof_CU;

            try
            {
                // Dampem the growth rate so it slows over time, dividing by 100 becasue GrpwthRate is in Percent ie 1% = .01
                double CurrentRate = Dampen(rate / 100, GrowthDamper, period);
                // Apply Adjustment to Production Growth if not 1
                if (AdjustGrowth_CU != 1)
                {
                    // Dampen the effect of AdjustGrowth so its impact increases over time.
                    double AdjustProductionGrowth = Dampen(AdjustGrowth_CU, AdjustDamper, period);
                    CurrentRate = CurrentRate * AdjustProductionGrowth;
                }
                double P = Math.Pow(1 + CurrentRate, period);
                // Calculates F in Future = Present * A 

                result = Base_CU * P;
            }
            // END EDIT 3/2/18

            catch (Exception ex)
            {
                // Ouch Only thing going here is Math.Pow or Dampen Functions
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
        // ========================================================================================
        //
        public static double weightingByDemand(double Urban, double Ag, double Industry, double Power, int UD, int AD, int ID, int PD)
        {
            double result = 0; 
            double U = 0;
            double A = 0;
            double I=0;
            double P = 0;          
            //
            double totalDemand = UD + AD + ID + PD;
            U = Urban;
            A = Ag;
            I = Industry;
            P = Power;
            //
            try
            {
                if(0 < totalDemand)
                result = ((U) * (UD / totalDemand)) + ((A) * (AD / totalDemand)) 
                    + ((I) * (ID / totalDemand)) + ((P) * (PD / totalDemand));

            }
            catch (Exception ex)
            {
                // Ouch Only thing going here is Math.Pow or Dampen Functions
            }



            return result;
        }




        //
        // ===========================
        // Sampson edits 07.08.2018
        //public static double indoorGPCD(double ppl_HH, double rate, double damper, double period)
        //{
        //    double result = 0;
        //    double indoorGPCD = 0;
        //    const double slope = -21.9;
        //    const double intercept = 76.416;
        //    try
        //    {
        //        if (0 < ppl_HH && ppl_HH <= 10)
        //        {
        //            indoorGPCD = (slope * Math.Log(ppl_HH) + intercept) * Dampen(rate, damper, period);
        //        }
        //        result = indoorGPCD;
        //    }
        //    catch {

        //    }
        //    return result;
        //}

        //public static double KieferKrentz(double ppl_HH, double unitsPerAcre, double rate, double damper, double period)
        //{
        //    double result = 0;
        //    double gpud = 0;
        //    double gpcd = 0;
        //    const double slope = -71.23;
        //    const double intercept = 379.44;
        //    if(0 < unitsPerAcre)
        //    {
        //        gpud = intercept + slope*Math.Log(unitsPerAcre);
        //        gpcd = gpud * (1 / ppl_HH) * Dampen(rate, damper, period);
        //    }
        //    return result;
        //}
        // End Sampson edits 07.08.2018
        // ===============================
    }

    #endregion
}
