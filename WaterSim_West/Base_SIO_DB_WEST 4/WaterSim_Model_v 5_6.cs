//-------------------------------------------------------------------------------------------------
// 
//
// summary:	Implements the water simulation model v 2 0 class
// This WaterSimModel class is essentiall the west model Manager, it manages multiple WaterSimCRFModel objects, one for each region of the west
// The heart of the Wets Model is found in the WaterSimCRF Model, however WaterSimModel is responsioble for loading the West Model data for
// each region, creating a WaterSimCRF model for each region, initializing it, and managing them all within WaterSim Manager
//  
// This model provides a shell to run multiple WaterSimCRFModels 
// This version is designed specifically for the West model
// 
// Version 5.01
// 9/10/20
//     Added Colorado River Model (DAS)
//     Added External Surface water model as proof of concept (rq)
//     Added Support for External Resource Models (RQ)
// 4/28/20
//     This contains the WaterSimModel class
//     This class manages multiple CRF models, one for each region
//     This is the class that is used to create the WestModel in the WaterSimDCDC_API_WaterSimWestRegion_Manager
//     which defines the WaterSimManager class inherited from WaterSim MangerClass
//     This contains a number of model properties including population, various demand, resource, and flux properties used to
//     support the multiple CRF models
// Notes
// 2-18-18 Quay Added several new routines to provide direct access to the model list
//              It is now standard that the order or providers/regions is now the order they
//              are in this model list.
// 
//        
//-------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using WaterSim_Base;
using System.Linq;
using System.Text;
using DemandModel_Base;
using CORiverModel;
using DCDC_Utilities;

namespace WaterSimDCDC.Generic
{

    #region MODEL ERRORS
    //==========================================================================================================================
    //  MODEL ERROR  
    //==========================================================================================================================
    /// <summary>   Model error. </summary>
    /// <remarks> This class is used to identify a model error </remarks>
    /// 
    public class ModelError
    {
        /// <summary> Name of the unit.</summary>
        readonly string FUnitName = "";
        /// <summary> The unit code.</summary>
        readonly int FUnitCode = 0;
        /// <summary> The error codes.</summary>
        List<int> FErrorCodes = new List<int>();
        /// <summary> The error msgs.</summary>
        List<string> FErrorMsgs = new List<string>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aUnitName">    Name of the unit. </param>
        /// <param name="aUnitCode">    The unit code. </param>
        /// <param name="ErrorCode">    The error code. </param>
        /// <param name="ErrorMsg">     Message describing the error. </param>
        ///-------------------------------------------------------------------------------------------------

        public ModelError(string aUnitName, int aUnitCode, int ErrorCode, string ErrorMsg)
        {
            FUnitCode = aUnitCode;
            FUnitName = aUnitName;
            FErrorCodes.Add(ErrorCode);
            FErrorMsgs.Add(ErrorMsg);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adds Error Code and Message.</summary>
        ///
        /// <param name="ErrorCode">    The error code. </param>
        /// <param name="ErrorMsg">     Message describing the error. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Add(int ErrorCode, string ErrorMsg)
        {
            FErrorCodes.Add(ErrorCode);
            FErrorMsgs.Add(ErrorMsg);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the error codes. </summary>
        ///
        /// <value> The error codes. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<int> ErrorCodes
        {
            get { return FErrorCodes; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the error messages.</summary>
        ///
        /// <value> The error messages. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> ErrorMessages
        {
            get { return FErrorMsgs; }
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

        public int UnitCode
        {
            get { return FUnitCode; }
        }

    }

    //==========================================================================================================================
    //  MODEL CALL BACK HANDLERS 
    //==========================================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary> AddError Call Back method.</summary>
    ///
    /// <remarks> If Provided, this method is called by the ModelErrorList if the model should be
    ///     stopped, then return a 1, if the model should continue, return a 0.</remarks>
    ///
    /// <param name="TheError"> the error.</param>
    ///
    /// <returns> 0 or 1.</returns>
    ///-------------------------------------------------------------------------------------------------

    public delegate int OnErrorAddHandler(ModelError TheError);

    //==========================================================================================================================
    //  MODEL ERROR EXCEPTION 
    //==========================================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Critical model error. </summary>
    ///<remarks>  Thrown by ModelErrorList when a CallBack returns !=0 value</remarks>
    /// <seealso cref="System.Exception"/>
    /// <seealso cref="ModelErrorList"/>
    ///-------------------------------------------------------------------------------------------------

    public class CriticalModelError : Exception
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="Message"> The message.</param>
        ///-------------------------------------------------------------------------------------------------

        public CriticalModelError(string Message)
            : base("Critical CRF Model Error: " + Message)
        {
        }
    }

    //==========================================================================================================================
    //  MODEL ERROR LIST 
    //==========================================================================================================================
    /// <summary>
    /// 
    /// </summary>
    public class ModelErrorList : List<ModelError>
    {
        /// <summary> The call back.</summary>
        readonly OnErrorAddHandler FCallBack;

        /// <summary>   Default constructor. </summary>
        public ModelErrorList() : base()
        {
            FCallBack = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="TheCallBack">  the AddError call back Handler. </param>
        ///-------------------------------------------------------------------------------------------------

        public ModelErrorList(OnErrorAddHandler TheCallBack) : base()
        {
            FCallBack = TheCallBack;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an error. </summary>
        ///
        /// <exception cref="CriticalModelError">   Thrown when critical model error. </exception>
        ///
        /// <param name="aUnitName">    Name of the unit. </param>
        /// <param name="aUnitCode">    The unit code. </param>
        /// <param name="ErrorCode">    The error code. </param>
        /// <param name="ErrorMsg">     Message describing the error. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddError(string aUnitName, int aUnitCode, int ErrorCode, string ErrorMsg)
        {
            ModelError ME = Find(delegate (ModelError AnError) { return AnError.UnitName == aUnitName; });
            if (ME != null)
            {
                ME.Add(ErrorCode, ErrorMsg);
            }
            else
            {
                ME = new ModelError(aUnitName, aUnitCode, ErrorCode, ErrorMsg);
            }
            if (FCallBack != null)
            {
                if (FCallBack(ME) > 0)
                {
                    throw new CriticalModelError("On Add Error Code: " + ErrorCode.ToString() + " " + ErrorMsg);
                }
            }
        }
    }

    #endregion MODEL ERRORS

    //==========================================================================================================================
    //  WATERSIM MODEL 
    //==========================================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A data Model for the water simulation crf. </summary>
    ///
    /// <remarks> Mcquay, 2/6/2017.
    ///           This is the core of the WterSim West MUlti Regional Model
    ///           Each region has a CRF Model and this manages them all
    ///           This is the class that is used to create the WestModel in the WaterSimDCDC_API_WaterSimWestRegion_Manager
    ///           which defines the WaterSimManager class inherited from WaterSim MangerClass
    ///
    ///           </remarks>
    ///-------------------------------------------------------------------------------------------------
    #region WaterSimModel

    public class WaterSimModel
    {
        /// <summary> The unit models.</summary>
        List<WaterSimCRFModel> FUnitModels = new List<WaterSimCRFModel>();
        /// <summary> The model errors.</summary>
        ModelErrorList FModelErrors = new ModelErrorList();
        /// <summary> True to fis model error.</summary>
        bool FisModelError = false;

        /// <summary> The fstart year.</summary>
        public int FstartYear = 0;
        /// <summary> The fend year.</summary>
        public int FendYear = 0;
        /// <summary> The fcurrent year.</summary>
        public int FcurrentYear = 0;
        /// <summary> The policy start year.</summary>
        int FPolicyStartYear = 0;
        /// <summary> The Trace Start Year </summary>
        readonly int FCOriverTraceStartYr = 1920;
     

        /// <summary> Information describing the rate.</summary>
        readonly RateDataClass FRateData;
        //
        readonly DataClassLCLU FDataLCLU;
        //
       // readonly WaterSimManager WS;
        //
        readonly DataClassTemperature FDataTemperature;
        /// <summary> Information describing the unit.</summary>
        UnitData FUnitData;
        /// <summary>
        /// 
        /// </summary>
        readonly UrbanDemand_GPCD UD;
        //
        // das edits 06.03.21
        readonly DataClassLcluArea FDataLCLUarea;
        readonly DataClassRCN FDataLCLUrcn;
        // end edits das o6.03.21

        // edits 01.19.22 das
        readonly ColoradoDesalExchangeClass FCODExchange;
        // end edits 01.19.22 das

        //Resource Model list
        ResourceModelList FResourceModels =  new ResourceModelList();
        //
        /// <summary>
        /// 
        /// </summary>
        readonly UrbanDensityDataClass UDproportions;
        // EDIT QUAY 9/10/20
        // Copied DAS code in then modified it in external models below, These models are now in WaterWim_CORiverModel class
        // <summary>
        // This is the central class for the Colorado River System
        // </summary>

        //public  COriverModel COS;
        //public  COriverAccounting COA;
        //COS = new WaterSimDCDC.Generic.Model(DataDirectoryName, TempDirectoryName);
        // END EDIT 9/10/20

        // EXTERNAL MODELS -----------------------------------------

        /// <summary> The surface model.</summary>
        readonly SurfaceModel FSurfaceModel;
        readonly WaterSim_CORiverModel FColoradoModel;
        /// <summary>
        ///  Temporary stream writer
        /// </summary>
        public StreamWriter swriter;
        DateTime now = DateTime.Now;

        //public WaterSim_CORiverModel COS;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor.</summary>
        ///
        /// <remarks>   Mcquay, 2/6/2017. </remarks>
        ///
        /// <exception cref="Exception"> Thrown when an exception error condition occurs.</exception>
        ///
        /// <param name="DataDirectoryName"> Pathname of the data directory.</param>
        /// <param name="TempDirectoryName"> Pathname of the temporary directory.</param>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimModel(string DataDirectoryName, string TempDirectoryName)
        {
            //           string UnitDataFielname = "Just11StatesLakeNoRuralPower.csv";// "Just5StatesLakeNoRural.csv"; //"Just5StatesLake.csv";// "JustSmithStates.csv";// "All_50_states.csv";
            // EDIT QUAY 2 13 18
            // CHANGED TO REGION DATA FILE
            //string UnitDataFielname = "BasinStatesSubRegionData.csv";// "USGSBasinRegionCountyWaterUse_2_28_17.csv";
            // string UnitDataFielname = "All West Data By Region Sumary v5.csv";
            //                         West Regions USGS with Colorado Ver 1.csv
           // string UnitDataFieldname = "West Regions USGS with Colorado Ver 5.csv";
            //string UnitDataFieldname = "WestRegionsUSGSwithColoradoVer_6.csv";
           //string UnitDataFieldname = "WestRegionsUSGSwithColoradoVer_7.csv";
            string UnitDataFieldname = "WestRegionsUSGSwithColoradoVer_8.csv";

            //string rates = "ElevenStateGrowthRates.csv";
            //string RateDataFilename = "ElevenStateGrowthRates3.csv";
            //string RateDataFilename = "West Model Growth Rate 2.csv";
            //string RateDataFilename = "West Model Growth Rate 3.csv";
            //string RateDataFilename = "WestModelGrowthRates_4.csv";
            //string RateDataFilename = "WestModelGrowthRates_5.csv";
            string RateDataFilename = "WestModelGrowthRates_6.csv";
            string AcerageDataFilename = "LCLUAcres.csv";
            //string TemperatureDataFilename = "Temperature.csv";
            string ClimateDataFilename = "ClimateMonthly_24.csv";
            // EDIT END 2 13 18
            // das edits 06.03.21
            // ICLUS ssp2 and ssp5 lclu data  - impervious area ..;
            string LCLUclassesFilename = "Oct21_Acres_ssp2.csv";
            //
            string LCLUrcnFilename = "RCNbyLCLU.csv";
            // end edits das 06.03.21
            // edits 08.10.21 das
            string RainFallFilename = "WSWestRainFall.csv";
            // end edits 08.10.21 das
            // edits 01.19.22 das
            string COexchangeFilename = "CO_desalExchangeCommand.csv";
            // end edits 01.19.22 das
            //
            // edits 02.08.22 das
            //string UrbanDensityPropFilename = "UrbanDensityProportion.csv";
            string UrbanDensityPropFilename = "UrbanDensityDUAandProportion.csv";
            // end edits 02.08.22 das

            string outputs = "\\Outputs\\";
            string addInputsDir = "\\Inputs\\";
            //

            try
            {
                StreamWriter(TempDirectoryName + outputs);
                //FUnitData = new UnitData(DataDirectoryName +  "//"  + UnitDataFieldname, UDI.UnitCodeField, UDI.UnitNameField);
                FUnitData = new UnitData(DataDirectoryName + addInputsDir + UnitDataFieldname, UDI.UnitCodeField, UDI.UnitNameField);
                FRateData = new RateDataClass(DataDirectoryName + addInputsDir, RateDataFilename);
                FDataLCLU = new DataClassLCLU(DataDirectoryName + addInputsDir, AcerageDataFilename);
                // Impervious area
                FDataLCLUarea = new DataClassLcluArea(DataDirectoryName + addInputsDir, LCLUclassesFilename);
                FDataLCLUrcn = new DataClassRCN(DataDirectoryName + addInputsDir, LCLUrcnFilename);
                //
                //
                //FDataTemperature = new DataClassTemperature(DataDirectoryName, ClimateDataFilename);
                DataClassTemperature TD = new DataClassTemperature(DataDirectoryName + addInputsDir, ClimateDataFilename);
                // string Filename = "CompareDemand.txt";
                //
                // swriter = new StreamWriter(Filename); 
                // 08.31.21 das
                DataClassRainFall RF = new DataClassRainFall(DataDirectoryName + addInputsDir, RainFallFilename);
                RainWaterHarvesting RW = new RainWaterHarvesting(RF, FDataLCLUarea, FUnitData);
                StormWater SW = new StormWater(FUnitData, RF, RW, FDataLCLUarea, FDataLCLUrcn);
                // end edits 08.31.21 das
                // edits 11.02.21 das
                //NewWater NW = new NewWater(FUnitData);
                int cloudy = 2;
                NewWater NW = new NewWater(FUnitData, cloudy);
                //  edits 01.19.22
                ColoradoDesalExchangeClass COD = new ColoradoDesalExchangeClass(DataDirectoryName + addInputsDir, COexchangeFilename);
                //  end edits 01.19.22 das

                // edits 02.08.22 das
                UDproportions = new UrbanDensityDataClass(DataDirectoryName + addInputsDir, UrbanDensityPropFilename);
                // end edits 02.08.22 das

                //end edits 11.02.21 das
                foreach (string Name in FUnitData.UnitNames)
                    {
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, Name);
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, Name);
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, FDataTemperature, Name);
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, FDataTemperature, Name,RW,SW,swriter); // 08.31.21 das
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, FDataTemperature, Name, RW, SW, NW, swriter); // 11.02.21 das
                        //WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, TD, Name, RW, SW, NW, COD, swriter); // 11.02.21 das
                        WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, TD, Name, RW, SW, NW, COD, UDproportions, swriter); // 02.08.22 das

                        FUnitModels.Add(TempModel);
                        set_DefaultDemandModel(TempModel);
                    
                        // modelCount += 1;
                    }
                
                // EDIT QUAY 9/8/20
                // Adding Surface Water Model
                //
                FSurfaceModel = new SurfaceModel(FUnitData);
                AddExternalModel(FSurfaceModel);
                // Adding Colorado River Model
                FColoradoModel = new WaterSim_CORiverModel(DataDirectoryName + addInputsDir, TempDirectoryName, UnitDataFieldname);             
                //FColoradoModel = new WaterSim_CORiverModel(DataDirectoryName, TempDirectoryName, UnitDataFieldname, UTwaterTransfers);
                AddExternalModel(FColoradoModel);

                 // sampson edits 09.04.18
                //set_GrayWaterPotential();
                // end sampson edits 09.04.18

              }
            catch (Exception ex)
            {
                throw ex;
            }
 
        }
        //
        //public bool UTwaterTransfers
        //{
        //    get; set;
        //}
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempDirectoryName"></param>
        public void StreamWriter(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "MyOutput" + "_" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            swriter = File.AppendText(filename);
        }
  
