using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsumerResourceModelFramework;
//using WaterSimDCDC.America;
using WaterSimDCDC.Generic;
using WaterSimDCDC.Documentation;

//using WaterSimDCDC.Processes;

namespace WaterSimDCDC
{
    // WaterSim West Region Manager
    // These classes are the heart of the WaterSim West Manager that manges the WaterSim West Model.  
    // The base eProviders are defined, 
    //
    // Build Notes:
    // ver 1.0.0 - First version
    // ver 1.1.1 - 02.28.16:  
    // ver 1.2.0 - 03.06.16: Added desalination (using epAugmented) (DESAL-Desal), Ag Growth Rate (AGGR-AgGrowthRate), and  void surfacelake() & Lake Water Management (LWM-d_lakeWaterControl)
    // ver 1.2.1 - 03.07.16: Changed Lake Water Management to work like the desalinaiton control.; added PST (policy Start Year); cleaned up WaterSimAmerica code
    //                       Ag growth, changed Lake Water Management to work as desalination, added "invokePolicies"
    // ver 1.2.2 - 03.10.16: Changed verbiage, added Power Energy and, I changed the way GPCD is calculated for each Consumer (i.e., now demand * 1000 / pop)
    // ver 1.3.0 - 04.03.16: I added Industrial water demand growth
    // ver 1.3.1 - 04.06.16: I changed Drought Override to a value of 60 (or 0.6 * flow)
    // ver 1.3.2 - 04.11.16: QUAY Brought in DAS Industrial Code and revised reading data.
    // ver 1.3.3 - 05.04.16: DAS - changed the reclaimed to add a constant amount each year based on the first year designation (to keep below maximum reclaimed by 2065)
    //                        also changed the default values to 0,1,2,3 to just make max reclaimed by 2065 with value 3 (Florida was my test case). 
    //                        Tested (and parameterized) on three states: 05.05.16
    // ver 1.3.5 - 05.06.16: DAS - added drought impacts on water demand for all sectors using  "public static double hyperbola(double droughtFactor)" and * modifyDemandCF();
    // ver 1.3.7 - 05.11.16: DAS - completely changed the controls for surface and groundwater management and in the America_Process.cs file
    //                       doubles to int's, 0 to 3 for the controls, 1 being baseline, 0 = less water, 2 and 3 = more water
    // ver 1.3.8 - 07.19.16  DAS added four state data for industry,added four state data for population, read industry data from a csv file, modified the population code (see notes)
    // ver 1.4.1 - 09.15.15  DAS We now read mean growth rates for population and industry from a file. We then use adapted algorithms that Ray wrote for agriculture
    //                       to model population and industry growth. I had to change the way the conservation parameter acts on these growth rates. I also deleted
    //                       code no longer being used. I have been testing Urban, Agriculture, PowerWater, and Industry for changes in population and changes in conservation.
    //                        I have one SigmaPlot program that highlights many of these tests (WAS_temp.jnb)
    // ver 2.0.0 - 12.08.16  I altered the parameter specs for the water management parameters to be consistent with the conservation management controls (i.e., same magnitude and function)
    //                       I altered the parameter specs for the reclaimed water management control to be consistent with the other controls
    // ver 2.0.1 - 01.31.17  SAL ( seti_Desalinization()) was incorrectly parameterized [was set at zero instead of 100]. And, I added the property Desalinization in place of using the constant
    //                      _desalinization.
    // ver 2.0.2 - 02.01.17 Fixed the Effluent control. I was compounding the values... instead, set to new each year. Line 1593 in WaterSimAmerica_2_* and,
    //                      it was being initialized to 100 instead of zero
    // ver 2.0   - 2-13-18    Modified to support West Region data  QUAY 
    // ver 2.0   - 2-13-18  Removed the single CRF model WSmith and all references
    // ver 2.0.5 - 3-11-18  Quay Add code to providerclass to support statenames
    //                      Added industry Employees parameter
    //                      Changed labels, web labels, button codes and values for Conservation and Surfaewater web controls
    // ver 2.0.6   3/18/18  Quay, changed popgrpwth rate and modify rate parameters, also change wording for policy and resource web control parameters
    // ver 2.0.7   3/19/18  Quay, fixed the gw to ag flux field code 
    // ver 2.0.8   3/19/18  Quay, fixed Power and Industry Conservation and added parameters for GPED and GPMWD
    // 
    // ver 2.0.14  3/30/18  Quay, Changed Efficiency controls button labels
    //=============================================================================================================================================================================================
    // Enums 
    //==========================================================
    //

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent providers. </summary>
    ///
    /// <remarks>   Mcquay, 1/12/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum eProvider
    {
         ////eArizona, eCalifornia, eColorado, eFlorida,  eIdaho, eIllinois, eMinnesota, eNevada, eNewMexico, eUtah, eWyoming, eBasin  
         //eArizona, eCalifornia, eColorado, eNevada, eNewMexico, eUtah, eWyoming, eBasin  
    	 eArizonaNorth, //10
         eArizonaSoutheast, //23
    	 eArizonaCentralNorth, //24
    	 eArizonaWest, //9
    	 eArizonaCentralSouth, //8
    	 eCaliforniaNorth, // 4
    	 eCaliforniaSoutheast, // 2
    	 eCaliforniaSouthwest, // 1
    	 eColoradoFrontRange, // 17
    	 eColoradoNotInBasin, // 19
    	 eColoradoInBasin, // 18    
    	 eNevadaNotInBasin, // 7
    	 eNevadaSouth, // 5
    	 eNevadaInBasin, // 6
    	 eNewMexicoCentral, // 20
    	 eNewMexicoGila, // 25
    	 eNewMexicoNotInBasin, // 22
    	 eNewMexicoInBasin, // 21
    	 eUtahNotInBasin, // 13 
    	 eUtahInBasin, // 12
    	 eUtahSaltLake, // 11
    	 eWyomingNotInBasin, // 16
    	 eWyomingInBasin, // 15 
    	 eWyomingSouthwest, //14
         eBasin 

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
        public const eProvider LastProvider = eProvider.eWyomingSouthwest;

        /// <summary>
        /// The first valid enum value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider FirstProvider = eProvider.eArizonaNorth;

        /// <summary>
        /// The Last valid Aggregator value
        /// </summary>
        /// <value>eProvider enum</value>
        public const eProvider LastAggregate = eProvider.eBasin;

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

        // QUAY EDIT 9/12/18
        // Whoops these provider codes are in the the wrong order, no big deal, this is only used by visual controls that can not reach unitdata, that has the list in the correct order

        /// <summary> List of provider names.</summary>
        public static string[] ProviderNameList = new string[TotalNumberOfProviderEnums]    {
        //"Arizona North",
        // "Arizona Southeast",
        // "Arizona Central North",
        // "Arizona West",
        // "Arizona Central South",
        //"North",
        // "Southeast",
        // "Central North",
        // "West",
        // "Central South",
        // "California North",
        // "California Southeast",
        // "California Southwest",
        // "Colorado Front Range",
        // "Colorado Not In Basin",
        // "Colorado In Basin",
        // "Nevada Not In Basin",
        // "Nevada South",
        // "Nevada In Basin",
        // "New Mexico Central",
        // "New Mexico Gila",
        // "New Mexico Not In Basin",
        // "New Mexico In Basin",
        // "Utah Not In Basin",
        // "Utah In Basin",
        // "Utah Salt Lake",
        // "Wyoming Not In Basin",
        // "Wyoming In Basin",
        // "Wyoming Southwest",
        // "Colorrado River Basin"
         "Arizona Central South",
         "Arizona West",
         "Arizona North",
         "Arizona Southeast",
         "Arizona Central North",
         "California Southwest",
         "California Southeast",
         "California North",
         "Colorado Front Range",
         "Colorado In Basin",
         "Colorado Not In Basin",
         "Nevada South",
         "Nevada In Basin",
         "Nevada Not In Basin",
         "New Mexico Central",
         "New Mexico In Basin",
         "New Mexico Not In Basin",
         "New Mexico Gila",
         "Utah Salt Lake",
         "Utah In Basin",
         "Utah Not In Basin",
         "Wyoming Southwest",
         "Wyoming In Basin",
         "Wyoming Not In Basin",
         "Colorado River Basin"
        };


        /// <summary> List of field names.</summary>
        public static string[] FieldNameList = new string[TotalNumberOfProviderEnums]  {      
 
      //      "AZ","CA","CO","FL","ID","IL","MN","NV","NM","UT","WY","CB"
      //      "AZ","CA","CO","NV","NM","UT","WY","CB"

            "AZCS8" , //Arizona Central South
            "AZW9" , //Arizona West
            "AZN10" , //Arizona North
            "AZSE23" , //Arizona Southeast
            "AZCN24" , //Arizona Central North
            "CASW1" , // California Southwest
            "CASE2" , // California Southeast
            "CAN4" , // California North
            "COFR17" , // Colorado Front Range
            "COIB18" , // Colorado In Basin
            "CONIB19" , // Colorado Not In Basin
            "NVS5" , // Nevada South
            "NVIB6" , // Nevada In Basin
            "NVNIB7" , // Nevada Not In Basin
            "NMC20" , // New Mexico Central
            "NMIB21" , // New Mexico In Basin
            "NMNIB22" , // New Mexico Not In Basin
            "NMG25" , // New Mexico Gila
            "UTSL11" , // Utah Salt Lake
            "UTIB12" , // Utah In Basin
            "UTNIB13" , // Utah Not In Basin
            "WYSW14" , // Wyoming Southwest
            "WYIB15" , // Wyoming In Basin
            "WYNIB16" , // Wyoming Not In Basin
            "COB"    // Colorado River Basin
           };


        /// <summary> The basin providers.</summary>
        public static eProvider[] BasinProviders = new eProvider[24] {
            //    eProvider.eArizona, eProvider.eCalifornia, eProvider.eColorado, eProvider.eNevada, eProvider.eNewMexico, eProvider.eUtah, eProvider.eWyoming
            //eProvider.eArizonaCentralNorth, eProvider.eArizonaCentralSouth, eProvider.eArizonaNorth, eProvider.eArizonaSoutheast, eProvider.eArizonaWest,
            //eProvider.eCaliforniaSoutheast, eProvider.eCaliforniaSouthwest, eProvider.eColoradoFrontRange, eProvider.eNevadaInBasin,
            //eProvider.eNevadaInBasin, eProvider.eNevadaSouth, eProvider.eNewMexicoCentral, eProvider.eNewMexicoGila, eProvider.eNewMexicoInBasin,
            //eProvider.eUtahInBasin, eProvider.eUtahSaltLake, eProvider.eWyomingInBasin, eProvider.eWyomingSouthwest
            eProvider.eArizonaCentralSouth,  eProvider.eArizonaWest, eProvider.eArizonaNorth, eProvider.eArizonaSoutheast,eProvider.eArizonaCentralNorth,
            eProvider.eCaliforniaSouthwest, eProvider.eCaliforniaSoutheast,  eProvider.eCaliforniaNorth,
            eProvider.eColoradoFrontRange,eProvider.eColoradoInBasin,eProvider.eColoradoNotInBasin,
            eProvider.eNevadaSouth, eProvider.eNevadaInBasin, eProvider.eNevadaNotInBasin,
            eProvider.eNewMexicoCentral, eProvider.eNewMexicoInBasin, eProvider.eNewMexicoNotInBasin, eProvider.eNewMexicoGila,
            eProvider.eUtahSaltLake, eProvider.eUtahInBasin, eProvider.eUtahNotInBasin,
            eProvider.eWyomingSouthwest, eProvider.eWyomingInBasin, eProvider.eWyomingNotInBasin      };

        // EDN EDIT QUAY 9/12/18
        // 
        /// <summary> Name of the region state.</summary>
        public static string[] RegionStateName = new string[TotalNumberOfProviderEnums] {
            "Arizona","Arizona","Arizona","Arizona","Arizona","California","California","California","Colorado","Colorado","Colorado",
            "Nevada","Nevada","Nevada","New Mexico","New Mexico","New Mexico","New Mexico","Utah","Utah","Utah","Wyoming","Wyoming","Wyoming","Multiple"};


        /// <summary> List of names of the states.</summary>
        public static string[] StateNames = new string[7] {
            "Arizona","California","Colorado","Nevada","New Mexico","Utah","Wyoming"};

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a region.</summary>
        /// <param name="ep"> The ep.</param>
        /// <returns> An array of e provider.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static eProvider[] GetRegion(eProvider ep)
        {
            switch (ep)
            {
                case eProvider.eBasin:
                    return BasinProviders;
                default:
                    return null;
            }
        }

    }
    /////-------------------------------------------------------------------------------------------------
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
        //public const int epPopulation = 5;
// NOTE the regional weight is based on parameter #3
        public const int eppulation = 3;

