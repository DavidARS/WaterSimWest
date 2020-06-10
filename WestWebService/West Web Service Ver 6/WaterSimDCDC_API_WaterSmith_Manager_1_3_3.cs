using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsumerResourceModelFramework;
using WaterSimDCDC.America;
using WaterSimDCDC.Documentation;
using WaterSimDCDC.Processes;

namespace WaterSimDCDC
{
    //
    // Build Notes:
    // ver 1.0.0 - First version
    // ver 1.1.1 - 02.28.16:  
    // ver 1.2.0 - 03.06.16: Added desalination (using epAugmented) (DESAL-Desal), Ag Growth Rate (AGGR-AgGrowthRate), and  void surfacelake() & Lake Water Management (LWM-d_lakeWaterControl)
    // ver 1.2.1 - 03.07.16: Changed Lake Water Management to work like the desalinaiton control.; added PST (policy Start Year); cleaned up WaterSimAmerica code
    // ver 1.2.2 - 03.10.16: Changed verbiage, added Power Energy and, I changed the way GPCD is calculated for each Consumer (i.e., now demand * 1000 / pop)
    // ver 1.3.0 - 04.03.16: I added Industrial water demand growth
    // ver 1.3.1 - 04.06.16: I changed Drought Override to a value od 60 (or 0.6 * flow)
    // ver 1.3.2 - 04.11.16: QUAY Brought in DAS Insutrial Code and revised reading data.
    //================================================================================================
    // Enums 
    //==========================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent providers. </summary>
    ///
    /// <remarks>   Mcquay, 1/12/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum eProvider
    {
        eState
    }

    /// <summary>
    /// Provider class is one provider = State
    /// </summary>
    public static partial class ProviderClass
    {
        // Provider Routines, Constants and enums
        /// <summary>
        /// The last valid provider enum value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider LastProvider = eProvider.eState;

        /// <summary>
        /// The first valid enum value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider FirstProvider = eProvider.eState;

        /// <summary>
        /// The Last valid Aggregator value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider LastAggregate = eProvider.eState;

        /// <summary>
        /// The number of valid Provider (eProvider) enum values for use with WaterSimModel and ProviderIntArray.
        /// </summary>
        /// <value>count of valid eProvider enums</value>
        /// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
        public const int NumberOfProviders = (int)LastProvider + 1;

        /// <summary>
        /// The number of valid Provide Aggregate (eProvider) enum values.
        /// </summary>
        /// <value>count of valid eProvider enums</value>
        /// <remarks>all providers after LastProvider are not considered one of the valid eProvider enum value</remarks>
        public static int NumberOfAggregates = ((int)LastAggregate - (int)LastProvider);

        internal const int TotalNumberOfProviderEnums = ((int)LastAggregate) + 1;

        private static string[] ProviderNameList = new string[TotalNumberOfProviderEnums]    {      
  
            "State",
             };

        private static string[] FieldNameList = new string[TotalNumberOfProviderEnums]  {      
 
            "st"
           };

        private static eProvider[] RegionProviders = new eProvider[NumberOfProviders] {
            eProvider.eState
        };