        //public WaterSimModel(UnitData TheUnitData, RateDataClass TheRateData, string TheUnitName)
        //{
        //    FUnitData = TheUnitData;
        //    FRateData = TheRateData;
        //   // string RuralRateDataFilename = "WestModelRuralGrowthRate_1.csv";

        //    foreach (string Name in TheUnitData.UnitNames)
        //    {
        //        WaterSimCRFModel TempModel = new WaterSimCRFModel(FUnitData, FRateData, Name);
        //        FUnitModels.Add(TempModel);
        //        //
        //        set_DemandModelIndex(TempModel);
        //        //DemandModel tempDEM = new UrbanDemand_GPCD(TempModel);

        //    }


        //}

        // NOTE QUAY 4/28/20
        // THIS Constructor is never used in WaterSim_WestStaes_SIO_DB_West 5
        // Utiklizes classes and methods found in Utilities.cs
        // follow FDataTemperature to find orphaned code
        // FOLLOW UP ON THIS
        ///-------------------------------------------------------------------------------------------------
        /// <summary> /public WaterSimModel(UnitData TheUnitData, RateDataClass TheRateData, DataClassLCLU
        ///     DataLCLU, string TheUnitName)</summary>
        ///
        /// <remarks>  This is a constructor that builds the multi models based on a pre created objects of 
        ///            RateDataClass, DataClassLCLUm, and a DataClassTempreature
        ///            based on unit names 
        ///            </remarks>
        ///
        /// <param name="TheUnitData"> Information describing the unit.</param>
        /// <param name="TheRateData"> Information describing the rate.</param>
        /// <param name="DataLCLU">    The data lclu.</param>
        /// <param name="T">           A DataClassTemperature to process.</param>
        /// <param name="TheUnitName"> Name of the unit.</param>

        ///-------------------------------------------------------------------------------------------------

        public WaterSimModel(UnitData TheUnitData, RateDataClass TheRateData, DataClassLCLU DataLCLU, DataClassTemperature T, string TheUnitName)
        {
            WaterSimCRFModel TempModel;
            FUnitData = TheUnitData;
            FRateData = TheRateData;
            FDataLCLU = DataLCLU;
            FDataTemperature = T;
            foreach (string Name in TheUnitData.UnitNames)
            {
                TempModel = new WaterSimCRFModel(FUnitData, FRateData, FDataLCLU, FDataTemperature, Name);
                FUnitModels.Add(TempModel);
            }
        }


        // EDIT QUAY 9/10/20
        // Added Links to External Models
        

         ///-------------------------------------------------------------------------------------------------
         /// <summary> Gets the surfacewater model.</summary>
         ///
         /// <value> The surfacewater model.</value>
         ///-------------------------------------------------------------------------------------------------

         public SurfaceModel SurfacewaterModel
         {
            get { return FSurfaceModel; }

         }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the colorado river model.</summary>
        ///
        /// <value> The colorado river model.</value>
        ///-------------------------------------------------------------------------------------------------