        public const int epGPCD_urban = 6;
        public const int epGPCD_ag = 7;
        public const int epGPCD_other = 8;

        // Policies
        public const int epPolicyStartYear = 11;
        public const int epUrbanWaterConservation = 12;
        public const int epAgWaterConservation = 13;
        public const int epPowerWaterConservation = 14;
        public const int epIndustrialWaterConservation = 15;
        public const int epGroundwaterManagement = 16;
        public const int epGroundwaterControl = 17;
        public const int epSurfaceWaterManagement = 18;
        public const int epSurfaceWaterControl = 19;
        public const int epReclainedWaterUse = 20;
        public const int epDroughtControl = 21;
        public const int epLakeWaterManagement = 22;
        public const int epAgriculturalGrowth = 23;

        // Externalities - Drivers
        public const int epPopGrowthAdjustment = 25;
        public const int epClimateDrought = 26;
        public const int epAgricultureProduction = 27;
        //        public const int epAgricultureDemand = 253;

        // =======================================================
        // Single Model Parameters
        // ======================================================

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

        //*
        // Sustainability Metrics
        public const int epSustainability_groundwater = 101;
        public const int epSustainability_surfacewater = 102;
        public const int epSustainability_personal = 103;
        public const int epSustainability_economy = 104;
        //
        // Other Metrics
        public const int epNetDemandDifference = 110;

        public const int epP_Population = 1005;
        public const int epP_GPCD_urban = 1006;
        public const int epP_GPCD_ag = 1007;
        public const int epP_GPCD_other = 1008;
        // QUAY EDIT 3/3/18
        // Added Ag Gallons Per Dollar Parameter
        public const int epP_AgGPDD = 1009;
        public const int epP_AgIGPDD = 1010;
        // END EIT 3/3/18

        // Policies
        public const int epP_PolicyStartYear = 1011;
        public const int epP_UrbanWaterConservation = 1012;
        public const int epP_AgWaterConservation = 1013;
        public const int epP_PowerWaterConservation = 1014;
        public const int epP_IndustrialWaterConservation = 1015;
        public const int epP_GroundwaterManagement = 1016;
        public const int epP_GroundwaterControl = 1017;
        public const int epP_SurfaceWaterManagement = 1018;
        public const int epP_SurfaceWaterControl = 1019;
        public const int epP_ReclainedWaterUse = 1020;
        public const int epP_LakeWaterManagement = 1022;
        public const int epP_AgriculturalGrowth = 1023;
        // sampson 08.18.18
        public const int epP_GrayWaterManagement = 1024;

        // end sampson edits 08.18.18


        // Externalities - Drivers
        public const int epP_PopGrowthRates = 1025;
        public const int epP_PopGrowthRateModifyer = 1026;

        public const int epP_ClimateDrought = 1027;
        public const int epP_AgricultureProduction = 1028;

        // EDIT 3/3/2018 QUAY
        // Added this Agriculture efficiency can be increase or decrease
        public const int epP_AgricultureGallonsPerDollarTarget = 1029;
        // EDIT END 3/3/2018 

        
        
        // Resources
        public const int epP_SurfaceFresh = 1031;
       // public const int epP_SurfaceLake = 1032;
        public const int epP_SurfaceSaline = 1033;
        public const int epP_Groundwater = 1034;
        public const int epP_Effluent = 1035;
        public const int epP_Augmented = 1036;
        public const int epP_TotalSupplies = 1037;
        public const int epP_UrbanWasteWater = 1038;

        // EDIT QUAY 9/12/18
        // Added colorado
        public const int epP_SurfaceColorado = 1039;      
        // END EDIT 

        // Resource Cap and Revise Controls
        public const int epP_DoCapLimits = 1043;
        public const int epP_DoReviseResource = 1044;


        // Consumers
        public const int epP_Urban = 1051;
        public const int epP_Agriculture = 1052;
        public const int epP_Industrial = 1053;
        public const int epP_Power = 1054;
        
        // EDIT QUAY 3/11/18
        public const int epP_IndustryEmployees = 1055;
        public const int epP_IndustrialGPED = 1056;
        // END EDIT 3/11/18

        // Climate Factors
        public const int epP_DroughtControl = 1060;
        public const int epP_DroughtActive = 1061;
        public const int epP_DroughtStartYear = 1062;
        public const int epP_DroughtLength = 1063;
        public const int epP_DroughtDepth = 1064;
        public const int epP_DroughtScenario = 1065;
 // QUAY EDIT 9/13/18
        public const int epP_ClimateChangeTarget = 1066;
        public const int epP_ClimateChangeTargetYear = 1067;
        public const int ep_ClimateChangeTarget = 1068;
        public const int ep_ClimateChangeTargetYear = 1069;
// END EDIT


        // Outcomes
        public const int epP_UrbanNet = 1071;
        public const int epP_RuralNet = 1072;
        public const int epP_AgricultureNet = 1073;
        public const int epP_IndustrialNet = 1074;
        public const int epP_PowerNet = 1075;
        public const int epP_PowerEnergy = 1076;
        public const int epP_PowerGPMWD = 1077;
        //
        public const int epP_SurfaceFreshNet = 1080;
        public const int epP_SurfaceSalineNet = 1081;
        public const int epP_SurfaceLakeNet = 1082;
        public const int epP_GroundwaterNet = 1083;
        public const int epP_EffluentNet = 1084;
        
        // EDIT QUAY 9/12/18
        // Added colorado
        public const int epP_SurfaceColoradoNet = 1085;
        // END EDIT 
        

        //
        // Sustainability Metrics
        public const int epP_Sustainability_groundwater = 1101;
        public const int epP_Sustainability_surfacewater = 1102;
        public const int epP_Sustainability_personal = 1103;
        public const int epP_Sustainability_economy = 1104;
        //
        // Other Metrics
        public const int epP_NetDemandRatio = 1110;
        public const int epP_TotalDemand = 1111;
        public const int epP_TotalDemandNet = 1112;


        public const int epP_UrbanSurfacewater = 1295;
        public const int epP_SurfaceLake = 1296;
        public const int epP_PowerSurfacewater = 1297;
        public const int epP_PowerSaline = 1298;
        public const int epP_PowerGW = 1299;

        public const int epP_SUR_UD =1901;
        public const int epP_SUR_AD =1902;
        public const int epP_SUR_ID =1903;
        public const int epP_SUR_PD =1904;
        public const int epP_SURL_UD =1905;
        public const int epP_SURL_AD =1906;
        public const int epP_SURL_ID =1907;
        public const int epP_SURL_PD =1908;
        public const int epP_GW_UD =1909;
        public const int epP_GW_AD =1910;
        public const int epP_GW_ID =1911;
        public const int epP_GW_PD =1912;
        public const int epP_REC_UD =1913;
        public const int epP_REC_AD =1914;
        public const int epP_REC_ID =1915;
        public const int epP_REC_PD =1916;
        public const int epP_SAL_UD =1917;
        public const int epP_SAL_AD =1918;
        public const int epP_SAL_ID =1919;
        public const int epP_SAL_PD =1920;