        public static eProvider[] GetRegion(eProvider ep)
        {
            switch (ep)
            {
                case eProvider.eState:
                    return RegionProviders;
                default:
                    return null;
            }
        }

    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A model parameter. </summary>
    ///
    /// <remarks>   Mcquay, 1/12/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static partial class eModelParam
    {
        //
        // Model Control
        public const int epState = 4;
        // Drivers
        public const int epPopulation = 5;
        public const int epGPCD_urban = 6;
        public const int epGPCD_ag = 7;
        public const int epGPCD_other = 8;

        // Policies
        public const int epPolicyStartYear = 11;
        public const int epUrbanWaterConservation = 12;
        public const int epAgWaterConservation = 13;
        public const int epPowerWaterConservation = 14;
        public const int epGroundwaterManagement = 15;
        public const int epGroundwaterControl = 16;
        public const int epSurfaceWaterManagement = 17;
        public const int epSurfaceWaterControl = 18;
        public const int epReclainedWaterUse = 19;
        public const int epDroughtControl = 20;
        public const int epLakeWaterManagement = 21;
        public const int epAgriculturalGrowth = 22;

        // Externalities - Drivers
        public const int epPopGrowthAdjustment = 25;
        public const int epClimateDrought = 26;
        public const int epAgricultureProduction = 27;
//        public const int epAgricultureDemand = 253;

        //
        // Resources
        public const int epSurfaceFresh = 31;
       // public const int epSurfaceLake = 32;
        public const int epSurfaceSaline = 33;
        public const int epGroundwater = 34;
        public const int epEffluent = 35;
        public const int epAugmented = 36;
        public const int epTotalSupplies = 37;

        // Consumers
        public const int epUrban = 51;
        public const int epAgriculture = 52;
        public const int epIndustrial = 53;
        public const int epPower = 54;
        // Outcomes
        public const int epUrbanNet = 71;
        public const int epRuralNet = 72;
        public const int epAgricultureNet = 73;
        public const int epIndustrialNet = 74;
        public const int epPowerNet = 75;
        public const int epPowerEnergy = 76;
        //
        public const int epSurfaceFreshNet = 80;
        public const int epSurfaceSalineNet = 81;
        public const int epSurfaceLakeNet = 82;
        public const int epGroundwaterNet = 83;
        public const int epEffluentNet = 84;

        //
        // Sustainability Metrics
        public const int epSustainability_groundwater = 101;
        public const int epSustainability_surfacewater = 102;
        public const int epSustainability_personal = 103;
        public const int epSustainability_economy = 104;
        //

    }
    
    //********************************************************************************
    //
    //
    // *******************************************************************************

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for water simulations. </summary>
    ///
    /// <seealso cref="WaterSimDCDC.WaterSimManagerClass"/>
    ///-------------------------------------------------------------------------------------------------
    //
    
    //
    public partial class WaterSimManager :  WaterSimManagerClass
    {
        protected WaterSimAmerica WSmith=null;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="DataDirectoryName">    Pathname of the data directory. </param>
        /// <param name="TempDirectoryName">    Pathname of the temp directory. </param>
        ///-------------------------------------------------------------------------------------------------
        WaterSimDCDC.Processes.AlterWaterManagementFeedbackProcess WM;

        public WaterSimManager(string DataDirectoryName, string TempDirectoryName)
            : base(DataDirectoryName, TempDirectoryName)
        {
            try
            {
                WSmith = new WaterSimAmerica(DataDirectory, TempDirectory);
                WM = new AlterWaterManagementFeedbackProcess("Alter Water Management");

                initialize_ModelParameters();
                //initialize_ExtendedDocumentation();
                initializeIndicators();
                initializeFluxParameters();
            }
            catch (Exception ex)
            {
                WSmith = null;
                MessageBox.Show("WaterSim America was not created" + ex);
                throw new ArgumentNullException();
                
            }
        }
     
