using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
//using USGSStateData;

using ConsumerResourceModelFramework;

namespace WaterSimDCDC.America
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A crf resource smith. </summary>
    ///
    /// <remarks>   Mcquay, 1/25/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CRF_Resource_Smith : CRF_Resource 
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Smith() : base()
        {
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcquay, 1/25/2016. </remarks>
        ///
        /// <param name="aName">    The name. </param>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource_Smith(string aName)
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

        public CRF_Resource_Smith(string aName, string aLabel, Color aColor)
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

        public CRF_Resource_Smith(string aName, string aLabel, Color aColor, double AvailableSupply)
            : base(aName, aLabel, aColor, AvailableSupply)
        {
        }
        //-----------------------


        public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

    public class CRF_Resource_SurfaceFresh : CRF_Resource_Smith
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

        public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

    public class CRF_Resource_SurfaceLake : CRF_Resource_Smith
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

        public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

         public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

         public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

         public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

     public class CRF_Resource_Effluent : CRF_Resource_Smith
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

         public override bool AllowFluxChangTo(CRF_DataItem FluxToItem)
         {
             // always allocate
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
             // can not return resources to effluent source
             return false;
         }

            }

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
        /// <param name="FluxItem"> The flux item. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

         public virtual bool AllowFluxChangTo(CRF_DataItem FluxToItem)
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

    public class CRF_Network_WaterSim_America : CRF_Network
    {
        // The Data
        StateData FStateData;
        string FStateName = "Florida";

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
        Color AugmentedColor = Color.GreenYellow;

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

        public CRF_Network_WaterSim_America(string USGSDataFilename, NetworkResetDelegate aCallback, string InitialStateName) : base()
        {

            FStateData = new StateData(USGSDataFilename);
            if (!FStateData.DataLoaded)
            {
                //YIKES THIS is  AN ERROR
                throw( new Exception("USGS data file did not load"));
            }

            if (InitialStateName!="")
            {
                FStateName = InitialStateName;
            }
            FResources = new CRF_ResourceList();
            FResources.Owner = this;
            FConsumers = new CRF_ConsumerList();
            FConsumers.Owner = this;
            FOther = new CRF_OtherList();
            FOther.Owner = this;
            BuildResources(FStateName);
            BuildConsumers(FStateName);
            // 09.08.16 das
            BuildOther(FStateName);
            //
            AssignFluxes(FStateName);

            FCallback = aCallback;
            if (FCallback != null)
            {
                FCallback();
            }

        }
        public CRF_Network_WaterSim_America(string USGSDataFilename, string InitialStateName)
        {

            FStateData = new StateData(USGSDataFilename);
            if (!FStateData.DataLoaded)
            {
                //YIKES THIS is  AN ERROR
                throw (new Exception("USGS data file did not load"));
            }

            if (InitialStateName != "")
            {
                FStateName = InitialStateName;
            }
            FResources = new CRF_ResourceList();
            FResources.Owner = this;
            FConsumers = new CRF_ConsumerList();
            FConsumers.Owner = this;
            FOther = new CRF_OtherList();
            FOther.Owner = this;

            BuildResources(FStateName);
            BuildConsumers(FStateName);
            // 09.08.16 das
            BuildOther(FStateName);
            AssignFluxes(FStateName);

            //FCallback = aCallback;
            //if (FCallback != null)
            //{
            //    FCallback();
            //}

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initial power generated. </summary>
        ///
        /// <param name="State">    The state. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------

        public int InitialPowerGenerated(string State)
        {
            int Power = FStateData.GetValue(State,StateData.PowerGeneratedFieldname);
            return Power;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds Other resources. </summary>
        ///
        /// <param name="StateName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------
        protected void BuildOther(string StateName)
        {
            StateData.eOther er;
            string FldName;
            int value;
            double temp = 0;
            //double correctTo2015 = 1.05;
            double correctTo2015 = 1.055;
            double correctToUnits = 1000;

            er = StateData.eOther.erPopulation;
            FldName = FStateData.OtherField(er);
            value = FStateData.GetValue(StateName, FldName);
            temp = value;
            value = Convert.ToInt32(temp * correctTo2015 * correctToUnits);
            FPopulation = new CRF_Other_Population(FldName, value);
            FOther.Add(FPopulation);
        }
        protected void BuildResources(string StateName)
        {
            StateData.eResource er;
            string FldName;
            int value;
            
            er = StateData.eResource.erSurfaceFresh;
            FldName = FStateData.ResourceField(er);
            value = FStateData.GetValue(StateName, FldName);
            FSurfaceFresh = new CRF_Resource_SurfaceFresh(FldName, FStateData.ResourceLabel(er), SurfaceFreshColor, value);
            FResources.Add(FSurfaceFresh);

            er = StateData.eResource.erSurfaceLake;
            FldName = FStateData.ResourceField(er);
            value = FStateData.GetValue(StateName, FldName);
            FSurfaceLake = new CRF_Resource_SurfaceLake(FldName, FStateData.ResourceLabel(er), SurfaceLakeColor, value);
            FResources.Add(FSurfaceLake);


            er = StateData.eResource.erGroundwater;
            FldName = FStateData.ResourceField(er);
            value = FStateData.GetValue(StateName, FldName);
            FGroundwater = new CRF_Resource_Groundwater(FldName, FStateData.ResourceLabel(er), GroundwaterColor, value);
            FResources.Add(FGroundwater);

            er = StateData.eResource.erReclained;
            FldName = FStateData.ResourceField(er);
            value = FStateData.GetValue(StateName, FldName);
            FEffluent = new CRF_Resource_Effluent(FldName, FStateData.ResourceLabel(er), EffluentColor, value);
            FResources.Add(FEffluent);

            er = StateData.eResource.erSurfaceSaline;
            FldName = FStateData.ResourceField(er);
            value = FStateData.GetValue(StateName, FldName);
            FSurfaceSaline = new CRF_Resource_SurfaceSaline(FldName, FStateData.ResourceLabel(er), SurfaceSalineColor, value);
            FResources.Add(FSurfaceSaline);
            // DAS 02.04.16
            //er = StateData.eResource.erAugmented;
            //FldName = FStateData.ResourceField(er);
            //value = FStateData.GetValue(StateName, FldName);
            //FAugmented = new CRF_Resource_Augmented(FldName, FStateData.ResourceLabel(er), AugmentedColor, value);
            //FResources.Add(FAugmented);



        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds the consumers. </summary>
        ///
        /// <param name="StateName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void BuildConsumers(string StateName)
        {
            StateData.eConsumer ec;
            string FldName;
            int value;

            ec = StateData.eConsumer.ecUrban;
            FldName = FStateData.ConsumerField(ec);
            value = FStateData.GetValue(StateName, FldName);
            FUrban = new CRF_Consumer_Urban(FldName, FStateData.ConsumerLabel(ec), UrbanColor, value);
            FConsumers.Add(FUrban);

            ec = StateData.eConsumer.ecAg;
            FldName = FStateData.ConsumerField(ec);
            value = FStateData.GetValue(StateName, FldName);
            FAg = new CRF_Consumer_Agriculture(FldName, FStateData.ConsumerLabel(ec),  AgColor, value);
            FConsumers.Add(FAg);

            //ec = StateData.eConsumer.ecRural;
            //FldName = FStateData.ConsumerField(ec);
            //value = FStateData.GetValue(StateName, FldName);
            //FRural = new CRF_Consumer_Agriculture(FldName, FStateData.ConsumerLabel(ec), RuralColor, value);
            //// END QUAY EDIT
            //FConsumers.Add(FRural);

            ec = StateData.eConsumer.ecInd;
            FldName = FStateData.ConsumerField(ec);
            value = FStateData.GetValue(StateName, FldName);
            FInd = new CRF_Consumer_Industrial(FldName, FStateData.ConsumerLabel(ec), IndColor, value);
            FConsumers.Add(FInd);

            ec = StateData.eConsumer.ecPower;
            FldName = FStateData.ConsumerField(ec);
            value = FStateData.GetValue(StateName, FldName);
            FPower = new CRF_Consumer_Power(FldName, FStateData.ConsumerLabel(ec), PowerColor, value);
            FConsumers.Add(FPower);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Assign fluxes. </summary>
        ///
        /// <param name="StateName">    Name of the state. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void AssignFluxes(string StateName)
        {
            List<SDResourceConsumerLink> TheFluxes = FStateData.ConstructFluxList();

            foreach (CRF_Resource Res in FResources)
            {
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
                                int Value = FStateData.GetValue(StateName, RCL.Flux);
                                Res.AddConsumer(Cons, Value, CRF_Flux.Method.amAbsolute);
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

        public void ChangeState(string NewStateName)
        {
            FStateName = NewStateName;
            FResources.Clear();
            FConsumers.Clear();
            FOther.Clear();

            BuildResources(FStateName);
            BuildConsumers(FStateName);
            BuildOther(FStateName);
            AssignFluxes(FStateName);

            if (FCallback != null)
            {
                FCallback();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a list of names of the states. </summary>
        ///
        /// <value> A list of names of the states. </value>
        ///-------------------------------------------------------------------------------------------------

        public string[] StateNames
        {
            get { return FStateData.StateNames; }
        }
        //
        // 
        public string StateName
        {
            get { return FStateName; }
            set { ChangeState(value); }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the surface fresh. </summary>
        ///
        /// <value> The surface fresh. </value>
        ///-------------------------------------------------------------------------------------------------

        public CRF_Resource SurfaceFresh
        { 
            get {return FSurfaceFresh;}
        }

        public CRF_Resource SurfaceLake
        {
            get { return FSurfaceLake; }
        }

        public CRF_Resource SurfaceSaline
        { 
            get {return FSurfaceSaline;}
        }

        public CRF_Resource Groundwater
        { 
            get {return FGroundwater;}
        }
        public CRF_Resource Effluent
        { 
            get {return FEffluent;}
        }
        public CRF_Resource Augmented
        {
            get { return FAugmented; }
        }
        //
        public CRF_Consumer Urban
        { 
            get {return FUrban;}
        }
        public CRF_Consumer Agriculture
        { 
            get {return FAg;}
        }
        public CRF_Consumer Industrial
        { 
            get {return FInd;}
        }
        public CRF_Consumer Power
        { 
            get {return FPower;}
        }
        public CRF_Consumer Rural
        { 
            get {return FRural;}
        }
        //
        public CRF_Other Population
        {
            get { return FPopulation; }
        }
    }


}