        // EDIT QUAY 9/12/18
        // Add for colorado
        public const int epP_COL_UD = 1921;
        public const int epP_COL_AD = 1922;
        public const int epP_COL_ID = 1923;
        public const int epP_COL_PD = 1924;

        // end EDIT
//
        // Demand Model
        /// <summary>
        ///   Sampson: 07.02.18 
        /// The Provider array property used to select the demand model to use
        /// </summary>
        public const int epP_DM_MODELtoUSE_Urban_P = 1950;
        public const int epP_DM_MODELtoUSE_Ag_P = 1951;
        public const int epP_DM_MODELtoUSE_Ind_P = 1952;
        public const int epP_DM_MODELtoUSE_Power_P = 1953;
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
//        protected WaterSimAmerica WSmith=null;
      

        //protected WaterSimCRFModel WSmith = null;
        protected WaterSimModel WestModel = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="DataDirectoryName">    Pathname of the data directory. </param>
        /// <param name="TempDirectoryName">    Pathname of the temp directory. </param>
        ///-------------------------------------------------------------------------------------------------
      //  WaterSimDCDC.Processes.AlterWaterManagementFeedbackProcess WM;

        public WaterSimManager(string DataDirectoryName, string TempDirectoryName)
            : base(DataDirectoryName, TempDirectoryName)
        {
            try
            {
                WestModel = new Generic.WaterSimModel(DataDirectory, TempDirectory);

                // EDIT QUAY 2 13 18
                // OK Finally put to rest this single run model
                // Removed all references to it, ie it is now gone.  
                // If you want a single model  Use this code  
                //WSmith = new WaterSimCRFModel(DataDirectory, TempDirectory);
                // EDIT END 2 13 18
                
                //  WM = new AlterWaterManagementFeedbackProcess("Alter Water Management");

                initialize_ModelParameters();
                //initialize_ExtendedDocumentation();
                initializeIndicators();
                //initializeFluxParameters();
            }
            catch (Exception ex)
            {
               // WSmith = null;
                WestModel = null;
                MessageBox.Show("WaterSim West was not created" + ex);
                throw new ArgumentNullException();
                
            }
        }

        ProviderIntArray Out = new ProviderIntArray(0);
        ProviderIntArray In = new ProviderIntArray(0);

        internal int[] MyValue = new int[ProviderClass.NumberOfProviders];

        public void tempLCLU(WaterSimManager MyWSIM)
        {
            for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { MyValue[i] = 2; }
            Out.Values = MyValue;
            //
            MyWSIM.WaterSimWestModel.DemandModelUrban_Index.setvalues(Out);
            //
            MyWSIM.WaterSimWestModel.DemandModelAg_Index.setvalues(Out);
            //
            MyWSIM.WaterSimWestModel.DemandModelInd_Index.setvalues(Out);

        }

        // EDIT QUAY 2 13 18
        // REMOVED ALL REFERENCES TO THE SINGLE RUN WATERSIM CRF

        //public CRF_Unit_Network TheCRFNetwork
        //{
        //    get { return WSmith.TheCRFNetwork; }
        //}