        //public CRF_Network_WaterSim_America TheCRFNetwork
        public CRF_Network_WaterSim_America TheCRFNetwork
        {
            get { return WSmith.TheCRFNetwork; } 
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Simulation cleanup. </summary>
        ///
        /// <seealso cref="WaterSimDCDC.WaterSimManagerClass.Simulation_Cleanup()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override void Simulation_Cleanup()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check if Model Setup Correct. </summary>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.WaterSimManagerClass.ValidModelCheck()"/>
        ///-------------------------------------------------------------------------------------------------

        protected override bool ValidModelCheck()
        {
            return (base.ValidModelCheck() && (WSmith != null));
        }
        //
        // This function makes the model accessible to the WaterManagement Process
        // 02.16.16
        public WaterSimAmerica WaterSimAmerica
        {
            get { return WSmith; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Provides Access to the WaterSimAmerica Model. </summary>
        ///
        /// <value> The water simulation america model. </value>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimAmerica WaterSimAmericaModel
        {
            get { return WSmith; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default maximum start year. </summary>
        ///
        /// <remarks>   Mcquay, 2/9/2016. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int DefaultMaxStartYear()
        {
            return 2064;
        }

        public override int DefaultStartYear()
        {
            return 2015;
        }

        public override int DefaultStopYear()
        {
            return 2065;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the year operation. </summary>
        ///
        /// <param name="year"> The year. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.WaterSimManagerClass.runYear(int)"/>
        ///-------------------------------------------------------------------------------------------------

        protected override bool runYear(int year)
        {
            return (RunModelYear(year) == 0);
        }

 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the model year operation. </summary>
        ///
        /// <exception cref="NotImplementedException"> Thrown when the requested operation is
        ///     unimplemented. </exception>
        ///
        /// <param name="year"> The year. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///
        /// <seealso cref="WaterSimDCDC.WaterSimManagerClass.RunModelYear(int)"/>
        ///-------------------------------------------------------------------------------------------------
        // DAS 01.21.16 
        protected override int RunModelYear(int year)
        {
            // Quay Edit 2/9/16
            int testrun = WSmith.runOneYear(year);
            return testrun;
         }

        public override void Simulation_Initialize()
        {
            base.Simulation_Initialize();
            WSmith.ResetNetwork();
            resetManager();
        }
        // -------------------------------------------------------------------------------------------------------------------------
        //
        // =======================================
        protected override string GetModelVersion()
        {
            return "1.3.2";
        }
        // =======================================
        protected override void initialize_ExtendedDocumentation()
        {
            throw new NotImplementedException();
        }
        // =========================================
        //
        protected override void initialize_ModelParameters()
        {
            WaterSimManager WSim = (this as WaterSimManager);
            WSim.ProcessManager.AddProcess(WM);
            //
            base.initialize_ModelParameters();
            ParameterManagerClass FPM = ParamManager;        
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;
            // =======================================================
             // Provider parameters
            // Inputs/Outputs
            //
            // Template(s)
            // ExtendDoc.Add(new WaterSimDescripItem(eModelParam.ep, "", "", "", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            // _pm.AddParameter(new ModelParameterClass(eModelParam.ep,"", "", rangeChecktype.rctCheckRange, 0, 0, geti_, seti_, RangeCheck.NoSpecialBase));
            //
            // NEW STUFF - State
            _pm.AddParameter(new ModelParameterClass(eModelParam.epState, "State Code", "ST", rangeChecktype.rctCheckRange, 0, 4, WSmith.geti_StateIndex, WSmith.seti_StateIndex, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epState, "The State Currently being Examined: one of five (Florida, Idaho, Illinois, Minnesota, Wyoming) in the initial work.", "", "The State Examined", "State", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Drivers
            _pm.AddParameter(new ModelParameterClass(eModelParam.epPopulation, "Population Served", "POP", geti_Pop));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPopulation, "State Population People in any given year- we use an estimate of slope to project out to 2065", "ppl", "State Population (ppl)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Outputs
            _pm.AddParameter(new ModelParameterClass(eModelParam.epGPCD_urban, "Urban GPCD", "UGPCD", WSmith.geti_gpcd));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGPCD_urban, "The GPCD (Gallons per Capita per Day) for delivered water for the Urban water sector.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epGPCD_ag, "Agricultural GPCD", "AGPCD", WSmith.geti_gpcdAg));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGPCD_ag, "The GPCD (Gallons per Capita per Day) for delivered water for Agricultural Uses.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epGPCD_other, "Other GPCD: Power and Industry", "OGPCD", WSmith.geti_gpcdOther));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGPCD_other, "The GPCD (Gallons per Capita per Day) for delivered water for Industrial Uses and Power Combined.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //

            // Resources
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceFresh, "Surface Water (Fresh)", "SUR", rangeChecktype.rctCheckRange, 0,20000 /* 50000000 */, WSmith.geti_SurfaceWaterFresh, WSmith.seti_SurfaceWaterFresh, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceFresh, "Fresh Water Deliveries from Surface Sources; this is total fresh water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceFreshNet, "Surface Water (Fresh) Net", "SURN", WSmith.geti_SurfaceWaterFreshNet));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceSaline, "Surface Water (Saline)", "SAL", rangeChecktype.rctCheckRange, 0,20000 /* 50000000 */, WSmith.geti_SurfaceWaterSaline, WSmith.seti_SurfaceWaterSaline, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceSaline, "Saline Water Deliveries from Surface Sources; this is total saline water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceSalineNet, "Surface Water (Saline) Net", "SALN", WSmith.geti_SurfaceWaterSalineNet));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epGroundwater, "Groundwater (Fresh)", "GW", rangeChecktype.rctCheckRange, 0,20000 /* 500000008*/, WSmith.geti_Groundwater, WSmith.seti_Groundwater, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGroundwater, "Fresh Water Deliveries from Pumped Groundwater; this is total water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epGroundwaterNet, "Groundwater (Fresh) Net", "GWN", WSmith.geti_GroundwaterNet));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epEffluent, "Effluent (Reclaimed)", "REC", rangeChecktype.rctCheckRange, 0, 20000 /*50000000*/, WSmith.geti_Effluent, WSmith.seti_Effluent, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epEffluent, "Effluent (reclaimed) Water Deliveries from Waste Water Treatment Plants; total withdrawals.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epEffluentNet, "Effluent (Reclaimed) Net", "RECN", WSmith.geti_EffluentNet));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceLake, "Surface Lake Water", "SURL", rangeChecktype.rctCheckRange, 0, 20000, WSmith.geti_SurfaceLake, WSmith.seti_SurfaceLake, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceLake, "Surface Lake Water", "mgd", "Million Gallons Per Day", "Surface Lake Water", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceLakeNet, "Surface Lake Water Net", "SURLN", WSmith.geti_SurfaceLakeNet));

            //
            _pm.AddParameter(new ModelParameterClass(eModelParam.epTotalSupplies, "Total Supplies", "TS", WSmith.geti_TotalSupplies));


            // CONSUMERS
            _pm.AddParameter(new ModelParameterClass(eModelParam.epUrban, "Urban Demand", "UD", rangeChecktype.rctCheckRange, 0,30000 /*50000000*/, WSmith.geti_Urban, WSmith.seti_Urban, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epUrban, "Urban Water Demand", "MGD ", "Million Gallons per Day", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epUrbanNet, "Urban Demand (Net)", "UDN", WSmith.geti_Urban_Net));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epUrbanNet, "Urban (residential) Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAgriculture, "Agriculture Demand", "AD", rangeChecktype.rctCheckRange, 0,30000 /*50000000*/, WSmith.geti_Agriculture, WSmith.seti_Agriculture, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgriculture, "Agriculture Water Demand; total withdrawals.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAgricultureNet, "Agriculture Demand (Net)", "ADN", WSmith.geti_Agriculture_Net));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureNet, "Agricultural Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epIndustrial, "Industrial Demand", "ID", rangeChecktype.rctCheckRange, 0,30000 /* 50000000*/, WSmith.geti_Industrial, WSmith.seti_Industrial, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epIndustrial, "Industrial Water Demand; total withdrawals. Water used for industries such as steel, chemical, paper, and petroleum refining. ", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epIndustrialNet, "Industrial Demand (Net)", "IDN", WSmith.geti_Industrial_Net));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epIndustrialNet, "Industrial Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epPower, "Power Demand", "PD", rangeChecktype.rctCheckRange, 0, 30000 /*50000000*/, WSmith.geti_PowerWater, WSmith.seti_PowerWater, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPower, "Water Use by Power: total withdrawals. Water used in the process of generating electricity with steam-driven turbine generators [Thermoelectric power, subcategories by cooling-system type (once-through, closed-loop/recirculation)].", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epPowerNet, "Power Demand (Net)", "PDN", WSmith.geti_PowerWater_Net));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPowerNet, "Power Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
             //
               _pm.AddParameter(new ModelParameterClass(eModelParam.epPowerEnergy, "Power Produced", "PE", WSmith.geti_PowerEnergy));

             //
            // Controls - Policy
            
            _pm.AddParameter(new ModelParameterClass(eModelParam.epUrbanWaterConservation, "Water Conservation (urban & rural)", "UCON", rangeChecktype.rctCheckRange, 50, 100, WSmith.geti_UrbanConservation, WSmith.seti_UrbanConservation, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epUrbanWaterConservation, "Urban Water Conservation: reduction in annual water use.", "", "Percent reduction in water use", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 100, 80, 65, 50 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAgWaterConservation, "Ag Water Conservation", "ACON", rangeChecktype.rctCheckRange, 50, 100, WSmith.geti_AgConservation, WSmith.seti_AgConservation, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgWaterConservation, "Agricultural Water Conservation: reduction in annual water used by the Ag sector.", "", "Percent reduction in water use", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 100, 80, 65, 50 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epPowerWaterConservation, "Power Water Conservation", "PCON", rangeChecktype.rctCheckRange, 50, 100, WSmith.geti_PowerConservation, WSmith.seti_PowerConservation, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPowerWaterConservation, "Power Water Conservation: reduction in annual water use for Thermoelectric power generation.", "", "Percent reduction in water use", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 100, 80, 65, 50 }, new ModelParameterGroupClass[] { }));
            //
            // Index Values
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceWaterManagement, "Use More Surface Water", "SWM", rangeChecktype.rctCheckRange, 0, 4, WSmith.geti_SurfaceWaterManagement, WSmith.seti_SurfaceWaterManagement, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceWaterManagement, "Controls Scenario Chosen for alteration in surface water supply: increased surface water withdrawals.", "", "Alteration in Available Surface Water", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 0, 1, 2, 3 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epGroundwaterManagement, "Change Groundwater Use", "GWM", rangeChecktype.rctCheckRange, 0, 5, WSmith.geti_GroundwaterManagement, WSmith.seti_GroundwaterManagement, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGroundwaterManagement, "Controls Scenario Chosen for alteration in groundwater supplies: increased or decreased groundwater withdrawals.", "", "Alteration in Groundwater Used", "", new string[4] { "None", "Less", "More", "Most" }, new int[4] { 0, 1, 3, 4 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epReclainedWaterUse, "Use Reclaimed Water", "RECM", rangeChecktype.rctNoRangeCheck, 0, 100, WSmith.geti_ReclaimedWaterManagement, WSmith.seti_ReclaimedWaterManagement, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epReclainedWaterUse, "Alteration in reclaimed (effluent) supplies: increased effluent withdrawals.", "", "% of indoor water use", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 0, 7, 15, 25 }, new ModelParameterGroupClass[] { }));
            //
            _pm.AddParameter(new ModelParameterClass(eModelParam.epLakeWaterManagement, "Increase Lake Water use", "LWM", rangeChecktype.rctCheckRange, 0, 25, WSmith.geti_LakeWaterManagement, WSmith.seti_LakeWaterManagement, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epLakeWaterManagement, "Controls Lake Water Management: increased or decreased groundwater withdrawals.", "", "Scenario changes in lake later withdrawals", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAugmented, "Augmented Desal", "DESAL", rangeChecktype.rctCheckRange, 0, 25, WSmith.geti_Desalinization, WSmith.seti_Desalinization, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAugmented, "Adds a percent of desalinaiton: increased surface saline withdrawals.", "", "Scenario changes in lake later withdrawals", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            _pm.AddParameter(new ModelParameterClass(eModelParam.epPolicyStartYear, "Policy Start Year", "PST", rangeChecktype.rctCheckRange, 2016, 2060, geti_PolicyStartYear, seti_PolicyStartYear, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPolicyStartYear, "Year that the Policies are implemented", "yr", "Year", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            _pm.AddParameter(new ModelParameterClass(eModelParam.epSurfaceWaterControl, "Surface Water Control- controls rate", "SWMC", rangeChecktype.rctCheckRange, 100, 150, WSmith.geti_SurfaceWaterControl, WSmith.seti_SurfaceWaterControl, RangeCheck.NoSpecialBase));
              ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epSurfaceWaterControl, "Controls timing and rate of Surface Water Management Scenario: increased surface withdrawals.", "Scenario-driven", "Scenario increases in surface water withdrawals", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epGroundwaterControl, "Groundwater Control- controls rate", "GWMC", rangeChecktype.rctCheckRange, 50, 150, WSmith.geti_GroundwaterControl, WSmith.seti_GroundwaterControl, RangeCheck.NoSpecialBase));
              ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epGroundwaterControl, "Controls timing and rate of Groundwater Management Scenario: increased or decreased groundwater withdrawals.", "Scenario-driven", "Scenario changes in groundwater withdrawals", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            // 
            // Controls - External Forcings
            _pm.AddParameter(new ModelParameterClass(eModelParam.epPopGrowthAdjustment, "Population Growth Projected", "POPGR", rangeChecktype.rctCheckRange, 0, 150, WSmith.geti_PopGrowthRate, WSmith.seti_PopGrowthRate, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPopGrowthAdjustment, "Adjustment in the Projected Population Growth Rate.", "%", "Population Growth", "", new string[4] { "Low", "Some", "Planned", "High" }, new int[4] { 60, 80, 100, 120 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epClimateDrought, "Drought Impacts on Rivers/Lakes ", "CLIM", rangeChecktype.rctCheckRange, 0, 4, WSmith.geti_DroughtImpacts, WSmith.seti_DroughtImpacts, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epClimateDrought, "Alteration in Fresh Water Withdrawals as a result of drought on supplies.", "Scenario-driven", "Drought Reductions in Surface Water", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            _pm.AddParameter(new ModelParameterClass(eModelParam.epDroughtControl, "Drought Impacts Control- controls rate", "DC", rangeChecktype.rctCheckRange, 50, 150, WSmith.geti_DroughtControl, WSmith.seti_DroughtControl, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epDroughtControl, "Percent reduction in Surface flows due to drought", "%", "Percent (%)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAgricultureProduction, "Agriculture Net $", "ANP", WSmith.geti_AgricutureProduction));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgricultureProduction, "Agriculture Net Annual Farm Income.", "M$", "Million Dollars ", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epAgriculturalGrowth, "Agriculture Growth", "AGGR", rangeChecktype.rctCheckRange, 50, 150, WSmith.geti_AgGrowthRate, WSmith.seti_AgGrowthRate, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epAgriculturalGrowth, "Agriculture Growth Rate Applied.", "%", "Percent of current growth", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // -----------------------------
            // Initialize Other
            WSmith.startYear = _StartYear;
            WSmith.endYear = _EndYear;
            WSmith.currentYear = _StartYear;
            // =============================
            //
            defaultSettings();
            //
        }
        void defaultSettings()
        {
            // default settings
            startSGWM = 2017;
            endSGWM = 2050;
            startDrought = 2015;
            endDrought = 2065;
            ParamManager.Model_ParameterBaseClass(eModelParam.epClimateDrought).Value = 0;
            // =============================
        }
        void resetManager()
        {
            int temp=100;
            seti_agTargetEfficiency(temp);
        }
        // =====================================================================================================================
        //
        /// <summary>
        /// Policy Start Year; starts the year in which any policy 
        /// starts; valid are 2016 to 2060 (at present)
        /// </summary>
        int _policyStartYear = 2016;
        public int geti_PolicyStartYear()
        {
            return _policyStartYear;
        }
        public void seti_PolicyStartYear(int value)
        {
            //_policyStartYear = value;
            startSGWM = value;
        }
        // ==============================
//        int _waterManagementStart = 2016;
        public int startSGWM
        {
            get { return _policyStartYear; }
            set { _policyStartYear = value; }
        }
        int _waterManagementEnd = 2065;
        public int endSGWM
        {
            get { return _waterManagementEnd; }
            set { _waterManagementEnd = value; }
        }
        // ============================================
        int _droughtStart = 0;
        public int startDrought
        {
            get { return _droughtStart; }
            set { _droughtStart = value; }
        }
        int _droughtEnd = 0;
        public int endDrought
        {
            get { return _droughtEnd; }
            set { _droughtEnd = value; }
        }
        int _droughtOverride = 0;
        public int droughtOverride
        {
            get { return _droughtOverride; }
            set { _droughtOverride = value; }
        }
        // 02.25.16 DAS
        int geti_Pop()
        {
            int year = Sim_CurrentYear;
            int pop = WSmith.Get_PopYear(year);
            return pop;
        }

        #region WaterSmithParamters
        // ---------------------------------------------------------------------------
        // ==============================================================================================================
     
    
        //
        //**************************************************************
        //
        // WATERSMITH PARAMETERS
        //
        //**************************************************************

      
        #endregion
        //
        public virtual bool Model()
        {
            bool success = false;
            
            return success;
        }
        //
        //What do I do with this?
        // 
        public const int FNumberOfStates = 5;
        string[] FStateNames = new string[FNumberOfStates] { "Florida", "Idaho", "Illinois", "Minnesota", "Wyoming" };

        public int FStateIndex = 0;


 
    }
    
    // new stuff

}
