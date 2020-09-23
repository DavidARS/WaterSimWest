// /////////////////////////////////////////////////////////////////////////
// Colroado Basin Specific River Models
//
// This uses classes from "WaterSimDCDC_API_CRF_Models" 
// It builds upon this base classes to create classes that are specfic to
// the Colorado River Basin
// These classes use  thge WaterSimDCDC.Generic namespace so they are aware of all the public classes 
// used in the WestModel.
//
// Version 1.0
// Author Ray Quay
//
// /////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterSimDCDC.Generic;

namespace WaterSimDCDC.Generic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A basin water resource by region.</summary>
    ///
    /// <remarks> Quay, 9/8/2020.</remarks>
    ///
    /// <seealso cref="WaterSimDCDC.Generic.AvailableResource"/>
    ///-------------------------------------------------------------------------------------------------

    public class BasinWater : AvailableResource
    {
        int[] FBasinWater = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.BasinWater class.</summary>
        /// <param name="NumberOfRegions"> Number of regions.</param>
        ///-------------------------------------------------------------------------------------------------

        public BasinWater(int NumberOfRegions)
        {
            if (NumberOfRegions >= 0)
            {
                FBasinWater = new int[NumberOfRegions];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.BasinWater class.</summary>
        ///
        /// <remarks> Uses ResourceData array to create Basinwater and populate the array</remarks>
        ///
        /// <param name="ResourceData"> Information describing the resource.</param>
        ///-------------------------------------------------------------------------------------------------

        public BasinWater(int[] ResourceData)
        {
            FBasinWater = ResourceData;
        }
        /// <summary>
        /// 
        /// </summary>
        public BasinWater()
        {
          
        }





        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the available region resource array.</summary>
        ///
        /// <value> The available water as an int array.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] AvailableWater
        {
            get { return FBasinWater; }
            set { FBasinWater = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resource amount for specific Region.</summary>
        /// <param name="index"> Zero-based index of the regions.</param>
        ///
        /// <returns> An int, amount of resource for index region.</returns>
        ///-------------------------------------------------------------------------------------------------
        public int RegionResource(int index)
        {
            if ((index>-1)&&(index<FBasinWater.Length))
            {
                return FBasinWater[index];
            }
            else
            {
                return 0;
            }
        }
       
    }




    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for CO Basin Rivers</summary>
    ///
    /// <remarks> Quay, 6/19/2020.</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModel"/>
    ///-------------------------------------------------------------------------------------------------

    public abstract class BasinRiverModel : ResourceModel
    {
        // Local Variables
        // The owner of the Resource Model
         WaterSimModel FOwner;
        
        // Number of Regions
        readonly int FRegionsN = 0;
        // Water Available by Region
        readonly BasinWater  FAvailableWater;
        //
        int FDroughtStartYearLF = 2015;
        int FDroughtLengthLF = 9999;
        int FDroughtActiveLF = 0;
        double FDroughtDepthLF = 0.0;
        public int DefaultDroughtStartYearLF = 2015;
        public int DefaultDroughtLengthLF = 9999;
        public int DefaultDroughtActiveLF = 0;
        public double DefaultDroughtDepthLF = 0.0;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the River Model with the given input data.</summary>
        ///
        /// <remarks> This needs to be implemented, Output and Input should be assined to Properties for retreival</remarks>
        ///
        /// <param name="InputData"> Information describing the input.</param>
        ///
        /// <returns> A RiverModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput Run(ResourceModelInput InputData, out int errCode, out string ErrString )
        {
            throw new NotImplementedException();
             
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
        ///
        /// <remarks>  Intended for models that need river model out put passed to it, such as a sperate model 
        ///            does allocation 
        ///     This needs to be implemented , this allocates to the various models Pseudo ocde is
        ///     provided.</remarks>
        ///
        /// <param name="Output"> The output.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract BasinWater Allocate(ResourceModelOutput Output, int year);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
        ///
        /// <remarks> This is intended for models where allocation is done outside of model and an array of values is needed
        ///           but river model results is maintained in the river model
        ///          This needs to be implemented , this allocates to the various models Pseudo ocde is provided</remarks>
        ///
        /// <param name="WaterSimCRFModels"> A list of watersim crf models.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract BasinWater Allocate(int year);
        //{
        //PSEUDO CODE of a general approach
        /*
        int index = 0;
        int[] AvailWater = new int[FregionsN];
        try
        {
          foreach (WaterSimCRFModel WSCRF in WaterSimCRFModels)
          {
              WSCRF.AResource = some value;
              AvailWater[indexer] = some value;
              indexer++
          }
          FAvailableWater.AvailableWater = AvailWater; 
          return true;
        }
        catch
        {
          //Can look at FAvailable water to see where error occured
          FAvailableWater.AvailableWater = AvailWater; 
          return false;
        }
        */
        //return false;
        //}


        public int DroughtActiveLF { get => FDroughtActiveLF; set => FDroughtActiveLF = value; }

        public int DroughtStartYearLF { get => FDroughtStartYearLF; set => FDroughtStartYearLF = value; }

        public int DroughtLengthLF { get => FDroughtLengthLF; set => FDroughtLengthLF = value; }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought depth.</summary>
        /// <remarks> an integer representing an integer expression of % drought to apply.
        ///           100 represent 100% of resource is available, ie no drought
        ///           50 represents 50% of resource is available
        ///           </remarks>
        /// <value> The drought depth adjustment</value>
        ///-------------------------------------------------------------------------------------------------
        public double DroughtDepthLF { get => FDroughtDepthLF; set => FDroughtDepthLF = value; }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the owner.</summary>
        ///
        /// <value> The owner a WaterSimModel object</value>
        ///-------------------------------------------------------------------------------------------------

        public WaterSimModel Owner
        {
            get { return FOwner; }
            set { FOwner = value; }
        }
        //
        /// <summary>
        /// 
        /// </summary>
        public int[] Fregion = { 12, 18 };
        //// Drought and Climate Change controls?
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentYear"></param>
        public void COdroughtAtLeeFerry(int currentYear)
        { // 12 is Utah in Basin, 18 is Colorado in Basin
             DroughtActiveLF = Math.Max(Owner.DroughtActive[12], Owner.DroughtActive[18]);

            double COdroughtEffect = 1;
            if (DroughtActiveLF > 0)
            {
                double sumLFdrought = 0;

                foreach (int reg in Fregion)
                {
                    DroughtStartYearLF = Owner.DroughtStartYear[reg];
                    if (DroughtStartYearLF <= currentYear)
                    {
                        DroughtStartYearLF = Owner.DroughtStartYear[reg];
                        DroughtDepthLF = Owner.DroughtDepth[reg]/100;
                        DroughtLengthLF = Owner.DroughtLength[reg];

                        int year = Owner.FastUnitModel(reg).currentYear; // FSURInput.CurrentYear;
                        int endYear = Owner.FastUnitModel(reg).endYear; // FSURInput.EndYear;
                        int startYear = Owner.FastUnitModel(reg).startYear; // FSURInput.StartYear;

                        int period = currentYear - DroughtStartYearLF; // FDroughtStartYear;
                        int StartPeriod = DroughtStartYearLF - WSMstartYear;
                        int SimulationYears = WSMstopYear - startYear;
                        double DroughtEffect = Owner.FastUnitModel(reg).DroughtFunction(period, StartPeriod, SimulationYears, DroughtLengthLF, DroughtDepthLF);
                        sumLFdrought += DroughtEffect;
                     }
                 }
                if (DroughtStartYearLF <= currentYear) { COdroughtEffect = sumLFdrought / Fregion.Length; }

            }
            Owner.ColoradoRiverModel.DroughtManagerForCOriverAtLeeFerry = COdroughtEffect;
        }
        /// <summary>
        /// 
        /// </summary>
        public int WSMstartYear { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int WSMstopYear { get; set; }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A colorado river input data.</summary>
    ///
    /// <seealso cref="WaterSimDCDC.Generic.ResourceModelInput"/>
    ///-------------------------------------------------------------------------------------------------

    public class ColoradoRiverInputData : ResourceModelInput
    {
        readonly double[] FTraceData;
        public ColoradoRiverInputData(double[] TheTraceData)
        {
            FTraceData = TheTraceData;

        }
    }
    // 09.17.20 das




    // //////////////////////////////////////////////////////////////////////////////////////
    // 
    //   SURFACE WATER MODEL CLASS AND SUPPORT CLASSES 
    // 
    // //////////////////////////////////////////////////////////////////////////////////////


    ///-------------------------------------------------------------------------------------------------
    /// <summary> A surface water input.</summary>
    ///
    /// <remarks> Use with Surfacewater Model.</remarks>
    ///
    /// <seealso cref="WaterSimDCDC.Generic.ResourceModelInput"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterInput : ResourceModelInput
    {
        // an integer representing an integer expression of % drought to apply
        // Initial Surface Water Resource
        BasinWater FInitial = null;
        // Year Variables
        int FYears = 0;
        int FEndYear = 0;
        int FCurrentYear = 0;
        int FStartYear = 0;
        // the Goal for Surface water based on policy control or policy test
        double[] FSurfaceGoal;  // 1 = 100% is default - This could change from year to year
        // the CLimate Change Target =
        double[] FClimateChangeTarget;  // 1 = 100% default - This could change from year to year
        // the Drought Factors - These can change from year to year
        int[] FDroughtActive; // 0 = default off
        int[] FDroughtStartYear; // 2015 = default
        int[] FDroughtLength; // 30 = default
        int[] FDroughtDepth; // 20 = default


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterInput
        ///               class.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterInput()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterInput
        ///     class.</summary>
        /// <param name="TheModels"> the models.</param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterInput(int[] InitialResource)
        {
            FInitial = new BasinWater(InitialResource);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought active.</summary>
        ///
        /// <value> The drought active.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] DroughtActive
        {
            get { return FDroughtActive; }
            set { FDroughtActive = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the surface goal.</summary>
        /// <remarks>     // 100% is default - This could change from year to year
        /// </remarks>
        /// <value> The surface goal.</value>
        ///-------------------------------------------------------------------------------------------------

        public double[] SurfaceGoal
        {
            get { return FSurfaceGoal; }
            set { FSurfaceGoal = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the climate change target.</summary>
        /// <remarks> an integer representing an integer expression of % climate changet to apply.
        ///           100 represent 100% of resource is available, ie no climate change
        ///           50 represents 50% of resource is available
        ///           </remarks>
        /// <value> The climate change target.</value>
        ///-------------------------------------------------------------------------------------------------

        public double[] ClimateChangeTarget
        {
            get { return FClimateChangeTarget; }
            set { FClimateChangeTarget = value; }
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the initial water for a surface model water run.</summary>
        ///
        /// <value> The initial water.</value>
        ///-------------------------------------------------------------------------------------------------

        public BasinWater InitialWater
        {
            get { return FInitial; }
            set { FInitial = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the numbers of years (lenght in years) of the model scenario.</summary>
        ///
        /// <value> The years of scenario.</value>
        ///-------------------------------------------------------------------------------------------------

        public int YearsOfScenario
        {
            get { return FYears; }
            set { FYears = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought start year.</summary>
        ///
        /// <value> The drought start year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] DroughtStartYear { get => FDroughtStartYear; set => FDroughtStartYear = value; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought length.</summary>
        ///
        /// <value> The length of the drought.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] DroughtLength { get => FDroughtLength; set => FDroughtLength = value; }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought depth.</summary>
        /// <remarks> an integer representing an integer expression of % drought to apply.
        ///           100 represent 100% of resource is available, ie no drought
        ///           50 represents 50% of resource is available
        ///           </remarks>
        /// <value> The drought depth adjustment</value>
        ///-------------------------------------------------------------------------------------------------
        public int[] DroughtDepth { get => FDroughtDepth; set => FDroughtDepth = value; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the end year.</summary>
        /// <remarks> Actual Year value , not index with first year 0</remarks>
        /// <value> The end year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int EndYear
        {
            get { return FEndYear; }
            set { 
                    FEndYear = value;
                    if (FStartYear!=0)
                    { 
                      FYears = (FEndYear - FStartYear)+1;
                    }
                }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the current year.</summary>
        /// <remarks> Actual Year value , not index with first year 0</remarks>
        /// <value> The current year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int CurrentYear { get => FCurrentYear; set => FCurrentYear = value; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the start year.</summary>
        /// <remarks> Actual Year value , not index with first year 0</remarks>
        /// <value> The start year.</value>
        ///-------------------------------------------------------------------------------------------------

        public int StartYear
        {
            get { return FStartYear; }
            set
            {
                FStartYear = value;
                if (FEndYear !=0)
                { 
                    FYears = (FEndYear - FStartYear) + 1;
                }
            }
        }

    }

    //--------------------------------------------------------------\
     
     
    //=================================================================
    // SURFACE WATER OUTPUT
    //================================================================
    
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A surface water output.</summary>
    ///
    /// <remarks> Quay, 6/19/2020.</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModelOutput"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterOutput : ResourceModelOutput
    {
        // Amount of Surface water available by region by year
        // [year][region] 
        //int[][] FSurfaceWaterResults = null;

        // list of region results, one for each year
        List<int[]> FSurfaceWaterResults = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterOutput
        ///     class.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterOutput()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterOutput
        ///     class.</summary>
        /// 
        /// <param name="years">               The years.</param>
        /// <param name="NumberOfRegions">     Number of regions.</param>
        /// <param name="InitialSurfaceWater"> The total number of surface water.</param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterOutput(int NumberOfRegions)
        {
            // Create the output results array by year and region, empty at this point
            FSurfaceWaterResults = new List<int[]>();
        }
       
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the surface water results.</summary>
        /// <remarks>  Array index is [year][region]</remarks>
        /// <value> The surface water results.</value>
        ///-------------------------------------------------------------------------------------------------

        public List<int[]> SurfaceWaterResults
        {
            get { return FSurfaceWaterResults;  }
            set { FSurfaceWaterResults = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Annual surfeacewater.</summary>
        /// <param name="year"> The year.</param>
        ///<remarks>  If Annual Surfacewater has not yet be created or assigned, will return a null
        ///           if index is out of range, will return a null</remarks>
        /// <returns> An int[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public int[] AnnualSurfeacewater(int year)
        {
            // check if has been created and or assigned data
            if (FSurfaceWaterResults != null)
            {
                //checked if index is within range
                if ((year > -1) && (year < FSurfaceWaterResults.Count))
                {
                    return (FSurfaceWaterResults[year]);
                }
                // else reurn a null value
                else
                {
                    return null;
                }
            }
            // else return a null value
            else
            {
                return null;
            }
        }



    }


    ///-------------------------------------------------------------------------------------------------
    /// <summary> A Model for surfacewater (non colorado).</summary>
    ///
    /// <remarks>  This model basically uses USGS surfacewater other data as the base for surface water</remarks>
    /// 
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModel"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceModel : BasinRiverModel
    {
        // The USGS Data in a UnitData Class
        UnitData FUSGSData = null;
        // number of regions
        readonly int FRegionNumber = 0;
        // model INput Object
        
        SurfaceWaterOutput FSUROutput;
        SurfaceWaterInput FSURInput;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.Generic.SurfaceModel class.</summary>
        /// <remarks> Intialize the Initial water by region array in the ModelInput, DOES NOT initialize
        ///           the output structure, or years, or drought parameters</remarks>
        /// <param name="TheBaseUSGSData"> Information describing the base usgs.</param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceModel( UnitData TheBaseUSGSData): base()
        {
            FUSGSData = TheBaseUSGSData;
            if (FUSGSData!=null)
            {
                FRegionNumber = FUSGSData.UnitCount;
            }
            if (FRegionNumber > 0)
            {

                // OK get the orignal USGS water by region and assign to initial water
                // Setitup
                //string ErrMsg = "";
                FSURInput = new SurfaceWaterInput();
                int[] TempBasinWater = new int[FRegionNumber];
                // OK fetch it, even if it fails set it to TempBasinWater
                FUSGSData.GetIntValues(UDI.SurfaceWaterFld, out TempBasinWater, out string ErrMsg);
                FSURInput.InitialWater = new BasinWater(TempBasinWater);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Resetoutputs this WaterSimDCDC.Generic.SurfaceModel.</summary>
        ///
        /// <remarks> Quay, 9/9/2020.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public void resetoutput()
        {
            // Create new OuputObaject
            FSUROutput = new SurfaceWaterOutput(FRegionNumber);
            // If FUSRInput is not null Make first year of results the Initial water settings
            if (FSURInput != null)
            {
                FSUROutput.SurfaceWaterResults.Add(FSURInput.InitialWater.AvailableWater);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ResetModel()
        {
            // Reset Output object
            resetoutput();
        }

  
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the River Model with the given input data.</summary>
        ///
        /// <remarks> NOT IMPLEMNETED</remarks>remarks>
        ///
        /// <param name="InputData"> Information describing the input.</param>
        ///
        /// <returns> A RiverModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput Run(ResourceModelInput InputData, out int errCode , out string errStr)
        {

            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        /// <remarks> NOT IMPLEMNETED</remarks>remarks>
        /// <param name="Input">   The ResourceModel input data.</param>
        /// <param name="year">    The year as 0 being the first year.</param>
        /// <param name="endyear"> The endyear.</param>
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput RunYear(ResourceModelInput Input, int year, int endyear, out int error, out string ErrStr)
        {
            throw new NotImplementedException();
        }
        //
         ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        /// <remarks> NOT IMPLEMNETED</remarks>remarks>
        /// <remarks> Intended for use on models that keep the ResourceModelInput locally in the model.</remarks>
        ///
        /// <param name="year">    The year as 0 being the first year.</param>
        /// <param name="endyear"> The endyear.</param>
        /// <param name="error">   [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr">  [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override ResourceModelOutput RunYear(int year, int endyear, out int error, out string ErrStr)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Executes a model run for one year as specified.</summary>
        ///
        /// <remarks> Intended for use on models that keep the ResourceModelInput locally in the model.</remarks>
        ///
        /// <param name="error">  [out] The error code, 0 = no error.</param>
        /// <param name="ErrStr"> [out] The error string.</param>
        ///
        /// <returns> A ResourceModelOutput object with model results.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool RunYear(out int error, out string ErrStr)
        {
            bool result = false;
            error = 0;
            ErrStr = "";
            // loop through each regions data and forecast
            int[] ModelResults = new int[FRegionNumber];
            for(int regi = 0;regi<FRegionNumber;regi++)
            {
                // setup params
                double InitialSurfacewater = FSURInput.InitialWater.RegionResource(regi);
                double SurfaceGoal = Owner.FastUnitModel(regi).geti_SurfaceWaterControl() / 100;
                double ClimateChangeTarget = Owner.FastUnitModel(regi).geti_ClimateChangeTarget() / 100; // FSURInput.ClimateChangeTarget[regi];
                int DroughtActive = Owner.FastUnitModel(regi).geti_DroughtActive(); // FSURInput.DroughtActive[regi];
                int DroughtStartYear = Owner.FastUnitModel(regi).geti_DroughtSartYear(); // FSURInput.DroughtStartYear[regi];
                int DroughtLength = Owner.FastUnitModel(regi).geti_DroughtLength(); // FSURInput.DroughtLength[regi];
                int DroughtDepth = Owner.FastUnitModel(regi).geti_DroughtDepth(); // FSURInput.DroughtDepth[regi];
                int year = Owner.FastUnitModel(regi).currentYear; // FSURInput.CurrentYear;
                int endYear = Owner.FastUnitModel(regi).endYear; // FSURInput.EndYear;
                int startYear = Owner.FastUnitModel(regi).startYear; // FSURInput.StartYear;
                int Forecast = ForecastSurface(year, endYear, startYear, SurfaceGoal, InitialSurfacewater, ClimateChangeTarget,
                                                    DroughtActive, DroughtStartYear, DroughtLength, DroughtDepth);
                int YearIndex = year - DroughtStartYear;
                //
               

                //
                ModelResults[regi] = Forecast;

            }
            FSUROutput.SurfaceWaterResults.Add(ModelResults);

            return result;
        }
        //
        /// <summary>
        ///  Place holder
        /// </summary>
        /// <param name="year"></param>
        /// <param name="error"></param>
        /// <param name="ErrStr"></param>
        /// <returns></returns>
        public override bool RunYear(int year, out int error, out string ErrStr)
        {
            bool result = false;
            error = 0;
            ErrStr = "";
            return result;
        }
    
            ///-------------------------------------------------------------------------------------------------
            /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
            ///
            /// <remarks> Intended for models that need river model out put passed to it, such as a sperate
            ///     model
            ///            does allocation
            ///     This needs to be implemented , this allocates to the various models Pseudo ocde is
            ///     provided.</remarks>
            ///
            /// <param name="Output"> The output.</param>
            ///
            /// <returns> True if it succeeds, false if it fails.</returns>
            ///-------------------------------------------------------------------------------------------------

        public override BasinWater Allocate(ResourceModelOutput Output, int year)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates results of model run.</summary>
        ///
        /// <remarks> Allocates results based on last model run, if first year, uses initial values.</remarks>
        ///
        /// <param name="year"> The year.</param>
        ///
        /// <returns> A BasinWater.</returns>
        ///-------------------------------------------------------------------------------------------------

        
        public override BasinWater Allocate(int year)
        {
            // Set up for allocation
            BasinWater NewAllocation;
            // Check if firt year, if so, use the Initail Resource values
            if (year == 0)
            {
                NewAllocation = FSURInput.InitialWater;
            }
            // Otherwise, use the results from the previous year model run
            else
            {
                NewAllocation = new BasinWater(FSUROutput.SurfaceWaterResults[year-1]);
            }
            return NewAllocation;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Annual exponential change.</summary>
        /// <param name="StartValue">      The start value.</param>
        /// <param name="period">          The period.</param>
        /// <param name="ExponentialCoef"> The exponential coef.</param>
        /// <param name="Limit">           The limit.</param>
        ///
        /// <returns> A double.</returns>
        ///-------------------------------------------------------------------------------------------------

        double AnnualExponentialChange(double StartValue, double period, double ExponentialCoef, double Limit)
        {
            double result = 1;
            double Temp1 = (StartValue - Limit);
            double Temp2 = (-1 * ExponentialCoef) * period;
            double Temp3 = Math.Exp(Temp2);
            double Temp4 = Temp3 * Temp1;
            result = Limit + ((StartValue - Limit) * Math.Exp((-1 * ExponentialCoef) * period));
            return result;
        }

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

       
        //=====================================================================
        // SURFACE WATER ANNUAL FORECAST

        // The min and max changes allowed for surface water
        const double MaxSurfaceChange = 2.0;
        const double MinSurfaceChange = 0.10;
        // The min and max changes allowed for climnate change
        const double MaxCCChange = 2.0;
        const double MinCCChange = 0.10;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Forecast surface.</summary>
        ///
        /// <remarks> These years are actual years, not index with start year 0;</remarks>
        ///
        /// <param name="currentYear">         The current year.</param>
        /// <param name="endYear">             The end year.</param>
        /// <param name="startYear">           The start year.</param>
        /// <param name="TheSurfaceGoal">      the surface goal.</param>
        /// <param name="InitialSurfaceWater"> The initial surface water.</param>
        /// <param name="ClimateChangeTarget"> The climate change target.</param>
        /// <param name="DroughtActive">       The drought active.</param>
        /// <param name="DroughtStartYear">    The drought start year.</param>
        /// <param name="DroughtLength">       Length of the drought.</param>
        /// <param name="DroughtDepth">        Depth of the drought.</param>
        ///
        /// <returns> An int.</returns>
        ///-------------------------------------------------------------------------------------------------
        /// 09.16.20 das 
        //int ForecastSurface(int currentYear, int endYear, int startYear, double TheSurfaceGoal, double InitialSurfaceWater, double ClimateChangeTarget,
         //                 int DroughtActive, int DroughtStartYear, int DroughtLength, int DroughtDepth)

        public int ForecastSurface(int currentYear, int endYear, int startYear, double TheSurfaceGoal, double InitialSurfaceWater, double ClimateChangeTarget,
                          int DroughtActive, int DroughtStartYear, int DroughtLength, int DroughtDepth)
        {
            // Calculate the ChangeCoefficient for Surface water to use with Annual Exponential change
            double YearsToTarget = endYear;
            double SurfaceChangeLimit = 0.0;
            if (TheSurfaceGoal < 1)
            {
                SurfaceChangeLimit = MinSurfaceChange;
            }
            else
            {
                SurfaceChangeLimit = MaxSurfaceChange;
            }
            double FSurfaceChangeCoefficient = utilities.ExponentialDecayCoef(TheSurfaceGoal, 1, SurfaceChangeLimit, 0, YearsToTarget);
            
            
            double NewSurfaceLimit = 0;
            // Setup Result
            int result = 0;
 
            // This is the cumulative effect variable
            double tempBase = 0;
            // This is how far into the simulation we are
            int SurfacePeriod = currentYear - startYear;

            // This is this the resource base change management
            // Calculate the cumulative annual change factor for this period
            double annualFactor = AnnualExponentialChange(1, SurfacePeriod, FSurfaceChangeCoefficient, SurfaceChangeLimit);
            // calculate the change in resource
            tempBase = annualFactor * InitialSurfaceWater;


            // This one is the climate change Management
            // Climate Change is affects the values after the surface management has been applied
            // 
            if (ClimateChangeTarget != 1.0)
            {
                double FCCChangeLimit = 0.0;
                if (ClimateChangeTarget < 1)
                {
                    FCCChangeLimit = MinCCChange;
                }
                else
                {
                    FCCChangeLimit = MaxCCChange;
                }
                double FCCChangeCoefficient = utilities.ExponentialDecayCoef(ClimateChangeTarget, 1, FCCChangeLimit, 0, YearsToTarget);
                double CCannualFactor = AnnualExponentialChange(1, SurfacePeriod, FCCChangeCoefficient, FCCChangeLimit);
                tempBase = tempBase * CCannualFactor;
            }

            // THis is the new drought management
            // drought is applied on top of Climate Change and Surface ater control.
            if (DroughtActive > 0)
            {
                if (DroughtStartYear <= currentYear)
                {
                    int period = currentYear - DroughtStartYear;
                    int StartPeriod = DroughtStartYear - startYear;
                    int SimulationYears = endYear - startYear;
                    double DroughtEffect = DroughtFunction(period, StartPeriod, SimulationYears, DroughtLength, DroughtDepth);
                    tempBase = tempBase * DroughtEffect;
                }
            }
            // OK, after cascading imapcts, finally set the new surface water limit
            NewSurfaceLimit = tempBase;
            // Return the forecast
            result = Convert.ToInt32(NewSurfaceLimit);
            return result;
        }

        SurfaceWaterInput ModelInput
        {
            get { return FSURInput; }
            // lets no do this yet
            //set { FSURInput = value; }
        }
    }
}
