// /////////////////////////////////////////////////////////////////////////
// Colroado Basin Specific River Models
//
// This uses classes from "RiverModelClass_Prototype" namespace code
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
using RiverModelClass_Prototype;
using WaterSimDCDC.Generic;

namespace WaterSimDCDC.WestRiverModels
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A data Model for CO Basin Rivers</summary>
    ///
    /// <remarks> Quay, 6/19/2020.</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModel"/>
    ///-------------------------------------------------------------------------------------------------

    public class WestRiverModel : RiverModel
    {
        // Local Variables
        // Number of Regions
        int FRegionsN = 0;
        // Water Available by Region
        BasinRegionWater FAvailableWater;
        // Model Output
        RiverModelOutput FModelOutput;
        // Model Input
        RiverModelInput FModelInput;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.WestRiverModel
        ///     class.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public WestRiverModel()
        {

        }

        public WestRiverModel(int NumberofRegions)
        {
            FRegionsN = NumberofRegions;
            FAvailableWater = new BasinRegionWater(FRegionsN);
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the River Model with the given input data.</summary>
        ///
        /// <remarks> This needs to be implemented, Output and Input should be assined to Properties for retreival</remarks>
        ///
        /// <param name="InputData"> Information describing the input.</param>
        ///
        /// <returns> A RiverModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override RiverModelOutput Run(RiverModelInput InputData)
        {
            throw new NotImplementedException();
             
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocates this models output to the various regions managed by WaterSimModel.</summary>
        ///
        /// <remarks>This needs to be implemented , this allocates to the various models Pseudo ocde is provided</remarks>
        ///
        /// <param name="WaterSimCRFModels"> A list of watersim crf models.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllocateToRegionModels(List<WaterSimCRFModel> WaterSimCRFModels)
        {
            throw new NotImplementedException();
            /* PSEUDO CODE of a general approach
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
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A basin region available water.</summary>
    ///
    /// <remarks> This is a general int array used to hold available water by Basin region</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RegionAvailableWater"/>
    ///-------------------------------------------------------------------------------------------------

    public class BasinRegionWater : RegionAvailableWater
    {
        int[] FBasinWater = null;
        public BasinRegionWater(int NumberOfRegions)
        {
            if (NumberOfRegions >= 0)
            {
                FBasinWater = new int[NumberOfRegions];
            }
        }
        
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the available water array.</summary>
        ///
        /// <value> The available water as an int array.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] AvailableWater
        {
            get { return FBasinWater; }
            set { FBasinWater = value; }
        }
    }

    //-------------------------------------------------------------------------------------------------
    // //////////////////////////////////////////////////////
    //      SURFACE WATER (non colorado) River Model Simply uses the surface water data column from
    //      the USGS data file as the base for allocation of surface water.  Uses an input data
    //      structure that allows passing of a % reduction for each region and an ouput structure
    //      that includes result available water by region
    //      ////////////////////////////////////////////////////.

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Input Class for yje surface water model.</summary>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModelInput"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterInput : RiverModelInput
    {
        // an integer representing an integer expression of % drought to apply
        int FDroughtAdjust = 100;
        // The LIst of WaterSimCRFModels
        List<WaterSimCRFModel> FWSCRFModels = null;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterInput
        ///     class.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterInput ()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.WestRiverModels.SurfaceWaterInput
        ///     class.</summary>
        /// <param name="TheModels"> the models.</param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceWaterInput(List<WaterSimCRFModel> aModelList)
        {
            FWSCRFModels = aModelList;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the drought adjust.</summary>
        /// <remarks> an integer representing an integer expression of % drought to apply.
        ///           100 represent 100% of resource is available, ie no drought
        ///           50 represents 50% of resource is available
        ///           </remarks>
        /// <value> The drought adjust.</value>
        ///-------------------------------------------------------------------------------------------------

        public int DroughtAdjust
        {
            get { return FDroughtAdjust; }
            set { FDroughtAdjust = value; }
        }


        public List<WaterSimCRFModel> TheModels
        {
            get { return FWSCRFModels; }
            set { FWSCRFModels = value; }
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A surface water output.</summary>
    ///
    /// <remarks> Quay, 6/19/2020.</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModelOutput"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceWaterOutput : RiverModelOutput
    {
        // Total amount of Surface water available 
        int FTotalSurfaceWater = 0;
        // Amount of Surface water available by region 
        int[] FRegionSurfaceWater = null;


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
        /// <summary> Gets or sets the total amount of surface water.</summary>
        ///
        /// <value> The total number of surface water.</value>
        ///-------------------------------------------------------------------------------------------------

        public int TotalSurfaceWater
        {
            get { return FTotalSurfaceWater; }
            set { FTotalSurfaceWater = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the region surfeacewater.</summary>
        ///
        /// <value> The region surfeacewater.</value>
        ///-------------------------------------------------------------------------------------------------

        public int[] RegionSurfeacewater
        {
            get { return FRegionSurfaceWater; }
            set { FRegionSurfaceWater = value;  }
        }


    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A River Model for surfacewater (non colorado).</summary>
    ///
    /// <remarks>  This model basically uses USGS surfacewater other data as the base for surface water</remarks>
    ///
    /// <seealso cref="T:RiverModelClass_Prototype.RiverModel"/>
    ///-------------------------------------------------------------------------------------------------

    public class SurfaceModel : WestRiverModel
    {
        // The USGS Data in a UnitData Class
        UnitData FUSGSData = null;
        // number of regions
        int RegionNumber = 0;
        // a BasinRegion data structure for base surface water 
        BasinRegionWater FBaseSurfaceWater;
        SurfaceWaterOutput FTheOutputWater;
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the WaterSimDCDC.Generic.SurfaceModel class.</summary>
        /// <param name="TheBaseUSGSData"> Information describing the base usgs.</param>
        ///-------------------------------------------------------------------------------------------------

        public SurfaceModel( UnitData TheBaseUSGSData): base()
        {
            FUSGSData = TheBaseUSGSData;
            if (FUSGSData!=null)
            {
                RegionNumber = FUSGSData.UnitCount;
            }
            if (RegionNumber > 0)
            {
                // OK get the orignal USGS water by region
                FBaseSurfaceWater = new BasinRegionWater(RegionNumber);
                FBaseSurfaceWater.AvailableWater = FUSGSData.BaseUnitData(UDI.SurfaceWaterFld);
                // OK create the output water
                FTheOutputWater = new SurfaceWaterOutput();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Runs the River Model with the given input data.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///
        /// <param name="InputData"> Information describing the input.</param>
        ///
        /// <returns> A RiverModelOutput.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override RiverModelOutput Run(RiverModelInput InputData)
        {
            SurfaceWaterOutput FSurfaceWater = null;
            // Check if inpout is right
            if (InputData is SurfaceWaterInput)
            {
                double TotalSurface = 0;
                foreach (int Value in FBaseSurfaceWater.AvailableWater)
                {
                    TotalSurface += (double)Value;
                }
                double DroughtValue = (double)(InputData as SurfaceWaterInput).DroughtAdjust / 100;
                double TempSurface = TotalSurface * DroughtValue;
                FSurfaceWater.TotalSurfaceWater = Convert.ToInt32(TempSurface);
            }
            return FSurfaceWater;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Allocate to region models.</summary>
        ///
        /// <remarks> Quay, 6/19/2020.</remarks>
        ///
        /// <param name="WaterSimCRFModels"> The water simulation crf models.</param>
        ///
        /// <returns> True if it succeeds, false if it fails.</returns>
        ///-------------------------------------------------------------------------------------------------

        public override  bool AllocateToRegionModels(List<WaterSimCRFModel> WaterSimCRFModels)
        {
            bool result = false;
            return result;
        }



    }
}
