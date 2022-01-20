﻿using System;
using System.Collections.Generic;
//using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ConsumerResourceModelFramework;


// /////////////////////////////////////////////////////////////////
// Notes
// Version 6.02
// 
// Quay 9/1/18
// Added West_CRF_UnitNetwork to add colorado
// Also Made buildresources a virtual method to overide
// Quay 4/28/20
// Moved to general source directory with link to project
// 

namespace WaterSimDCDC.Generic
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf resource smith. </summary>
    ///
    /// <remarks>   Mcquay, 1/25/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_Water : CRF_Resource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Water()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Water(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Water(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Water(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }
        //-----------------------

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        ///
        /// <remarks>
        /// This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux
        /// TO a certiain class of dataitem.
        /// </remarks>
        ///
        /// <param name="FluxToItem">   The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool allowChange = false;
            if (FluxToItem is CRF_Consumer)
            {
                if ((FluxToItem as CRF_Consumer).Net > 0)
                {
                    allowChange = true;
                }
            }
            return allowChange;
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf resource smith SurfaceFresh. </summary>
    ///
    /// <remarks>   Mcquay, 1/25/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_SurfaceFresh : CRF_Resource_Water
    {


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceFresh()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceFresh(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceFresh(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceFresh(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            return true;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;//false;
        }
    }

    public class CRF_Resource_SurfaceLake : CRF_Resource_Water
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceLake()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceLake(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceLake(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceLake(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            return true;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
    }
    //=======================================================================================================
    ///-------------------------------------------------------------------------------------------------
    /// <summary>  A crf resource surface saline. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_SurfaceSaline : CRF_Resource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceSaline()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceSaline(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceSaline(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_SurfaceSaline(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool result = true;
            if (FluxToItem is CRF_Consumer_Agriculture)
            {
                return false;
            }
            //return true;
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
        //public override bool AllowUnlimitedFluxFrom(CRF_DataItem FluxFromItem)
        //{
        //    return true;
        //}

    }

    //=======================================================================================================
    //=======================================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>  A crf resource augmented. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_Augmented : CRF_Resource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Augmented()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Augmented(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Augmented(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Augmented(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }

        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool result = true;
            if (FluxToItem is CRF_Consumer_Agriculture)
            {
                return false;
            }
            //return true;
            return result;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>  A crf resource groundwater. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_Groundwater : CRF_Resource
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Groundwater()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Groundwater(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Groundwater(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Groundwater(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FluxToItem"></param>
        /// <returns></returns>
        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            return true;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
    }

    //===============================================================================
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf resource effluent. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_Effluent : CRF_Resource_Water
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Effluent()
            : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Effluent(string aName)
            : base(aName)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Effluent(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">            The name. </param>
        /// <param name="aLabel">           The label. </param>
        /// <param name="aColor">           The color. </param>
        /// <param name="AvailableSupply">  The available supply. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Effluent(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FluxToItem"></param>
        /// <returns></returns>
        public override bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool result = true;

            return result;

            // always allocate
            // return true;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   Does not allow unused resources to be sent back</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            // can not return resources to effluent source
            return false;
        }
        //
        // Change the available reclained water available to a region. Currently, read in from the file. We want to be 
        // able to ask "what if" regions begin using effluent/ reclaimed water that they are NOT NOW using.
        // edits 11.29.21 das
        public void AddEffluentCapabilities(double additions, double defaultAvailable)
        {

        }


        // end edits 11.29.21 das

        //
        /// Ad code here from  protected virtual void ResetLimit(double NewLimit) in ConsumerResourceData (line 1394)
        /// 
        protected override void ResetLimit(double NewLimit)
        {
            // check of NewLimit is larger or smaller
            //if (NewLimit > FValue)
            if (NewLimit > FValue)
            {
                // OK, need to adjust each flux so original new allocated values stays the same as old
                // Set Allocation will adjust the Allocated value of flux based on method of allocation being used 
                // get the oldvalues
                List<double> OldValues = new List<double>();
                foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                {
                    OldValues.Add(Flux.Allocated());
                }
                // set the new limit
                FValue = NewLimit;

                // loop through the fluxes and set values
                int index = 0;
                foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                {
                    // Check if a flux transfer is allowed, base CRF Resource should say no unless their is a need
                    // if not, then reset value to old value
                    if (!CRF_Utility.AllowFluxChange(this, Flux.Target))
                    {
                        Flux.SetAllocation(OldValues[index]);
                        index++;
                    }
                }

            }
            else
            {
                if (NewLimit == FValue) { }
                else
                {
                    //List<double> OldValues = new List<double>();
                    //foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                    //{
                    //    OldValues.Add(Flux.Allocated());
                    //}
                    // set the new limit
                    FValue = NewLimit;
                    List<double> OldValues = new List<double>();
                    List<double> NewValues = new List<double>();
                    foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                    {
                        if (Flux.Source.Label == "Recycled Water")
                        {
                            OldValues.Add(Flux.Allocated());
                            //if(OldValues)
                            Flux.SetAllocation(FValue);
                            NewValues.Add(Flux.Allocated());
                        };
                        // NewValues.Add(Flux.Allocated());
                    }

                    // loop through the fluxes and set values
                    int index = 0;
                    foreach (CRF_Flux Flux in ToFluxs) //FFluxList)
                    {
                        // Check if a flux transfer is allowed, base CRF Resource should say no unless their is a need
                        // if not, then reset value to old value
                        if (!CRF_Utility.AllowFluxChange(this, Flux.Target))
                        {
                            //Flux.SetAllocation(OldValues[index]);
                            Flux.SetAllocation(NewValues[index]);
                            index++;
                        }
                    }
                }
                //bool stop = true;
            }

            // ok just do it if less everyone lives with consequences, if ratio not reset, they get alot more
            FValue = NewLimit;


            //
        }
    }
        // ==================================================================================================================
      
        // ==================================================================================================================

        //=====================================================================================
        class CRF_Consumer_Urban : CRF_Consumer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Urban(string aName)
            : base(aName)
        {
            FMStyle = CRF_Utility.ManagementStyle.msExpand;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Urban(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            FMStyle = CRF_Utility.ManagementStyle.msExpand;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Urban(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor, Demand)
        {
            FMStyle = CRF_Utility.ManagementStyle.msExpand;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux TO a certiain class of dataitem</remarks>
        /// <param name="FluxToItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            //bool result = false;
            bool result = true;
            // if it is groundwater then if I have extra, then use return it
            //if (FluxToItem is CRF_Resource_Groundwater)
            //{
            //    // negative for consumer means more than I can use
            //    if (Net < 0)
            //    {
            //        result = true;
            //    }
            //}
            //else
            //{
            //    switch (FMStyle)
            //    {
            //        // Sure if I have extra,
            //        case CRF_Utility.ManagementStyle.msCooperate:
            //            // negative for consumer means more than I can use
            //            if (Net < 0)
            //            {
            //                result = true;
            //            }
            //            break;
            //        // No keeping what I have
            //        case CRF_Utility.ManagementStyle.msProtect:
            //        case CRF_Utility.ManagementStyle.msExpand:
            //            result = false;
            //            break;
            //    }

            //    // otherwise, jsut say no
            //}
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux FROM a certiain class of dataitem</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            bool result = true;
            //bool result = false;
            //switch (FMStyle)
            //{
            //    // only if I need it
            //    case CRF_Utility.ManagementStyle.msCooperate:
            //    case CRF_Utility.ManagementStyle.msProtect:
            //        // negative cosumer means I have excess
            //        // not if already have more tha I can use
            //        if (Net > 0)
            //        {
            //            result = true;
            //        }
            //        break;
            //    // all I can get
            //    case CRF_Utility.ManagementStyle.msExpand:
            //        result = true;
            //        break;

            //    // otherwise no thankyou

            //}
            return result;
        }
        public virtual bool AllowUnlimitedFluxFrom(CRF_DataItem FluxFromItem)
        {
            bool result = true;

            return result;
        }


    }

    //=====================================================================================
    class CRF_Consumer_Agriculture : CRF_Consumer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Agriculture(string aName)
            : base(aName)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Agriculture(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Agriculture(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor, Demand)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux TO a certiain class of dataitem</remarks>
        /// <param name="FluxItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangeTo(CRF_DataItem FluxToItem)
        {
            bool result = true;
            //bool result = false;
            //// I am a farmer, unless I am in a cooperative mood, I dod not care about groundwater
            //switch (FMStyle)
            //    {
            //        // Sure if I have extra,
            //        case CRF_Utility.ManagementStyle.msCooperate:
            //            // negative for consumer means more than I can use
            //          if (FluxToItem is CRF_Resource_Groundwater)
            //          {
            //              // negative for consumer means more than I can use
            //              if (Net < 0)
            //              {
            //                  result = true;
            //              }
            //           }
            //          break;
            //        // No keeping what I have
            //        case CRF_Utility.ManagementStyle.msProtect:
            //        case CRF_Utility.ManagementStyle.msExpand:
            //            result = false;
            //            break;
            //    }


            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux FROM a certiain class of dataitem</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            bool result = false;
            switch (FMStyle)
            {
                // only if I need it
                case CRF_Utility.ManagementStyle.msCooperate:
                case CRF_Utility.ManagementStyle.msProtect:
                    // negative cosumer means I have excess
                    // not if already have more tha I can use
                    if (Net > 0)
                    {
                        result = true;
                    }
                    break;
                // all I can get
                case CRF_Utility.ManagementStyle.msExpand:
                    result = true;
                    break;

                // otherwise no thankyou

            }
            return result;
        }
    }

    //=====================================================================================

    ///-------------------------------------------------------------------------------------------------
    /// <summary>  A crf consumer industrial. </summary>
    /// <remarks>  Same as Urban, but Cooperative </remarks>
    ///-------------------------------------------------------------------------------------------------

    class CRF_Consumer_Industrial : CRF_Consumer_Urban
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///  <remarks> </remarks>
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Industrial(string aName)
            : base(aName)
        {
            FMStyle = CRF_Utility.ManagementStyle.msCooperate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Industrial(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            FMStyle = CRF_Utility.ManagementStyle.msCooperate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Industrial(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor, Demand)
        {
            FMStyle = CRF_Utility.ManagementStyle.msCooperate;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux TO a certiain class of dataitem</remarks>
        /// <param name="FluxItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        // SAME AS URBAN
        // 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux FROM a certiain class of dataitem</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        // SAME AS URBAN

    }
    //=================================================================================
    ///-------------------------------------------------------------------------------------------------
    /// <summary>  A crf consumer Power. </summary>
    /// <remarks>  Same as Urban, but Protective </remarks>
    ///-------------------------------------------------------------------------------------------------

    class CRF_Consumer_Power : CRF_Consumer_Urban
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///  <remarks> </remarks>
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Power(string aName)
            : base(aName)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Power(string aName, string aLabel, Color aColor)
            : base(aName, aLabel, aColor)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="aName">    The name. </param>
        /// <param name="aLabel">   The label. </param>
        /// <param name="aColor">   The color. </param>
        /// <param name="Demand">   The Demand of this resource. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer_Power(string aName, string aLabel, Color aColor, double Demand)
            : base(aName, aLabel, aColor, Demand)
        {
            FMStyle = CRF_Utility.ManagementStyle.msProtect;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change flux. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux TO a certiain class of dataitem</remarks>
        /// <param name="FluxItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        // SAME AS URBAN
        // 
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine if we allow flux change from. </summary>
        /// <remarks>   This is enforce the DataItems rule about changing fluxes.  A class could deny altering a flux FROM a certiain class of dataitem</remarks>
        /// <param name="FluxFromItem"> The flux from item. </param>
        ///
        /// <returns>   true if we allow flux change from, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual bool AllowUnlimtedFluxChangeFrom(CRF_DataItem FluxFromItem)
        {
            return true;
        }
        // SAME AS URBAN

    }
    public class CRF_Other_Population : CRF_Other
    {


        public CRF_Other_Population(string aName)
            : base(aName)
        {
            FMStyle = CRF_Utility.ManagementStyle.msExpand;
        }
        public CRF_Other_Population(string aName, double Value)
            : base(aName, Value)
        {
            FMStyle = CRF_Utility.ManagementStyle.msExpand;
        }





    }

    //======================================================================
    // 
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf network water simulation america. </summary>
    ///
    /// <remarks>   Mcquay, 1/27/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Unit_Network : CRF_Network
    {
        // The Data
        protected UnitData FData;

        // Resources
        CRF_Resource_SurfaceFresh FSurfaceFresh;
        CRF_Resource_SurfaceLake FSurfaceLake;
        CRF_Resource_SurfaceSaline FSurfaceSaline;
        CRF_Resource_Groundwater FGroundwater;
        CRF_Resource_Effluent FEffluent;
        CRF_Resource_Augmented FAugmented;

        Color EffluentColor = Color.LightGray;
        Color SurfaceFreshColor = Color.LightBlue;
        Color SurfaceLakeColor = Color.Aqua;
        Color SurfaceSalineColor = Color.SeaGreen;
        Color GroundwaterColor = Color.Blue;
        Color AugmentedColor = Color.Coral;

        // Consumers
        CRF_Consumer_Urban FUrban;
        CRF_Consumer_Agriculture FAg;
        CRF_Consumer_Industrial FInd;
        CRF_Consumer_Power FPower;
        CRF_Consumer_Agriculture FRural;

        Color UrbanColor = Color.CadetBlue;
        Color AgColor = Color.DarkGreen;
        Color IndColor = Color.Brown;
        Color PowerColor = Color.LightSkyBlue;
        Color RuralColor = Color.LightCoral;
        //
        // Other
        CRF_Other_Population FPopulation;
        // This is the initial population
        int FInitialPopulation = UDI.BadIntValue;
        // This is the initial PowerGenerated value as read from the database
        int FInitialPowerGen = UDI.BadIntValue;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="Exception">    Thrown when exception. Unit Data not loadewd</exception>
        ///
        /// <param name="USGSDataFilename"> Filename of the usgs data file. </param>
        /// <param name="aCallback">        The callback. </param>
        /// <param name="InitialStateName"> Name of the initial state. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network(string USGSDataFilename, NetworkResetDelegate aCallback, string InitialStateName)
            : base()
        {

            FData = new UnitData(USGSDataFilename);
            if (!FData.DataLoaded)
            {
                //YIKES THIS is  AN ERROR
                throw (new Exception("USGS data file did not load"));
            }

            if (InitialStateName != "")
            {
                FName = InitialStateName;
            }
            else
            {
                FName = UniqueID.ToString();
            }
            FResources = new CRF_ResourceList();
            FResources.Owner = this;
            FConsumers = new CRF_ConsumerList();
            FConsumers.Owner = this;
            FOther = new CRF_OtherList();
            FOther.Owner = this;
            BuildResources(FName);
            BuildConsumers(FName);
            // 09.08.16 das
            BuildOther(FName);
            //
            AssignFluxes(FName);

            FCallback = aCallback;
            if (FCallback != null)
            {
                FCallback();
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="Exception">    Thrown when exception. UnitData Not loaded</exception>
        ///
        /// <param name="USGSDataFilename"> Filename of the usgs data file. </param>
        /// <param name="InitialStateName"> Name of the initial state. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network(string USGSDataFilename, string InitialStateName)
        {

            FData = new UnitData(USGSDataFilename);
            if (!FData.DataLoaded)
            {
                //YIKES THIS is  AN ERROR
                throw (new Exception("USGS data file did not load"));
            }

            if (InitialStateName != "")
            {
                FName = InitialStateName;
            }
            else
            {
                FName = UniqueID.ToString();  
            }

            FResources = new CRF_ResourceList();
            FResources.Owner = this;
            FConsumers = new CRF_ConsumerList();
            FConsumers.Owner = this;
            FOther = new CRF_OtherList();
            FOther.Owner = this;

            BuildResources(FName);
            BuildConsumers(FName);
            // 09.08.16 das
            BuildOther(FName);
            AssignFluxes(FName);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <exception cref="Exception">    Thrown when exception. Unitdata not loaded</exception>
        ///
        /// <param name="TheUnitData">     The Data (with datatable) being used to define each unit </param>
        /// <param name="InitialUnitName">  Name of the initial unit. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network(UnitData TheUnitData, string InitialUnitName)
        {

            FData = TheUnitData;
           // value = FData.GetValue(InitialUnitName, UnitData.FData.FldName);
            if (!FData.DataLoaded)
            {
                //YIKES THIS is  AN ERROR
                throw (new Exception("USGS data file did not load"));
            }

            if (InitialUnitName != "")
            {
                // Yikes this is also an erro
                FName = InitialUnitName;
            }
            else
            {
                FName = UniqueID.ToString();
            }
            // Ok use Unitdata to build the Resource, Consumer, and Other list
            FResources = new CRF_ResourceList();
            FResources.Owner = this;
            FConsumers = new CRF_ConsumerList();
            FConsumers.Owner = this;
            FOther = new CRF_OtherList();
            FOther.Owner = this;

            BuildResources(FName);
            BuildConsumers(FName);
            // 09.08.16 das
            BuildOther(FName);
            AssignFluxes(FName);


        }
       

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initial power generated. </summary>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        //public int InitialPowerGenerated(string State)
        public int InitialPowerGenerated
        {
            get {return FInitialPowerGen; }
            // Ouch should not be getting this from database, should not change once model is initilized, need to grab and save this
            //int Value = UDI.BadIntValue;
            //string errMessage = "";
            //if (FData.GetValue(State, UnitData.PowerGeneratedFieldname, out Value, out errMessage))
            //{

            //};
            //return Power;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the initial population.</summary>
        ///
        /// <value> The initial population.</value>
        ///-------------------------------------------------------------------------------------------------

        public int InitialPopulation
        {
            get { return FInitialPopulation; }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds Other resources. </summary>
        ///
        /// <param name="UnitName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------
        protected void BuildOther(string UnitName)
        {


            // Population
            int value = UDI.BadIntValue;
            double temp = 0;
            string errMsg = "";
            // Not Sure where these came from, left them in for now?
            double correctTo2015 = 1.055;
            double correctToUnits = 1000;

            string FldName = FData.OtherField(UDI.eOther.eoPopulation);

            if (FData.GetValue(UnitName, FldName, out value, out errMsg))
            {
                // adjust the population from file
                temp = value;
                  
                double revisedpop = temp * correctTo2015 * correctToUnits;
                try
                {
                    value = Convert.ToInt32(revisedpop);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            FInitialPopulation = value;

            // Build Pop OtherPopulation
            FPopulation = new CRF_Other_Population("Population", FInitialPopulation);

            // Power
            // NOTE QUAY 3 9 18
            // This is not an other CRF variable so why are we doing this here?
            // set to error value
            value = UDI.BadIntValue;
            errMsg = "";
            // get the PowerGen fieldname
            FldName = FData.OtherField(UDI.eOther.eoPowerGen);
            // now get the value from tyhe database
            if (FData.GetValue(UnitName, FldName, out value, out errMsg))
            {
                // Ouch!  This will stay as the error value
                // Not revising Power Gen
            }
            FInitialPowerGen = value;
            // END NOTE 3 9 18
        }

        protected virtual void BuildResources(string aUnitName)
        {
            UDI.eResource er;
            string FldName;
            int value;
            string errMsg = "";

            er = UDI.eResource.erSurfaceFresh;
            FldName = FData.ResourceField(er);

            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FSurfaceFresh = new CRF_Resource_SurfaceFresh(FldName, FData.ResourceLabel(er), SurfaceFreshColor, value);
            if (errMsg != "")
            {
                // ok so what is happening here.  If there was not an error retrieving this data, "value" is a good value and errMsg = "",
                // However, if there was an error, "value" = UDI.BadValue and errMsg has the error message.
                // So in this case we add the errmsg to the log for this data_item.
                FSurfaceFresh.AddError(errMsg);
            }
            FResources.Add(FSurfaceFresh);

            er = UDI.eResource.erSurfaceLake;
            FldName = FData.ResourceField(er);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FSurfaceLake = new CRF_Resource_SurfaceLake(FldName, FData.ResourceLabel(er), SurfaceLakeColor, value);
            if (errMsg != "")
            {
                // See above
                FSurfaceLake.AddError(errMsg);
            }
            FResources.Add(FSurfaceLake);


            er = UDI.eResource.erGroundwater;
            FldName = FData.ResourceField(er);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FGroundwater = new CRF_Resource_Groundwater(FldName, FData.ResourceLabel(er), GroundwaterColor, value);
            if (errMsg != "")
            {
                // See above
                FGroundwater.AddError(errMsg);
            }
            FResources.Add(FGroundwater);

            er = UDI.eResource.erReclained;
            FldName = FData.ResourceField(er);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FEffluent = new CRF_Resource_Effluent(FldName, FData.ResourceLabel(er), EffluentColor, value);
            if (errMsg != "")
            {
                // See above
                FEffluent.AddError(errMsg);
            }
            FResources.Add(FEffluent);

            er = UDI.eResource.erSurfaceSaline;
            FldName = FData.ResourceField(er);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FSurfaceSaline = new CRF_Resource_SurfaceSaline(FldName, FData.ResourceLabel(er), SurfaceSalineColor, value);
            if (errMsg != "")
            {
                // See above
               FSurfaceSaline.AddError(errMsg);
            }
            FResources.Add(FSurfaceSaline);
            // DAS 02.04.16
            // edits 01.13.22 das
            er = UDI.eResource.erAugmented;
            FldName = FData.ResourceField(er);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FAugmented = new CRF_Resource_Augmented(FldName, FData.ResourceLabel(er), AugmentedColor, value);
            FResources.Add(FAugmented);
            // end edits 01.13.22 das
            if (errMsg != "")
            {
                // See above
                FAugmented.AddError(errMsg);
            }
            FResources.Add(FAugmented);
            // end edits 01.13.22 das

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the consumers. </summary>
        ///
        /// <param name="aUnitName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void BuildConsumers(string aUnitName)
        {
            UDI.eConsumer ec;
            string FldName;
            int value;
            int otherConsumerSum = 0;
            int ptot = 0;
            double powerConsumption = 0;
            string errMsg;

            ec = UDI.eConsumer.ecUrban;
            FldName = FData.ConsumerField(ec);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FUrban = new CRF_Consumer_Urban(FldName, FData.ConsumerLabel(ec), UrbanColor, value);
            if (errMsg != "")
            {
                // ok so what is happening here.  If there was not an error retrieving this data, "value" is a good value and errMsg = "",
                // However, if there was an error, "value" = UDI.BadValue and errMsg has the error message.
                // So in this case we add the errmsg to the log for this data_item.
                FUrban.AddError(errMsg);
            }
            FConsumers.Add(FUrban);

            ec = UDI.eConsumer.ecAg;
            FldName = FData.ConsumerField(ec);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FAg = new CRF_Consumer_Agriculture(FldName, FData.ConsumerLabel(ec), AgColor, value);
            if (errMsg != "")
            {
                // See above
                FAg.AddError(errMsg);
            }
            FConsumers.Add(FAg);

            //ec = UDI.eConsumer.ecRural;
            //FldName = FData.ConsumerField(ec);
            //value = FData.GetValue(aUnitName, FldName);
            //FRural = new CRF_Consumer_Agriculture(FldName, FData.ConsumerLabel(ec), RuralColor, value);
            //// END QUAY EDIT
            //FConsumers.Add(FRural);

            ec = UDI.eConsumer.ecInd;
            FldName = FData.ConsumerField(ec);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            FInd = new CRF_Consumer_Industrial(FldName, FData.ConsumerLabel(ec), IndColor, value);
            if (errMsg != "")
            {
                // See above
                FInd.AddError(errMsg);
            }
            FConsumers.Add(FInd);

            ec = UDI.eConsumer.ecPower;
            FldName = FData.ConsumerField(ec);
            FData.GetValue(aUnitName, FldName, out value, out errMsg);
            ptot = value;

            //Edit : SUHAS - 3/2/2018 - Getting values for OPGW, OPSF and OPSURL and adding the values
            FData.GetValue(aUnitName, UDI.OPGW, out value, out errMsg);
            otherConsumerSum += value;

            FData.GetValue(aUnitName, UDI.OPSF, out value, out errMsg);
            otherConsumerSum += value;

            FData.GetValue(aUnitName, UDI.OPSURL, out value, out errMsg);
            otherConsumerSum += value;

            // Taking 4% of the sum value and adding it to PTOT value
            powerConsumption = ptot + (otherConsumerSum * 0.04);

            FPower = new CRF_Consumer_Power(FldName, FData.ConsumerLabel(ec), PowerColor, powerConsumption);
            // EDIT end - SUHAS : 3/2/2018

            if (errMsg != "")
            {
                // See above
                FPower.AddError(errMsg);
            }
            FConsumers.Add(FPower);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Assign fluxes. </summary>
        ///
        /// <param name="StateName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------

        protected virtual void AssignFluxes(string StateName)
        {
            string errMsg = "";
            // RQ 1/12/22  WHAT THE Why Do this twide ConstructFluxlist has already benne created.
            List<SDResourceConsumerLink> TheFluxes = FData.ConstructFluxList();

            foreach (CRF_Resource Res in FResources)
            {
                string whatthe = Res.Name;
                // go through each of the SDResourceConsumerLinks looking for a match
                foreach (SDResourceConsumerLink RCL in TheFluxes)
                {
                    // found one
                    if (Res.Name == RCL.Resource)
                    {
                        // lookin for a match consumer
                        foreach (CRF_Consumer Cons in FConsumers)
                        {
                            // found it. add this flux
                            if (RCL.Consumer == Cons.Name)
                            {
                                int Value = 0;
                                if (FData.GetValue(StateName, RCL.Flux, out Value, out errMsg))
                                {

                                    Res.AddConsumer(Cons, Value, CRF_Flux.Method.amAbsolute);
                                }
                            }
                        }
                    }
                }
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Change state. </summary>
        ///
        /// <param name="NewStateName"> Name of the new state. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ChangeUnit(string NewUnitName)
        {
           FName = NewUnitName;
            FResources.Clear();
            FConsumers.Clear();
            FOther.Clear();

            BuildResources(FName);
            BuildConsumers(FName);
            BuildOther(FName);
            AssignFluxes(FName);

            if (FCallback != null)
            {
                FCallback();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the unit. </summary>
        ///
        /// <value> The name of the unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public string unitName
        {
            get { return FName; }
            set { ChangeUnit(value); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the information describing the crf unit. </summary>
        ///
        /// <value> Information describing the crf unit. </value>
        ///-------------------------------------------------------------------------------------------------

        public UnitData CRFUnitData
        {
            get
            {
                return FData;
            }
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the surface fresh. </summary>
        ///
        /// <value> The surface fresh. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource SurfaceFresh
        {
            get { return FSurfaceFresh; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the surface lake. </summary>
        ///
        /// <value> The surface lake. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource SurfaceLake
        {
            get { return FSurfaceLake; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the surface saline. </summary>
        ///
        /// <value> The surface saline. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource SurfaceSaline
        {
            get { return FSurfaceSaline; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the groundwater. </summary>
        ///
        /// <value> The groundwater. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource Groundwater
        {
            get { return FGroundwater; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the effluent. </summary>
        ///
        /// <value> The effluent. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource Effluent
        {
            get { return FEffluent; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the augmented. </summary>
        ///
        /// <value> The augmented. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource Augmented
        {
            get { return FAugmented; }
        }
        //

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the urban. </summary>
        ///
        /// <value> The urban. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer Urban
        {
            get { return FUrban; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the agriculture. </summary>
        ///
        /// <value> The agriculture. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer Agriculture
        {
            get { return FAg; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the industrial. </summary>
        ///
        /// <value> The industrial. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer Industrial
        {
            get { return FInd; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the power. </summary>
        ///
        /// <value> The power. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer Power
        {
            get { return FPower; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the rural. </summary>
        ///
        /// <value> The rural. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Consumer Rural
        {
            get { return FRural; }
        }
        //

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the population. </summary>
        ///
        /// <value> The population. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Other Population
        {
            get { return FPopulation; }
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   List of crf unit networks. </summary>
    ///
    /// <seealso cref="System.Collections.Generic.List<WaterSimDCDC.Generic.CRF_Unit_Network>"/>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Unit_Network_List : List<CRF_Unit_Network>
    {

        /// <summary>   Default constructor. </summary>
        public CRF_Unit_Network_List()
            : base()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="TheUnitData">  A UnitData object that is loaded from a USGS file. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network_List(UnitData TheUnitData)
            : base()
        {
            List<string> DataUnitNames = TheUnitData.UnitNames;
            int DupCnt = 0;
            foreach (string TheUnitName in DataUnitNames)
            {
                // ok create CRF_Network;
                // QUAY EDIT 9/11/18 Colorado
                West_CRF_Unit_Network TempNetwork = new West_CRF_Unit_Network(TheUnitData, TheUnitName);
                //CRF_Unit_Network TempNetwork = new CRF_Unit_Network(TheUnitData, TheUnitName);
                // END EDIT
                Add(TempNetwork);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Crf network. </summary>
        ///
        /// <param name="UnitName"> Name of the unit. </param>
        ///
        /// <returns>  The CRF_Unit_Network with the name passed, otherwise return null . </returns>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Unit_Network CRFNetwork(string UnitName)
        {
                CRF_Unit_Network Temp = Find(
                    delegate(CRF_Unit_Network UNW)
                    {
                        return UNW.Name == UnitName;
                    }
                );

                return Temp;
        }

    }


}