        // EDIT END 2 13 18

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
            //return (base.ValidModelCheck() && (WSmith != null));
            return (base.ValidModelCheck() && (WestModel != null));
        }
        //
        ////public WaterSimAmerica WaterSimAmerica
        //public WaterSimCRFModel WaterSimModel
        //{
        //    get { return WSmith; }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Provides Access to the WaterSimAmerica Model. </summary>
        ///
        /// <value> The water simulation america model. </value>
        ///-------------------------------------------------------------------------------------------------

        //public WaterSimAmerica WaterSimAmericaModel
        //public WaterSimCRFModel WaterSimModel
        //{
        //    get { return WSmith; }
        //}

        public WaterSimModel WaterSimWestModel
        {
            get { return WestModel; }
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
            return 2049;
        }

        public override int DefaultStartYear()
        {
            return 2015;
        }

        public override int DefaultStopYear()
        {
            return 2050;
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
            //int testrun = WSmith.runOneYear(year);
            //
            // DAS edit 06.09.20
            // Colorado River Model
            ToClose(year);
            bool runCOmodel = WestModel.COS.RunCOoneYear(year);
            // DAS end edits 06.09.20
            //
            // WATERSIMMODEL
            int  testrun = WestModel.runOneYear(year);

            return testrun;
         }
        //
        private void ToClose(int year)
        {
            if (year == Simulation_End_Year) WestModel.COS.streamCO = true;
        }
        //
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Must be called to setup a Simulation All simulations should be stopped with
        ///     StopSimulation(), which will make sure all files are closed.</summary>
        ///
        /// <remarks> Sets SimulationLock to false, _inRun to false, runs ProcessInitializeAll(), and
        ///     _simulationStarted to true override methods should use following structure
        ///        protected override Simulation_Initialize()
        ///        {
        ///           callmodelinitilaization();
        ///           base.Simulation_Initialize()
        ///           checkformodelerror()
        ///        }</remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void Simulation_Initialize()
        {
            base.Simulation_Initialize();
            //QUAY EDIT 
            // This is now a multi model framwework
            //WSmith.ResetNetwork();
            
            // Any WaterSimManager parameters should be reset at this point
            resetManagerParameters();

            // need to reset all the model networks
            WestModel.ResetNetwork();
            // End Edit
            // Sampson edits 09.04.18 SHOULD NOT BE HERE, but where?
            // WestModel.set_GrayWaterPotential();
            // end Sampson edits 09.04.18
            Simulation_End_Year = 2020;
        }

        // -------------------------------------------------------------------------------------------------------------------------
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the model version.</summary>
        /// <returns> The model version.</returns>
        ///-------------------------------------------------------------------------------------------------

        protected override string GetModelVersion()
        {
// QUAY EDIT 4/28/20
            return "WSW.6.0.1";
// END EDIT 
        }
        // =======================================
        protected override void initialize_ExtendedDocumentation()
        {
            throw new NotImplementedException();
        }
        // =========================================

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes the model parameters.</summary>
        ///-------------------------------------------------------------------------------------------------

        protected override void initialize_ModelParameters()
        {
            WaterSimManager WSim = (this as WaterSimManager);
            
          //  WSim.ProcessManager.AddProcess(WM);
            base.initialize_ModelParameters();
            this.Sim_EndYear = 2100;
            ParameterManagerClass FPM = ParamManager;        
            Extended_Parameter_Documentation ExtendDoc = FPM.Extended;
            // =======================================================
             // Provider parameters
            // Inputs/Outputs

             //
            // Controls - Policy
            

            //// Index Values
            _pm.AddParameter(new ModelParameterClass(eModelParam.epPolicyStartYear, "Policy Start Year", "PST", rangeChecktype.rctCheckRange, 2016, 2060, geti_PolicyStartYear, seti_PolicyStartYear, RangeCheck.NoSpecialBase));
               ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epPolicyStartYear, "Year that the Policies are implemented", "yr", "Year", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            // Controls - External Forcings

            
            // -----------------------------
            // Initialize Other
               WestModel.startYear = _StartYear;
               WestModel.endYear = _EndYear;
               WestModel.currentYear = _StartYear;

            //WSmith.startYear = _StartYear;
            //WSmith.endYear = _EndYear;
            //WSmith.currentYear = _StartYear;
            // =============================
            //
            defaultSettings();
            //
            // 12.14.16 added
            WestModel.policyStartYear = geti_PolicyStartYear();

            //WSmith.policyStartYear = geti_PolicyStartYear();

            #region WestModelParameters
            //============================================================================================================================
            // WEST MODEL PARAMETERS
            // ===========================================================================================================================

            
            // Population
            WestModel.Population = new providerArrayProperty(_pm, eModelParam.epP_Population, WestModel.geti_Pop, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Population, "Population Served", "POP_P", WestModel.Population));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Population, "Region Population in any given year", "ppl", "State Population (ppl)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // GPCD_urban
            WestModel.GPCD_urban = new providerArrayProperty(_pm, eModelParam.epP_GPCD_urban, WestModel.geti_gpcd, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GPCD_urban, "Urban GPCD", "UGPCD_P", WestModel.GPCD_urban));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GPCD_urban, "The GPCD (Gallons per Capita per Day) for delivered water for the Urban water sector.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // GPCD_ag
            WestModel.GPCD_ag = new providerArrayProperty(_pm, eModelParam.epP_GPCD_ag, WestModel.geti_gpcdAg, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GPCD_ag, "Agricultural GPCD", "AGPCD_P", WestModel.GPCD_ag));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GPCD_ag, "The GPCD (Gallons per Capita per Day) for delivered water for Agricultural Uses.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // GPCD_other
            WestModel.GPCD_other = new providerArrayProperty(_pm, eModelParam.epP_GPCD_other, WestModel.geti_gpcdOther, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GPCD_other, "Other GPCD: Power and Industry", "OGPCD_P", WestModel.GPCD_other));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GPCD_other, "The GPCD (Gallons per Capita per Day) for delivered water for Industrial Uses and Power Combined.", "GPCD", "Gallons per Capita per Day (GPCD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // SurfaceFresh
            WestModel.SurfaceFresh = new providerArrayProperty(_pm, eModelParam.epP_SurfaceFresh, WestModel.geti_SurfaceWaterFresh, WestModel.seti_SurfaceWaterFresh, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceFresh, "Surface Water (Fresh)", "SUR_P", WestModel.SurfaceFresh));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SurfaceFresh, "Fresh Water Deliveries from Surface Sources; this is total fresh water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "Surface Water", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // SurfaceFreshNet
            WestModel.SurfaceFreshNet = new providerArrayProperty(_pm, eModelParam.epP_SurfaceFreshNet, WestModel.geti_SurfaceWaterFreshNet, eProviderAggregateMode.agSum);
            // QUAY EDIT 9/13/18
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceFreshNet, "Surface Water (Fresh) Net", "SURN_P", WestModel.SurfaceFreshNet));
            // END EDIT

            // EDIT QUAY 9/12/18
            // Added Colorado
            // 
            // SurfaceColorado
            WestModel.SurfaceColorado = new providerArrayProperty(_pm, eModelParam.epP_SurfaceColorado, WestModel.geti_SurfaceColorado, WestModel.seti_SurfaceColorado, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceColorado, "Colorado Water (Fresh)", "COL_P", WestModel.SurfaceColorado));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SurfaceColorado, "Water Deliveries from Colorado River.", "MGD", "Million Gallons per Day (MGD)", "Colorado Water", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // SurfaceColoradoNet
            WestModel.SurfaceColoradoNet = new providerArrayProperty(_pm, eModelParam.epP_SurfaceColoradoNet, WestModel.geti_SurfaceColoradoNet, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceColoradoNet, "Colorado Water (Fresh) Net", "COLN_P", WestModel.SurfaceColoradoNet));


            // End EDIT 9/21/18 

            // SurfaceSaline
            WestModel.SurfaceSaline = new providerArrayProperty(_pm, eModelParam.epP_SurfaceSaline, WestModel.geti_SurfaceWaterSaline, WestModel.seti_SurfaceWaterSaline, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceSaline, "Surface Water (Saline)", "SAL_P", WestModel.SurfaceSaline));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SurfaceSaline, "Saline Water Deliveries from Surface Sources; this is total saline water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // SurfaceSalineNet
            WestModel.SurfaceSalineNet = new providerArrayProperty(_pm, eModelParam.epP_SurfaceSalineNet, WestModel.geti_SurfaceWaterSalineNet, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceSalineNet, "Surface Water (Saline) Net", "SALN_P", WestModel.SurfaceSalineNet));

            // Groundwater
            WestModel.Groundwater = new providerArrayProperty(_pm, eModelParam.epP_Groundwater, WestModel.geti_Groundwater, WestModel.seti_Groundwater, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Groundwater, "Groundwater (Fresh)", "GW_P", WestModel.Groundwater));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Groundwater, "Fresh Water Deliveries from Pumped Groundwater; this is total water withdrawals.", "MGD", "Million Gallons per Day (MGD)", "Groundwater", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // GroundwaterNet
            WestModel.GroundwaterNet = new providerArrayProperty(_pm, eModelParam.epP_GroundwaterNet, WestModel.geti_GroundwaterNet, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GroundwaterNet, "Groundwater (Fresh) Net", "GWN_P", WestModel.GroundwaterNet));

            // Effluent NOTE, this is really not "effluent" ie water discharged from a wastewater treatment plant, this is reclaimed water
            WestModel.Effluent = new providerArrayProperty(_pm, eModelParam.epP_Effluent, WestModel.geti_Effluent, WestModel.seti_Effluent, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Effluent, "Effluent (Reclaimed)", "REC_P", WestModel.Effluent));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Effluent, "Effluent (reclaimed) Water Deliveries from Waste Water Treatment Plants; total withdrawals.", "MGD", "Million Gallons per Day (MGD)", "Reclaimed Water", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // EffluentNet
            WestModel.EffluentNet = new providerArrayProperty(_pm, eModelParam.epP_EffluentNet, WestModel.geti_EffluentNet, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_EffluentNet, "Effluent (Reclaimed) Net", "RECN_P", WestModel.EffluentNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_EffluentNet, "Effluent (reclaimed) Water from Waste Water Treatment Plants that is discharged and not used for reuse.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Urban Wastewater (the real thing)
            WestModel.UrbanWastewaterFlow = new providerArrayProperty(_pm, eModelParam.epP_UrbanWasteWater, WestModel.geti_UrbanWasteWaterFlow,eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_UrbanWasteWater, "Urban Wastewater", "WSTW_P", WestModel.UrbanWastewaterFlow));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_UrbanWasteWater, "An estimate of the potential wastewater generated a wastewater treatment plant based on urban consumer water use.", "MGD", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            // SurfaceLake
            WestModel.SurfaceLake = new providerArrayProperty(_pm, eModelParam.epP_SurfaceLake, WestModel.geti_SurfaceLake, WestModel.seti_SurfaceLake, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceLake, "Surface Lake Water", "SURL_P", WestModel.SurfaceLake));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SurfaceLake, "Surface Lake Water", "mgd", "Million Gallons Per Day", "Surface Lake Water", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // SurfaceLakeNet
            WestModel.SurfaceLakeNet = new providerArrayProperty(_pm, eModelParam.epP_SurfaceLakeNet, WestModel.geti_SurfaceLakeNet, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceLakeNet, "Surface Lake Water Net", "SURLN_P", WestModel.SurfaceLakeNet));

            // TotalSupplies
            WestModel.TotalSupplies = new providerArrayProperty(_pm, eModelParam.epP_TotalSupplies, WestModel.geti_TotalSupplies, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_TotalSupplies, "Total Supplies", "TS_P", WestModel.TotalSupplies));

            // Urban
            WestModel.Urban = new providerArrayProperty(_pm, eModelParam.epP_Urban, WestModel.geti_Urban, WestModel.seti_Urban, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Urban, "Cities and Towns Demand", "UD_P", WestModel.Urban));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Urban, "Cities and Towns Water Demand", "MGD ", "Million Gallons per Day", "Cities and Towns", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // UrbanNet
            WestModel.UrbanNet = new providerArrayProperty(_pm, eModelParam.epP_UrbanNet, WestModel.geti_Urban_Net, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_UrbanNet, "Urban Demand (Net)", "UDN_P", WestModel.UrbanNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_UrbanNet, "Urban (residential) Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Agriculture
            WestModel.Agriculture = new providerArrayProperty(_pm, eModelParam.epP_Agriculture, WestModel.geti_Agriculture, WestModel.seti_Agriculture, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Agriculture, "Agriculture Demand", "AD_P", WestModel.Agriculture));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Agriculture, "Agriculture Water Demand; total withdrawals.", "MGD ", "Million Gallons per Day (MGD)", "Agriculture", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // AgricultureNet
            WestModel.AgricultureNet = new providerArrayProperty(_pm, eModelParam.epP_AgricultureNet, WestModel.geti_Agriculture_Net, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgricultureNet, "Agriculture Demand (Net)", "ADN_P", WestModel.AgricultureNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureNet, "Agricultural Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Industrial
            WestModel.Industrial = new providerArrayProperty(_pm, eModelParam.epP_Industrial, WestModel.geti_Industrial, WestModel.seti_Industrial, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Industrial, "Industry Demand", "ID_P", WestModel.Industrial));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Industrial, "Industrial Water Demand; total withdrawals. Water used for industries such as steel, chemical, paper, and petroleum refining. ", "MGD ", "Million Gallons per Day (MGD)", "Industry", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // IndustrialNet
            WestModel.IndustrialNet = new providerArrayProperty(_pm, eModelParam.epP_IndustrialNet, WestModel.geti_Industrial_Net, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_IndustrialNet, "Industrial Demand (Net)", "IDN_P", WestModel.IndustrialNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_IndustrialNet, "Industrial Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            
            // IndustrialGPED
            WestModel.IndustrialGPED = new providerArrayProperty(_pm, eModelParam.epP_IndustrialGPED, WestModel.geti_IndustrialGPED, eProviderAggregateMode.agAverage);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_IndustrialGPED, "Industrial GPED", "IDGPED_P", WestModel.IndustrialGPED));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_IndustrialGPED, "Industrial Gallons of demand per industrial employee.", "GPED", "Gallons Per Employee per Day (GPED)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            // EDIT QUAY 3/11/18
            // Industry Employees
            WestModel.IndustryEmployees = new providerArrayProperty(_pm, eModelParam.epP_IndustryEmployees, WestModel.geti_IndustryEmployees, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_IndustryEmployees, "Industrial Employees", "IDEMP_P", WestModel.IndustryEmployees));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_IndustryEmployees, "Number of people employeed in the major industry sectors such as steel, chemical, paper, and petroleum refining. ", "Person(,000)", "Thousand People", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            // Power
            WestModel.Power = new providerArrayProperty(_pm, eModelParam.epP_Power, WestModel.geti_PowerWater, WestModel.seti_PowerWater, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Power, "Power Demand", "PD_P", WestModel.Power));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Power, "Water Use by Power: total withdrawals. Water used in the process of generating electricity with steam-driven turbine generators [Thermoelectric power, subcategories by cooling-system type (once-through, closed-loop/recirculation)].", "MGD ", "Million Gallons per Day (MGD)", "Electricity Production", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // PowerNet
            WestModel.PowerNet = new providerArrayProperty(_pm, eModelParam.epP_PowerNet, WestModel.geti_PowerWater_Net, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PowerNet, "Power Demand (Net)", "PDN_P", WestModel.PowerNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerNet, "Power Net Water Balance; the difference between source withdrawals and demand.", "MGD ", "Million Gallons per Day (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // PowerEnergy
            WestModel.PowerEnergy = new providerArrayProperty(_pm, eModelParam.epP_PowerEnergy, WestModel.geti_PowerEnergy, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PowerEnergy, "Power Produced", "PE_P", WestModel.PowerEnergy));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerEnergy, "The number of MegaWatts of electricity produced per day", "MW ", "Million Watts Per Day(MW)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // PowerGPMWD
            WestModel.PowerGPMWD = new providerArrayProperty(_pm, eModelParam.epP_PowerGPMWD, WestModel.geti_PowerGPMWD, eProviderAggregateMode.agAverage);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PowerGPMWD, "Power GPMWD", "PGPMWD_P", WestModel.PowerGPMWD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerGPMWD, "The Number of gallons of water needed to produce one megawatt of electrical power.", "GPMWD", "Gallons Per MegaWatt of Power per Day (GPMWD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // NetDemandDifference
            WestModel.NetDemandDifference = new providerArrayProperty(_pm, eModelParam.epP_NetDemandRatio, WestModel.geti_NetDemandDifference, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_NetDemandRatio, "Net Demand Difference", "DDIF_P", WestModel.NetDemandDifference));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_NetDemandRatio, "The ratio of net demand to total demand for all consumers; ", "% ", "Percent (%)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // TotalDemand
            WestModel.TotalDemand = new providerArrayProperty(_pm, eModelParam.epP_TotalDemand, WestModel.geti_TotalDemand, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_TotalDemand, "Total Demand", "TOTD_P", WestModel.TotalDemand));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_TotalDemand, "The total of demand for all consumers; ", "MGD ", "MGD", "Total Demand", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            WestModel.TotalDemandNet = new providerArrayProperty(ParamManager, eModelParam.epP_TotalDemandNet, WestModel.geti_TotalDemandNet, eProviderAggregateMode.agSum);
            ParamManager.AddParameter(new ModelParameterClass(eModelParam.epP_TotalDemandNet, "Total Demand (Net)", eModelFields.epP_TotalDemandNet, WestModel.TotalDemandNet));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_TotalDemandNet, "Total NetWater Demand for all consumers, essentially Water Sources - Demand", "MGD", "Total Net Water DEmand (MGD)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            //=======================================================
            // Model Controls
            // UrbanWaterConservation
            //         public ModelParameterClass(int aModelParam, string aLabel, string aFieldname, rangeChecktype aRangeCheckType, int aLowRange, int aHighRange, DcheckProvider specialProviderRangeCheck, providerArrayProperty Providerproperty)
            //  rangeChecktype.rctCheckRange, 50, 100,

            // use these for standards, change here instead of multiple places
            int[] StandardResourceAdjustments = new int[5] { 60,  80, 100, 120, 140};
            int[] StandardConsumerAdjustments = new int[5] { 100, 85,  70,  55,  40 };


            WestModel.UrbanWaterConservation = new providerArrayProperty(_pm, eModelParam.epP_UrbanWaterConservation, WestModel.geti_UrbanConservation, WestModel.seti_UrbanConservation, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_UrbanWaterConservation, "Cities and Towns Water Use", "UCON_P", rangeChecktype.rctCheckRange, 20, 100,null, WestModel.UrbanWaterConservation));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_UrbanWaterConservation, "Reduces annual water use by improving efficiency of cities and towns water use.", "", "Reduce Cities and Towns Water Use", "Cities and Towns", new string[5] { "No Change", "Slight", "Moderate", "Severe", "Extreme" }, StandardConsumerAdjustments, new ModelParameterGroupClass[] { }));

            // AgWaterConservation
            WestModel.AgWaterConservation = new providerArrayProperty(_pm, eModelParam.epP_AgWaterConservation, WestModel.geti_AgConservation, WestModel.seti_AgConservation, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgWaterConservation, "Agriculure Water Use", "ACON_P", rangeChecktype.rctCheckRange, 20, 100, null, WestModel.AgWaterConservation));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgWaterConservation, "Reduces annual water use by improving efficiency of agriculture water use.", "", "Reduce Agriculture Water Use", "Agriculture", new string[5] { "No Change", "Slight", "Moderate", "Severe", "Extreme" }, StandardConsumerAdjustments, new ModelParameterGroupClass[] { }));

            // PowerWaterConservation
            WestModel.PowerWaterConservation = new providerArrayProperty(_pm, eModelParam.epP_PowerWaterConservation, WestModel.geti_PowerConservation, WestModel.seti_PowerConservation, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PowerWaterConservation, "Electric Water Use", "PCON_P", rangeChecktype.rctCheckRange, 20, 100, null, WestModel.PowerWaterConservation));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PowerWaterConservation, "Reduces annual water use by improving efficiency of electrical power production water use.", "", "Reduce Power Water Use", "Electricity Production", new string[5] { "No Change", "Slight", "Moderate", "Severe", "Extreme" }, StandardConsumerAdjustments, new ModelParameterGroupClass[] { }));

            // IndustrialWaterConservation
            WestModel.IndustrialWaterConservation = new providerArrayProperty(_pm, eModelParam.epP_IndustrialWaterConservation, WestModel.geti_IndustryConservation, WestModel.seti_IndustryConservation, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_IndustrialWaterConservation, "Industrial Water Use", "ICON_P", rangeChecktype.rctCheckRange, 20, 100, null, WestModel.IndustrialWaterConservation));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_IndustrialWaterConservation, "Reduces annual water by improving efficiency of industrial water use.", "", "Reduce Industry Water Use", "Industry", new string[5] { "No Change", "Slight", "Moderate", "Severe", "Extreme" }, StandardConsumerAdjustments, new ModelParameterGroupClass[] { }));

            // SurfaceWaterManagement
            WestModel.SurfaceWaterManagement = new providerArrayProperty(_pm, eModelParam.epP_SurfaceWaterManagement, WestModel.geti_SurfaceWaterControl, WestModel.seti_SurfaceWaterControl, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SurfaceWaterManagement, "Surface Water", "SWM_P", rangeChecktype.rctCheckRange, 0, 200, null, WestModel.SurfaceWaterManagement));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SurfaceWaterManagement, "Change the amount of Surface Water that will be available for use by water consumers.", "", "Change Available Surface Water", "Surface Water", new string[5] { "Least", "Less", "No Change", "More", "Most" }, StandardResourceAdjustments, new ModelParameterGroupClass[] { }));

            // GroundwaterManagement
            WestModel.GroundwaterManagement = new providerArrayProperty(_pm, eModelParam.epP_GroundwaterManagement, WestModel.geti_GroundwaterControl, WestModel.seti_GroundwaterControl, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GroundwaterManagement, "Groundwater", "GWM_P", rangeChecktype.rctCheckRange, 0, 200, null, WestModel.GroundwaterManagement));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GroundwaterManagement, "Change the amount of Ground Water that can be pumped for use by water consumers.", "", "Change Available Groundwater", "Groundwater", new string[5] { "Least", "Less", "No Change", "More", "Most" }, StandardResourceAdjustments, new ModelParameterGroupClass[] { }));

            // ReclainedWaterUse
            WestModel.ReclainedWaterUse = new providerArrayProperty(_pm, eModelParam.epP_ReclainedWaterUse, WestModel.geti_ReclaimedWaterManagement, WestModel.seti_ReclaimedWaterManagement, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_ReclainedWaterUse, "Reclaimed Water", "RECM_P", rangeChecktype.rctCheckRange, 0, 100, null, WestModel.ReclainedWaterUse));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ReclainedWaterUse, "Change the amount of reclaimed (effluent) supplies that can be used by water consumers.", "", "Change Available Reclaimed Water", "Reclaimed Water", new string[5] { "None", "Low", "Moderate", "High", "All" }, new int[5] { 0, 25, 50, 75, 100 }, new ModelParameterGroupClass[] { }));

            // LakeWaterManagement
            WestModel.LakeWaterManagement = new providerArrayProperty(_pm, eModelParam.epP_LakeWaterManagement, WestModel.geti_LakeWaterManagement, WestModel.seti_LakeWaterManagement, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_LakeWaterManagement, "Increase Lake Water use", "LWM_P", rangeChecktype.rctCheckRange, 0, 200, null, WestModel.LakeWaterManagement));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_LakeWaterManagement, "Controls Lake Water Management: increased or decreased lake water withdrawals.", "", "Scenario changes in lake later withdrawals", "", new string[4] { "Less", "None", "More", "Most" }, new int[4] { 80, 100, 120, 140 }, new ModelParameterGroupClass[] { }));
            //
            // Sampson edits 08.18.18
            // GrayWaterManagement
            WestModel.GrayWaterManagement = new providerArrayProperty(_pm, eModelParam.epP_GrayWaterManagement, WestModel.geti_GrayWaterManagement, WestModel.seti_GrayWaterManagement, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GrayWaterManagement, "Use Gray Water ", "GYWM_P", rangeChecktype.rctCheckRange, 0, 100, null, WestModel.GrayWaterManagement));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GrayWaterManagement, "Controls Gray Water Management: increased Gray water.", "", "Scenario changes in Gray Water Use", "", new string[4] { "None", "Some", "More", "Most" }, new int[4] {0, 33, 66, 100}, new ModelParameterGroupClass[] { }));             
            // End Sampson edits 08.18.18

            // Augmented
            WestModel.Augmented = new providerArrayProperty(_pm, eModelParam.epP_Augmented, WestModel.geti_Desalinization, WestModel.seti_Desalinization, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_Augmented, "Augmented Desal", "DESAL_P", rangeChecktype.rctCheckRange, 0, 200, null, WestModel.Augmented));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_Augmented, "Adds a percent of desalinaiton: increased surface saline withdrawals.", "", "Scenario changes in saline surface water withdrawals", "", new string[4] { "None", "Low", "Med", "High" }, new int[4] { 0, 100, 150, 200 }, new ModelParameterGroupClass[] { }));

            // PopGrowthRate
            WestModel.PopGrowthRate = new providerArrayProperty(_pm, eModelParam.epP_PopGrowthRates, WestModel.geti_PopGrowthRate, WestModel.seti_PopGrowthRate, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PopGrowthRates, "Population Growth Rate", "POPRTE_P", rangeChecktype.rctCheckRange, 40, 150, null, WestModel.PopGrowthRate));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PopGrowthRates, "The Population Growth Rate used to project future population.", "%", "Rate of Population Growth", "", new string[4] { "Low", "Some", "Planned", "High" }, new int[4] { 60, 80, 100, 120 }, new ModelParameterGroupClass[] { }));
            //
            // 08.07.17 das
            // Population Growth Rate Modifier
            // Input Parameter 
            WestModel.PopGrowthRateModifyer = new providerArrayProperty(_pm, eModelParam.epP_PopGrowthRateModifyer, WestModel.geti_PopGrowthRateMod, WestModel.seti_PopGrowthRateMod, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_PopGrowthRateModifyer, "Projected Population Growth", "POPGRM_P", rangeChecktype.rctCheckRange, 0, 150, RangeCheck.NoSpecialProvider,WestModel.PopGrowthRateModifyer));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_PopGrowthRateModifyer, "Increase or decrease the Projected Population Growth Rate.", "%", "Change Projected Population Growth", "", new string[5] { "Lowest", "Low","Lower", "Planned", "Higher" }, new int[5] { 40, 60, 80, 100, 120 }, new ModelParameterGroupClass[] { }));

            // ClimateDrought
            WestModel.ClimateDrought = new providerArrayProperty(_pm, eModelParam.epP_ClimateDrought, WestModel.geti_DroughtImpacts, WestModel.seti_DroughtImpacts, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_ClimateDrought, "Drought Impacts on Rivers/Lakes ", "CLIM_P", WestModel.ClimateDrought));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ClimateDrought, "Alteration in Fresh Water Withdrawals as a result of drought on supplies.", "Scenario-driven", "Drought Reductions in Surface Water", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            //new string[5] {"No Change", "Slight", "Moderate", "Severe", "Extreme"},new int[5] { 100, 85, 70, 55, 40 }
            //  -----------------
            //  Drought Factors
            //  -----------------

            // DroughtControl
            WestModel.DroughtControl = new providerArrayProperty(_pm, eModelParam.epP_DroughtControl, WestModel.geti_DroughtControl, WestModel.seti_DroughtControl, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtControl, "Drought Impacts Control- controls rate", "DC_P", WestModel.DroughtControl));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtControl, "Percent reduction in Surface flows due to drought", "%", "Percent (%)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            WestModel.DroughtActive = new providerArrayProperty(_pm, eModelParam.epP_DroughtActive, WestModel.geti_DroughtActive, WestModel.seti_DroughtActive, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtActive, "Drought Active", "DACT_P", rangeChecktype.rctCheckRange, 0, 1, null, WestModel.DroughtActive));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtActive, "A flag used to indicate if drought conditions will be applied to the simulation, 1=Active and 0=Inactive", "FLAG", "Activate Drought", "", new string[2] {"Turn Off","Turn On" }, new int[2] {0,1 }, new ModelParameterGroupClass[] { }));

            WestModel.DroughtStartYear = new providerArrayProperty(_pm, eModelParam.epP_DroughtStartYear, WestModel.geti_DroughtStartYear, WestModel.seti_DroughtStartYear, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtStartYear, "Drought Start Year", "DSTYR_P", rangeChecktype.rctCheckRange, 2015, 2100, null, WestModel.DroughtStartYear));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtStartYear, "The first year of a series of years subject to drought conditions", "Yr", "Year", "Drought Start Year", new string[5] {"2015","2020","2025","2030","2035" }, new int[5] { 2015, 2020, 2025, 2030, 2035 }, new ModelParameterGroupClass[] { }));

            WestModel.DroughtLength = new providerArrayProperty(_pm, eModelParam.epP_DroughtLength, WestModel.geti_DroughtLength, WestModel.seti_DroughtLength, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtLength, "Length of Drought", "DLEN_P", rangeChecktype.rctCheckRange, 5, 100, null, WestModel.DroughtLength));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtLength, "The length in years of a series of years subject to drought conditions", "Yrs", "Years", "Drought Length", new string[5] { "50 Yrs", "40 Yrs", "30 Yrs", "30 Yrs", "10 Yrs" }, new int[5] { 50, 40, 30, 20, 10 }, new ModelParameterGroupClass[] { }));

            WestModel.DroughtDepth = new providerArrayProperty(_pm, eModelParam.epP_DroughtDepth, WestModel.geti_DroughtDepth, WestModel.seti_DroughtDepth, eProviderAggregateMode.agNone);
           
