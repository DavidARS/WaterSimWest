using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UniDB;
using ConsumerResourceModelFramework;
using WaterSimDCDC.Documentation;
//using WaterSimDCDC.America;
//=======================================================================================
// LEAPING INDICATORS!
// EMPTY 
// 
// version 1.5
//  2/29/16
// Status
// - - - - - - - - - - - - - - - - -
// Economic                 Draft
// Groundwater              Draft
// Urban Surface Water      Draft
// Ag Effciency             Draft
// Power Efficincy          Not STarted
// State Surface Warer      Not Started
// Urban Envirionmental     Not Started
// Urban Efficiency         Not Started
//
//  
//  NOTES
//   
//   1) Econmic needs some nonlinear scaling to make worse as numbers increase
//   2) 

//   NASTY
//   1) Circumvented WaterSmith agricultural projections by replacing code in WaterSim_Manager
//   2) Replaced POP poarameter with Magic Fix

namespace WaterSimDCDC
{


    //#######################################################################
    //  
    //   WaterSimManager  Partial Classes
    //    
    //##########################################################################
    #region WaterSimManager Classes
    /// <summary>   Model parameter. </summary>
    public static partial class eModelParam
    {

    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Manager for water simulations. </summary>
    ///
    /// <remarks>   Mcquay, 2/23/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public partial class WaterSimManager : WaterSimManagerClass
    {

        public bool initializeIndicators()
        {
            bool result = true;
            try
            {

            }
            catch (Exception ex)
            {
                string Mesg = ex.Message;
                // ouch need error trapping
            }

           
            return result;
        }
    }
#endregion

}