        public WaterSim_CORiverModel ColoradoRiverModel
        {
            get { return FColoradoModel; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets information describing the model unit.</summary>
        ///
        /// <value> Information describing the model unit.</value>
        ///-------------------------------------------------------------------------------------------------

        public UnitData ModelUnitData
        {
            get { return FUnitData; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets information describing the model rate.</summary>
        ///
        /// <value> Information describing the model rate.</value>
        ///-------------------------------------------------------------------------------------------------

        public RateDataClass ModelRateData
        {
            get { return FRateData; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> GetUnitModel Retunrn the UnitModel based on its name.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="aUnitName"> string name of CRF model.</param>
        ///
        /// <returns> The unit model.</returns>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel GetUnitModel(string aUnitName)
        {
            WaterSimCRFModel WSMod = FUnitModels.Find(delegate (WaterSimCRFModel WSCRF) { return WSCRF.UnitName == aUnitName; });
            return WSMod;

        }
        

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets an unit model. </summary>
        ///
        /// <param name="aUnitCode">    The unit code. </param>
        ///
        /// <returns>   The unit model. </returns>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel GetUnitModel(int aUnitCode)
        {
            WaterSimCRFModel WSMod = FUnitModels.Find(delegate (WaterSimCRFModel WSCRF) { return WSCRF.unitCode == aUnitCode; });
            return WSMod;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Getunit model.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <param name="eP"> The e p.</param>
        /// <returns> A WaterSimCRFModel.</returns>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel GetUnitModel(eProvider eP)
        {
            return GetUnitModel((int)eP);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Fast unit model.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <param name="index"> Zero-based index of the.</param>
        /// <returns> A WaterSimCRFModel.</returns>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimCRFModel FastUnitModel(int index)
        {
            if ((index > -1) && (index < FUnitModels.Count))
                return FUnitModels[index];
            else
                return null;
        }
        // ======================================================
        // edits 11.09.21 das
        internal bool RainWaterHarvesting
        { get; set; }
        /// <summary>
        /// Switch to invoke rainwater harvesting
        /// </summary>
        public bool rainWaterHarvesting
        {
            set
            {
                bool RainWaterHarvesting = value;
                //FPolicyStartYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.rainWaterHarvest = RainWaterHarvesting;
                }
            }
            get { return RainWaterHarvesting; }
        }
        // end edits 11.09.21 das
        // ======================================================


        // ======================================================
        // edits 01.13.22 das
        internal int DesalPoliciesForAgent
        { get; set; }
        /// <summary>
        /// At the moment, not used
        /// </summary>
        public int desalPoliciesForAgent
        {
            set
            {
                int DesalPoliciesForAgent = value;
                //FPolicyStartYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.desalinationPolicy = DesalPoliciesForAgent;
                }
            }
            get { return DesalPoliciesForAgent; }
        }
        // end edits 11.09.21 das
        // ======================================================





        ///-------------------------------------------------------------------------------------------------
        /// <summary> Getunit model index.</summary>
        /// <remarks> Quay, 2/19/2018.</remarks>
        /// <param name="Model"> The model.</param>
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------

        public int GetIndex(WaterSimCRFModel Model)
        {
            int temp = FUnitModels.IndexOf(Model);
            return temp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets a list of names of the unit models.</summary>
        ///
        /// <remarks> Quay 2/18/18.</remarks>
        ///
        /// <value> A list of names of the unit models.</value>
        ///-------------------------------------------------------------------------------------------------

        public List<string> UnitModelNames
        {
            get { return FUnitData.UnitNames; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the water simulation crf models.</summary>
        ///
        /// <remarks> Quay 2/18/18.</remarks>
        ///
        /// <value> The water simulation crf models.</value>
        ///-------------------------------------------------------------------------------------------------

        public List<WaterSimCRFModel> WaterSimCRFModels
        {
            get { return FUnitModels; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the number of models.</summary>
        ///
        /// <remarks> Quay 2/18/18.</remarks>
        ///
        /// <value> The number of models.</value>
        ///-------------------------------------------------------------------------------------------------

        public int ModelCount
        {
            get { return FUnitModels.Count; }
        }
        //
        // 09.18.20 das
  


        ///-------------------------------------------------------------------------------------------------
        /// <summary> The crf network.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="aUnitName"> Name of the unit.</param>
        ///
        /// <returns> A CRF_Unit_Network.</returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network TheCRFNetwork(string aUnitName)
        {
            WaterSimCRFModel WSMod = GetUnitModel(aUnitName);
            if (WSMod != null)
            {
                return WSMod.TheCRFNetwork;
            }
            else
            {
                return null;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether a model error has occurred. </summary>
        ///
        /// <value> true if model error, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool isModelError
        {
            get { return FisModelError; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the model errors. </summary>
        ///
        /// <value> The model errors. </value>
        ///-------------------------------------------------------------------------------------------------

        public ModelErrorList ModelErrors
        {
            get { return FModelErrors; }
        }


        /// <summary>   Resets the model errors. </summary>
        public void ResetModelErrors()
        {
            FisModelError = false;
            FModelErrors.Clear();
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the start year. </summary>
        ///
        /// <value> The start year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int StartYear
        {
            get { return FstartYear; }
            set
            {
                FstartYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.startYear = FstartYear;
                }

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the end year. </summary>
        ///
        /// <value> The end year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int EndYear
        {
            get { return FendYear; }
            set
            {
                FendYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.endYear = FendYear;
                }

            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the current year. </summary>
        ///
        /// <value> The current year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int CurrentYear
        {
            get { return FcurrentYear; }
            set
            {
                FcurrentYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.currentYear = FcurrentYear;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the policy start year. </summary>
        ///
        /// <value> The policy start year. </value>
        ///-------------------------------------------------------------------------------------------------

        public int policyStartYear
        {
            set
            {
                FPolicyStartYear = value;
                foreach (WaterSimCRFModel WSM in FUnitModels)
                {
                    WSM.policyStartYear = FPolicyStartYear;
                }
            }
            get { return FPolicyStartYear; }
        }
        //
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adjust population. </summary>
        /// <param name="aUnitName">    Name of the unit. </param>
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double AdjustPopulation(string aUnitName)
        {
            WaterSimCRFModel WS = GetUnitModel(aUnitName);
            if (WS != null)
            {
                return WS.AdjustPopulation;
            }
            else
            {
                return 0.0;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Ag conservation. </summary>
        ///
        /// <remarks>   2/6/2017. </remarks>
        ///
        /// <param name="aUnitName">    Name of the unit. </param>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double AgConservation(string aUnitName)
        {
            WaterSimCRFModel WS = GetUnitModel(aUnitName);
            if (WS != null)
            {
                return WS.AgConservation;
            }
            else
            {
                return 0.0;
            }
        }

 
        //public string[] State
        //{
        //    get 
        //    {
        //        int count = FUnitModels.Count;
        //        string[] States = new string[count];
        //        for (int i = 0; i < count; i++)
        //        {
        //            States[i] = FUnitModels[i].UnitName;
        //        }
        //        return States; 
        //    }
        //}

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes the one year operation.</summary>
        ///
        /// <remarks> if Callback on error returns value >0 then this routine will break on critical error
        ///     and stop execution. returns a 0 if no critical errors.  But non critical erros can occur
        ///     and execution continues.  Check isError and ModelErros for non critical erros.</remarks>
        ///
        /// <param name="year"> The year.</param>
        ///
        /// <returns> int, 0 if no critical erros occurred, >0 if a critical error occured  .</returns>
        ///
        ///-------------------------------------------------------------------------------------------------

        public virtual int runOneYear(int year)
        {
            int result = 0;
            //
            

            //
            // Modified Quay 9/8/2020
            // Added support for Alternative Resource Models
            // Go through the ResourceModel List, see if each model has been initiated
            // if so run the models allocate function
            int yearindex = year - StartYear;
            foreach(ResourceModel RM in FResourceModels)
            {
                // Surface Model
                if (RM is SurfaceModel)
                {
                    int[] AllocatedSurfaceWater = (RM as SurfaceModel).Allocate(yearindex).AvailableWater;
                    seti_SurfaceWaterFresh(AllocatedSurfaceWater);
                }

                // Colorado River Model
                if (RM is WaterSim_CORiverModel)
                {
                    int[] AllocatedColorado = (RM as WaterSim_CORiverModel).Allocate(yearindex).AvailableWater;
                    seti_SurfaceColorado(AllocatedColorado);
                }
            }
            // END MODIFIED Quay 9/8/2020

            foreach (WaterSimCRFModel WSModel in FUnitModels)
            {

                // OK, now run the models
                int tempresult = WSModel.runOneYear(year);

                if (tempresult > 0)
                {
                    FisModelError = true;
                    FModelErrors.AddError(WSModel.UnitName, 1, tempresult, "on runOneYear");
                }
                else
                {
                    result = tempresult;


                    //Quay edit 9/8/2020
                    // I deleted this, I would think that we should run all the models, and add errors to the list if more than one error
                    //break;
                    // Edn Edit
                }
            }
            // Modified Quay 9/8/2020
            // Added support for Alternative Resource Models
            // Now that models have run Run resource models
            // Check to see if in list and run this year
            foreach (ResourceModel RM in FResourceModels)
            {
               if (RM is SurfaceModel)
               {
                    //int errCode = 0;
                    //string errStr = "";
                    (RM as SurfaceModel).RunYear(out int errCode,out string errStr);
               }
                if (RM is WaterSim_CORiverModel)
                {
                    //int errCode = 0;
                    //var errStr = "";
                    // 09.15.20 das
                    (RM as WaterSim_CORiverModel).RunYear(year, out int errCode, out string errStr);
                    // end 09.15.20 das
                }
            }
            // END MODIFIED Quay 9/8/2020


            return result;
        }

        //========================================
        // EDIT QUAY 9 8 20
        // While working on this, I found that the reset network and reset variables was cross wired and used incorrectly
        // I have rewritten to uncross wires, reset variables was demoted to protected ResetNetwork calls reset variables
        // -----------------------------------------
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resets the network.</summary>
        ///
        /// <remarks> Call ResetNetwork for each of the Models.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void ResetNetwork()
        {
            // EDIT QUAY 9/8/20
            // Reset each models network and variables
            foreach (WaterSimCRFModel WSModel in FUnitModels)
            {
                WSModel.ResetNetwork();
                WSModel.ResetVariables();
            }
            // Reset all external models
            foreach(ResourceModel RM in FResourceModels)
            {
                RM.ResetModel();
            }
            // END EDIT
        }

       
        /// <summary>   Resets the variables. </summary>
        public void ResetVariables()
        {
            foreach (WaterSimCRFModel WSModel in FUnitModels)
            {
                WSModel.ResetVariables();
                // EDIT QUAY 9/8/20
                // This is redundant and wrong order
                // WSModel.ResetNetwork();
                // END EDIT
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Adds an external model.</summary>
        /// <param name="NewModel"> The new model.</param>
        ///-------------------------------------------------------------------------------------------------

        public void AddExternalModel(ResourceModel NewModel)
        {
            // check to make sure no other such model, if so delete model in list
            foreach(ResourceModel RM in FResourceModels)
            {
                if (RM.GetType() == NewModel.GetType())
                {
                    FResourceModels.Remove(RM);
                    break;
                }
            }
            if (NewModel is BasinRiverModel)
            {
                (NewModel as BasinRiverModel).Owner = this;
                FResourceModels.Add(NewModel);
            }
        }

        //======================================================================
        // PROVIDER PROPERTIES
        // ====================================================================
        #region ProviderProperties

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Unit Models Data Array index. </summary>
        /// <param name="UnitCode"> The code. </param>
        /// <remarks>   This returns the index into ProviderintArrays for the unitCode </remarks>
        /// <returns>   The unit array index. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int GetUnitIndex(int UnitCode)
        {
            return FUnitModels.FindIndex(delegate (WaterSimCRFModel WSCRF) { return WSCRF.unitCode == UnitCode; });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Unit Models Data Array index. </summary>
        /// <param name="UnitName"> The code. </param>
        /// <remarks>   This returns the index into ProviderintArrays for the UnitName </remarks>
        /// <returns>   The unit array index. </returns>
        ///-------------------------------------------------------------------------------------------------
        public int GetUnitIndex(string UnitName)
        {
            return FUnitModels.FindIndex(delegate (WaterSimCRFModel WSCRF) { return WSCRF.UnitName == UnitName; });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a value. </summary>
        /// <remarks>   This is a utility routine to help setting a value for a particul;ar provider in a Provider Array Property
        ///             This is not particulalry fast, so using it to set more than one UnitModel will not be
        ///             Efficient</remarks>
        /// <param name="PAP">          The ProviderArrayProperty. </param>
        /// <param name="ArrayIndex">   Zero-based index of the ProviderIntArray. </param>
        /// <param name="Value">        The value. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected bool SetProviderIntValue(providerArrayProperty PAP, int ArrayIndex, int Value)
        {
            bool result = false;
            try
            {
                ProviderIntArray PIA = PAP.getvalues();
                int[] NewValues = PIA.Values;
                NewValues[ArrayIndex] = Value;
                PIA.Values = NewValues;
                PAP.setvalues(PIA);
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Not used", ex);
                // Do nothing for now
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets unit value. </summary>
        /// <remarks>   This is a utility to set a value in and int[] in the location that corresponds 
        ///             to the UnitModel's poistion in the array using the UnitModel's code 
        ///             This is not particulalry fast, so using it to set more than one UnitModel will not be
        ///             Efficient</remarks>
        /// <param name="UnitCode">         The code. </param>
        /// <param name="UnitValueArray">    [in,out] Array of Unit Values. </param>
        /// <param name="Value">        The value. </param>
        /// <remarks> This places the Value into the proper location in the UnitValueArry based on the UnitCode passed</remarks>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------


        public bool SetUnitValue(int UnitCode, ref int[] UnitValueArray, int Value)
        {
            bool result = false;
            int Index = GetUnitIndex(UnitCode);
            if (Index > -1)
            {
                UnitValueArray[Index] = Value;
                result = true;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets unit value. </summary>
        /// <remarks>   This is a utility to set a value for a particular UnitModel in a ProviderArray Porperty 
        ///             using the UnitModel's code 
        ///             This is not particulalry fast, so using it to set more than one UnitModel will not be
        ///             Efficient</remarks>
        /// <param name="Code">     The code. </param>
        /// <param name="PAP">      [in,out] The pap. </param>
        /// <param name="Value">    The value. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetUnitValue(int Code, ref providerArrayProperty PAP, int Value)
        {
            bool result = false;
            int Index = GetUnitIndex(Code);
            if (Index > -1)
            {
                result = SetProviderIntValue(PAP, Index, Value);
            }
            return result;
        }

        /// <summary>
        ///  This places the Value into the proper location in the UnitValueArry based on the UniTName passed
        ///           This is not particulalry fast, so using it to set more than one UnitModel will not be
        ///             Efficient</summary>
        /// <param name="Name"></param>
        /// <param name="UnitValueArray"></param>
        /// <param name="Value"></param>
        /// <returns> true if it succeeds, false if it fails</returns>
        public bool SetUnitValue(string Name, ref int[] UnitValueArray, int Value)
        {
            bool result = false;
            int Index = GetUnitIndex(Name);
            if (Index > -1)
            {
                UnitValueArray[Index] = Value;
                result = true;
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets unit value. </summary>
        /// <remarks> This places the Value into the proper location in a ProviderArrayProperty based on the UnitName passed
        ///           This is not particulalry fast, so using it to set more than one UnitModel will not be
        ///             Efficient</remarks>
        /// <param name="Name">     The name. </param>
        /// <param name="PAP">      [in,out] The pap. </param>
        /// <param name="Value">    The value. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetUnitValue(string Name, ref providerArrayProperty PAP, int Value)
        {
            bool result = false;
            int Index = GetUnitIndex(Name);
            if (Index > -1)
            {
                result = SetProviderIntValue(PAP, Index, Value);
            }
            return result;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets unit value. </summary>
        /// <param name="Codes">            The codes. </param>
        /// <param name="UnitValueArray">   [in,out] Array of unit values. </param>
        /// <param name="Values">           The values. </param>
        /// <param name="BadCode">          [out] List of which codes were good (true) and bad (false). </param>
        /// <remarks> This places each value in Values into the proper location in the UnitValueArry based on the code value in Codes
        ///           This allows multiple parameters to be sent at once.  Not much faster than just doing each one, one at a time using
        ///           SetUnitValue with single code and value, k=just more convenient.
        ///           Definantly better to build the array, set the values, and then assign the whole array
        ///           </remarks>
        /// 
        /// <returns>   true if it succeeds, false if one of the assignments failed. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetUnitValue(List<int> Codes, ref int[] UnitValueArray, List<int> Values, out List<bool> BadCode)
        {
            bool result = true;
            // OK all good
            BadCode = new List<bool>();
            // loop through each of the codes
            for (int i = 0; i < Codes.Count; i++)
            {
                // find this code in the unit list
                int Index = GetUnitIndex(Codes[i]);
                if (Index > -1)
                {
                    // found it, set the value and BadCode
                    UnitValueArray[Index] = Values[i];
                    BadCode.Add(false);
                }
                else
                {
                    // not found, this is an error state, set the BadCode
                    result = false;
                    BadCode.Add(true);
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets unit value. </summary>
        /// <param name="Names">            The names. </param>
        /// <param name="UnitValueArray">   [in,out] Array of unit values. </param>
        /// <param name="Values">           The values. </param>
        /// <param name="BadCode">          [out] List of which codes were good (true) and bad (false). </param>
        /// <remarks> This places each value in Values into the proper location in the UnitValueArry based on the code value in Names
        ///           This allows multiple parameters to be sent at once.  Not much faster than just doing each one, one at a time using
        ///           SetUnitValue with single code and value, k=just more convenient.
        ///           Definantly better to build the array, set the values, and then assign the whole array
        ///           </remarks>
        /// <returns>   true if it succeeds, false if one of the assignments failed. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool SetUnitValue(List<string> Names, ref int[] UnitValueArray, List<int> Values, out List<bool> BadCode)
        {
            bool result = true;
            BadCode = new List<bool>();
            // loop through each of the codes
            for (int i = 0; i < Names.Count; i++)
            {
                // find this code in the unit list
                int Index = GetUnitIndex(Names[i]);
                if (Index > -1)
                {
                    // found it, set the value and BadCode
                    UnitValueArray[Index] = Values[i];
                    BadCode.Add(false);
                }
                else
                {
                    // not found, this is an error state, set the BadCode
                    result = false;
                    BadCode.Add(true);
                }
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a population. </summary>
        ///
        /// <param name="Values">   The values. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Set_population(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].population = Values[i];
            }
        }


        //=======================================================
        //  Population
        //=======================================================
        // 08.03.17 das
        /// <summary>
        ///  modify population
        /// </summary>
        public double _popGrowthRateModifier = 1.0;
        /// <summary>
        ///  same
        /// </summary>
        public double PopulationGrowthRateModifier
        {
            get { return _popGrowthRateModifier; }
            set { _popGrowthRateModifier = value; }
        }

        #region Population
        ///------------------------------------------------------
        /// <summary> The Population provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Population;

        ///------------------------------------------------------
        /// <summary> Gets the Population  </summary>
        ///<returns> the Population </returns>

        public int[] geti_Pop()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].population;
            }
            return result;
        }

        #endregion Population
        //=======================================================
        //  GPCD_urban
        //=======================================================
        #region GPCD_urban
        ///------------------------------------------------------
        /// <summary> The GPCD_urban provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty GPCD_urban;

        ///------------------------------------------------------
        /// <summary> Gets the GPCD_urban  </summary>
        ///<returns> the GPCD_urban </returns>

        public int[] geti_gpcd()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_gpcd();
            }
            return result;
        }

        #endregion GPCD_urban
        //=======================================================
        //  GPCD_ag
        //=======================================================
        #region GPCD_ag
        ///------------------------------------------------------
        /// <summary> The GPCD_ag provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty GPCD_ag;

        ///------------------------------------------------------
        /// <summary> Gets the GPCD_ag  </summary>
        ///<returns> the GPCD_ag </returns>

        public int[] geti_gpcdAg()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_gpcdAg();
            }
            return result;
        }

        #endregion GPCD_ag
        //=======================================================
        //  GPCD_other
        //=======================================================
        #region GPCD_other
        ///------------------------------------------------------
        /// <summary> The GPCD_other provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty GPCD_other;

        ///------------------------------------------------------
        /// <summary> Gets the GPCD_other  </summary>
        ///<returns> the GPCD_other </returns>

        public int[] geti_gpcdOther()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_gpcdOther();
            }
            return result;
        }

        #endregion GPCD_other

        // EDIT QUAY 9/8/20
        // Add external Surface Water Model
        //======================================================
        // Use External Surface Model
        //======================================================
        #region UseSurfaceExternal
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty UseSurfaceExternal;
        /// <summary>
        ///  Rays code
        /// </summary>
        /// <returns></returns>
        public int[] geti_UseSurfaceExternal()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceFreshUseExternal();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti use surface external.</summary>
        /// <remark>  Will not set values if a surfacemodel is not in the FResourceModels list</remark>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_UseSurfaceExternal(int[] Values)
        {  
            if (FResourceModels.ContainsType(Type.GetType("SurfaceModel")))
                {
                int ArraySize = FUnitModels.Count;
                if (ArraySize > Values.Length)
                {
                    ArraySize = Values.Length;
                }
                for (int i = 0; i < ArraySize; i++)
                {
                    FUnitModels[i].seti_SurfaceFreshUseExternal(Values[i]);
                }
            }
        }

        // ENDEDIT 9/8/20
        #endregion UseSurfaceExternal


        #region UseColoradoRiverExternal

        /// <summary> The use colorado external property array.</summary>
        public providerArrayProperty UseColoradoExternal;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti use colorado external model.</summary>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_UseColoradoExternal()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ColoradoUseExternal();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti use Colorado River external model.</summary>
        /// <remark>  Will not set values if a surfacemodel is not in the FResourceModels list</remark>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_UseColoradoExternal(int[] Values)
        {
            // 10.07.20 das is this a typo?
            //if (FResourceModels.ContainsType(Type.GetType("WaterSim_CORiverMode")))
            if (FResourceModels.ContainsType(Type.GetType("WaterSim_CORiverModel")))
                {
                int ArraySize = FUnitModels.Count;
                if (ArraySize > Values.Length)
                {
                    ArraySize = Values.Length;
                }
                for (int i = 0; i < ArraySize; i++)
                {
                    FUnitModels[i].seti_ColoradoUseExternal(Values[i]);
                }
            }
        }

        #endregion UseColoradoRiverExternal
        //=======================================================
        //  SurfaceFresh
        //=======================================================
        #region SurfaceFresh
        ///------------------------------------------------------
        /// <summary> The SurfaceFresh provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceFresh;
      
        
        ///------------------------------------------------------
        /// <summary> Gets the SurfaceFresh  </summary>
        ///<returns> the SurfaceFresh </returns>

        public int[] geti_SurfaceWaterFresh()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceWaterFresh();
            }
            return result;
        }


        //------------------------------------------------------
        /// <summary> Sets a SurfaceFresh  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SurfaceWaterFresh(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SurfaceWaterFresh(Values[i]);
            }
        }
        #endregion SurfaceFresh
        //=======================================================
        //  SurfaceFreshNet
        //=======================================================
        #region SurfaceFreshNet
        ///------------------------------------------------------
        /// <summary> The SurfaceFreshNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceFreshNet;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceFreshNet  </summary>
        ///<returns> the SurfaceFreshNet </returns>

        public int[] geti_SurfaceWaterFreshNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceWaterFreshNet();
            }
            return result;
        }

        #endregion SurfaceFreshNet

        // QUAY EDIT 9/12/18
        // Added Colorado Resource Access

        //=======================================================
        //  SurfaceColorado
        //=======================================================
        #region SurfaceColorado
        ///------------------------------------------------------
        /// <summary> The SurfaceFresh provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceColorado;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceColorado  </summary>
        ///<returns> the SurfaceColorado </returns>

        public int[] geti_SurfaceColorado()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceColorado();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a SurfaceColorado  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SurfaceColorado(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SurfaceColorado(Values[i]);
            }
        }
        #endregion SurfaceColorado

        //=======================================================
        //  SurfaceColoradoNet
        //=======================================================
        #region SurfaceColoradoNet
        ///------------------------------------------------------
        /// <summary> The SurfaceColoradoNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceColoradoNet;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceFreshNet  </summary>
        ///<returns> the SurfaceFreshNet </returns>

        public int[] geti_SurfaceColoradoNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceColoradoNet();
            }
            return result;
        }

        #endregion SurfaceColoradoNet
        // END EDIT 9/12/18

        //=======================================================
        //  SurfaceSaline
        //=======================================================
        #region SurfaceSaline
        ///------------------------------------------------------
        /// <summary> The SurfaceSaline provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceSaline;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceSaline  </summary>
        ///<returns> the SurfaceSaline </returns>

        public int[] geti_SurfaceWaterSaline()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceWaterSaline();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a SurfaceSaline  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SurfaceWaterSaline(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SurfaceWaterSaline(Values[i]);
            }
        }
        #endregion SurfaceSaline
        //=======================================================
        //  SurfaceSalineNet
        //=======================================================
        #region SurfaceSalineNet
        ///------------------------------------------------------
        /// <summary> The SurfaceSalineNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceSalineNet;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceSalineNet  </summary>
        ///<returns> the SurfaceSalineNet </returns>

        public int[] geti_SurfaceWaterSalineNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceWaterSalineNet();
            }
            return result;
        }

        #endregion SurfaceSalineNet
        //=======================================================
        //  Groundwater
        //=======================================================
        #region Groundwater
        ///------------------------------------------------------
        /// <summary> The Groundwater provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Groundwater;

        ///------------------------------------------------------
        /// <summary> Gets the Groundwater  </summary>
        ///<returns> the Groundwater </returns>

        public int[] geti_Groundwater()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Groundwater();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets a Groundwater.</summary>
        ///
        /// <remarks> Quay, 2/18/2018.</remarks>
        ///
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_Groundwater(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_Groundwater(Values[i]);
            }
        }
        #endregion Groundwater
        //=======================================================
        //  GroundwaterNet
        //=======================================================
        #region GroundwaterNet
        ///------------------------------------------------------
        /// <summary> The GroundwaterNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty GroundwaterNet;

        ///------------------------------------------------------
        /// <summary> Gets the GroundwaterNet  </summary>
        ///<returns> the GroundwaterNet </returns>

        public int[] geti_GroundwaterNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GroundwaterNet();
            }
            return result;
        }

        #endregion GroundwaterNet
        //=======================================================
        //  Effluent
        //=======================================================
        #region Effluent
        ///------------------------------------------------------
        /// <summary> The Effluent provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Effluent;

        ///------------------------------------------------------
        /// <summary> Gets the Effluent  </summary>
        ///<returns> the Effluent </returns>

        public int[] geti_Effluent()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Effluent();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Effluent  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_Effluent(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_Effluent(Values[i]);
            }
        }
        /// <summary>
        ///  Sampson edits 08.08.18
        /// </summary>
        /// <returns></returns>
        public int[] geti_AvailableReclaimed()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = Convert.ToInt32(FUnitModels[i].AvailableReclaimed());
            }
            return result;
        }

        #endregion Effluent
        //=======================================================
        //  EffluentNet
        //=======================================================
        #region EffluentNet
        ///------------------------------------------------------
        /// <summary> The EffluentNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty EffluentNet;

        ///------------------------------------------------------
        /// <summary> Gets the EffluentNet  </summary>
        ///<returns> the EffluentNet </returns>

        public int[] geti_EffluentNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_EffluentNet();
            }
            return result;
        }

        #endregion EffluentNet
        //=======================================================
        //  SurfaceLake
        //=======================================================
        #region SurfaceLake
        ///------------------------------------------------------
        /// <summary> The SurfaceLake provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceLake;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceLake  </summary>
        ///<returns> the SurfaceLake </returns>

        public int[] geti_SurfaceLake()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceLake();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a SurfaceLake  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SurfaceLake(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SurfaceLake(Values[i]);
            }
        }
        #endregion SurfaceLake
        //=======================================================
        //  SurfaceLakeNet
        //=======================================================
        #region SurfaceLakeNet
        ///------------------------------------------------------
        /// <summary> The SurfaceLakeNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceLakeNet;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceLakeNet  </summary>
        ///<returns> the SurfaceLakeNet </returns>

        public int[] geti_SurfaceLakeNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceLakeNet();
            }
            return result;
        }

        #endregion SurfaceLakeNet
        //=======================================================
        //  TotalSupplies
        //=======================================================
        #region TotalSupplies
        ///------------------------------------------------------
        /// <summary> The TotalSupplies provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty TotalSupplies;

        ///------------------------------------------------------
        /// <summary> Gets the TotalSupplies  </summary>
        ///<returns> the TotalSupplies </returns>

        public int[] geti_TotalSupplies()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_TotalSupplies();
            }
            return result;
        }

        #endregion TotalSupplies
        //=======================================================
        //  Urban
        //=======================================================
        #region Urban
        ///------------------------------------------------------
        /// <summary> The Urban provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Urban;

        ///------------------------------------------------------
        /// <summary> Gets the Urban  </summary>
        ///<returns> the Urban </returns>

        public int[] geti_Urban()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Urban();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Urban  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_Urban(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_Urban(Values[i]);
            }
        }
        #endregion Urban
        //=======================================================
        //  UrbanNet
        //=======================================================
        #region UrbanNet
        ///------------------------------------------------------
        /// <summary> The UrbanNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty UrbanNet;

        ///------------------------------------------------------
        /// <summary> Gets the UrbanNet  </summary>
        ///<returns> the UrbanNet </returns>

        public int[] geti_Urban_Net()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Urban_Net();
            }
            return result;
        }