// QUAY EDIT 9/13/18
            //_pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtDepth, "Drought Severity", "DDPTH_P", rangeChecktype.rctCheckRange, 0, 100, null, WestModel.DroughtDepth));
            //ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtDepth, "The severity of drought, as a percentage of base surface water flow to be realized at the mid point of the drought", "%", "Percent", "Drought Severity", new string[5] { "Extreme", "Severe", "Moderate", "Slight", "Minimal" }, new int[5] { 10, 30, 50, 70, 90 }, new ModelParameterGroupClass[] { }));
             _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtDepth, "Drought Depth", "DDPTH_P", rangeChecktype.rctCheckRange, 0, 100, null, WestModel.DroughtDepth));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtDepth, "The depth (severity) of drought, as a percentage of base surface water flow to be realized at the mid point of the drought", "%", "Percent", "Drought Severity", new string[5] { "Extreme", "Severe", "Moderate", "Slight", "Minimal" }, new int[5] { 10, 30, 50, 70, 90 }, new ModelParameterGroupClass[] { }));

            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DroughtScenario, "Drought Severity", "DSCN_P", rangeChecktype.rctCheckRange, 0, 4, geti_SimpleAllRegionsDroughtScenario, seti_SimpleAllRegionsDroughtScenario, RangeCheck.NoSpecialBase));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DroughtScenario, "Scenarios for drought ranging from none to extreme", "#", "Secanario", "Drought Severity", new string[5] { "None", "Slight", "Moderate", "Severe", "Extreme" }, new int[5] { 0, 1, 2, 3, 4 }, new ModelParameterGroupClass[] { }));

            // --------------------------
            // CLIMATE CHANGE
            // --------------------------

            WestModel.ClimateChangeTarget = new providerArrayProperty(_pm, eModelParam.epP_ClimateChangeTarget, WestModel.geti_ClimateChangeTarget, WestModel.seti_ClimateChangeTarget, eProviderAggregateMode.agWeighted);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_ClimateChangeTarget, "Climate Change Impact", "CCT_P", WestModel.ClimateChangeTarget));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ClimateChangeTarget, "Percent reduction in Surface flows due to climate change", "%", "Percent (%)", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            WestModel.ClimateChangeTargetYear = new providerArrayProperty(_pm, eModelParam.epP_ClimateChangeTargetYear, WestModel.geti_ClimateChangeTargetYear, WestModel.seti_ClimateChangeTargetYear, eProviderAggregateMode.agAverage);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_ClimateChangeTargetYear, "Climate Change Target Year", "CCY_P", WestModel.ClimateChangeTargetYear));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_ClimateChangeTargetYear, "Year in which terageted climate change reduction in Surface flows is realized", "Yr", "Year", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // this parameter is a little funky, creating a base parameter that you set actually set values in a provider parameter, one value all models.  But the value returned is the basin eighted average of the models values
            _pm.AddParameter(new ModelParameterClass(eModelParam.ep_ClimateChangeTarget, "Climate Change Impact", "CCT", rangeChecktype.rctCheckRange, 10, 100, WestModel.geti_ClimateChangeTargetBase,  WestModel.seti_ClimateChangeTargetBase, RangeCheck.NoSpecialBase));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.ep_ClimateChangeTarget, "Average Percent reduction in Surface flows due to climate change", "%", "Percent (%)", "Climate Change", new string[5] { "None", "Slight", "Moderate", "Severe", "Extreme" }, new int[5] { 100, 80, 60, 40, 20 }, new ModelParameterGroupClass[] { }));

            // this parameter is a little funky, creating a base parameter that you set actually set values in a provider parameer, one value all models.  But the year returned is the average of the models years
            _pm.AddParameter(new ModelParameterClass(eModelParam.ep_ClimateChangeTargetYear, "Climate Change Target Year", "CCY", rangeChecktype.rctCheckRange, 2015, 2100, WestModel.geti_ClimateChangeTargetYearBase, WestModel.seti_ClimateChangeTargetYearBase, RangeCheck.NoSpecialBase));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.ep_ClimateChangeTargetYear, "Average Year in which terageted climate change reduction in Surface flows is realized, returns a Missing Value", "Yr", "Year", "", new string[] {}, new int[] {}, new ModelParameterGroupClass[] { }));