        #endregion UrbanNet
        //=======================================================
        //  Agriculture
        //=======================================================
        #region Agriculture
        ///------------------------------------------------------
        /// <summary> The Agriculture provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Agriculture;

        ///------------------------------------------------------
        /// <summary> Gets the Agriculture  </summary>
        ///<returns> the Agriculture </returns>

        public int[] geti_Agriculture()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Agriculture();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Agriculture  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_Agriculture(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_Agriculture(Values[i]);
            }
        }
        #endregion Agriculture
        //=======================================================
        //  AgricultureNet
        //=======================================================
        #region AgricultureNet
        ///------------------------------------------------------
        /// <summary> The AgricultureNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty AgricultureNet;

        ///------------------------------------------------------
        /// <summary> Gets the AgricultureNet  </summary>
        ///<returns> the AgricultureNet </returns>

        public int[] geti_Agriculture_Net()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Agriculture_Net();
            }
            return result;
        }

        #endregion AgricultureNet
        //=======================================================
        //  Industrial
        //=======================================================
        #region Industrial
        ///------------------------------------------------------
        /// <summary> The Industrial provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Industrial;

        ///------------------------------------------------------
        /// <summary> Gets the Industrial  </summary>
        ///<returns> the Industrial </returns>

        public int[] geti_Industrial()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Industrial();
            }
            return result;
        }

        /// <summary> The industrial gped.</summary>
        public providerArrayProperty IndustrialGPED;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti industrial gped.</summary>
        ///
        /// <remarks> Quay, 3/20/2018.</remarks>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_IndustrialGPED()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_IndustrialGPED();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Industrial  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_Industrial(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_Industrial(Values[i]);
            }
        }


        /// <summary> The industry employees.</summary>
        public providerArrayProperty IndustryEmployees;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti industry employees.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <remarks> This has no effect, will make parameter read only</remarks>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_IndustryEmployees(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_IndustryEmployees(Values[i]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti industry employees.</summary>
        /// <remarks> Quay, 3/11/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_IndustryEmployees()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_IndustryEmployees();
            }
            return result;
        }

        #endregion Industrial
        //=======================================================
        //  IndustrialNet
        //=======================================================
        #region IndustrialNet
        ///------------------------------------------------------
        /// <summary> The IndustrialNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty IndustrialNet;

        ///------------------------------------------------------
        /// <summary> Gets the IndustrialNet  </summary>
        ///<returns> the IndustrialNet </returns>

        public int[] geti_Industrial_Net()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_Industrial_Net();
            }
            return result;
        }

        #endregion IndustrialNet
        //=======================================================
        //  Power
        //=======================================================
        #region Power
        ///------------------------------------------------------
        /// <summary> The Power provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Power;

        ///------------------------------------------------------
        /// <summary> Gets the Power  </summary>
        ///<returns> the Power </returns>

        public int[] geti_PowerWater()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PowerWater();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Power  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_PowerWater(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_PowerWater(Values[i]);
            }
        }


        /// <summary> The power gpmwd.</summary>
        public providerArrayProperty PowerGPMWD;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti power gpmwd.</summary>
        /// <remarks> Quay, 3/20/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_PowerGPMWD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PowerGPMWD();
            }
            return result;
        }

        #endregion Power
        //=======================================================
        //  PowerNet
        //=======================================================
        #region PowerNet
        ///------------------------------------------------------
        /// <summary> The PowerNet provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty PowerNet;

        ///------------------------------------------------------
        /// <summary> Gets the PowerNet  </summary>
        ///<returns> the PowerNet </returns>

        public int[] geti_PowerWater_Net()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PowerWater_Net();
            }
            return result;
        }

        #endregion PowerNet
        //=======================================================
        //  PowerEnergy
        //=======================================================
        #region PowerEnergy
        ///------------------------------------------------------
        /// <summary> The PowerEnergy provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty PowerEnergy;

        ///------------------------------------------------------
        /// <summary> Gets the PowerEnergy  </summary>
        ///<returns> the PowerEnergy </returns>

        public int[] geti_PowerEnergy()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PowerEnergy();
            }
            return result;
        }

        #endregion PowerEnergy

        //=======================================================
        //      TOTAL DEMAND
        //======================================================
        #region TotalDEmand

        /// <summary> The total demand.</summary>
        public providerArrayProperty TotalDemand;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti total demand.</summary>
        /// <remarks> Quay, 3/31/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_TotalDemand()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_TotalDemand();
            }
            return result;
        }


        #endregion TotalDemand

        // Total Water Demand Net
        #region TotalWaterDemandNet
        /// <summary>
        ///  Net water balance of total water demand
        /// </summary>
        public providerArrayProperty TotalDemandNet;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the geti total demand net. </summary>
        ///
        /// <remarks>   2/16/2018. </remarks>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_TotalDemandNet()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_TotalDemandNet();
            }
            return result;
        }
        #endregion


        //=======================================================
        //  NetDemandDifference
        //=======================================================
        #region NetDemandDifference
        ///------------------------------------------------------
        /// <summary> The NetDemandDifference provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty NetDemandDifference;

        ///------------------------------------------------------
        /// <summary> Gets the NetDemandDifference  </summary>
        ///<returns> the NetDemandDifference </returns>

        public int[] geti_NetDemandDifference()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_NetDemandDifference();
            }
            return result;
        }

        #endregion NetDemandDifference
        //=======================================================
        //  UrbanWaterConservation
        //=======================================================
        #region UrbanWaterConservation
        ///------------------------------------------------------
        /// <summary> The UrbanWaterConservation provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty UrbanWaterConservation;

        ///------------------------------------------------------
        /// <summary> Gets the UrbanWaterConservation  </summary>
        ///<returns> the UrbanWaterConservation </returns>

        public int[] geti_UrbanConservation()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanConservation();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a UrbanWaterConservation  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanConservation(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanConservation(Values[i]);
            }
        }
        #endregion UrbanWaterConservation
        //=======================================================
        //  AgWaterConservation
        //=======================================================
        #region AgWaterConservation
        ///------------------------------------------------------
        /// <summary> The AgWaterConservation provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty AgWaterConservation;

        ///------------------------------------------------------
        /// <summary> Gets the AgWaterConservation  </summary>
        ///<returns> the AgWaterConservation </returns>

        public int[] geti_AgConservation()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AgConservation();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a AgWaterConservation  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_AgConservation(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_AgConservation(Values[i]);
            }
        }
        #endregion AgWaterConservation
        //=======================================================
        //  PowerWaterConservation
        //=======================================================
        #region PowerWaterConservation
        ///------------------------------------------------------
        /// <summary> The PowerWaterConservation provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty PowerWaterConservation;

        ///------------------------------------------------------
        /// <summary> Gets the PowerWaterConservation  </summary>
        ///<returns> the PowerWaterConservation </returns>

        public int[] geti_PowerConservation()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PowerConservation();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a PowerWaterConservation  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_PowerConservation(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_PowerConservation(Values[i]);
            }
        }
        #endregion PowerWaterConservation
        //=======================================================
        //  IndustrialWaterConservation
        //=======================================================
        #region IndustrialWaterConservation
        ///------------------------------------------------------
        /// <summary> The IndustrialWaterConservation provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty IndustrialWaterConservation;

        ///------------------------------------------------------
        /// <summary> Gets the IndustrialWaterConservation  </summary>
        ///<returns> the IndustrialWaterConservation </returns>

        public int[] geti_IndustryConservation()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_IndustryConservation();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a IndustrialWaterConservation  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_IndustryConservation(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_IndustryConservation(Values[i]);
            }
        }
        #endregion IndustrialWaterConservation
        //=======================================================
        //  SurfaceWaterManagement
        //=======================================================
        #region SurfaceWaterManagement
        ///------------------------------------------------------
        /// <summary> The SurfaceWaterManagement provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty SurfaceWaterManagement;

        ///------------------------------------------------------
        /// <summary> Gets the SurfaceWaterManagement  </summary>
        ///<returns> the SurfaceWaterManagement </returns>

        public int[] geti_SurfaceWaterControl()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SurfaceWaterControl();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a SurfaceWaterManagement  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SurfaceWaterControl(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SurfaceWaterControl(Values[i]);
            }
        }
        #endregion SurfaceWaterManagement
        //=======================================================
        //  GroundwaterManagement
        //=======================================================
        #region GroundwaterManagement
        ///------------------------------------------------------
        /// <summary> The GroundwaterManagement provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty GroundwaterManagement;

        ///------------------------------------------------------
        /// <summary> Gets the GroundwaterManagement  </summary>
        ///<returns> the GroundwaterManagement </returns>

        public int[] geti_GroundwaterControl()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GroundwaterControl();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a GroundwaterManagement  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_GroundwaterControl(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GroundwaterControl(Values[i]);
            }
        }
        #endregion GroundwaterManagement


        //=======================================================
        //  ReclainedWaterUse and Wastewater flow
        //=======================================================
        #region ReclainedWaterUse

        //======================================================
        // Urban Wastewater Flow
        // =====================================================


        /// <summary> The urban wastewater flow.</summary>
        public providerArrayProperty UrbanWastewaterFlow;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti urban waste water flow.</summary>
        /// <remarks> Quay, 3/22/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_UrbanWasteWaterFlow()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_WasteWaterFlow();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> The ReclainedWaterUse provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty ReclainedWaterUse;

        ///------------------------------------------------------
        /// <summary> Gets the Ratio of Allowed Effluent that can be used for Reclaimed Water Use  </summary>
        ///<returns> the ReclainedWaterUse </returns>
        ///------------------------------------------------------

        public int[] geti_ReclaimedWaterManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ReclaimedWaterManagement();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets the Ratio of Allowed Effluent that can be used for Reclaimed Water Use  </summary>
        /// <param name="Values">   The values. </param>
        ///--------------------------------------------------------
        public void seti_ReclaimedWaterManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ReclaimedWaterManagement(Values[i]);
            }
        }
        #endregion ReclainedWaterUse

        #region Gray Water Use

        
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty GrayWaterManagement;

        ///------------------------------------------------------
        /// <summary> Gets the Ratio of Allowed Effluent that can be used for Reclaimed Water Use  </summary>
        ///<returns> the ReclainedWaterUse </returns>
        ///------------------------------------------------------
         /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] geti_GrayWaterManagement()
        {

            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            int[] temp = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GrayWaterManagement();
               
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets the Ratio of Allowed Effluent that can be used for Reclaimed Water Use  </summary>
        /// <param name="Values">   The values. </param>
        ///--------------------------------------------------------
        public void seti_GrayWaterManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GrayWaterManagement(Values[i]);
            }
        }
        //public int[] geti_GrayWaterFlow()
        //{

        //    int ArraySize = FUnitModels.Count;
        //    int[] result = new int[ArraySize];
        //    int[] temp = new int[ArraySize];
        //    for (int i = 0; i < ArraySize; i++)
        //    {
        //        result[i] = FUnitModels[i].geti_GrayWaterFlow();

        //    }
        //    return result;
        //}



        #endregion Gray Water Use

        //=======================================================
        //  LakeWaterManagement
        //=======================================================
        #region LakeWaterManagement
        ///------------------------------------------------------
        /// <summary> The LakeWaterManagement provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty LakeWaterManagement;

        ///------------------------------------------------------
        /// <summary> Gets the LakeWaterManagement  </summary>
        ///<returns> the LakeWaterManagement </returns>

        public int[] geti_LakeWaterManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_LakeWaterManagement();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a LakeWaterManagement  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_LakeWaterManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_LakeWaterManagement(Values[i]);
            }
        }
        #endregion LakeWaterManagement
        //=======================================================
        //  Augmented
        //=======================================================
        #region Augmented
        ///------------------------------------------------------
        /// <summary> The Augmented provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Augmented;

        ///------------------------------------------------------
        /// <summary> Gets the Augmented  </summary>
        ///<returns> the Augmented </returns>

        public int[] geti_Augmented()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
               // result[i] = FUnitModels[i].;
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Augmented  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_Augmented(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                //FUnitModels[i].(Values[i]);
            }
        }
        #endregion Augmented
        //=======================================================
        //  Desal
        //=======================================================
        #region Desalination
        ///------------------------------------------------------
        /// <summary> The Augmented provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty Desalination;

        ///------------------------------------------------------
        /// <summary> Gets the Augmented  </summary>
        ///<returns> the Augmented </returns>

        public int[] geti_Desalinization()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].Geti_DesalPolicy();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a Augmented  </summary>
        /// <param name="Values">   The values. </param>
        // 01.13.22 used (set) in the manager das
        public void seti_Desalinization(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].Seti_DesalPolicy(Values[i]);
            }
        }
        #endregion Desalination


        //=======================================================
        #region Recycle
        //public providerArrayProperty Recycle;

        /////------------------------------------------------------
        ///// <summary> Gets the Augmented  </summary>
        /////<returns> the Augmented </returns>

        //public int[] geti_RecycleWasteWater()
        //{
        //    int ArraySize = FUnitModels.Count;
        //    int[] result = new int[ArraySize];
        //    for (int i = 0; i < ArraySize; i++)
        //    {
        //        result[i] = FUnitModels[i].geti_Recycle();
        //    }
        //    return result;
        //}


        /////------------------------------------------------------
        ///// <summary> Sets a Augmented  </summary>
        ///// <param name="Values">   The values. </param>

        //public void seti_RecycleWasteWater(int[] Values)
        //{
        //    int ArraySize = FUnitModels.Count;
        //    if (ArraySize > Values.Length)
        //    {
        //        ArraySize = Values.Length;
        //    }
        //    for (int i = 0; i < ArraySize; i++)
        //    {
        //        FUnitModels[i].seti_Recycle(Values[i]);
        //    }
        //}
        #endregion Recycle







        //=======================================================
        //  PopGrowthRate
        //=======================================================
        #region PopGrowthRate
        ///------------------------------------------------------
        /// <summary> The PopGrowthRate provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty PopGrowthRate;

        ///------------------------------------------------------
        /// <summary> Gets the PopGrowthRate  </summary>
        ///<returns> the PopGrowthRate </returns>

        public int[] geti_PopGrowthRate()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PopGrowthRate();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a PopGrowthAdjustment  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_PopGrowthRate(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_PopGrowthRate(Values[i]);
            }
        }
        #endregion PopGrowthRate

        #region PopGrowthRate Modifyer
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty PopGrowthRateModifyer;

        ///------------------------------------------------------
        /// <summary> Gets the PopGrowthRate  </summary>
        ///<returns> the PopGrowthRate </returns>

        public int[] geti_PopGrowthRateMod()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_PopGRateModifier();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a PopGrowthAdjustment  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_PopGrowthRateMod(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_PopGRateModifier(Values[i]);
            }
        }
        #endregion PopGrowthRate Modifyer
        // ------------------------------------------------------
        
        // EDIT QUAY 9/13/18

        //=======================================================
        //  ClimateDrought
        //=======================================================
        #region ClimateDrought


        /// <summary>   The climate change target. </summary>
        public providerArrayProperty ClimateChangeTarget;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target. </summary>
        /// <returns>   An int[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_ClimateChangeTarget()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ClimateChangeTarget();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target. </summary>
        /// <param name="Values">   The values. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTarget(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ClimateChangeTarget(Values[i]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target. </summary>
        /// <remarks>   This sets all the models ClimateChange Target to the same value</remarks>
        /// <param name="Value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTargetBase(int Value)
        {
            int ArraySize = FUnitModels.Count;
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ClimateChangeTarget(Value);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target base. </summary>
        /// <remarks>   This returns the Basin average for the ClimateChange Target based on teh aggregate mode (weighted average)
        ///             NOTE!! if a call to seti_ClimateChangeTargetBase() is followed by a call to  seti_ClimateChangeTarget() then the value returned 
        ///             by this method may not be equal to the value initially set using  seti_ClimateChangeTargetBase()  </remarks>
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int geti_ClimateChangeTargetBase()
        {
            int result = ClimateChangeTarget.RegionalValue(eProvider.eBasin);
            return result;
        }

        /// <summary>   The climate change target year. </summary>
        public providerArrayProperty ClimateChangeTargetYear;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target year. </summary>
        /// <returns>   An int[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_ClimateChangeTargetYear()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ClimateChangeTargetYear();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target year. </summary>
        /// <param name="Values">   The values. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTargetYear(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ClimateChangeTargetYear(Values[i]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Seti climate change target year. </summary>
        /// <remarks>   This sets the target year value for all models to the same year</remarks>
        /// <param name="Value">    The value. </param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_ClimateChangeTargetYearBase(int Value)
        {
            int ArraySize = FUnitModels.Count;
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ClimateChangeTargetYear(Value);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Geti climate change target year base. </summary>
        /// <remarks>  NOTE !!!! This will return a missing value since years are not agregated, so there is no BASE value
        ///            use geti_ClimateChangeTargetYear() to see the target year for each parameter</remarks>
        /// 
        /// <returns>   An int. </returns>
        /// <see cref="geti_ClimateChangeTargetYear"/>
        ///-------------------------------------------------------------------------------------------------

        public int geti_ClimateChangeTargetYearBase()
        {
            int result = ClimateChangeTargetYear.RegionalValue(eProvider.eBasin);
            return result;
        }

        // END EDIT 9/12/18
        #endregion ClimateDrought
        //------------------------------------------------------



        // ======================================================

        // ------------------------------------------------------
        //
        // Denver Demand Model Variables
        // 06.01.18,06.03.18
        // das

        //=================================================================================================
        //  Demand Model
        //=================================================================================================
        #region Model To Use
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempModel"></param>
        public void set_DefaultDemandModel(WaterSimCRFModel TempModel)
        {
            DemandModel tempUrban = new UrbanDemand_GPCD(TempModel);
            TempModel.URBAN = tempUrban;
            DemandModel tempAg = new AgriculturalDemand_income(TempModel);
            TempModel.AG = tempAg;
            DemandModel tempInd = new IndustryDemand_employee(TempModel);
            TempModel.INDUSTRY = tempInd;
            DemandModel tempPower = new PowerDemand_wp(TempModel);
            TempModel.POWER = tempPower;
        }

        ///------------------------------------------------------
        /// <summary> URBAN  </summary>
        ///------------------------------------------------------
        public providerArrayProperty DemandModelUrban_Index;

        ///------------------------------------------------------
        /// <summary> Gets the AgriculturalGrowth  </summary>
        ///<returns> the AgriculturalGrowth </returns>

        public int[] geti_DemandModelUrbanIndex()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_aDemandModelUrbanIndex();
                //         
            }
            return result;
        }
        ///------------------------------------------------------
        /// das123
        /// <summary> Sets a AgriculturalGrowth  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_DemandModelUrbanIndex(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                set_DemandModelUrban(FUnitModels[i], Values[i]);
                FUnitModels[i].seti_aDemandModelUrbanIndex(Values[i]);
            }

        }
          //
        /// <summary>
        /// This method sets the demand model to use based on the value being passed in.
        /// </summary>
        /// <param name="TempModel"></param>
        /// <param name="model"></param>
        public void set_DemandModelUrban(WaterSimCRFModel TempModel, int model)
        {
            switch (model)
            {
                case 1:
                    DemandModel tempUrban = new UrbanDemand_GPCD(TempModel);
                    TempModel.URBAN = tempUrban;
                    break;
                case 2:
                    DemandModel tempUrban_2 = new RuralDemand_LCLU_urban(TempModel, FRateData, FDataLCLU);
                    TempModel.URBAN = tempUrban_2;
                    break;
                case 3:
                    // ICLUS version II data - urban classes
                    //DemandModel tempUrban_3 = new RuralDemand_LCLU_urban(TempModel, FRateData, FDataLCLU, FDataLCLUarea);
                    //TempModel.URBAN = tempUrban_3;
                    // 02.09.22 das
                    DemandModel tempUrban_3 = new RuralDemand_LCLU_urban(TempModel, FRateData, FDataLCLU, FDataLCLUarea, UDproportions);
                    TempModel.URBAN = tempUrban_3;
                    break;

                default:
                    DemandModel tempUrban_d = new UrbanDemand_GPCD(TempModel);
                    TempModel.URBAN = tempUrban_d;
                    break;
            }
          }
        //=================================================================================================
        ///------------------------------------------------------
        /// <summary> AG  </summary>
        ///------------------------------------------------------
        public providerArrayProperty DemandModelAg_Index;

        ///------------------------------------------------------
        /// <summary> Gets the AgriculturalGrowth  </summary>
        ///<returns> the AgriculturalGrowth </returns>

        public int[] geti_DemandModelAgIndex()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_aDemandModelAgIndex();
                //         
            }
            return result;
        }


        ///------------------------------------------------------
        /// das123
        /// <summary> Sets a AgriculturalGrowth  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_DemandModelAgIndex(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                set_DemandModelAg(FUnitModels[i], Values[i]);
                FUnitModels[i].seti_aDemandModelAgIndex(Values[i]);
            }

        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempModel"></param>
        /// <param name="model"></param>
        public void set_DemandModelAg(WaterSimCRFModel TempModel, int model)
        {
            switch (model)
            {
                case 1:
                    DemandModel tempAg = new AgriculturalDemand_income(TempModel);
                    TempModel.AG = tempAg;
                    break;
                case 2:
                    DemandModel tempAg_2 = new RuralDemand_LCLU_ag(TempModel, FRateData, FDataLCLU);
                    TempModel.AG = tempAg_2;
                    break;
                case 3:
                    DemandModel tempAg_3 = new RuralDemand_LCLU_ag(TempModel, FRateData, FDataLCLU, FDataLCLUarea);
                    TempModel.AG = tempAg_3;

                    break;


                default:
                    DemandModel tempAg_d = new AgriculturalDemand_income(TempModel);
                    TempModel.AG = tempAg_d;
                    break;
            }
        }
        //=================================================================================================
        ///------------------------------------------------------
        /// <summary> INDUSTRY  </summary>
        ///------------------------------------------------------
        public providerArrayProperty DemandModelInd_Index;

        ///------------------------------------------------------
        /// <summary> Gets the AgriculturalGrowth  </summary>
        ///<returns> the AgriculturalGrowth </returns>

        public int[] geti_DemandModelIndIndex()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_aDemandModelIndIndex();
                //         
            }
            return result;
        }


        ///------------------------------------------------------
        /// das123
        /// <summary> Sets a AgriculturalGrowth  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_DemandModelIndIndex(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                set_DemandModelInd(FUnitModels[i], Values[i]);
                FUnitModels[i].seti_aDemandModelIndIndex(Values[i]);
            }

        }
        //
        /// <summary>
        ///  NOTE: default settings are found in Manager line 688 (tempLCLU)
        /// </summary>
        /// <param name="TempModel"></param>
        /// <param name="model"></param>
        public void set_DemandModelInd(WaterSimCRFModel TempModel, int model)
        {

            switch (model)
            {
                case 1:
                    DemandModel tempInd = new IndustryDemand_employee(TempModel);
                    TempModel.INDUSTRY = tempInd;
                    break;
                case 2:
                    DemandModel tempInd_2 = new RuralDemand_LCLU_industry(TempModel, FRateData, FDataLCLU);
                    TempModel.INDUSTRY = tempInd_2;
                    break;
                case 3:
                    break;


                default:
                    DemandModel tempInd_d = new IndustryDemand_employee(TempModel);
                    TempModel.INDUSTRY = tempInd_d;
                    break;
            }
        }


        //
        #endregion Model To Use

        // ------------------------------------------------------
        // =======================================================================
        // Original code
        //=======================================================
        //  Urban Density Management of ICLUS urban classes
        // 09.20.21 das
        //=======================================================
        #region Urban Density
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty UrbanHighDensity;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanHighDensityManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanHighDensity();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanHighDensityManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanHighDensity(Values[i]);
            }
        }
        // =======================================================================
        // findMe
        // =======================================================================
        // NEW code as of February 2022 das
        // ===========================================
        /// <summary>
        ///  Using Denver Water Small multi-family data
        ///  to drive this density class
        /// </summary>
        public providerArrayProperty UrbanHighDensitySMF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanHighDensityManagementSMF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanHighDensitySMF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanHighDensityManagementSMF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanHighDensitySMF(Values[i]);
            }
        }
        // =======================================================================
        /// <summary>
        /// using Denver water Three story walk-up density class
        /// to drive this DUA density class
        /// </summary>
        public providerArrayProperty UrbanHighDensityWMF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanHighDensityManagementWMF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanHighDensityWMF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanHighDensityManagementWMF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanHighDensityWMF(Values[i]);
            }
        }
        // =======================================================================
        /// <summary>
        ///  Using Denver Water Medium Multi-family density DUA data to drive this
        ///  density class
        /// </summary>
        public providerArrayProperty UrbanHighDensityMMF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanHighDensityManagementMMF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanHighDensityMMF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanHighDensityManagementMMF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanHighDensityMMF(Values[i]);
            }
        }
        // =======================================================================
        /// <summary>
        ///  Using Denver Water High density multi-family data class
        ///  to drive this DUA class
        /// </summary>
        public providerArrayProperty UrbanHighDensityHMF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanHighDensityManagementHMF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanHighDensityHMF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanHighDensityManagementHMF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanHighDensityHMF(Values[i]);
            }
        }
        // =======================================================================


        // =============================================================
        // Original & NEW code 
        #region low density access to CRF model
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty UrbanLowDensity;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanLowDensityManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanLowDensity();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanLowDensityManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanLowDensity(Values[i]);
            }
        }
        // ------------------------------------------------------
        // =======================================================
        //
        // New code in January/ February 2022 das
        // Denver Water vernacular for density classifications
        // =======================================================================
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty UrbanLowDensityLSF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanLowDensityManagementLSF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanLowDensityLSF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanLowDensityManagementLSF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanLowDensityLSF(Values[i]);
            }
        }
        // ------------------------------------------------------
        // =======================================================
        // =============================================================
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty UrbanLowDensityTSF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanLowDensityManagementTSF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanLowDensityTSF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanLowDensityManagementTSF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanLowDensityTSF(Values[i]);
            }
        }
        // ------------------------------------------------------
        // =======================================================
        // =============================================================
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty UrbanLowDensitySSF;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_UrbanLowDensityManagementSSF()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_UrbanLowDensitySSF();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_UrbanLowDensityManagementSSF(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_UrbanLowDensitySSF(Values[i]);
            }
        }
        // ------------------------------------------------------
        // =======================================================================
        #endregion low (housing) density access to CRF model
        // =============================================================
        #region suburban, Exurban housing density access to CRF model
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty SuburbanDensity;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_SuburbanDensityManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SuburbanDensity();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SuburbanDensityManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SuburbanDensity(Values[i]);
            }
        }
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty ExurbanHighDensity;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_ExurbanHighDensityManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ExurbanHighDensity();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_ExurbanHighDensityManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ExurbanHighDensity(Values[i]);
            }
        }
        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty ExurbanLowDensity;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_ExurbanLowDensityManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_ExurbanLowDensity();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_ExurbanLowDensityManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_ExurbanLowDensity(Values[i]);
            }
        }
        #endregion suburan, exhurban density
        #endregion Urban Density Management
        //==============================================================
        #region Updated Policies- October 2021

        ///------------------------------------------------------      
        ///<summary> Urban High intensity management control</summary>
        public providerArrayProperty AirWaterExtraction;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_AirWaterManagement()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AirWater();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_AirWaterManagement(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_AirWater(Values[i]);
            }
        }

        /// <summary>
        ///  THe perentage of users that install Atmospheric water systems
        /// </summary>
        public providerArrayProperty AirWaterCompliance;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>
        public int[] geti_AirWaterInstallations()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AirWaterInstallations();
            }
            return result;
        }
        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_AirWaterInstallations(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_AirWaterInstallations(Values[i]);
            }
        }
        //
        // =========================================================
        // edits 02.10.22 das
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty NewWaterSavingsOnDemand;
        /// <summary>
        ///  Urban water demand savings from atmos and rainwater
        /// </summary>
        /// <returns></returns>
        public int[] geti_NewWaterSavingsOnDemand()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].NewWaterSavings;
            }
            return result;
        }
        /// <summary>
        ///  THe potential amount of stormwater runoff, measured in gallons per acre per day
        /// </summary>
        public providerArrayProperty StormwaterPotential;
        /// <summary>
        ///  Urban water demand savings from atmos and rainwater
        /// </summary>
        /// <returns></returns>
        public int[] geti_StormwaterPotential()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].Stormwater;
            }
            return result;
        }
  

        // =========================================================
        #endregion Updated Policies - October 2021
        //=======================================================

        //  ClimateDrought
        //=======================================================
        #region ClimateDrought
        ///------------------------------------------------------
        /// <summary> The ClimateDrought provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty ClimateDrought;

        ///------------------------------------------------------
        /// <summary> Gets the ClimateDrought  </summary>
        ///<returns> the ClimateDrought </returns>

        public int[] geti_DroughtImpacts()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtImpacts();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a ClimateDrought  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_DroughtImpacts(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughtImpacts(Values[i]);
            }
        }
        #endregion ClimateDrought
        //=======================================================
        //  Drought ControlS
        //=======================================================
        #region DroughtControls



        ///------------------------------------------------------
        /// <summary> The DroughtControl provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty DroughtControl;

        ///------------------------------------------------------
        /// <summary> Gets the DroughtControl  </summary>
        ///<returns> the DroughtControl </returns>

        public int[] geti_DroughtControl()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtControl();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a DroughtControl  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_DroughtControl(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughtControl(Values[i]);
            }
        }


        /// <summary> The drought active Provider Array.</summary>
        public providerArrayProperty DroughtActive;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought active.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DroughtActive()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtActive();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought active.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtActive(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughtActive(Values[i]);
            }
        }

        /// <summary> The drought Start Year Provider Array.</summary>
        public providerArrayProperty DroughtStartYear;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought start year.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DroughtStartYear()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtSartYear();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought start year.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtStartYear(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughStartYear(Values[i]);
            }
        }

        /// <summary> The drought length Provider Array.</summary>
        public providerArrayProperty DroughtLength;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought length.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DroughtLength()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtLength();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought length.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtLength(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughtLength(Values[i]);
            }
        }

        /// <summary> The drought depth Provider Array.</summary>
        public providerArrayProperty DroughtDepth;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti drought depth.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DroughtDepth()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DroughtDepth();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti drought depth.</summary>
        /// <remarks> Quay, 3/24/2018.</remarks>
        /// <param name="Values"> The values.</param>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DroughtDepth(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DroughtDepth(Values[i]);
            }
        }

        #endregion DroughtControls

        //===========================================================
        // CAP Resource Limits to Demand and Revise Resource Controls
        // ==========================================================


        #region CapAndReviseResources

        /// <summary> The DoCapLimits Provider Array.</summary>
        public providerArrayProperty DoCapLimits;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti DoCapLmits.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <remarks>  Turn on off the methods to cap resource limits to demand, prevents over allocation of resources</remarks> 
        /// <param name="Values"> The values 0=False , off any other value = True, on.</param>
        /// <seealso cref="WaterSimCRFModel.DoCapLimits"/>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DoCapLimits(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DoCapLimits(Values[i]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti DoCapLimits.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <remarks> gets the state of the model DoCapLimits property</remarks>
        /// <returns> An int[].</returns>
        /// <seealso cref="WaterSimCRFModel.DoCapLimits"/>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DoCapLimits()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DoCapLimits();
            }
            return result;
        }

        /// <summary> The DoReviseResources Provider Array.</summary>
        public providerArrayProperty DoReviseResources;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Seti DoReviseResources.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <remarks>  Turn on off the methods to revise the resources controls to match cap esources at the end of the scenari
        ///            will not work unless DoCapLimits is turned on</remarks> 
        /// <param name="Values"> The values.0=False , off any other value = True, on.</param>
        /// <seealso cref="WaterSimCRFModel.DoReviseResources"/>
        ///-------------------------------------------------------------------------------------------------

        public void seti_DoReviseResources(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_DoReviseResources(Values[i]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti DoReviseResources.</summary>
        /// <remarks> Quay, 4/15/2018.</remarks>
        /// <remarks> gets the state of the model DoReviseResources property</remarks>
        /// <returns> An int[]. 0=False , 1 = True, on.</returns>
        /// <seealso cref="WaterSimCRFModel.DoReviseResources"/>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_DoReviseResources()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_DoReviseResources();
            }
            return result;
        }

        #endregion CaqpAndReviseResources
        //
        //=======================================================
        //  AgricultureProduction
        //=======================================================
        #region AgricultureProduction
        ///------------------------------------------------------
        /// <summary> The AgricultureProduction provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty AgricultureProduction;

        ///------------------------------------------------------
        /// <summary> Gets the AgricultureProduction  </summary>
        ///<returns> the AgricultureProduction </returns>

        public int[] geti_AgricutureProduction()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AgricutureProduction();
            }
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Geti agriculture gpdd.</summary>
        ///
        /// <remarks> Quay, 3/4/2018.</remarks>
        ///
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] geti_agricultureGPDD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_agricultureGPDD();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty AgricultureGPDD;

        #endregion AgricultureProduction

        //=======================================================
        //  Initial Agricultural  GPDD
        //=======================================================
        #region AgricultureInitialGPDD
        /// <summary>
        /// 
        /// </summary>
        public providerArrayProperty initialAgricultureGPDD;
        /// <summary>
        ///  is this gallons per dollar per day? Ray?
        /// </summary>
        /// <returns></returns>
        public int[] geti_initialAgricultureGPDD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AgricultureInitialGPDD();
            }
            return result;
        }
        #endregion AgricultureInitialGPDD
        //=======================================================
        //  AgriculturalGrowth
        //=======================================================
        #region AgriculturalGrowth
        ///------------------------------------------------------
        /// <summary> The AgriculturalGrowth provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty AgriculturalGrowth;

        ///------------------------------------------------------
        /// <summary> Gets the AgriculturalGrowth  </summary>
        ///<returns> the AgriculturalGrowth </returns>

        public int[] geti_AgGrowthRate()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_AgGrowthRate();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a AgriculturalGrowth  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_AgGrowthRate(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_AgGrowthRate(Values[i]);
            }
        }
        #endregion AgriculturalGrowth

        //==================================





       //
        // FLUXES
        //==================================

        //=======================================================
        //  _SUR_UD
        //=======================================================
        #region _SUR_UD
        ///------------------------------------------------------
        /// <summary> The _SUR_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SUR_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _SUR_UD  </summary>
        ///<returns> the _SUR_UD </returns>

        public int[] geti_SUR_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SUR_UD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SUR_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SUR_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SUR_UD(Values[i]);
            }
        }
        #endregion _SUR_UD
        //=======================================================
        //  _SUR_AD
        //=======================================================
        #region _SUR_AD
        ///------------------------------------------------------
        /// <summary> The _SUR_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SUR_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _SUR_AD  </summary>
        ///<returns> the _SUR_AD </returns>

        public int[] geti_SUR_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SUR_AD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SUR_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SUR_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SUR_AD(Values[i]);
            }
        }
        #endregion _SUR_AD
        //=======================================================
        //  _SUR_ID
        //=======================================================
        #region _SUR_ID
        ///------------------------------------------------------
        /// <summary> The _SUR_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SUR_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _SUR_ID  </summary>
        ///<returns> the _SUR_ID </returns>

        public int[] geti_SUR_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SUR_ID();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SUR_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SUR_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SUR_ID(Values[i]);
            }
        }
        #endregion _SUR_ID
        //=======================================================
        //  _SUR_PD
        //=======================================================
        #region _SUR_PD
        ///------------------------------------------------------
        /// <summary> The _SUR_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SUR_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _SUR_PD  </summary>
        ///<returns> the _SUR_PD </returns>

        public int[] geti_SUR_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SUR_PD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SUR_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SUR_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SUR_PD(Values[i]);
            }
        }
        #endregion _SUR_PD
        //=======================================================
        //  _SURL_UD
        //=======================================================
        #region _SURL_UD
        ///------------------------------------------------------
        /// <summary> The _SURL_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SURL_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _SURL_UD  </summary>
        ///<returns> the _SURL_UD </returns>

        public int[] geti_SURL_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SURL_UD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SURL_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SURL_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SURL_UD(Values[i]);
            }
        }
        #endregion _SURL_UD
        //=======================================================
        //  _SURL_AD
        //=======================================================
        #region _SURL_AD
        ///------------------------------------------------------
        /// <summary> The _SURL_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SURL_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _SURL_AD  </summary>
        ///<returns> the _SURL_AD </returns>

        public int[] geti_SURL_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SURL_AD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SURL_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SURL_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SURL_AD(Values[i]);
            }
        }
        #endregion _SURL_AD
        //=======================================================
        //  _SURL_ID
        //=======================================================
        #region _SURL_ID
        ///------------------------------------------------------
        /// <summary> The _SURL_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SURL_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _SURL_ID  </summary>
        ///<returns> the _SURL_ID </returns>

        public int[] geti_SURL_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SURL_ID();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SURL_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SURL_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SURL_ID(Values[i]);
            }
        }
        #endregion _SURL_ID
        //=======================================================
        //  _SURL_PD
        //=======================================================
        #region _SURL_PD
        ///------------------------------------------------------
        /// <summary> The _SURL_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SURL_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _SURL_PD  </summary>
        ///<returns> the _SURL_PD </returns>

        public int[] geti_SURL_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SURL_PD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SURL_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SURL_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SURL_PD(Values[i]);
            }
        }
        #endregion _SURL_PD
        //=======================================================
        //  _GW_UD
        //=======================================================
        #region _GW_UD
        ///------------------------------------------------------
        /// <summary> The _GW_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _GW_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_UD  </summary>
        ///<returns> the _GW_UD </returns>

        public int[] geti_GW_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GW_UD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _GW_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_GW_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GW_UD(Values[i]);
            }
        }
        #endregion _GW_UD
        //=======================================================
        //  _GW_AD
        //=======================================================
        #region _GW_AD
        ///------------------------------------------------------
        /// <summary> The _GW_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _GW_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_AD  </summary>
        ///<returns> the _GW_AD </returns>

        public int[] geti_GW_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GW_AD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _GW_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_GW_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GW_AD(Values[i]);
            }
        }
        #endregion _GW_AD
        //=======================================================
        //  _GW_ID
        //=======================================================
        #region _GW_ID
        ///------------------------------------------------------
        /// <summary> The _GW_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _GW_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_ID  </summary>
        ///<returns> the _GW_ID </returns>

        public int[] geti_GW_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GW_ID();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _GW_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_GW_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GW_ID(Values[i]);
            }
        }
        #endregion _GW_ID
        //=======================================================
        //  _GW_PD
        //=======================================================
        #region _GW_PD
        ///------------------------------------------------------
        /// <summary> The _GW_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _GW_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_PD  </summary>
        ///<returns> the _GW_PD </returns>

        public int[] geti_GW_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_GW_PD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _GW_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_GW_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_GW_PD(Values[i]);
            }
        }
        #endregion _GW_PD
        //=======================================================
        //  _REC_UD
        //=======================================================
        #region _REC_UD
        ///------------------------------------------------------
        /// <summary> The _REC_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _REC_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _REC_UD  </summary>
        ///<returns> the _REC_UD </returns>

        public int[] geti_REC_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_REC_UD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _REC_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_REC_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_REC_UD(Values[i]);
            }
        }
        #endregion _REC_UD
        //=======================================================
        //  _REC_AD
        //=======================================================
        #region _REC_AD
        ///------------------------------------------------------
        /// <summary> The _REC_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _REC_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _REC_AD  </summary>
        ///<returns> the _REC_AD </returns>

        public int[] geti_REC_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_REC_AD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _REC_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_REC_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_REC_AD(Values[i]);
            }
        }
        #endregion _REC_AD
        //=======================================================
        //  _REC_ID
        //=======================================================
        #region _REC_ID
        ///------------------------------------------------------
        /// <summary> The _REC_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _REC_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _REC_ID  </summary>
        ///<returns> the _REC_ID </returns>

        public int[] geti_REC_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_REC_ID();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _REC_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_REC_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_REC_ID(Values[i]);
            }
        }
        #endregion _REC_ID
        //=======================================================
        //  _REC_PD
        //=======================================================
        #region _REC_PD
        ///------------------------------------------------------
        /// <summary> The _REC_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _REC_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _REC_PD  </summary>
        ///<returns> the _REC_PD </returns>

        public int[] geti_REC_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_REC_PD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _REC_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_REC_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_REC_PD(Values[i]);
            }
        }
        #endregion _REC_PD
        //=======================================================
        #region total REC Demand from fluxes
        /// <summary>
        ///  Reclaimed water demand
        /// </summary>
        /// <returns></returns>
        public int[] geti_REC_D()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_REC_AD() + 
                    FUnitModels[i].geti_REC_ID() + 
                    FUnitModels[i].geti_REC_PD() + 
                    FUnitModels[i].geti_REC_UD();
            }
            return result;
        }
        #endregion total REC demand from fluxes
        // ======================================================


        //  _SAL_UD
        //=======================================================
        #region _SAL_UD
        ///------------------------------------------------------
        /// <summary> The _SAL_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SAL_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _SAL_UD  </summary>
        ///<returns> the _SAL_UD </returns>

        public int[] geti_SAL_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SAL_UD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SAL_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SAL_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SAL_UD(Values[i]);
            }
        }
        #endregion _SAL_UD
        //=======================================================
        //  _SAL_AD
        //=======================================================
        #region _SAL_AD
        ///------------------------------------------------------
        /// <summary> The _SAL_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SAL_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _SAL_AD  </summary>
        ///<returns> the _SAL_AD </returns>

        public int[] geti_SAL_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SAL_AD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SAL_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SAL_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SAL_AD(Values[i]);
            }
        }
        #endregion _SAL_AD
        //=======================================================
        //  _SAL_ID
        //=======================================================
        #region _SAL_ID
        ///------------------------------------------------------
        /// <summary> The _SAL_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SAL_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _SAL_ID  </summary>
        ///<returns> the _SAL_ID </returns>

        public int[] geti_SAL_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SAL_ID();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SAL_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SAL_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SAL_ID(Values[i]);
            }
        }
        #endregion _SAL_ID
        //=======================================================
        //  _SAL_PD
        //=======================================================
        #region _SAL_PD
        ///------------------------------------------------------
        /// <summary> The _SAL_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _SAL_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _SAL_PD  </summary>
        ///<returns> the _SAL_PD </returns>

        public int[] geti_SAL_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_SAL_PD();
            }
            return result;
        }


        ///------------------------------------------------------
        /// <summary> Sets a _SAL_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_SAL_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_SAL_PD(Values[i]);
            }
        }
        #endregion _SAL_PD
        // QUAY EDIT 9/12/18 Very late at Night!
        // Add colorado properties here

        //=======================================================
        //  _COL_AD
        //=======================================================
        #region _COL_AD
        ///------------------------------------------------------
        /// <summary> The _COL_AD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _COL_AD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_AD  </summary>
        ///<returns> the _GW_AD </returns>

        public int[] geti_COL_AD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_COL_AD();
            }
            return result;
        }

        ///------------------------------------------------------
        /// <summary> Sets a _COL_AD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_COL_AD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_COL_AD(Values[i]);
            }
        }

        #endregion _COL_AD

        //=======================================================
        //  _COL_UD
        //=======================================================
        #region _COL_UD
        ///------------------------------------------------------
        /// <summary> The _COL_UD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _COL_UD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_AD  </summary>
        ///<returns> the _GW_AD </returns>

        public int[] geti_COL_UD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_COL_UD();
            }
            return result;
        }

        ///------------------------------------------------------
        /// <summary> Sets a _COL_UD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_COL_UD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_COL_UD(Values[i]);
            }
        }

        #endregion _COL_UD

        //=======================================================
        //  _COL_ID
        //=======================================================
        #region _COL_ID
        ///------------------------------------------------------
        /// <summary> The _COL_ID provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _COL_ID;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_AD  </summary>
        ///<returns> the _GW_AD </returns>

        public int[] geti_COL_ID()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_COL_ID();
            }
            return result;
        }

        ///------------------------------------------------------
        /// <summary> Sets a _COL_ID  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_COL_ID(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_COL_ID(Values[i]);
            }
        }
        #endregion _COL_ID

        //=======================================================
        //  _COL_PD
        //=======================================================
        #region _COL_PD
        ///------------------------------------------------------
        /// <summary> The _COL_PD provider property  </summary>
        ///------------------------------------------------------
        public providerArrayProperty _COL_PD;

        ///------------------------------------------------------
        /// <summary> Gets the _GW_AD  </summary>
        ///<returns> the _GW_AD </returns>

        public int[] geti_COL_PD()
        {
            int ArraySize = FUnitModels.Count;
            int[] result = new int[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                result[i] = FUnitModels[i].geti_COL_PD();
            }
            return result;
        }

        ///------------------------------------------------------
        /// <summary> Sets a _COL_PD  </summary>
        /// <param name="Values">   The values. </param>

        public void seti_COL_PD(int[] Values)
        {
            int ArraySize = FUnitModels.Count;
            if (ArraySize > Values.Length)
            {
                ArraySize = Values.Length;
            }
            for (int i = 0; i < ArraySize; i++)
            {
                FUnitModels[i].seti_COL_PD(Values[i]);
            }
        }

        #endregion _COL_AD
        // END EDIT 9/12/18
        // ==========================================


        //=================================================================
        #endregion ProviderProperties
    }

    #endregion WaterSimModel
}