// END EDIT 

            //--------------------------
            // AgricultureProduction
            WestModel.AgricultureProduction = new providerArrayProperty(_pm, eModelParam.epP_AgricultureProduction, WestModel.geti_AgricutureProduction, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgricultureProduction, "Agriculture Net $", "ANP_P", WestModel.AgricultureProduction));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgricultureProduction, "Agriculture Net Annual Farm Income.", "M$", "Million Dollars ", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // AgriculturalGrowth
            WestModel.AgriculturalGrowth = new providerArrayProperty(_pm, eModelParam.epP_AgriculturalGrowth, WestModel.geti_AgGrowthRate, WestModel.seti_AgGrowthRate, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgriculturalGrowth, "Agriculture Growth Rate", "AGGR_P", WestModel.AgriculturalGrowth));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgriculturalGrowth, "Agriculture Growth Rate Applied.", "%", "Percent of current growth", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Agricultural Gallons Per Dollar per Day
            WestModel.AgricultureGPDD = new providerArrayProperty(_pm, eModelParam.epP_AgGPDD, WestModel.geti_agricultureGPDD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgGPDD, "Agriculture GPDD", "AGGPDD_P", WestModel.AgricultureGPDD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgGPDD, "Agriculture Demand Gallons per Dollar of Farm Income.", "G", "Ag Gallons per Dollar", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            // Agricultural Gallons Per Dollar per Day   OutPut
            WestModel.initialAgricultureGPDD = new providerArrayProperty(_pm, eModelParam.epP_AgIGPDD, WestModel.geti_initialAgricultureGPDD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_AgIGPDD, "Agriculture Initial GPDD", "AGIGPDD_P", WestModel.initialAgricultureGPDD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_AgGPDD, "Initial Agriculture Demand Gallons per Dollar of Farm Income.", "G", "Ag Gallons per Dollar", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Cap Resources to Demand Toggle
            WestModel.DoCapLimits = new providerArrayProperty(_pm, eModelParam.epP_DoCapLimits, WestModel.geti_DoCapLimits, WestModel.seti_DoCapLimits, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DoCapLimits, "DoCapLimits", "CAPL_P", rangeChecktype.rctCheckRange, 0, 1, null, WestModel.DoCapLimits));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DoCapLimits, "A flag used to indicate if resources will be capped to actual demand to prevent over allocation of resources, 1=Active and 0=Inactive", "FLAG", "DoCapLimits", "", new string[2] { "Turn Off", "Turn On" }, new int[2] { 0, 1 }, new ModelParameterGroupClass[] { }));

            // Revise Resources if Limit is Capped
            WestModel.DoReviseResources = new providerArrayProperty(_pm, eModelParam.epP_DoReviseResource, WestModel.geti_DoReviseResources, WestModel.seti_DoReviseResources, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DoReviseResource, "DoReviseResources", "REVR_P", rangeChecktype.rctCheckRange, 0, 1, null, WestModel.DoReviseResources));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DoReviseResource, "A flag used to indicate if resources web controls will be revised at the end of a simulation to a value the reflects capped demand limits.  Will not work of DoCapLimits is not active, 1=Active and 0=Inactive", "FLAG", "DoReviseResources", "", new string[2] { "Turn Off", "Turn On" }, new int[2] { 0, 1 }, new ModelParameterGroupClass[] { }));

            //==========================================
            // FLUXES
            // =======================================
            // _SUR_UD
            WestModel._SUR_UD = new providerArrayProperty(_pm, eModelParam.epP_SUR_UD, WestModel.geti_SUR_UD, WestModel.seti_SUR_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SUR_UD, "SUR to UTOT Allocation", "SUR_UD_P", WestModel._SUR_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SUR_UD, "SUR Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "SUR to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SUR_AD
            WestModel._SUR_AD = new providerArrayProperty(_pm, eModelParam.epP_SUR_AD, WestModel.geti_SUR_AD, WestModel.seti_SUR_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SUR_AD, "SUR to ATOT Allocation", "SUR_AD_P", WestModel._SUR_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SUR_AD, "SUR Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "SUR to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SUR_ID
            WestModel._SUR_ID = new providerArrayProperty(_pm, eModelParam.epP_SUR_ID, WestModel.geti_SUR_ID, WestModel.seti_SUR_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SUR_ID, "SUR to ITOT Allocation", "SUR_ID_P", WestModel._SUR_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SUR_ID, "SUR Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "SUR to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SUR_PD
            WestModel._SUR_PD = new providerArrayProperty(_pm, eModelParam.epP_SUR_PD, WestModel.geti_SUR_PD, WestModel.seti_SUR_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SUR_PD, "SUR to PTOT Allocation", "SUR_PD_P", WestModel._SUR_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SUR_PD, "SUR Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "SUR to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SURL_UD
            WestModel._SURL_UD = new providerArrayProperty(_pm, eModelParam.epP_SURL_UD, WestModel.geti_SURL_UD, WestModel.seti_SURL_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SURL_UD, "SURL to UTOT Allocation", "SURL_UD_P", WestModel._SURL_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SURL_UD, "SURL Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "SURL to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SURL_AD
            WestModel._SURL_AD = new providerArrayProperty(_pm, eModelParam.epP_SURL_AD, WestModel.geti_SURL_AD, WestModel.seti_SURL_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SURL_AD, "SURL to ATOT Allocation", "SURL_AD_P", WestModel._SURL_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SURL_AD, "SURL Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "SURL to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SURL_ID
            WestModel._SURL_ID = new providerArrayProperty(_pm, eModelParam.epP_SURL_ID, WestModel.geti_SURL_ID, WestModel.seti_SURL_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SURL_ID, "SURL to ITOT Allocation", "SURL_ID_P", WestModel._SURL_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SURL_ID, "SURL Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "SURL to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SURL_PD
            WestModel._SURL_PD = new providerArrayProperty(_pm, eModelParam.epP_SURL_PD, WestModel.geti_SURL_PD, WestModel.seti_SURL_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SURL_PD, "SURL to PTOT Allocation", "SURL_PD_P", WestModel._SURL_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SURL_PD, "SURL Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "SURL to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _GW_UD
            WestModel._GW_UD = new providerArrayProperty(_pm, eModelParam.epP_GW_UD, WestModel.geti_GW_UD, WestModel.seti_GW_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GW_UD, "GW to UTOT Allocation", "GW_UD_P", WestModel._GW_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_UD, "GW Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "GW to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _GW_AD
            WestModel._GW_AD = new providerArrayProperty(_pm, eModelParam.epP_GW_AD, WestModel.geti_GW_AD, WestModel.seti_GW_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GW_AD, "GW to ATOT Allocation", "GW_AD_P", WestModel._GW_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_AD, "GW Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "GW to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _GW_ID
            WestModel._GW_ID = new providerArrayProperty(_pm, eModelParam.epP_GW_ID, WestModel.geti_GW_ID, WestModel.seti_GW_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GW_ID, "GW to ITOT Allocation", "GW_ID_P", WestModel._GW_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_ID, "GW Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "GW to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _GW_PD
            WestModel._GW_PD = new providerArrayProperty(_pm, eModelParam.epP_GW_PD, WestModel.geti_GW_PD, WestModel.seti_GW_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_GW_PD, "GW to PTOT Allocation", "GW_PD_P", WestModel._GW_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_GW_PD, "GW Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "GW to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _REC_UD
            WestModel._REC_UD = new providerArrayProperty(_pm, eModelParam.epP_REC_UD, WestModel.geti_REC_UD, WestModel.seti_REC_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_REC_UD, "REC to UTOT Allocation", "REC_UD_P", WestModel._REC_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_REC_UD, "REC Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "REC to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _REC_AD
            WestModel._REC_AD = new providerArrayProperty(_pm, eModelParam.epP_REC_AD, WestModel.geti_REC_AD, WestModel.seti_REC_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_REC_AD, "REC to ATOT Allocation", "REC_AD_P", WestModel._REC_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_REC_AD, "REC Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "REC to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _REC_ID
            WestModel._REC_ID = new providerArrayProperty(_pm, eModelParam.epP_REC_ID, WestModel.geti_REC_ID, WestModel.seti_REC_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_REC_ID, "REC to ITOT Allocation", "REC_ID_P", WestModel._REC_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_REC_ID, "REC Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "REC to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _REC_PD
            WestModel._REC_PD = new providerArrayProperty(_pm, eModelParam.epP_REC_PD, WestModel.geti_REC_PD, WestModel.seti_REC_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_REC_PD, "REC to PTOT Allocation", "REC_PD_P", WestModel._REC_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_REC_PD, "REC Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "REC to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SAL_UD
            WestModel._SAL_UD = new providerArrayProperty(_pm, eModelParam.epP_SAL_UD, WestModel.geti_SAL_UD, WestModel.seti_SAL_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SAL_UD, "SAL to UTOT Allocation", "SAL_UD_P", WestModel._SAL_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SAL_UD, "SAL Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "SAL to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SAL_AD
            WestModel._SAL_AD = new providerArrayProperty(_pm, eModelParam.epP_SAL_AD, WestModel.geti_SAL_AD, WestModel.seti_SAL_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SAL_AD, "SAL to ATOT Allocation", "SAL_AD_P", WestModel._SAL_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SAL_AD, "SAL Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "SAL to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SAL_ID
            WestModel._SAL_ID = new providerArrayProperty(_pm, eModelParam.epP_SAL_ID, WestModel.geti_SAL_ID, WestModel.seti_SAL_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SAL_ID, "SAL to ITOT Allocation", "SAL_ID_P", WestModel._SAL_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SAL_ID, "SAL Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "SAL to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _SAL_PD
            WestModel._SAL_PD = new providerArrayProperty(_pm, eModelParam.epP_SAL_PD, WestModel.geti_SAL_PD, WestModel.seti_SAL_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_SAL_PD, "SAL to PTOT Allocation", "SAL_PD_P", WestModel._SAL_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_SAL_PD, "SAL Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "SAL to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            // Demand Model Parameters
            // Last Write: 06.01.18
            // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Demand Model to Use
            WestModel.DemandModelUrban_Index = new providerArrayProperty(_pm, eModelParam.epP_DM_MODELtoUSE_Urban_P, WestModel.geti_DemandModelUrbanIndex, WestModel.seti_DemandModelUrbanIndex, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DM_MODELtoUSE_Urban_P, "Demand Model to Use", "DEMMODELU_P", rangeChecktype.rctCheckRange, 0, 5, RangeCheck.NoSpecialProvider, WestModel.DemandModelUrban_Index));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DM_MODELtoUSE_Urban_P, "The Water Demand Model to Use", "NA", "No Units", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            WestModel.DemandModelAg_Index = new providerArrayProperty(_pm, eModelParam.epP_DM_MODELtoUSE_Ag_P, WestModel.geti_DemandModelAgIndex, WestModel.seti_DemandModelAgIndex, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DM_MODELtoUSE_Ag_P, "Demand Model to Use", "DEMMODELA_P", rangeChecktype.rctCheckRange, 0, 5, RangeCheck.NoSpecialProvider, WestModel.DemandModelAg_Index));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DM_MODELtoUSE_Ag_P, "The Water Demand Model to Use", "NA", "No Units", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));
            //
            WestModel.DemandModelInd_Index = new providerArrayProperty(_pm, eModelParam.epP_DM_MODELtoUSE_Ind_P, WestModel.geti_DemandModelIndIndex, WestModel.seti_DemandModelIndIndex, eProviderAggregateMode.agNone);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_DM_MODELtoUSE_Ind_P, "Demand Model to Use", "DEMMODELI_P", rangeChecktype.rctCheckRange, 0, 5, RangeCheck.NoSpecialProvider, WestModel.DemandModelInd_Index));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_DM_MODELtoUSE_Ind_P, "The Water Demand Model to Use", "NA", "No Units", "", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // Input Parameter 
// EDIT QUAY 9/12/18 Very Very Late at night
            // Added 
            // _COL_UD
            WestModel._COL_UD = new providerArrayProperty(_pm, eModelParam.epP_COL_UD, WestModel.geti_COL_UD, WestModel.seti_COL_UD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_COL_UD, "COL to UTOT Allocation", "COL_UD_P", WestModel._COL_UD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_COL_UD, "Colorado Water Supply allocated to UTOT water consumption", "MGD", "Million Gallons Per Day", "COL to UTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _COL_AD
            WestModel._COL_AD = new providerArrayProperty(_pm, eModelParam.epP_COL_AD, WestModel.geti_COL_AD, WestModel.seti_COL_AD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_COL_AD, "COL to ATOT Allocation", "COL_AD_P", WestModel._COL_AD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_COL_AD, "Colorado  Water Supply allocated to ATOT water consumption", "MGD", "Million Gallons Per Day", "COL to ATOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _COL_ID
            WestModel._COL_ID = new providerArrayProperty(_pm, eModelParam.epP_COL_ID, WestModel.geti_COL_ID, WestModel.seti_COL_ID, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_COL_ID, "COL to ITOT Allocation", "COL_ID_P", WestModel._COL_ID));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_COL_ID, "Colorado  Water Supply allocated to ITOT water consumption", "MGD", "Million Gallons Per Day", "COL to ITOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));

            // _COL_PD
            WestModel._COL_PD = new providerArrayProperty(_pm, eModelParam.epP_COL_PD, WestModel.geti_COL_PD, WestModel.seti_COL_PD, eProviderAggregateMode.agSum);
            _pm.AddParameter(new ModelParameterClass(eModelParam.epP_COL_PD, "COL to PTOT Allocation", "COL_PD_P", WestModel._COL_PD));
            ExtendDoc.Add(new WaterSimDescripItem(eModelParam.epP_COL_PD, "Colorado  Water Supply allocated to PTOT water consumption", "MGD", "Million Gallons Per Day", "COL to PTOT", new string[] { }, new int[] { }, new ModelParameterGroupClass[] { }));


            // END EDIT 9/12/18 even later


            // [Median Dwelling Units Per Acre]	
            // [Persons Per Household (Assumed)]
            // [Indoor Gallons Per Capita Per Day]
            // [Pervious Sq.Ft. per Unit]
            // [Median Gallons Per Sq.Ft. Pervious Area]
            // Inputs
            //(out double Demand, out double Indoor, out double Outdoor, out double Process, out double Households, out double Persons, out double Acres, out double PerviousSqFt,
            //              out double AvgHHSize, out double AvgHHPerAcre, out double AvgGPCD, out double AvgGPSQFT, out double AvgGPU, out double HH_Moved, out double PES_Cost, out double MovedOutDemand,
            //              out double MovedDemand)
        }
        #endregion WestModelParameters
        //
        //int FModelCount = 0;
        //int ModelCount = 0;
        ////
        //void setGrayWaterPotential()
        //{
        //    //
        //    ModelCount = FModelCount;
        //    int[] RecToUrban = new int[FModelCount];
        //    int[] multiply = new int[FModelCount];
        //    int[] gray = new int[FModelCount];

        //    RecToUrban =  WestModel.geti_Effluent();
        //    multiply = WestModel.geti_GrayWaterManagement();
        //    gray = WestModel.geti_GrayWaterFlow();
        //    grayWaterPotential(RecToUrban, gray,multiply);

        // }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="baseRecUrban"></param>
        ///// <param name="GrayWater"></param>
        ///// <param name="management"></param>
        //public void grayWaterPotential(int[] baseRecUrban, int[] GrayWater, int[] management)
        //{
        //    ModelCount = this.FModelCount;
        //    //
        //    int[] iGray = new int[FModelCount];
        //    double G = 0;
        //    for (int i = 0; i < FModelCount; i++)
        //    {
        //        int result = 0;
        //        double Temp = 0;
        //        G = (double)GrayWater[i];
        //        if (baseRecUrban[i] <= 2)
        //            Temp = management[i] * G;
        //        else
        //            Temp = baseRecUrban[i];
        //        result = (int)Temp;
        //        iGray[i] = result;
        //    }
           
        //   // ModelParameterClass MP = FWSim.ParamManager.Model_Parameter(eModelParam.epP_Effluent);
        //    // MP.ProviderProperty.setvalues(In);
        //    WestModel.seti_Effluent(iGray);
        //    //  this.MYWaterSimModel.Effluent.setvalues(In);

        //}


        // ==================================
        // SIMPLE WEB DROUGHT CONTROL
        // ==================================

        #region SimpleDrought

        // This is the number of scenarios
        const int DroughtScenarioCount = 5;

        // These arrays are the scenario values for each of the drought variables
        int[] FDroughtScenarioDepths = new int[DroughtScenarioCount] { 100, 80, 60, 40, 20 };
        int[] FDroughtScenarioActives = new int[DroughtScenarioCount] { 0, 1, 1, 1, 1 };
        int[] FDroughtScenarioLength = new int[DroughtScenarioCount] { 40, 40, 40, 40, 40 };
        int[] FDroughtScenarioStart = new int[DroughtScenarioCount] { 2020, 2020, 2020, 2020, 2020 };

        // this is the simple drought controls value
        int FTheSimpleDroughtValue = 0;
        // this is the simple drought control default
        const int DefaultSimpleDroughtControl = 0;

        /// <summary> The simple all regions drought.</summary>

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti simple all regions drought.</summary>
        /// <remarks> Quay, 3/25/2018.</remarks>
        /// 
        /// <param name="value"> The value ranges from 0-no drought to 4-Serious Drought.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_SimpleAllRegionsDroughtScenario(int value)
        {
            // Setup The Parameter arrays to send to the models
            int[] DActive = new int[WestModel.ModelCount];
            int[] DStartYear = new int[WestModel.ModelCount];
            int[] DLength = new int[WestModel.ModelCount];
            int[] DDepth = new int[WestModel.ModelCount];

            // make sure the scenario value is in the right range
            if (value < 0) value = 0;
            if (value > DroughtScenarioCount) value = DroughtScenarioCount;

            // Keep this value
            FTheSimpleDroughtValue = value;

            // set the arrays for each paramter to the scenario values
            for (int i = 0; i < WestModel.ModelCount;i++)
            {
                DStartYear[i] = FDroughtScenarioStart[value];
                DLength[i] = FDroughtScenarioLength[value];
                DDepth[i] = FDroughtScenarioDepths[value];
                DActive[i] = FDroughtScenarioActives[value];
            }

            // OK, send these values to the models
            WestModel.seti_DroughtStartYear(DStartYear);
            WestModel.seti_DroughtLength(DLength);
            WestModel.seti_DroughtDepth(DDepth);
            WestModel.seti_DroughtActive(DActive);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti simple all regions drought scenario.</summary>
        /// <remarks> Quay, 3/25/2018.</remarks>
        /// <returns> An int. The value ranges from 0-no drought to 5-Serious Drought</returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_SimpleAllRegionsDroughtScenario()
        {
            return FTheSimpleDroughtValue;
        }

        #endregion SimpleDrought

        void defaultSettings()
        {
            // default settings
            // ------------------------------------
            // QUAY EDIT 3/24/18
            // These are now set to defaults in the Generic.WaterSIMCRF model reset variables
            //startDrought = 2016;
            //endDrought = 2050;
            // END EDIT

            // NOTE QUAY
            // Not sure what this is?
            seti_PolicyStartYear(2015);
            // ====================================
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets WaterSimManager Parameters.</summary>
        /// <remarks> Any parameters or fields that are maintain at this level, WaterSimManager
        ///           need to be reset to their deafults in this method</remarks>
        /// <remarks> Quay, 3/28/2018.</remarks>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void resetManagerParameters()
        {
            // Reset the Simple Drought Web control to its default;
            FTheSimpleDroughtValue = DefaultSimpleDroughtControl;
        }

        // =====================================================================================================================
        //
        /// <summary>
        /// Policy Start Year; starts the year in which any policy 
        /// starts; valid are 2016 to 2060 (at present)
        /// </summary>
        int _policyStartYear = 2015;
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
        int _waterManagementEnd = 2050;
        public int endSGWM
        {
            get { return _waterManagementEnd; }
            set { _waterManagementEnd = value; }
        }
        // ============================================

        // 02.25.16 DAS
        //int geti_Pop()
        //{
        //    // 07.19.16 DAS
        //    // stop from throwing an index error
        //    int pop = 0;
        //    int stopyear = WSmith.endYear;
        //    if (Sim_CurrentYear <= stopyear)
        //    {
        //        int year = Sim_CurrentYear;
        //        //pop = WSmith.Get_PopYear(year);
        //        pop = WSmith.geti_NewPopulation();// .Get_PopYear(year);
        //    }
        //    return pop;
        //}

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

        //public virtual bool Model()
        //{
        //    bool success = false;

        //    return success;
        //}
        //
         
        // EDIT QUAY 3/30/18 
        //  This code indexes the old single WaterSim America Model
        //  Not being used, commneted it out to disconnect
        //public const int FNumberOfStates = 11;
        //public static string[] FStateNames = new string[FNumberOfStates] { "Florida", "Idaho", "Illinois", "Minnesota", "Wyoming", "Arizona", "Colorado", "Nevada", "California", "Utah", "NewMexico" };

        //public int FStateIndex = 0;
        // EDN EDIT


    }
    
    // new stuff

}
